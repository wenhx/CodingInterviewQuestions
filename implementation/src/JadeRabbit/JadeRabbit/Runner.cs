using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    public class Runner
    {
        public static Status Run(string commandSequence)
        {
            Status status = new Status { Point = new Point { Y = 0, X = 0 }, Direction = Direction.East  };
            return Run(status, commandSequence);
        }
        public static Status Run(Status init, string commandSequence)
        {
            //if (init == null)
            //    throw new ArgumentNullException("init");
            //if (string.IsNullOrEmpty(commandSequence) || commandSequence.Length == 0)
            //    throw new ArgumentException("commandSequence");

            var parser = new CommandParser();
            var commands = parser.Parse(commandSequence);
            var status = init;
            foreach (var command in commands)
            {
                status = command.DoAction(status);
            }
            return status;
        }
    }
}
