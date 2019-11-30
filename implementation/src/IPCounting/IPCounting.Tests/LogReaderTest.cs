using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IPCounting.Tests
{
    public class LogReaderTest
    {
        [Fact(DisplayName = "空文件")]
        public void ReadTest1()
        {
            var log = "";
            var reader = new LogReader(Helper.CreateStreamReader(log));
            var line = reader.Read();
            Assert.Null(line);
        }

        [Fact(DisplayName = "一行正确的格式")]
        public void ReadTest2()
        {
            var log = "2010-04-25 09:49:31 GET /index.asp 183.64.134.12 HTTP/1.1 200 220202";
            var reader = new LogReader(Helper.CreateStreamReader(log));
            var line = reader.Read();
            Assert.NotNull(line);
            Assert.Equal(DateTime.Parse("2010-04-25 09:49:31"), line.Created);
            Assert.Equal("183.64.134.12", line.IPAddress);
        }

        [Fact(DisplayName = "一行日期错误的格式")]
        public void ReadTest3()
        {
            var log = "2010-04-25 09049:31 GET /index.asp 183.64.134.12 HTTP/1.1 200 220202";
            var reader = new LogReader(Helper.CreateStreamReader(log));
            var line = reader.Read();
            Assert.Null(line);
        }

        [Fact(DisplayName = "一行日期错误的格式和一行正确的格式")]
        public void ReadTest4()
        {
            var log = "2010-04-25 09049:31 GET /index.asp 183.64.134.12 HTTP/1.1 200 220202" + Environment.NewLine
                + "2010-04-25 09:49:32 GET /index.asp 183.64.134.15 HTTP/1.1 200 220202";
            var reader = new LogReader(Helper.CreateStreamReader(log));
            var line = reader.Read();
            Assert.NotNull(line);
            Assert.Equal(DateTime.Parse("2010-04-25 09:49:32"), line.Created);
            Assert.Equal("183.64.134.15", line.IPAddress);
        }

        [Fact(DisplayName = "一行未正确结束的格式")]
        public void ReadTest5()
        {
            var log = "2010-04-25 09:49:31 GET /index.asp 183.64.134";
            var reader = new LogReader(Helper.CreateStreamReader(log));
            var line = reader.Read();
            Assert.Null(line);
        }

        [Fact(DisplayName = "多行正确的格式")]
        public void ReadTest8()
        {
            var log = "2010-04-25 09:49:31 GET /index.asp 183.64.134.12 HTTP/1.1 200 220202" + Environment.NewLine
                + "2010-04-25 09:49:32 GET /index.asp 183.64.134.15 HTTP/1.1 200 220202";
            var reader = new LogReader(Helper.CreateStreamReader(log));
            var line = reader.Read();
            Assert.NotNull(line);
            Assert.Equal(DateTime.Parse("2010-04-25 09:49:31"), line.Created);
            Assert.Equal("183.64.134.12", line.IPAddress);
            line = reader.Read();
            Assert.NotNull(line);
            Assert.Equal(DateTime.Parse("2010-04-25 09:49:32"), line.Created);
            Assert.Equal("183.64.134.15", line.IPAddress);
        }
    }
}
