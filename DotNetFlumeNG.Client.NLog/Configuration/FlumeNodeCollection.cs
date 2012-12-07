using System.Configuration;

namespace DotNetFlumeNG.Client.NLog.Configuration
{
    public class FlumeNodeCollection : ConfigurationElementCollection
    {
        public FlumeNode this[int index]
        {
            get { return (FlumeNode)BaseGet(index); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FlumeNode();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element.ToString();
        }
    }
}