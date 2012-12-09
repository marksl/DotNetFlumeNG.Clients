using System;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client.Thrift
{
    public class ThriftClientPooled : ThriftClient
    {
        private readonly Pool<IFlumeClient> _pool;
        private readonly DateTime _start;

        public ThriftClientPooled(Pool<IFlumeClient> pool, string host, int port) :
            base(host, port)
        {
            _pool = pool;
            _start = DateTime.UtcNow;
        }

        protected bool SixtySecondsElapsed
        {
            get { return DateTime.UtcNow.Subtract(_start).TotalSeconds > 60; }
        }

        protected override void Dispose(bool disposing)
        {
            if (SixtySecondsElapsed)
            {
                // Release the underlying connection if it's older than 60 seconds
                base.Dispose(disposing);

                _pool.Release();
            }
            else
            {
                // If this connection is still 'fresh', then keep it open.
                _pool.ReturnToPool(this);
            }
        }
    }
}