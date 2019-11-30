using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace LogMaker
{
    class Program
    {
        static readonly Random _DefaultRandom = new Random();
        static readonly string _LogLineTemplate = "{0} GET /index.asp {1} HTTP/1.1 200 220202";
        static readonly string _IPAddressPrefix = "183.65.134.";
        static DateTime _StartingDateTime = DateTime.Now;

        static void Main(string[] args)
        {
            Console.WriteLine("input the lines number you want to make (more than 10000):");
            int lineNumber = int.Parse(Console.ReadLine());

            using var w = new StreamWriter("log.txt", false);
            var now = DateTime.Now;
            var counts = new Dictionary<string, int>();
            WriteLog(lineNumber * 0.5, w, counts, 2);
            WriteLog(lineNumber * 0.5, w, counts, 1);

            //var total = 0;
            //foreach (var item in counts.OrderByDescending(p => p.Value).Where(p => p.Value > 100))
            //{
            //    Console.WriteLine("IP Address: {0}, Count: {1}.", item.Key, item.Value);
            //    total++;
            //}
            //Console.WriteLine("Total over limitation: {0}", total);
            Console.WriteLine("Total : {0}", counts.Count);

            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        }

        static void WriteLog(double number, StreamWriter w, Dictionary<string, int> counts, int rndType)
        {
            for (int i = 0; i < number; i++)
            {
                string ipAddress = rndType == 1 ? GetRandomIPAddress1() : GetRandomIPAddress2();
                if (!counts.ContainsKey(ipAddress))
                {
                    counts[ipAddress] = 1;
                }
                else
                {
                    counts[ipAddress]++;
                }

                var log = string.Format(_LogLineTemplate, _StartingDateTime, ipAddress);
                w.WriteLine(log);
                _StartingDateTime = _StartingDateTime.AddMilliseconds(_DefaultRandom.Next(100));

                if (i % 1000 == 0)
                {
                    w.Flush();
                }
            }
        }

        static string GetRandomIPAddress1()
        {
            int lastByte = GetRandomNumber();
            return _IPAddressPrefix + lastByte;
        }

        static string GetRandomIPAddress2()
        {
            var arr = new int[4];
            for (int i = 0; i < 4; i++)
            {
                arr[i] = GetRandomNumber();
            }
            return string.Join('.', arr);
        }

        static int GetRandomNumber()
        {
            double u1 = 1.0 - _DefaultRandom.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - _DefaultRandom.NextDouble();
            double rand = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            var num = (int)(125 + 12 * rand);
            if (num > 254 || num < 1)
            {
                num = _DefaultRandom.Next(1, 255);
            }
            return num;
        }
    }
}
