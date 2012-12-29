// 
//     Copyright 2013 Mark Lamley
//  
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//  
//         http://www.apache.org/licenses/LICENSE-2.0
//  
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.

using DotNetFlumeNG.Client.Core;
using NUnit.Framework;

namespace DotNetFlumeNG.Client.NLog.Tests.Core
{
    [TestFixture]
    public class PoolTests
    {
        private class Data
        {
        }

        private static Data Create()
        {
            return new Data();
        }

        [Test]
        public void AcquireReturnToPool_HappyPath_Succeeds()
        {
            var data = new Pool<Data>(50, Create);
            var t = data.Acquire();

            data.ReturnToPool(t);
        }
    }
}