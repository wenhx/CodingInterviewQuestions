using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace IPCounting
{
    public class LogLineCounter
    {
        int m_LimitationToCount;
        TimeSpan m_LimitativeTimeSpan;
        Dictionary<string, LogLinePeriod> m_Dict;
        DateTime m_LatestCleanUp = DateTime.Now;
        LogLineCounterSettings m_Settings;

        public LogLineCounter(int limitationToCount) : this(limitationToCount, TimeSpan.FromMinutes(1))
        {

        }

        public LogLineCounter(int limitationToCount, TimeSpan limitativeTimeSpan, LogLineCounterSettings? settings = null)
        {
            if (limitationToCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(limitationToCount));
            if (limitativeTimeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(limitativeTimeSpan));

            m_LimitationToCount = limitationToCount;
            m_LimitativeTimeSpan = limitativeTimeSpan;

            m_Settings = settings ?? new LogLineCounterSettings();
            m_Dict = new Dictionary<string, LogLinePeriod>(m_Settings.DefaultDictionaryCapacity);
        }

        public void Count(LogLine line)
        {
            if (!m_Dict.ContainsKey(line.IPAddress) || m_Dict[line.IPAddress] == null)
            {
                m_Dict[line.IPAddress] = new LogLinePeriod(line.IPAddress, m_LimitationToCount, m_LimitativeTimeSpan);
            }

            m_Dict[line.IPAddress].Record(line);
            //如果有非常多的IP地址，那么实际上很多IP地址是没有超限制的，也会一直记录在内存中。
            //所以，这里应该定期清理字典中无用的IP地址。
            //因为日志是按时间顺序写入的，比如2分钟以前还没有超出限制的IP地址的记录就没用了，可以清除掉。
            //LogMaker生成1000万行日志可以直观的看到这种情况。
            CleanUp(line.Created);
        }

        public IEnumerable<string> GetOverLimitativeIPAddresses()
        {
            foreach (var item in m_Dict)
            {
                if (item.Value.IsOverLimitation)
                    yield return item.Key;
            }
        }

        public IEnumerable<LogLine> GetOverLimitativeLines(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress) || !m_Dict.ContainsKey(ipAddress))
                yield break;

            foreach (var item in m_Dict[ipAddress].OverLimitativeLines)
            {
                yield return item;
            }
        }

        /// <summary>
        /// 这是一个简易的算法，实际算法要根据实际的日志文件来优化。
        /// </summary>
        /// <param name="currentLineTime"></param>
        void CleanUp(DateTime currentLineTime)
        {
            if ((DateTime.Now - m_LatestCleanUp) < m_Settings.CleanUpInterval)
                return;
            var usage = GC.GetTotalMemory(false) / 1024 / 1024;
            if (usage < m_Settings.MemoryUsageLimitation)
                return;

            var dict2 = new Dictionary<string, LogLinePeriod>(m_Dict.Count);
            foreach (var item in m_Dict)
            {
                if ((currentLineTime - item.Value.LatestRequestTime) < m_Settings.CleanUpBefore ||
                    item.Value.IsOverLimitation)
                {
                    dict2[item.Key] = item.Value;
                }
                if (item.Value.IsOverLimitation)
                {
                    Console.WriteLine(item.Key);
                }
            }
            Tracer.WriteLine("dict1: {0}, dict2: {1}.", m_Dict.Count, dict2.Count);
            m_Dict = dict2;
            m_LatestCleanUp = DateTime.Now;
            GC.Collect();
        }

        class LogLinePeriod
        {
            bool m_IsOverLimitation = false;
            int m_LimitationToCount;
            TimeSpan m_LimitativeTimeSpan;
            string m_IPAddress;
            LinkedList<LogLine> m_InnerList = new LinkedList<LogLine>();

            public LogLinePeriod(string ipAddress, int limitationToCount, TimeSpan limitativeTimeSpan)
            {
                m_IPAddress = ipAddress;
                m_LimitationToCount = limitationToCount;
                m_LimitativeTimeSpan = limitativeTimeSpan;
            }

            public bool IsOverLimitation
            {
                get
                {
                    return m_IsOverLimitation;
                }
            }

            public DateTime LatestRequestTime => m_InnerList.Last!.Value.Created;

            public LinkedList<LogLine> OverLimitativeLines => m_InnerList;

            internal void Record(LogLine line)
            {
                Debug.Assert(line.IPAddress == m_IPAddress);
                if (m_IsOverLimitation)
                    return;

                m_InnerList!.AddLast(line);
                if (m_InnerList.Count > m_LimitationToCount)
                {
                    m_InnerList.RemoveFirst();
                    if ((line.Created - m_InnerList.First!.Value.Created) <= m_LimitativeTimeSpan)
                    {
                        m_IsOverLimitation = true;
                        //m_InnerList = null; //free memory.
                    }
                }
            }
        }
    }
}
