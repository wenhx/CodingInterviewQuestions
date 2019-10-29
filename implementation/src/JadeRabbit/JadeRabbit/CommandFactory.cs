using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    class CommandFactory
    {
        static readonly Dictionary<char, Command> _SupportedCommand = new Dictionary<char, Command>();
        static CommandFactory()
        {
            _SupportedCommand.Add('F', new ForwardCommand());
            _SupportedCommand.Add('L', new TurnLeftCommand());
            _SupportedCommand.Add('R', new TurnRightCommand());
        }
        //public static bool IsCommandSupported(char command)
        //{
        //    try
        //    {
        //        return _SupportedCommand.ContainsKey(command);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public static Command Create(char command)
        {
            //if (string.IsNullOrEmpty(command))
            //    throw new ArgumentException("command");

            try
            {
                return _SupportedCommand[command];
            }
            catch (KeyNotFoundException)
            {
                throw new NotSupportedException();
            }
        }
    }
}
