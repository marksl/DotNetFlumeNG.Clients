using System.Collections.Generic;
using System.Threading;
using Thrift.Server;
using Thrift.Transport;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    public class MockThriftServer
    {
        public MockThriftServer()
        {
            _handler = new Handler();
            var processor = new ThriftFlumeEventServer.Processor(_handler);
            _serverTransport = new TServerSocket(9090);
            var server = new TThreadedServer(processor, _serverTransport);

            var serveOnSeparateThread = new Thread(server.Serve);
            serveOnSeparateThread.Start();
        }

        public void Close()
        {
            _serverTransport.Close();
        }

        private readonly TServerTransport _serverTransport;

        private class Handler : ThriftFlumeEventServer.Iface
        {
            private readonly object obj = new object();
            private readonly List<ThriftFlumeEvent> _events = new List<ThriftFlumeEvent>();

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

            public List<ThriftFlumeEvent> Event
            {
                get { return _events; }
            }
        }

        private readonly Handler _handler;

        public List<ThriftFlumeEvent> ReceivedEvents
        {
            get { return _handler.Event; }
        }
    }

}