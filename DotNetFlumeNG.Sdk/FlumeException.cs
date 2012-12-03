using System;

namespace NFlumeNG.Sdk
{
    public class FlumeException : Exception
    {
        public FlumeException(string message)
            : base(message)
        {
        }

        public FlumeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}