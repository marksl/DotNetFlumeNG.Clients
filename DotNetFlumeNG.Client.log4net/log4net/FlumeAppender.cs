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
using log4net.Appender;
using log4net.Core;

namespace DotNetFlumeNG.Client.log4net
{
    public class FlumeAppender : AppenderSkeleton
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Environment { get; set; }

        private AvroClient client;

        public override void ActivateOptions()
        {
            client = new AvroClient(Host, Port);

            base.ActivateOptions();
        }

        protected override void Append(LoggingEvent logEvent)
        {
            if (logEvent.Level == Level.Off)
            {
                return;
            }

            string formattedText = RenderLoggingEvent(logEvent);
            var nLogEventAdapter = new Log4NetEventAdapter(formattedText, logEvent, Environment);

            client.Append(nLogEventAdapter);
        }

        protected override void OnClose()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }

            base.OnClose();
        }
    }
}