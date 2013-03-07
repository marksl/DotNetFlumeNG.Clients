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
using System.Collections.Generic;
using System.Globalization;
using DotNetFlumeNG.Client.Core;
using log4net.Core;

namespace DotNetFlumeNG.Client.log4net
{
    internal class Log4NetEventAdapter : LogEvent
    {
        private readonly Dictionary<object, object> _fields;
        private readonly LoggingEvent _logEventInfo;
        private readonly string _message;

        public Log4NetEventAdapter(string message, LoggingEvent logEventInfo)
            : base(logEventInfo != null ? logEventInfo.TimeStamp : DateTime.UtcNow)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (logEventInfo == null) throw new ArgumentNullException("logEventInfo");

            _message = message;
            _logEventInfo = logEventInfo;
            _fields = new Dictionary<object, object>();

            foreach (string key in logEventInfo.Properties.GetKeys())
            {
                _fields[key] = logEventInfo.Properties[key];
            }
        }

        public override LogPriority Priority
        {
            get
            {
                if (_logEventInfo.Level == Level.Log4Net_Debug
                    || _logEventInfo.Level == Level.Debug)
                    return LogPriority.Debug;

                if (_logEventInfo.Level == Level.Emergency
                    || _logEventInfo.Level == Level.Alert
                    || _logEventInfo.Level == Level.Critical
                    || _logEventInfo.Level == Level.Severe
                    || _logEventInfo.Level == Level.Error)
                    return LogPriority.Error;

                if (_logEventInfo.Level == Level.Fatal)
                    return LogPriority.Fatal;

                if (_logEventInfo.Level == Level.Info)
                    return LogPriority.Info;

                if (_logEventInfo.Level == Level.Trace
                    || _logEventInfo.Level == Level.Fine
                    || _logEventInfo.Level == Level.Finer
                    || _logEventInfo.Level == Level.Finest
                    || _logEventInfo.Level == Level.Verbose
                    || _logEventInfo.Level == Level.All)
                    return LogPriority.Trace;

                if (_logEventInfo.Level == Level.Warn
                    || _logEventInfo.Level == Level.Notice)
                    return LogPriority.Warn;

                const string thisLoggingLevelIsNotSupported = "This logging level is not supported - {0}";

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
            get { return _fields; }
        }

        public override string LoggerName
        {
            get { return _logEventInfo.LoggerName; }
        }
    }
}