using System;

namespace IPCounting
{
    class Tracer
    {
        public static void WriteLine(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
