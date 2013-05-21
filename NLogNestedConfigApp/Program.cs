// 
//     Copyright 2013 Mark Lamley
//  
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//  
//         http://www.apache.org/licenses/LICENSE-2.0
//  
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.

using System;
using System.Threading;
using DotNetFlumeNG.Client.NLog.Tests;
using NLog;
using org.apache.flume.source.avro;

namespace NLogNestedConfigApp
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        private static void Main(string[] args)
        {
            // Can't run these until there is a new version of the NLog client.

            var server = new MockAvroServer();

            logger.Info("Test Message");
            logger.Info("Test Message");
            Thread.Sleep(100);

            // Release the connection to the server
            LogManager.Configuration.AllTargets[0].Dispose();

            // Cleanup the server
            server.Close();

            if (1 != server.ReceivedEvents.Count)
                throw new InvalidProgramException("The server should receive 1 event.");

            AvroFlumeEvent receivedEvent = server.ReceivedEvents[0];
            string actualMessage = StringHelpers.GetString(receivedEvent.body);

            if ("20000" != receivedEvent.headers["flume.client.log4j.log.level"])
                throw new InvalidProgramException();

            if (!actualMessage.Contains("Test Message"))
                throw new InvalidProgramException();
        }
    }
}
