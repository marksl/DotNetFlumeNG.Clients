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
using System.Text;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client.LegacyThrift
{
    public class LegacyThriftFlumeEventAdapter : ThriftFlumeEvent
    {
        public LegacyThriftFlumeEventAdapter(LogEvent logEvent)
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

        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length*sizeof (char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return Encoding.Default.GetBytes(str);
        }
    }
}