using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    abstract class Command
    {
        public abstract Status DoAction(Status origin);
    }
}
