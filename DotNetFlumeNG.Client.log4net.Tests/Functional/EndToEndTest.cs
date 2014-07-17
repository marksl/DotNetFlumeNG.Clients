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

using System.Threading;
using DotNetFlumeNG.Client.LegacyThrift;
using NUnit.Framework;
using log4net;
using log4net.Config;

namespace DotNetFlumeNG.Client.log4net.Tests.Functional
{
    [TestFixture]
    public class EndToEndTest
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof (EndToEndTest));

        /// <summary>
        ///     This tests the following:
        ///     1. Creates a thrift server.
        ///     2. Load log4net configuration from App.config.
        ///     3. Using log4net, logs a message.
        ///     4. Verifies the thrift server receives the message.
        /// </summary>
        [Test]
        public void TestEndtoEnd()
        {
            XmlConfigurator.Configure();

            var server = new MockThriftServer();

            logger.Info("Test Message");
            Thread.Sleep(100);
            server.Close();

            Assert.AreEqual(1, server.ReceivedEvents.Count, "The server should receive 1 event.");
            ThriftFlumeEvent receivedEvent = server.ReceivedEvents[0];
            string actualMessage = StringHelpers.GetString(receivedEvent.Body);
            Assert.AreEqual(Priority.INFO, receivedEvent.Priority);
            Assert.IsTrue(actualMessage.Contains("Test Message"));
        }
    }
}