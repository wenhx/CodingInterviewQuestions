using IPCounting;
using System;
using System.Linq;
using Xunit;

namespace IPCounting.Tests
{
    public class LogLineCounterTest
    {
        static readonly string _IPAddress1 = "192.168.1.11";
        static readonly string _IPAddress2 = "192.168.1.12";
        static readonly string _IPAddress3 = "192.168.1.13";
        static readonly string _IPAddress4 = "192.168.1.14";
        static readonly string _IPAddress5 = "192.168.1.15";

        [Fact(DisplayName = "有1个超过限制IP数量的日志")]
        public void CountTest1()
        {
            var counter = new LogLineCounter(3);
            var beginning = DateTime.Now;
            counter.Count(CreateLogLine(beginning, _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(10), _IPAddress2));
            counter.Count(CreateLogLine(beginning.AddSeconds(20), _IPAddress3));
            counter.Count(CreateLogLine(beginning.AddSeconds(30), _IPAddress4));
            counter.Count(CreateLogLine(beginning.AddSeconds(40), _IPAddress5));
            counter.Count(CreateLogLine(beginning.AddSeconds(50), _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(59), _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(60), _IPAddress1));
            var addresses = counter.GetOverLimitativeIPAddresses().ToList();
            Assert.Single(addresses);
            Assert.Equal(_IPAddress1, addresses[0]);
        }

        [Fact(DisplayName = "没有超过限制IP数量的日志")]
        public void CountTest2()
        {
            var counter = new LogLineCounter(3);
            var beginning = DateTime.Now;
            counter.Count(CreateLogLine(beginning, _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(10), _IPAddress2));
            counter.Count(CreateLogLine(beginning.AddSeconds(20), _IPAddress3));
            counter.Count(CreateLogLine(beginning.AddSeconds(30), _IPAddress4));
            counter.Count(CreateLogLine(beginning.AddSeconds(40), _IPAddress5));
            counter.Count(CreateLogLine(beginning.AddSeconds(50), _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(60), _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(120), _IPAddress1));
            var addresses = counter.GetOverLimitativeIPAddresses().ToList();
            Assert.Empty(addresses);
        }

        [Fact(DisplayName = "有2个超过限制IP数量的日志")]
        public void CountTest3()
        {
            var counter = new LogLineCounter(3);
            var beginning = DateTime.Now;
            counter.Count(CreateLogLine(beginning, _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(10), _IPAddress2));
            counter.Count(CreateLogLine(beginning.AddSeconds(20), _IPAddress3));
            counter.Count(CreateLogLine(beginning.AddSeconds(30), _IPAddress4));
            counter.Count(CreateLogLine(beginning.AddSeconds(40), _IPAddress5));
            counter.Count(CreateLogLine(beginning.AddSeconds(50), _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(59), _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(60), _IPAddress1));
            counter.Count(CreateLogLine(beginning.AddSeconds(70), _IPAddress5));
            counter.Count(CreateLogLine(beginning.AddSeconds(71), _IPAddress5));
            counter.Count(CreateLogLine(beginning.AddSeconds(71), _IPAddress4));
            counter.Count(CreateLogLine(beginning.AddSeconds(72), _IPAddress5));
            counter.Count(CreateLogLine(beginning.AddSeconds(80), _IPAddress3));
            counter.Count(CreateLogLine(beginning.AddSeconds(90), _IPAddress5));
            var addresses = counter.GetOverLimitativeIPAddresses().ToList();
            Assert.Equal(2, addresses.Count);
            Assert.Single(addresses, _IPAddress1);
            Assert.Single(addresses, _IPAddress5);
        }

        LogLine CreateLogLine(DateTime created, string ipAddress)
        {
            return new LogLine(created, ipAddress);
        }
    }
}
