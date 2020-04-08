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

        public Tuple AddTuple(Tuple tuple)
        {
            double sum = 0;
            for(int i = 0; i < tuple.Data.Length; i++)
            {
                sum += tuple.Data[i];
            }

            Tuple result = new Tuple();
            result.Data = new double[1];
            result.Data[0] = sum;
            result.Name = "Suma  " + tuple.Name;
            return result;
            
        }
    }
}
