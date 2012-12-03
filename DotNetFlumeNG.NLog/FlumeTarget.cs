using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NFlumeNG.Sdk;
using NFlumeNG.Sdk.Api;
using NFlumeNG.Sdk.Events;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace NFlumeNG.NLog
{
    public class FlumeTarget : Target
    {
        private readonly string hostname;
        private readonly object lockObj = new object();
        private readonly int port;
        private IRpcClient rpcClient;

        public FlumeTarget()
        {
        }

        public FlumeTarget(string hostName, int port)
            : this()
        {
            hostname = hostName;
            this.port = port;
        }

        protected override void CloseTarget()
        {
            lock (lockObj)
            {
                if (rpcClient != null)
                {
                    rpcClient.Close();
                    rpcClient = null;
                }
            }
        }

        protected override void InitializeTarget()
        {
            try
            {
                rpcClient = RpcClientFactory.GetDefaultInstance(hostname, port);
            }
            catch (FlumeException e)
            {
                String errormsg = "RPC client creation failed! " +
                                  e.Message;

                InternalLogger.Error(errormsg);

                throw;
            }

            base.InitializeTarget();
        }

        private void reconnect()
        {
            CloseTarget();
            InitializeTarget();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (rpcClient == null)
                throw new FlumeException("Cannot Append to Appender! Appender either closed or not setup correctly!");

            if (!rpcClient.IsActive)
            {
                reconnect();
            }

            var hdrs = new Dictionary<string, string>();
            hdrs[AvroHeaders.LOGGER_NAME] = logEvent.LoggerName;
            hdrs[AvroHeaders.TIMESTAMP] = logEvent.TimeStamp.ToString(CultureInfo.InvariantCulture);
            hdrs[AvroHeaders.LOG_LEVEL] = logEvent.Level.Name;
            // hmmm... Encoding.UTF8
            hdrs[AvroHeaders.MESSAGE_ENCODING] = "UTF8";

            IEvent flumeEvent = EventBuilder.WithBody(logEvent.Message, Encoding.UTF8, hdrs);

            try
            {
                rpcClient.Append(flumeEvent);
            }
            catch (EventDeliveryException e)
            {
                const string msg = "Flume append() failed.";
                InternalLogger.Error(msg);

                throw new FlumeException(msg + " Exception follows.", e);
            }

            base.Write(logEvent);
        }
    }
}