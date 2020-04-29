using System;
using System.Collections.Generic;
using System.Text;

namespace Progeto.Lua
{
    public class Answer
    {
       
        public string output { get; set; }
        public string svg { get; set; }
        public string time { get; set; }

        public Answer(string o, string s, string t)
        {
            output = o;
            svg = s;
            time = t;
        }
    }
}
