using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class Dummy
    {
        public int i { get; set; }
        public string s { get; set; }
        [Range(0, 1.0)]
        public double d { get; set; }
    }
}
