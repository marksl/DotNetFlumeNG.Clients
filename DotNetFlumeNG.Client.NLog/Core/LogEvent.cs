using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace DotNetFlumeNG.Client.Core
{
    public abstract class LogEvent
    {
        private static readonly DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
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

        // Hidden
        //private long _nanos;
        //private string _host;
        //private Dictionary<string, byte[]> _fields;
    }
}
