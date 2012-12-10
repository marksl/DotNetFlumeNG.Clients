using NLog.Config;

namespace DotNetFlumeNG.Client.Core
{
    [NLogConfigurationItem]
    public class FlumeSource
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}