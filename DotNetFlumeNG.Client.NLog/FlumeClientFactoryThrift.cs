using System;
using System.Globalization;
using DotNetFlumeNG.Client.Core;
using DotNetFlumeNG.Client.Thrift;

namespace DotNetFlumeNG.Client
{
    internal static partial class FlumeClientFactory
    {
        private static readonly Random rand = new Random();

        private static IFlumeClient CreateConnection()
        {
            if (_clientType == ClientType.Thrift)
            {
                var source = _flumeSources[rand.Next(_flumeSources.Count)];

                return UsePooling
                           ? new ThriftClientPooled(_pool, source.Host, source.Port)
                           : new ThriftClient(source.Host, source.Port);
            }

            throw new NotSupportedException(
                string.Format(CultureInfo.InvariantCulture,
                              "The client type [{0}] is not supported. The only supported type is Thrift.",
                              _clientType));
        }
    }
}