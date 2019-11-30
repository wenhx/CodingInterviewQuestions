using System;
using System.Collections.Generic;
using System.Text;

namespace IPCounting
{
    public class LogLineCounterSettings
    {
        public TimeSpan CleanUpInterval { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan CleanUpBefore { get; set; } = TimeSpan.FromMinutes(2);

        public int MemoryUsageLimitation { get; set; } = 200; //M

        public int DefaultDictionaryCapacity { get; set; } = 10000;
    }
}
