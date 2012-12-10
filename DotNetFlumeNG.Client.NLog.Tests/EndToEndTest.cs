using System.Threading;
using NLog;
using NUnit.Framework;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    [TestFixture]
    public class EndToEndTest
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// This tests the following:
        /// 
        /// 1. Creates a thrift server.
        /// 2. Load NLog configuration from App.config.
        /// 3. Using Nlog, logs a message.
        /// 4. Verifies the thrift server receives the message.
        /// 
        /// </summary>
        [Test]
        public void TestEndtoEnd()
        {
            var server = new MockThriftServer();

            logger = LogManager.GetLogger("EndToEndTest");
            logger.Info("Test Message");

            Thread.Sleep(2000);

            server.Close();

            Assert.AreEqual(1, server.ReceivedEvents.Count, "The server should receive 1 event.");
            ThriftFlumeEvent receivedEvent = server.ReceivedEvents[0];
            string actualMessage = StringHelpers.GetString(receivedEvent.Body);
            Assert.AreEqual(Priority.INFO, receivedEvent.Priority);
            Assert.IsTrue(actualMessage.Contains("Test Message"));
        }
    }
}