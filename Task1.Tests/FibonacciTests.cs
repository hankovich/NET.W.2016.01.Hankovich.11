using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Task1.Logic;

namespace Task1.Tests
{
    [TestFixture]
    public class FibonacciTests
    {
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(3, 2)]
        [TestCase(4, 3)]
        [TestCase(50, 12586269025)]
        [Test]
        public void GetSequence(int number, long expected)
        {
            IEnumerator<long> sequence = Fibonacci.GetSequence().GetEnumerator();
            bool b = false;
            for (int i = 0; i <= number; i++)
                b = sequence.MoveNext();
            if (!b)
                Assert.Fail($"Smth gonna wrong at number {number}");
            Assert.AreEqual(expected, sequence.Current);
        }
    }
}
