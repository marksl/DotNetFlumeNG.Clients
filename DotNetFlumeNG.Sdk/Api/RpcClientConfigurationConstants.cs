using System;

namespace NFlumeNG.Sdk.Api
{
    public class RpcClientConfigurationConstants
    {
        /**
  * Hosts configuration key to specify a space delimited list of named
  * hosts. For example:
  * <pre>
  * hosts = h1 h2
  * </pre>
  */
        public static String CONFIG_HOSTS = "hosts";

        /**
         * Hosts prefix to specify address of a particular named host. For example
         * <pre>
         * hosts.h1 = server1.example.com:12121
         * hosts.h2 = server2.example.com:12121
         * </pre>
         */
        public static String CONFIG_HOSTS_PREFIX = "hosts.";

        /**
         * Configuration key used to specify the batch size. Default batch size is
         * {@value DEFAULT_BATCH_SIZE}.
         */
        public static String CONFIG_BATCH_SIZE = "batch-size";

        /**
         * Configuration key to specify connection timeout in milliseconds. The
         * default connection timeout is {@value DEFAULT_CONNECT_TIMEOUT_MILLIS}.
         */
        public static String CONFIG_CONNECT_TIMEOUT = "connect-timeout";

        /**
         * Configuration key to specify request timeout in milliseconds. The
         * default request timeout is {@value DEFAULT_REQUEST_TIMEOUT_MILLIS}.
         */
        public static String CONFIG_REQUEST_TIMEOUT = "request-timeout";

        /**
         * Default batch size.
         */
        public static int DEFAULT_BATCH_SIZE = 100;

        /**
         * Default connection, handshake, and initial request timeout in milliseconds.
         */

        public static long DEFAULT_CONNECT_TIMEOUT_MILLIS = new TimeSpan(0, 0, 20).Milliseconds;
            //TimeUnit.MILLISECONDS.convert(20, TimeUnit.SECONDS);

        /**
         * Default request timeout in milliseconds.
         */

        public static long DEFAULT_REQUEST_TIMEOUT_MILLIS = new TimeSpan(0, 0, 20).Milliseconds;
            //TimeUnit.MILLISECONDS.convert(20, TimeUnit.SECONDS);

        /**
         * Maximum attempts to be made by the FailoverRpcClient in case of
         * failures.
         */
        public static String CONFIG_MAX_ATTEMPTS = "max-attempts";

        /**
         * Configuration key to specify the RpcClient type to be used. The available
         * values are <tt>DEFAULT</tt> which results in the creation of a regular
         * <tt>NettyAvroRpcClient</tt> and <tt>DEFAULT_FAILOVER</tt> which results
         * in the creation of a failover client implementation on top of multiple
         * <tt>NettyAvroRpcClient</tt>s. The default value of this configuration
         * is {@value #DEFAULT_CLIENT_TYPE}.
         *
         */
        public static String CONFIG_CLIENT_TYPE = "client.type";

        /**
         * The default client type to be created if no explicit type is specified.
         */
        public static String DEFAULT_CLIENT_TYPE = "DEFAULT";
        //RpcClientFactory.ClientType.DEFAULT.name();

        /**
         * The selector type used by the <tt>LoadBalancingRpcClient</tt>. This
         * value of this setting could be either <tt>round_robin</tt>,
         * <tt>random</tt>, or the fully qualified name class that implements the
         * <tt>LoadBalancingRpcClient.HostSelector</tt> interface.
         */
        public static String CONFIG_HOST_SELECTOR = "host-selector";

        public static String HOST_SELECTOR_ROUND_ROBIN = "ROUND_ROBIN";
        public static String HOST_SELECTOR_RANDOM = "RANDOM";

        public static String CONFIG_MAX_BACKOFF = "maxBackoff";
        public static String CONFIG_BACKOFF = "backoff";
        public static String DEFAULT_BACKOFF = "false";

    }
}