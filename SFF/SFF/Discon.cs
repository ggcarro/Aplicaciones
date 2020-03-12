using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class Discon : Packet
    {

        public Discon(int new_bodyLength, byte[] new_body, int new_type = 3) : base(new_bodyLength, new_body, new_type)
        {
        }
    }
}
