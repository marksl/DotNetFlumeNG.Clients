using System.Runtime.InteropServices;

namespace DotNetFlumeNG.Client.Core
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern ulong GetTickCount64();
    }
}