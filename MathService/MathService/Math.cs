using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathService
{
    class Math : IMath
    {
        public bool Prime(int num)
        {
            int div = 2;
            int rest = 0;
            while(div < num)
            {
                rest = num % div;
                if(rest == 0){
                    return false;
                }
                div++;
            }
            return true;
        }
    }
}

