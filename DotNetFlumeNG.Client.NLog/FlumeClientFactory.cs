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
using System.Collections.Generic;
using DotNetFlumeNG.Client.Core;

namespace DotNetFlumeNG.Client
{
    internal static partial class FlumeClientFactory
    {
        private static ClientType _clientType;
        private static Pool<IFlumeClient> _pool;
        private static IList<FlumeSource> _flumeSources;

        private static bool UsePooling
        {
            get { return _pool != null; }
        }

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
    }
}