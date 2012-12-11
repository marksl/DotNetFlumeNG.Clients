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