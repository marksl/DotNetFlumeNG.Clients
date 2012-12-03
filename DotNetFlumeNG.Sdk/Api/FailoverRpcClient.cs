using System.Collections.Generic;

namespace NFlumeNG.Sdk.Api
{
    // TODO: Once the NettyAvroClient is built.
    public class FailoverRpcClient : AbstractRpcClient
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
    }
}