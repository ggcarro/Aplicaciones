using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class AckNewFile : Packet
    {

        public AckNewFile(int new_bodyLength, byte[] new_body, int new_type = 10) : base(new_bodyLength, new_body, new_type)
        {
   
        }

    }
}
