using System;
using DotNetFlumeNG.Client.Core;
using Thrift.Protocol;
using Thrift.Transport;

namespace DotNetFlumeNG.Client.Thrift
{
    public class ThriftClient : IFlumeClient
    {
        private readonly ThriftFlumeEventServer.Client _client;
        private TSocket _transport;
        private bool _disposed;

        public ThriftClient(string host, int port)
        {
            _transport = new TSocket(host, port);
            TProtocol protocol = new TBinaryProtocol(_transport);
            _client = new ThriftFlumeEventServer.Client(protocol);
            _transport.Open();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Append(LogEvent logEvent)
        {
            _client.append(new ThriftFlumeEventAdapter(logEvent));
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