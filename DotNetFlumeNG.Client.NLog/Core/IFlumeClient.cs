using System;

namespace DotNetFlumeNG.Client.Core
{
    public interface IFlumeClient : IDisposable
    {
        void Append(LogEvent logEvent);
    }
}