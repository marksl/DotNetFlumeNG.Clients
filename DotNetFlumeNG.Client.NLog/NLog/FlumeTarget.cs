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

using System.Collections.Generic;
using System.Linq;
using DotNetFlumeNG.Client.Avro;
using DotNetFlumeNG.Client.Core;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace DotNetFlumeNG.Client.NLog
{
    [Target("Flume")]
    public sealed class FlumeTarget : TargetWithLayout
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Environment { get; set; }

        private AvroClient _client;
        private readonly object _lockObj = new object();

        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent.Level == LogLevel.Off)
            {
                return;
            }

            string formattedText = Layout.Render(logEvent);
            var nLogEventAdapter = new NLogEventAdapter(formattedText, logEvent, Environment);

            GetClient().Append(nLogEventAdapter);
        }

        AvroClient GetClient()
        {
            if (_client == null)
            {
                lock (_lockObj)
                {
                    InternalLogger.Debug(string.Format("Connecting to {0}:{1} via avro...", Host, Port));
                    _client = new AvroClient(Host, Port);
                    InternalLogger.Debug(string.Format("Connection succeeded"));
                }
            }

            return _client;
        }

        protected override void Write(global::NLog.Common.AsyncLogEventInfo[] logEvents)
        {
            if (!logEvents.Any())
                return;

            var events = new List<LogEvent>();

            foreach (var e in logEvents)
            {
                LogEventInfo logEvent = e.LogEvent;
                if (logEvent.Level == LogLevel.Off)
                    continue;

                string formattedText = Layout.Render(logEvent);
                var nLogEventAdapter = new NLogEventAdapter(formattedText, logEvent, Environment);

                events.Add(nLogEventAdapter);
            }

            InternalLogger.Debug(string.Format("Writing {0} events to remote host...", events.Count));
            GetClient().AppendBatch(events.ToArray());
            InternalLogger.Debug(string.Format("Completed writing events."));
        }

        protected override void CloseTarget()
        {
            lock (_lockObj)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }

            base.CloseTarget();
        }
    }
}