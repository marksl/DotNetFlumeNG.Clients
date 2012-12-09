using System;
using System.Diagnostics;
using DotNetFlumeNG.Client.Core;
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