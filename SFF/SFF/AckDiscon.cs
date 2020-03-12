using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class AckDiscon : Packet
    {
        public AckDiscon(int new_bodyLength, byte[] new_body, int new_type = 30) : base(new_bodyLength, new_body, new_type)
        {

        }
    }
}
