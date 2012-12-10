using System.Runtime.InteropServices;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern ulong GetTickCount64();
    }
}