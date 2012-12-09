using System;
using DotNetFlumeNG.Client.Core;
using Thrift.Protocol;
using Thrift.Transport;

namespace DotNetFlumeNG.Client.Thrift
{
    public class ThriftClient : IFlumeClient
    {
        private readonly ThriftFlumeEventServer.Client client;
        private TSocket transport;
        private bool disposed;

        public ThriftClient(string host, int port)
        {
            transport = new TSocket(host, port);
            TProtocol protocol = new TBinaryProtocol(transport);
            client = new ThriftFlumeEventServer.Client(protocol);
        }

        public void Dispose()
        {
            // We start by calling Dispose(bool) with true
            Dispose(true);
            // Now suppress finalization for this object, since we've already handled our resource cleanup tasks
            GC.SuppressFinalize(this);
        }

        public void Append(LogEvent logEvent)
        {
            client.append(new ThriftFlumeEventAdapter(logEvent));
        }

        ~ThriftClient()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Use our disposed flag to allow us to call this method multiple times safely.
            // This is a requirement when implementing IDisposable
            if (!disposed)
            {
                if (disposing)
                {
                    transport.Dispose();
                    transport = null;
                }
            }

            // Mark us as disposed, to prevent multiple calls to dispose from having an effect, 
            // and to allow us to handle ObjectDisposedException
            disposed = true;
        }
    }
}