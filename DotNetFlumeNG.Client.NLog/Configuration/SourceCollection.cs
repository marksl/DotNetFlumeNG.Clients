using System.Configuration;

namespace DotNetFlumeNG.Client.NLog.Configuration
{
    public class SourceCollection : ConfigurationElementCollection
    {
        public Source this[int index]
        {
            get { return (Source)BaseGet(index); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Source();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Source) element).Name;
        }
    }
}