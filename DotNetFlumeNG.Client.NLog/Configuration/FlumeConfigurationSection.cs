using System.Configuration;

namespace DotNetFlumeNG.Client.NLog.Configuration
{
    public class FlumeConfigurationSection : ConfigurationSection
    {
        private static readonly FlumeConfigurationSection settings =
            ConfigurationManager.GetSection("FlumeConfigurationSection") as FlumeConfigurationSection;

        public static FlumeConfigurationSection Settings
        {
            get { return settings; }
        }

        [ConfigurationProperty("Collectors", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof (CollectorCollection),
            AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public CollectorCollection Collectors
        {
            get { return (CollectorCollection) base["Collectors"]; }
        }

        [ConfigurationProperty("Sources", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof (SourceCollection),
            AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public SourceCollection Sources
        {
            get { return (SourceCollection) base["Sources"]; }
        }

        [ConfigurationProperty("connectTimeout", DefaultValue = "3000", IsRequired = false)]
        public int ConnectTimeout
        {
            get { return (int) this["ConnectTimeout"]; }
            set { this["ConnectTimeout"] = value; }
        }
    }
}