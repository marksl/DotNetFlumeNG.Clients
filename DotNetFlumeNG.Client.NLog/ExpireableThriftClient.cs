using System;
using DotNetFlumeNG.Client.NLog.Pooling;
using Thrift.Protocol;
using Thrift.Transport;

namespace DotNetFlumeNG.Client.NLog
{
    internal class ExpireableThriftClient : ExpireableItem, IDisposable
    {
        private readonly ThriftFlumeEventServer.Client client;
        private readonly Pool<ExpireableThriftClient> _pool;
        private readonly TSocket transport;

        public ExpireableThriftClient(Pool<ExpireableThriftClient> pool, TSocket transport, ThriftFlumeEventServer.Client client)
        {
            if (transport == null) throw new ArgumentNullException("transport");
            if (client == null) throw new ArgumentNullException("client");

            _pool = pool;
            this.transport = transport;
            this.client = client;
        }

        // Hmmm this might get wonky.
        public void Dispose()
        {
            if (_pool.IsDisposed)
            {
                transport.Dispose();
            }
            else
            {
                _pool.Release(this);
            }
        }

        public static ExpireableThriftClient CreateConnection(Pool<ExpireableThriftClient> arg)
        {
            var transport = new TSocket("TODO", 123);
            TProtocol protocol = new TBinaryProtocol(transport);
            var client = new ThriftFlumeEventServer.Client(protocol);
            return new ExpireableThriftClient(arg,  transport, client);
        }

        public void Append(ThriftFlumeEvent thriftFlumeEvent)
        {
            client.append(thriftFlumeEvent);
        }
    }
}