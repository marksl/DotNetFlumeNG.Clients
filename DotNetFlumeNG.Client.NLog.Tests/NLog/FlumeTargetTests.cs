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

namespace DotNetFlumeNG.Client.NLog.Tests.NLog
{
    [TestFixture]
    public class FlumeTargetTests
    {
        [Test]
        public void ConstructorAndProperties_WorkProperly()
        {
            const string host = "host";
            const int port = 3;
            var f = new FlumeTarget
                        {
                            Host = host,
                            Port = port
                        };

            Assert.AreEqual(host, f.Host);
            Assert.AreEqual(port, f.Port);
        }
    }
}