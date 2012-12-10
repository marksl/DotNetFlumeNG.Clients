using System;
using System.Collections.Generic;
using System.ComponentModel;
using DotNetFlumeNG.Client.Core;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace DotNetFlumeNG.Client.NLog
{
    [Target("Flume")]
    public sealed class FlumeTarget : TargetWithLayout
    {
        public FlumeTarget()
        {
            FlumeSources = new List<FlumeSource>();
        }

        [DefaultValue(false)]
        public bool UsePool { get; set; }

        [DefaultValue(20)]
        public int PoolSize { get; set; }

        [DefaultValue(3)]
        public int Retries { get; set; }

        [DefaultValue(ClientType.Thrift)]
        public ClientType Client { get; set; }

        [ArrayParameter(typeof(FlumeSource), "source")]
        public IList<FlumeSource> FlumeSources { get; private set; }

        protected override void InitializeTarget()
        {
            InternalLogger.Info("Starting Flume Target.");

            FlumeClientFactory.Init(Client, UsePool, PoolSize, FlumeSources);

            base.InitializeTarget();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            InternalLogger.Info("Write.");

            int count = Retries;

            do
            {
                try
                {
                    using (var client = FlumeClientFactory.CreateClient())
                    {
                        client.Append(new NLogEventAdapter(logEvent));
                    }

                    return;
                }
                catch (Exception ex)
                {
                    InternalLogger.Error(ex.ToString());
                }
            } while (count-- > 0);

        }
    }
}