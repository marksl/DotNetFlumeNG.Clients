using System.Collections.Generic;

namespace NFlumeNG.Sdk.Events
{
    public class SimpleEvent : IEvent
    {
        public SimpleEvent()
        {
            Headers = new Dictionary<string, string>();
            Body = new byte[0];
        }

        public Dictionary<string, string> Headers { get; set; }
        public byte[] Body { get; set; }

        public override string ToString()
        {
            int bodyLen = 0;
            if (Body != null) bodyLen = Body.Length;
            return "[Event headers = " + Headers + ", body.length = " + bodyLen + " ]";
        }
    }
}