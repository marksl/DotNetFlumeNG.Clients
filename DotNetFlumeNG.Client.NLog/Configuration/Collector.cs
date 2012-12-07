using System.Configuration;

namespace DotNetFlumeNG.Client.NLog.Configuration
{
    public class Collector : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return (string) this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("Nodes", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof (FlumeNodeCollection),
            AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public FlumeNodeCollection Nodes
        {
            get { return (FlumeNodeCollection) base["Nodes"]; }
        }
    }
}