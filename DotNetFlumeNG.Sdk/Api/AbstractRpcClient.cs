using System.Collections.Generic;

namespace NFlumeNG.Sdk.Api
{
    public abstract class AbstractRpcClient : IRpcClient
    {
        public abstract void Append(IEvent evt);
        public abstract void AppendBatch(List<IEvent> events);
        public abstract bool IsActive { get; }
        public abstract void Close();

        protected long connectTimeout = RpcClientConfigurationConstants.DEFAULT_CONNECT_TIMEOUT_MILLIS;
        protected long requestTimeout = RpcClientConfigurationConstants.DEFAULT_REQUEST_TIMEOUT_MILLIS;

        public int BatchSize { get { return RpcClientConfigurationConstants.DEFAULT_BATCH_SIZE; } }
        
        
    }
}