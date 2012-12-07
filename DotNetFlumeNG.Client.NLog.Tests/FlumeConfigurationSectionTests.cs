using DotNetFlumeNG.Client.NLog.Configuration;
using NUnit.Framework;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    [TestFixture]
    public class FlumeConfigurationSectionTests
    {
        [Test]
        public void FlumeConfigurationSection_Instance_HasDataFromAppConfig()
        {
            FlumeConfigurationSection settings = FlumeConfigurationSection.Settings;
            Assert.IsNotNull(settings, "FlumeConfigurationSection should have been loaded from App.config.");
            Assert.AreEqual(2, settings.Collectors.Count, "There should be 2 collectors.");
            Assert.AreEqual(3, settings.Sources.Count, "There should be 3 sources.");

            Source firstSource = settings.Sources[0];
            Assert.AreEqual("source-a", firstSource.Name);
            Assert.AreEqual("first-collector", firstSource.Collector);

            Collector firstCollector = settings.Collectors[0];
            Assert.AreEqual(2, firstCollector.Nodes.Count, "The first collector should have 2 nodes.");

            FlumeNode firstNode = firstCollector.Nodes[0];
            Assert.AreEqual("127.0.0.1", firstNode.Host);
            Assert.AreEqual(2015, firstNode.Port);
            Assert.AreEqual(true, firstNode.EnablePool);
            Assert.AreEqual(50, firstNode.PoolSize);
        }
    }
}