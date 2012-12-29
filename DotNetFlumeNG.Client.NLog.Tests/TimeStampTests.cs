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
using System.Diagnostics;
using NUnit.Framework;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    [TestFixture]
    public class TimeStampTests
    {
        [Test]
        public void ViewTimeStamps()
        {
            // System.nanoTime() - Returns the current value of the most precise available system timer, in nanoseconds.
            // StopWatch.GetTimestamp() - Returns the tick counter of the timer. A single tick is 100 nanoseconds.

            // These both seem to give very different numbers from the following Java code:

            // class hello {
            //   public static void main(String[] args) {
            //     System.out.println(System.nanoTime()); // Display the string 5881507088960
            //   }
            // }
            // 5881507088960 - System.nanoTime()
            // 14887657041 - StopWatch.GetTimestamp()

            ulong time = NativeMethods.GetTickCount64();
            long time2 = Stopwatch.GetTimestamp();
            long time3 = DateTime.UtcNow.ToFileTime();
            Assert.AreNotEqual(0, time);
            Assert.AreNotEqual(0, time2);
            Assert.AreNotEqual(0, time3);
        }
    }
}