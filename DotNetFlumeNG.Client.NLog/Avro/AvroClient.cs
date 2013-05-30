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
using System.Linq;
using Avro.ipc;
using Avro.ipc.Specific;
using DotNetFlumeNG.Client.Core;
using org.apache.flume.source.avro;

#if _NLOG_
using NLog.Common;
#endif


namespace DotNetFlumeNG.Client.Avro
{
    public class AvroClient : IFlumeClient
    {
        private SocketTransceiver _socketTransceiver;
        private readonly AvroSourceProtocol _client;
        private bool _disposed;

        public AvroClient(string host, int port)
        {
            if (host == null) throw new ArgumentNullException("host");
            if (port < 0) throw new ArgumentOutOfRangeException("port", "Port must be greater than or equal to zero.");

            _socketTransceiver = new SocketTransceiver(host, port);
            _client = SpecificRequestor.CreateClient<AvroSourceProtocol>(_socketTransceiver);
        }

        public bool IsClosed
        {
            get { return _socketTransceiver.IsConnected; }
        }

        public void Append(LogEvent logEvent)
        {
            var e = new AvroFlumeEventAdapter(logEvent);

            Status result = _client.append(e);
#if _NLOG_
            InternalLogger.Debug("{0} - {1}", result, e.body);
#endif
        }


        public void AppendBatch(LogEvent[] logEvents)
        {
            var events = logEvents.Select(l => new AvroFlumeEventAdapter(l)).ToArray();

            Status result = _client.appendBatch(events);
#if _NLOG_
            foreach (var e in events)
            {
                InternalLogger.Debug("{0} - {1}", result, e.body);
            }
#endif
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AvroClient()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _socketTransceiver.Disconnect();
                    _socketTransceiver = null;
                }
            }

            _disposed = true;
        }
    }
}