using System.Collections.Generic;

namespace NFlumeNG.Sdk.Api
{
    public interface IRpcClient
    {
        int BatchSize { get; }
        bool IsActive { get; }

        void Append(IEvent evt);
        void AppendBatch(List<IEvent> events);
        void Close();
    }
}