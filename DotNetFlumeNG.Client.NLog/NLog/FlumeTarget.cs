using System.Collections.Generic;
using System.ComponentModel;
using DotNetFlumeNG.Client.Core;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace DotNetFlumeNG.Client.NLog
{
    public class FlumeTarget : Target
    {
        public FlumeTarget()
        {
            FlumeSources = new List<FlumeSource>();
        }

        [DefaultValue(false)]
        public bool UsePool { get; set; }

        [DefaultValue(20)]
        public int PoolSize { get; set; }

        [DefaultValue(ClientType.Thrift)]
        public ClientType Client { get; set; }

        [ArrayParameter(typeof(FlumeSource), "flume-sources")]
        public IList<FlumeSource> FlumeSources { get; private set; }

        protected override void InitializeTarget()
        {
            FlumeClientFactory.Init(Client, UsePool, PoolSize, FlumeSources);

            base.InitializeTarget();
        }
        
        protected override void Write(LogEventInfo logEvent)
        {
            using (var client = FlumeClientFactory.CreateClient())
            {
                client.Append(new NLogEventAdapter(logEvent));
            }
        }
    }
}