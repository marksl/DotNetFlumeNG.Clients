using System.Configuration;

namespace DotNetFlumeNG.Client.NLog.Configuration
{
    public class CollectorCollection : ConfigurationElementCollection
    {
        public Collector this[int index]
        {
            get { return (Collector)BaseGet(index); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Collector();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Collector) element).Name;
        }
    }
}