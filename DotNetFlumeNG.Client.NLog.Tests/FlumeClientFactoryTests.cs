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

using System;
using System.Collections.Generic;
using DotNetFlumeNG.Client.Core;
using NUnit.Framework;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    [TestFixture]
    public class FlumeClientFactoryTests
    {
        [SetUp]
        public void InitMockServer()
        {
            _mockServer = new MockThriftServer();
        }

        [TearDown]
        public void DeleteServer()
        {
            _mockServer.Close();
        }

// ReSharper disable NotAccessedField.Local
        private MockThriftServer _mockServer;
// ReSharper restore NotAccessedField.Local

        private static void Init()
        {
            FlumeClientFactory.Init(ClientType.Thrift, "localhost", 9090);
        }

        [Test]
        public void CreateClient_PoolingDisabled_ClientCreated()
        {
            Init();

            using (var client = FlumeClientFactory.CreateClient())
            {
                Assert.IsNotNull(client);
            }
        }
        
        [Test]
        public void Init_HostNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => FlumeClientFactory.Init(ClientType.Thrift, null, 9090));
        }

        [Test]
        public void Init_ValidParameters_Succeeds()
        {
            FlumeClientFactory.Init(ClientType.Thrift, "localhost", 9090);
        }
    }
}