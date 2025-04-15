using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heyhey
{
    internal class MathUtils
    {
        public static int Lerp(int a, int b, float t)
        {
            return (int)(a + (b - a) * t);
        }
    }
}
