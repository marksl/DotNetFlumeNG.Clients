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
using DotNetFlumeNG.Client.Core;
using NLog;
using NUnit.Framework;

namespace DotNetFlumeNG.Client.NLog.Tests.NLog
{
    [TestFixture]
    public class NLogEventAdapterTests
    {
        private static LogPriority GetPriorityFromAdapter(LogLevel logLevel)
        {
            return new NLogEventAdapter("Valid formatted message",
                                        new LogEventInfo(logLevel, "logger", "valid message")).Priority;
        }

        [Test]
        public void Body_MessageSet_BodyEqualsMessage()
        {
            const string expected = "message";

            var logEventInfo = new LogEventInfo();
            var nLogEventAdapter = new NLogEventAdapter(expected, logEventInfo);

            Assert.AreEqual(expected, nLogEventAdapter.Body);
        }

        [Test]
        public void Constructor_NoLogEventInfo_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new NLogEventAdapter("valid message", null));
        }

        [Test]
        public void Constructor_NoMessage_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new NLogEventAdapter(null, new LogEventInfo()));
        }

        [Test]
        public void Fields_PropertiesSet_FieldsEqualsProperties()
        {
            var logEventInfo = new LogEventInfo();
            logEventInfo.Properties.Add("foo", "bar");

            var nLogEventAdapter = new NLogEventAdapter("message", logEventInfo);

            Assert.IsNotNull(nLogEventAdapter.Fields);
            Assert.AreEqual("bar", nLogEventAdapter.Fields["foo"]);
        }

        [Test]
        public void Priority_MapsCorrectlyToErrorLevel()
        {
            Assert.AreEqual(LogPriority.Debug, GetPriorityFromAdapter(LogLevel.Debug));
            Assert.AreEqual(LogPriority.Error, GetPriorityFromAdapter(LogLevel.Error));
            Assert.AreEqual(LogPriority.Fatal, GetPriorityFromAdapter(LogLevel.Fatal));
            Assert.AreEqual(LogPriority.Info, GetPriorityFromAdapter(LogLevel.Info));
            Assert.AreEqual(LogPriority.Trace, GetPriorityFromAdapter(LogLevel.Trace));
            Assert.AreEqual(LogPriority.Warn, GetPriorityFromAdapter(LogLevel.Warn));

            Assert.Throws<NotSupportedException>(() => GetPriorityFromAdapter(LogLevel.Off),
                                                 "The FlumeTarget should not send any messages if the LogLevel is Off. The Adapter will throw an exception if the LogLevel is Off.");
        }
    }
}