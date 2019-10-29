using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    class ForwardCommand : Command
    {
        public override Status DoAction(Status origin)
        {
            var mover = MoverFactory.Create(origin.Direction);
            return new Status 
            { 
                Direction = origin.Direction,
                Point = mover.Move(origin.Point)
            };
        }
    }
}
