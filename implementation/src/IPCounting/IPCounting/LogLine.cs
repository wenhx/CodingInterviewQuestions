using System;

namespace IPCounting
{
    public class LogLine
    {
        public LogLine(DateTime created, string ipAddress)
        {
            Created = created;
            IPAddress = ipAddress;
        }

        public DateTime Created { get; private set; }

        public string IPAddress { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Created, IPAddress);
        }
    }
}
