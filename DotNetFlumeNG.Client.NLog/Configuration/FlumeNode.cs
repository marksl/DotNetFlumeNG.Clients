using System.Configuration;

namespace DotNetFlumeNG.Client.NLog.Configuration
{
    public class FlumeNode : ConfigurationElement
    {
        public FlumeNode()
        {
            Host = "localhost";
            Enabled = true;
            Port = 2014;
            DangerZoneThreshold = 5;
            DangerZoneSeconds = 30;
            EnablePool = true;
            TimeOut = 120;
            PoolSize = 50;
            ConnectionLifetimeMinutes = 30;
        }

        [ConfigurationProperty("Enabled")]
        public bool Enabled
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }

        [ConfigurationProperty("Host")]
        public string Host
        {
            get { return (string)this["Host"]; }
            set { this["Host"] = value; }
        }

        [ConfigurationProperty("Port")]
        public int Port
        {
            get { return (int)this["Port"]; }
            set { this["Port"] = value; }
        }

        [ConfigurationProperty("TimeOut")]
        public int TimeOut
        {
            get { return (int)this["TimeOut"]; }
            set { this["TimeOut"] = value; }
        }

        [ConfigurationProperty("DangerZoneThreshold")]
        public int DangerZoneThreshold
        {
            get { return (int)this["DangerZoneThreshold"]; }
            set { this["DangerZoneThreshold"] = value; }
        }

        [ConfigurationProperty("DangerZoneSeconds")]
        public int DangerZoneSeconds
        {
            get { return (int)this["DangerZoneSeconds"]; }
            set { this["DangerZoneSeconds"] = value; }
        }

        [ConfigurationProperty("EnablePool")]
        public bool EnablePool
        {
            get { return (bool)this["EnablePool"]; }
            set { this["EnablePool"] = value; }
        }

        [ConfigurationProperty("PoolSize")]
        public int PoolSize
        {
            get { return (int)this["PoolSize"]; }
            set { this["PoolSize"] = value; }
        }

        [ConfigurationProperty("ConnectionLifetimeMinutes")]
        public int ConnectionLifetimeMinutes
        {
            get { return (int)this["ConnectionLifetimeMinutes"]; }
            set { this["ConnectionLifetimeMinutes"] = value; }
        }
        
        public override string ToString()
        {
            return Host + ":" + Port;
        }

        public override int GetHashCode()
        {
            return Host.GetHashCode() + Port.GetHashCode();
        }
    }

}