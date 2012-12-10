using System;
using System.Collections.Generic;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client.Thrift
{
    public class ThriftFlumeEventAdapter : ThriftFlumeEvent
    {
        public ThriftFlumeEventAdapter(LogEvent logEvent)
        {
            Host = logEvent.Host;
            Nanos = logEvent.Nanos;
            Timestamp = logEvent.TimestampInMilliseconds;

            Body = GetBytes(logEvent.Body);
            Priority = GetPriority(logEvent.Priority);

            SetFields(logEvent);
        }

        private static Priority GetPriority(LogPriority priority)
        {
            switch (priority)
            {
                case LogPriority.Debug:
                    return Priority.DEBUG;
                case LogPriority.Error:
                    return Priority.ERROR;
                case LogPriority.Fatal:
                    return Priority.FATAL;
                case LogPriority.Info:
                    return Priority.INFO;
                case LogPriority.Trace:
                    return Priority.TRACE;
                case LogPriority.Warn:
                    return Priority.WARN;
            }

            throw new NotSupportedException(string.Format("That priority level [{0}] is not supported", priority));
        }

        private void SetFields(LogEvent logEvent)
        {
            Fields = new Dictionary<string, byte[]>();
            if (logEvent.Fields != null && logEvent.Fields.Count > 0)
            {
                
                foreach (var i in logEvent.Fields.Keys)
                {
                    var str = logEvent.Fields[i].ToString();

                    Fields[i.ToString()] = GetBytes(str);
                }
            }
        }

        static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

    }
}