using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MathService
{
    public class Math : IMath
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

        public double[] EqSys(double[] A1, double[] A2, int N, double[] B)
        {
            double[,] A = new double[2, 2];
            A[0, 0] = A1[0];
            A[0, 1] = A1[1];
            A[1, 0] = A2[0];
            A[1, 1] = A2[1];
 
            int i;
            alglib.rmatrixsolvefast(A, N, ref B, out i);
            if (i > 0)
            {
                return B;
            }

            else return new double[1] { 54.321 };
        }
    } 
}
