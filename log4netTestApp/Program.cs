using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetFlumeNG.Client.NLog.Tests;
using log4net;
using log4net.Config;

namespace log4netTestApp
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var server = new MockThriftServer();

            logger.Info("Test Message");
            Thread.Sleep(100);
            server.Close();

            ThriftFlumeEvent receivedEvent = server.ReceivedEvents[0];
            string actualMessage = StringHelpers.GetString(receivedEvent.Body);

            if (1 != server.ReceivedEvents.Count)
                throw new InvalidProgramException("The server should receive 1 event.");

            if (Priority.INFO != receivedEvent.Priority)
                throw new InvalidProgramException();

            if (!actualMessage.Contains("Test Message"))
                throw new InvalidProgramException();
        }
    }
}
