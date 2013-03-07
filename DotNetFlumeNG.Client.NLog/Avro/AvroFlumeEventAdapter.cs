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
using System.Text;
using DotNetFlumeNG.Client.Core;
using org.apache.flume.source.avro;

namespace DotNetFlumeNG.Client.Avro
{
    // This class is mainly a port of
    // C:\git\flume-trunk\flume-ng-clients\flume-ng-log4jappender\src\main\java\org\apache\flume\clients\log4jappender\Log4jAppender.java
    public class AvroFlumeEventAdapter : AvroFlumeEvent
    {
        public AvroFlumeEventAdapter(LogEvent logEvent)
        {
            headers = new Dictionary<string, string>();
            headers["flume.client.log4j.logger.name"] = logEvent.LoggerName;
            headers["flume.client.log4j.log.level"] = GetLevel(logEvent.Priority).ToString();
            headers["flume.client.log4j.timestamp"] = logEvent.TimestampInMilliseconds.ToString();
            headers["flume.client.log4j.message.encoding"] = "UTF8";

            // Not used...
            // OTHER("flume.client.log4j.logger.other"),

            // These don't seem to have mappings to Avro.
            //Host = logEvent.Host
            //Nanos = logEvent.Nanos;
            
            SetFields(logEvent);

            body = GetBytes(logEvent.Body);
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

                    headers[i.ToString()] = str;
                }
            }
        }

    }
}