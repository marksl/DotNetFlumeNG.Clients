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
        private readonly string _message;
        private readonly LogEventInfo _logEventInfo;

        public NLogEventAdapter(string message, LogEventInfo logEventInfo)
        {
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
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, thisLoggingLevelIsNotSupported, _logEventInfo.Level));
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
    }
}