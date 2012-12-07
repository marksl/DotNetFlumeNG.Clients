using DotNetFlumeNG.Client.NLog.Pooling;
using NLog;
using NLog.Targets;

namespace DotNetFlumeNG.Client.NLog
{
    public class FlumeTarget : Target
    {
        private Pool<ExpireableThriftClient> pool;

        protected override void InitializeTarget()
        {
            pool = new Pool<ExpireableThriftClient>(50, ExpireableThriftClient.CreateConnection, LoadingMode.Lazy, AccessMode.LIFO);

            base.InitializeTarget();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            using (ExpireableThriftClient client = pool.Acquire())
            {
                client.Append(new ThriftFlumeEvent());
            }
        }
    }
}