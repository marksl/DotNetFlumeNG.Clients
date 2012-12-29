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
using System.Threading;
using Thrift.Server;
using Thrift.Transport;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    public class MockThriftServer
    {
        private readonly Handler _handler;
        private readonly TServerTransport _serverTransport;

        public MockThriftServer()
        {
            _handler = new Handler();
            var processor = new ThriftFlumeEventServer.Processor(_handler);
            _serverTransport = new TServerSocket(9090);
            var server = new TThreadedServer(processor, _serverTransport);

            var serveOnSeparateThread = new Thread(server.Serve);
            serveOnSeparateThread.Start();
        }

        public List<ThriftFlumeEvent> ReceivedEvents
        {
            get { return _handler.Event; }
        }

        public void Close()
        {
            _serverTransport.Close();
        }

        private class Handler : ThriftFlumeEventServer.Iface
        {
            private readonly List<ThriftFlumeEvent> _events = new List<ThriftFlumeEvent>();
            private readonly object obj = new object();

            public List<ThriftFlumeEvent> Event
            {
                get { return _events; }
            }

            public void append(ThriftFlumeEvent evt)
            {
                lock (obj)
                {
                    _events.Add(evt);
                }
            }

            public void close()
            {
            }
        }
    }
}