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
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace DotNetFlumeNG.Client.Core
{
    public abstract class LogEvent
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public long TimestampInMilliseconds { get; private set; }

        public DateTime Timestamp
        {
            set { TimestampInMilliseconds = (long) (value - Jan1st1970).TotalMilliseconds; }
        }

        public long Nanos
        {
            get
            {
                // This seems to be the closet thing to System.nanoTime(). The units on my local machine appear to be way off considering
                // that System.nanoTime() is in nano seconds, and Stopwatch.GetTimestamp() is in 100 ns units. On my local machine
                // Based on this comment from Log4JEventAdaptor.java I believe Stopwatch.GetTimestamp() is what I want.
                // This is needed to differentiate between events at the same millisecond.
                return Stopwatch.GetTimestamp();
            }
        }

        public string Host
        {
            get
            {
                IPHostEntry he = Dns.GetHostEntry(Environment.UserDomainName);
                return he.HostName;
            }
        }

        public abstract LogPriority Priority { get; }
        public abstract string Body { get; }
        public abstract IDictionary<object, object> Fields { get; }
    }
}