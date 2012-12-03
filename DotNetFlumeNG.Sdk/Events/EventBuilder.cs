using System;
using System.Collections.Generic;
using System.Text;

namespace NFlumeNG.Sdk.Events
{
    public class EventBuilder
    {
        public static IEvent WithBody(byte[] body, Dictionary<String, String> headers)
        {
            IEvent evt = new SimpleEvent();

            evt.Body = body;

            if (headers != null)
            {
                evt.Headers = headers;
            }

            return evt;
        }

        public static IEvent WithBody(string message, Encoding headers, Dictionary<string, string> hdrs)
        {
            var bytes = new byte[message.Length*sizeof (char)];
            Buffer.BlockCopy(message.ToCharArray(), 0, bytes, 0, bytes.Length);

            return WithBody(bytes, hdrs);
        }
    }
}