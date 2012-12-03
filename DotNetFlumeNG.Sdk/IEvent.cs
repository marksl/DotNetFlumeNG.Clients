using System;
using System.Collections.Generic;

namespace NFlumeNG.Sdk
{
    public interface IEvent
    {
        Dictionary<String, String> Headers { get; set; }
        byte[] Body { get; set; }
    }
}