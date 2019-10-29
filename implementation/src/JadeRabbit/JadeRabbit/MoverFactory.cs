using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    class MoverFactory
    {
        static readonly Dictionary<Direction, Mover> _MoverPool = new Dictionary<Direction, Mover>();
        static MoverFactory()
        {
            _MoverPool.Add(Direction.North, new NorthMover());
            _MoverPool.Add(Direction.East, new EastMover());
            _MoverPool.Add(Direction.South, new SouthMover());
            _MoverPool.Add(Direction.West, new WestMover());
        }
        public static Mover Create(Direction direction)
        {
            try
            {
                return _MoverPool[direction];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
