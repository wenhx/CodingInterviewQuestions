using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    public class Status
    {
        public Point Point { get; set; }
        public Direction Direction { get; set; }
        public override string ToString()
        {
            return string.Format("方向: {0}， 坐标： X = {1}， Y = {2}。",
                Direction, Point.X, Point.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var right = obj as Status;
            if (right == null)
                return false;
            return Direction == right.Direction &&
                Point.X == right.Point.X &&
                Point.Y == right.Point.Y;
        }

        public override int GetHashCode()
        {
            return Point.X * 10000 + Point.Y * 100 + (int)Direction;
        }
    }
}
