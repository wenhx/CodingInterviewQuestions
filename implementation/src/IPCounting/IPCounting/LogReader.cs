using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IPCounting
{
    public class LogReader
    {
        static readonly int _SkipSpacesCount = 2; // skip these spaces to the ip address, it depends on the log file format.
        StreamReader m_InnerReader;

        public LogReader(string path)
        {
            m_InnerReader = new StreamReader(path);
        }

        public LogReader(StreamReader reader)
        {
            m_InnerReader = reader;
        }

        public LogLine? Read()
        {
            DateTime? created = null;
            string? ipAddress = null;

            while (!m_InnerReader.EndOfStream)
            {
                var dateTimePart = ReadNextPart(spacesCount: 2);
                if (dateTimePart.Result == ReadPartResult.EndOfLine)
                    continue;
                else if (dateTimePart.Result == ReadPartResult.EndOfStream)
                    break;

                var datetimeString = new string(dateTimePart.Value.ToArray());
                try
                {
                    created = DateTime.Parse(datetimeString);
                }
                catch
                {
                    Tracer.WriteLine("datetime string format error: " + datetimeString);
                    MoveCursorToNextLine();
                    continue;
                }

                if (!MoveCursorToIPAddress())
                    continue;

                var ipAddressPart = ReadNextPart();
                if (ipAddressPart.Result == ReadPartResult.EndOfLine)
                    continue;
                else if (ipAddressPart.Result == ReadPartResult.EndOfStream)
                    break;

                ipAddress = new string(ipAddressPart.Value.ToArray());
                MoveCursorToNextLine();

                if (created.HasValue && ipAddress != null)
                    return new LogLine(created.Value, ipAddress);
            }

            return null; ;
        }

        void MoveCursorToNextLine()
        {
            while (!m_InnerReader.EndOfStream)
            {
                var ch = (char)m_InnerReader.Read();
                // Note the following common line feed chars:
                // \n - UNIX   \r\n - DOS   \r - Mac
                if (ch == '\r' || ch == '\n')
                {
                    ch = (char)m_InnerReader.Peek();
                    if (ch == '\n')
                    {
                        m_InnerReader.Read();
                    }
                    break;
                }
            }
        }

        bool MoveCursorToIPAddress()
        {
            var spacesCount = 0;
            while (!m_InnerReader.EndOfStream && spacesCount < _SkipSpacesCount)
            {
                var ch = (char)m_InnerReader.Read();
                if (ch == ' ')
                {
                    spacesCount++;
                }
                if (ch == '\r' || ch == '\n')
                {
                    ch = (char)m_InnerReader.Peek();
                    if (ch == '\n')
                    {
                        m_InnerReader.Read();
                    }
                    return false;
                }
            }
            return true;
        }

        (ReadPartResult Result, List<char> Value) ReadNextPart(int spacesCount = 1)
        {
            var chars = new List<char>();
            var readSpaces = 0;
            while (true)
            {
                var ch = (char)m_InnerReader.Read();
                if (ch == ' ' && ++readSpaces == spacesCount)
                    break;
                else if (ch == '\r' || ch == '\n')
                {
                    ch = (char)m_InnerReader.Peek();
                    if (ch == '\n')
                    {
                        m_InnerReader.Read();
                    }
                    return (ReadPartResult.EndOfLine, chars);
                }
                else if (m_InnerReader.EndOfStream)
                    return (ReadPartResult.EndOfStream, chars);
                else
                {
                    chars.Add(ch);
                }
            }
            return (ReadPartResult.Success, chars);
        }

        public enum ReadPartResult
        {
            Success = 0,
            EndOfLine = 1,
            EndOfStream = 2
        }
    }
}
