using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    abstract class TurnCommand : Command
    {
        protected abstract Redirector Redirector { get; }
        public override Status DoAction(Status origin)
        {
            return new Status
            {
                Direction = Redirector.Redirect(origin.Direction),
                Point = origin.Point
            };
        }
    }
}
