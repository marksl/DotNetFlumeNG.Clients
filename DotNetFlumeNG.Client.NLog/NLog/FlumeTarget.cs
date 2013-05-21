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

using DotNetFlumeNG.Client.Avro;
using NLog;
using NLog.Targets;

namespace DotNetFlumeNG.Client.NLog
{
    [Target("Flume")]
    public sealed class FlumeTarget : TargetWithLayout
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Environment { get; set; }

        protected override void InitializeTarget()
        {
            client = new AvroClient(Host, Port);

            base.InitializeTarget();
        }

        private AvroClient client;
        protected override void Write(LogEventInfo logEvent)
        {
            if (logEvent.Level == LogLevel.Off)
            {
                return;
            }

            string formattedText = Layout.Render(logEvent);
            var nLogEventAdapter = new NLogEventAdapter(formattedText, logEvent, Environment);

            client.Append(nLogEventAdapter);
        }

        protected override void CloseTarget()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }

            base.CloseTarget();
        }
    }
}