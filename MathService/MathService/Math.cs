using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathService
{
    class Math : IMath
    {
        public bool Prime(int value)
        {
            int divider = 2;
            int remainder = 0;
            while (divider < value)
            {
                remainder = value % divider;
                if (remainder == 0)
                {
                    return false;
                }
                divider++;
            }
            return true;
        }
    }
}
