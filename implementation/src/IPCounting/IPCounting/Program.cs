using System;
using System.Threading.Tasks;

namespace IPCounting
{
    class Program
    {
        static readonly int _DefaultLimitRequestCountPerMiniuteBySameIPAddress = 70;

        static void Main(string[] args)
        {
            int limit = _DefaultLimitRequestCountPerMiniuteBySameIPAddress;
            var logFilePath = @"..\..\..\..\LogMaker\bin\Debug\netcoreapp3.0\log.txt"; //run project LogMaker to make this log file.
            if (args.Length == 1)
            {
                logFilePath = args[0];
            }
            else if (args.Length > 1)
            {
                limit = int.Parse(args[0]);
                logFilePath = args[1];
            }

            var reader = new LogReader(logFilePath);
            var counter = new LogLineCounter(limit);
            while (true)
            {
                var line = reader.Read();
                if (line == null)
                    break;

                counter.Count(line);
            }

            var total = 0;
            foreach (var ip in counter.GetOverLimitativeIPAddresses())
            {
                total++;
                Console.WriteLine(ip);
            }
            Console.WriteLine("Total: {0}", total);

            Console.WriteLine("Input the ip address if you want to print the request time, if not press enter key.");
            var ipAddress = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                foreach (var line in counter.GetOverLimitativeLines(ipAddress))
                {
                    Console.WriteLine(line.Created);
                }
            }

            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        }
    }
}
