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

using DotNetFlumeNG.Client.Core;
using log4net.Appender;
using log4net.Core;

namespace DotNetFlumeNG.Client.log4net
{
    public class FlumeAppender : AppenderSkeleton
    {
        public FlumeAppender()
        {
            Client = ClientType.Thrift;
        }

        public ClientType Client { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public override void ActivateOptions()
        {
            FlumeClientFactory.Init(Client, Host, Port);

            base.ActivateOptions();
        }

        protected override void Append(LoggingEvent logEvent)
        {
            if (logEvent.Level == Level.Off)
            {
                return;
            }

            string formattedText = RenderLoggingEvent(logEvent);
            var nLogEventAdapter = new Log4NetEventAdapter(formattedText, logEvent);

            var client = FlumeClientFactory.CreateClient();
            client.Append(nLogEventAdapter);
        }

        protected override void OnClose()
        {
            FlumeClientFactory.Close();

            base.OnClose();
        }
    }
}