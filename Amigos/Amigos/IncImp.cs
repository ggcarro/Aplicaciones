using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amigos
{
    public class IncImpl : IInc
    {
        int x = 0;

        public IncImpl()
        {
            x = 0;
        }
        public int Inc()
        {
            x++;
            return x;
        }
    }

}
