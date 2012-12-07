using System.Configuration;

namespace DotNetFlumeNG.Client.NLog.Configuration
{
    public class Source : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Collector", IsRequired = true)]
        public string Collector
        {
            get { return (string)this["Collector"]; }
            set { this["Collector"] = value; }
        }
    }
}