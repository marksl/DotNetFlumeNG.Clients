// 
//    Copyright 2013 Mark Lamley
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetFlumeNG.Client.Core;
using NLog;
using NLog.Common;

namespace DotNetFlumeNG.Client.NLog
{
    public class NLogEventAdapter : LogEvent
    {
        private readonly LogEventInfo _logEventInfo;
        private readonly string _message;

        public NLogEventAdapter(string message, LogEventInfo logEventInfo)
            : base(logEventInfo != null ? logEventInfo.TimeStamp.ToUniversalTime() : DateTime.UtcNow)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (logEventInfo == null) throw new ArgumentNullException("logEventInfo");

            _message = message;
            _logEventInfo = logEventInfo;
        }

        public override LogPriority Priority
        {
            get
            {
                if (_logEventInfo.Level == LogLevel.Debug)
                    return LogPriority.Debug;

                if (_logEventInfo.Level == LogLevel.Error)
                    return LogPriority.Error;

                if (_logEventInfo.Level == LogLevel.Fatal)
                    return LogPriority.Fatal;

                if (_logEventInfo.Level == LogLevel.Info)
                    return LogPriority.Info;

                if (_logEventInfo.Level == LogLevel.Trace)
                    return LogPriority.Trace;

                if (_logEventInfo.Level == LogLevel.Warn)
                    return LogPriority.Warn;

                const string thisLoggingLevelIsNotSupported = "This logging level is not supported - {0}";

                InternalLogger.Error(thisLoggingLevelIsNotSupported, _logEventInfo.Level);
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
                                                              thisLoggingLevelIsNotSupported, _logEventInfo.Level));
            }
        }

        public override string Body
        {
            get { return _message; }
        }

        public override IDictionary<object, object> Fields
        {
            get { return _logEventInfo.Properties; }
        }

        public override string LoggerName
        {
            get { return _logEventInfo.LoggerName; }
        }
    }
}