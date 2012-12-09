using System;
using System.Collections.Generic;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client
{
    internal static partial class FlumeClientFactory
    {
        private static ClientType _clientType;
        private static Pool<IFlumeClient> _pool;
        private static IList<FlumeSource> _flumeSources;

        public static void Init(ClientType clientType, bool usePooling, int poolSize, IList<FlumeSource> flumeSources)
        {
            if (flumeSources == null) throw new ArgumentNullException("flumeSources");

            _clientType = clientType;
            _flumeSources = flumeSources;

            if (usePooling)
            {
                _pool = new Pool<IFlumeClient>(poolSize, CreateConnection, LoadingMode.Lazy, AccessMode.FIFO);
            }
        }

        public static IFlumeClient CreateClient()
        {
            return UsePooling ? _pool.Acquire() : CreateConnection();
        }

        private static bool UsePooling
        {
            get { return _pool != null; }
        }
    }
}