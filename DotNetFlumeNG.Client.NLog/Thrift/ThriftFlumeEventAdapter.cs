// 
//    Copyright 2014 Mark Lamley
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

namespace DotNetFlumeNG.Client.Thrift
{
    public class ThriftFlumeEventAdapter : ThriftFlumeEvent
    {   
        public ThriftFlumeEventAdapter(LogEvent logEvent)
        {
            Headers = new Dictionary<string, string>();
            Headers["flume.client.log4j.logger.name"] = logEvent.LoggerName;
            Headers["flume.client.log4j.log.level"] = GetLevel(logEvent.Priority).ToString();
            Headers["flume.client.log4j.timestamp"] = logEvent.TimestampInMilliseconds.ToString();
            Headers["flume.client.log4j.message.encoding"] = "UTF8";

            // Not used...
            // OTHER("flume.client.log4j.logger.other"),

            // These don't seem to have mappings to Avro.
            //Host = logEvent.Host
            //Nanos = logEvent.Nanos;

            SetFields(logEvent);

            Body = GetBytes(logEvent.Body);
        }

        // TODO: fix Duplicate code
        private static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return Encoding.UTF8.GetBytes(str);
        }

        // This is based on:
        // http://www.docjar.com/html/api/org/apache/log4j/Priority.java.html
        // http://www.docjar.com/html/api/org/apache/log4j/Level.java.html
        //
        private int GetLevel(LogPriority priority)
        {
            switch (priority)
            {
                case LogPriority.Fatal:
                    return 50000;

                case LogPriority.Error:
                    return 40000;

                case LogPriority.Warn:
                    return 30000;

                case LogPriority.Info:
                    return 20000;

                case LogPriority.Debug:
                    return 10000;

                case LogPriority.Trace:
                    return int.MinValue;
            }

            throw new NotSupportedException(string.Format("That priority level [{0}] is not supported", priority));
        }

        private void SetFields(LogEvent logEvent)
        {
            if (logEvent.Fields != null && logEvent.Fields.Count > 0)
            {
                foreach (var i in logEvent.Fields.Keys)
                {
                    var str = logEvent.Fields[i].ToString();

                    Headers[i.ToString()] = str;
                }
            }
        }
    }
}
