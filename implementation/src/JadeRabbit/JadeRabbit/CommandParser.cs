using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    class CommandParser
    {
        public IEnumerable<Command> Parse(string commandSequence)
        { 
            return commandSequence.Select(c => CommandFactory.Create(c));
        }
    }
}
