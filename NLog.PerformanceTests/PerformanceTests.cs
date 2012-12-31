using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using DotNetFlumeNG.Client.Core;
using DotNetFlumeNG.Client.NLog;
using NLog.Common;
using NLog.Config;
using NUnit.Framework;

namespace NLog.PerformanceTests
{
    [TestFixture]
    public class PerformanceTests
    {
        private static MemoryStream InitConfiguration(int poolSize, int numberOfThreads, int numberOfRandomStrings = 400)
        {
            var config = new LoggingConfiguration();
            
            var flumeTarget = new FlumeTarget
                                  {
                                      Client = ClientType.Thrift,
                                      Layout = "${date:format=HH\\:MM\\:ss} ${logger} ${message}",
                                  };
            if (poolSize == 0)
            {
                flumeTarget.UsePool = false;
            }
            else
            {
                flumeTarget.UsePool = true;
                flumeTarget.PoolSize = 4;
                flumeTarget.Retries = 0;
            }

            flumeTarget.FlumeSources.Add(new FlumeSource { Host = "192.168.1.65", Port = 9090 });
            config.AddTarget("flume", flumeTarget);

            var flume = new LoggingRule("*", LogLevel.Debug, flumeTarget);
            config.LoggingRules.Add(flume);

            var memoryStream = new MemoryStream();
            InternalLogger.LogWriter = new StreamWriter(memoryStream);
            LogManager.Configuration = config;

            var threads = StartThreads(numberOfThreads, numberOfRandomStrings);

            WaidForThreads(threads);

            return memoryStream;
        }

        [Test]
        public void NoPools_FourThreads()
        {
            // This generates thousands of TCP connections
            var errorStream = InitConfiguration(poolSize: 0, numberOfThreads: 4);

            AssertIsEmpty(errorStream);
        }

        [Test]
        public void PoolSizeFour_FourThreads()
        {
            var errorStream = InitConfiguration(poolSize: 4, numberOfThreads: 4, numberOfRandomStrings:4000);

            AssertIsEmpty(errorStream);
        }


        private static void WaidForThreads(List<Thread> threads)
        {
            while (threads.Any(x => x.IsAlive))
            {
                Thread.Sleep(5000);
            }
        }

        private static void AssertIsEmpty(Stream memoryStream)
        {
            var reader = new StreamReader(memoryStream);
            var errors = reader.ReadToEnd();

            Assert.IsNullOrEmpty(errors);
        }

        private static List<Thread> StartThreads(int numberOfThreads, int numberOfRandomStrings)
        {
            var threads = new List<Thread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                var thread = new Thread(SendRandomString) {IsBackground = true};
                thread.Start(numberOfRandomStrings);

                threads.Add(thread);
            }
            return threads;
        }

        private static void SendRandomString(object numRandomStrings)
        {
            var logger = LogManager.GetLogger("Four-Threads-PoolSize-Four");
            for (int i = 0; i < (int)numRandomStrings; i++)
            {
                logger.Info(Path.GetRandomFileName());
            }
        }
    }
}
