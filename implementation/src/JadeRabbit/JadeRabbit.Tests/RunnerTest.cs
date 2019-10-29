using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JadeRabbit.Tests
{
    [TestFixture]
    public class RunnerTest
    {
        [Test]
        public void Runner_Should_Get_Correct_Status_When_Input_Correct_CommandSequence()
        {
            var commandSequence = "FFLFRFLL";
            var actual = Runner.Run(commandSequence);
            var expected = new Status { Direction = Direction.West, Point = new Point { X = 3, Y = 1 } };

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void Runner_Should_Get_Correct_Status_When_Input_Correct_CommandSequence_And_InitStatus()
        {
            var commandSequence = "FFLFRFLL";
            var init = new Status { Point = new Point { Y = 0, X = 0 }, Direction = Direction.East };
            var actual = Runner.Run(init, commandSequence);
            var expected = new Status { Direction = Direction.West, Point = new Point { X = 3, Y = 1 } };

            Assert.AreEqual(expected, actual);
        }   
    }
}
