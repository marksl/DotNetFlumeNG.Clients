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
using Avro.ipc;
using Avro.ipc.Specific;
using org.apache.flume.source.avro;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    public class MockAvroServer
    {
        private class AvroSourceProtocolImpl : AvroSourceProtocol
        {
            public AvroSourceProtocolImpl()
            {
                Events = new List<AvroFlumeEvent>();
            }

            public override Status append(AvroFlumeEvent evt)
            {
                Events.Add(evt);

                return Status.OK;
            }

            public override Status appendBatch(IList<AvroFlumeEvent> events)
            {
                throw new System.NotSupportedException();
            }

            public List<AvroFlumeEvent> Events { get; private set; }
        }

        private readonly AvroSourceProtocolImpl _handler;
        private readonly SocketServer _server;

        public MockAvroServer()
        {
            _handler = new AvroSourceProtocolImpl();
            
            _server = new SocketServer("localhost", 9090);
            _server.Start();

            var responder = new SpecificResponder<AvroSourceProtocol>(_handler);
            _server.SetResponder(responder);
        }

        public int Port
        {
            get { return _server.Port; }
        }

        public List<AvroFlumeEvent> ReceivedEvents
        {
            get { return _handler.Events; }
        }

        public void Close()
        {
            _server.Stop();
        }
    }
}