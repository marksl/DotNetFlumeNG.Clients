using System.Collections.Generic;

namespace NFlumeNG.Sdk.Api
{
    public class NettyAvroRpcClient : AbstractRpcClient
    {
        public override void Append(IEvent evt)
        {
            throw new System.NotImplementedException();
        }

        public override void AppendBatch(List<IEvent> events)
        {
            throw new System.NotImplementedException();
        }

        public override void Close()
        {
            throw new System.NotImplementedException();
        }

        public override bool IsActive
        {
            get { throw new System.NotImplementedException(); }
        }

        public IRpcClient Configure(Dictionary<string, string> props)
        {
            throw new System.NotImplementedException();
        }
    }
}