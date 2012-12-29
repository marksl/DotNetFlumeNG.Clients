// 
//    Copyright 2013 Mark Lamley
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

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

        [ArrayParameter(typeof (FlumeSource), "source")]
        public IList<FlumeSource>FlumeSources { get; private set; }

        protected override void InitializeTarget()
        {
            InternalLogger.Info("Starting Flume Target.");

            FlumeClientFactory.Init(Client, UsePool, PoolSize, FlumeSources);

            base.InitializeTarget();
        }

        protected override void CloseTarget()
        {
            FlumeClientFactory.Close();

            base.CloseTarget();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            InternalLogger.Debug("Write.");

            if (logEvent.Level == LogLevel.Off)
            {
                InternalLogger.Debug("Off.");
                return;
            }

            int count = Retries;

            do
            {
                try
                {
                    using (var client = FlumeClientFactory.CreateClient())
                    {
                        client.Append(new NLogEventAdapter(Layout.Render(logEvent), logEvent));
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