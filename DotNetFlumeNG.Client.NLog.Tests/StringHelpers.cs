using System;

namespace DotNetFlumeNG.Client.NLog.Tests
{
    internal class StringHelpers
    {
        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length/sizeof (char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}