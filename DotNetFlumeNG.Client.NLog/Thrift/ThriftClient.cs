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
using DotNetFlumeNG.Client.Core;
using Thrift.Protocol;
using Thrift.Transport;

namespace DotNetFlumeNG.Client.Thrift
{
    public class ThriftClient : IFlumeClient
    {
        private readonly ThriftFlumeEventServer.Client _client;
        private bool _disposed;
        private TSocket _transport;

        public ThriftClient(string host, int port)
        {
            if (host == null) throw new ArgumentNullException("host");

            _transport = new TSocket(host, port);
            TProtocol protocol = new TBinaryProtocol(_transport);
            _client = new ThriftFlumeEventServer.Client(protocol);
            _transport.Open();
        }

        public bool IsClosed { get { return !_transport.IsOpen; }}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Append(LogEvent logEvent)
        {
            var thriftFlumeEvent = new ThriftFlumeEventAdapter(logEvent);
            _client.append(thriftFlumeEvent);
        }

        ~ThriftClient()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transport.Close();
                    _transport.Dispose();
                    _transport = null;
                }
            }

            _disposed = true;
        }
    }
}