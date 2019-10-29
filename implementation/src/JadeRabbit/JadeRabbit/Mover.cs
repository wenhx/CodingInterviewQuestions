using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    abstract class Mover
    {
        public abstract Point Move(Point origin);
    }
    class NorthMover : Mover
    {
        public override Point Move(Point origin)
        {
            return new Point { X = origin.X, Y = origin.Y + 1 };
        }
    }
    class EastMover : Mover
    {
        public override Point Move(Point origin)
        {
            return new Point { X = origin.X + 1, Y = origin.Y };
        }
    }
    class SouthMover : Mover
    {
        public override Point Move(Point origin)
        {
            return new Point { X = origin.X, Y = origin.Y - 1 };
        }
    }
    class WestMover : Mover
    {
        public override Point Move(Point origin)
        {
            return new Point { X = origin.X - 1, Y = origin.Y };
        }
    }

}
