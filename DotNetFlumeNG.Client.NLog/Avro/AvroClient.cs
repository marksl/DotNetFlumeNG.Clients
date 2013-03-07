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
using System.IO;
using System.Reflection;
using Avro;
using Avro.ipc;
using Avro.ipc.Specific;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client.Avro
{
    public class AvroClient : IFlumeClient
    {
        private readonly Protocol _protocol;
        private readonly SocketTransceiver _socketTransceiver;
        private readonly SpecificRequestor _specificRequestor;
        private bool _disposed;

        public AvroClient(string host, int port)
        {
            _socketTransceiver = new SocketTransceiver();
            _socketTransceiver.Connect(host, port);

            string json = GetJsonFromEmbeddedResource();
            _protocol = Protocol.Parse(json);

            _specificRequestor = new SpecificRequestor(_socketTransceiver, _protocol);
        }
       
        public bool IsClosed { get; private set; }

        public void Append(LogEvent logEvent)
        {
            var adapter = new AvroFlumeEventAdapter(logEvent);
            _specificRequestor.Request("append", new object[] {adapter});
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
                }
            }

            _disposed = true;
        }

        private static string GetJsonFromEmbeddedResource()
        {
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("DotNetFlumeNG.Client.Avro.flume.avpr"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}