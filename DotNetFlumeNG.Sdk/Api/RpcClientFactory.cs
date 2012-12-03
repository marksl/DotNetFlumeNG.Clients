using System;
using System.Collections.Generic;
using System.Globalization;

namespace NFlumeNG.Sdk.Api
{
    public class RpcClientFactory
    {
        public static IRpcClient GetDefaultInstance(string hostName, int port, int batchSize = 0)
        {
            if (hostName == null) throw new ArgumentNullException("hostName");

            var props = new Dictionary<string, string>();
            props[RpcClientConfigurationConstants.CONFIG_HOSTS] = "h1";
            props[RpcClientConfigurationConstants.CONFIG_HOSTS_PREFIX + "h1"] = string.Format("{0}:{1}", hostName, port);
            props[RpcClientConfigurationConstants.CONFIG_BATCH_SIZE] = batchSize.ToString(CultureInfo.InvariantCulture);

            var client = new NettyAvroRpcClient();
            client.Configure(props);
            return client;
        }
    }
}