using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class Data : Packet
    {
        private int _seq;

        public int Seq
        {
            get => _seq;
            set => _seq = value;
        }

        public Data(int new_bodyLength, byte[] new_body, int new_seq, int new_type = 2) : base(new_bodyLength, new_body, new_type)
        
        {
            _seq = new_seq;
        }
    }
}
