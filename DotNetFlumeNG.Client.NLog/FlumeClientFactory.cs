// 
//    Copyright 2013 Mark Lamley
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client
{
    internal static partial class FlumeClientFactory
    {
        private static ClientType _clientType;
        private static string _host;
        private static int _port;

        public static void Init(ClientType clientType, string host, int port)
        {
            if (host == null) throw new ArgumentNullException("host");

            _clientType = clientType;
            _host = host;
            _port = port;
        }
    }
}