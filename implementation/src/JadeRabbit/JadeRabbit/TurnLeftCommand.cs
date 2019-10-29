using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    class TurnLeftCommand : TurnCommand
    {
        protected override Redirector Redirector
        {
            get { return new LeftRedirector(); }
        }
    }
}
