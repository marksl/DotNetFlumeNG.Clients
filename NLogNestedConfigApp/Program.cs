using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DotNetFlumeNG.Client.NLog.Tests;
using NLog;

namespace NLogNestedConfigApp
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        private static void Main(string[] args)
        {
            // Can't run these until there is a new version of the NLog client.

            var server = new MockThriftServer();

            logger.Info("Test Message");
            logger.Info("Test Message");
            Thread.Sleep(100);

            // Release the connection to the server
            LogManager.Configuration.AllTargets[0].Dispose();

            // Cleanup the server
            server.Close();

            if (1 != server.ReceivedEvents.Count)
                throw new InvalidProgramException("The server should receive 1 event.");

            ThriftFlumeEvent receivedEvent = server.ReceivedEvents[0];
            string actualMessage = StringHelpers.GetString(receivedEvent.Body);

            if (Priority.INFO != receivedEvent.Priority)
                throw new InvalidProgramException();

            if (!actualMessage.Contains("Test Message"))
                throw new InvalidProgramException();
        }
    }
}
