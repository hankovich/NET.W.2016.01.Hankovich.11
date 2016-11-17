using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1.Logic
{
    public class Fibonacci
    {
        /// <summary>
        /// Get Fibonacci sequance
        /// </summary>
        /// <returns>Fibonacci sequance</returns>
        public static IEnumerable<long> GetSequence()
        {
            long prelast = 1;
            long last = 0;
            long current;
            while (true)
            {
                yield return last;
                try
                {
                    checked
                    {
                        current = last + prelast;
                    }
                }
                catch (OverflowException)
                {
                    yield break;
                }
                prelast = last;
                last = current;
            }
        }
    }
}
