using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit
{
    abstract class Redirector
    {
        protected static readonly int DirectionCount = 4;
        public abstract Direction Redirect(Direction origin);
    }
    class LeftRedirector : Redirector
    {
        public override Direction Redirect(Direction origin)
        {
            return (Direction)((((int)origin - 1) + DirectionCount) % DirectionCount);
        }
    }
    class RightRedirector : Redirector
    {
        public override Direction Redirect(Direction origin)
        {
            return (Direction)(((int)origin + 1) % DirectionCount);
        }
    }

}
