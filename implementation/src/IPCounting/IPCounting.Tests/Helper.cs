using System;
using System.IO;
using System.Text;

namespace IPCounting.Tests
{
    internal class Helper
    {
        internal static StreamReader CreateStreamReader(string content)
        {
            return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(content)));
        }
    }
}