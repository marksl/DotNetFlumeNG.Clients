using System;

namespace DotNetFlumeNG.Client.NLog.Pooling
{
    public abstract class ExpireableItem
    {
        private readonly DateTime start;

        protected ExpireableItem()
        {
            start = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow.Subtract(start).TotalSeconds > 60;
        }
    }
}