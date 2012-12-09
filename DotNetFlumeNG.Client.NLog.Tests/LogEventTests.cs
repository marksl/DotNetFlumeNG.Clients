using DotNetFlumeNG.Client.Core;
using Moq;
using NUnit.Framework;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    [TestFixture]
    public class LogEventTests
    {
        private LogEvent logEvent;

        [SetUp]
        public void Init()
        {
            logEvent = new Mock<LogEvent>().Object;
        }
         
        [Test]
        public void HostTest()
        {
            Assert.IsNotNull(logEvent.Host, "The Host should not be null.");
        }
    }
}