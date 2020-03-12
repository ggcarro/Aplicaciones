using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class Data : Packet
    {
        private int _seq;
        private byte[] _info;

        public int Seq
        {
            get => _seq;
            set => _seq = value;
        }

        public byte[] Info
        {
            get => _info;
            set => _info = value;
        }

        public Data(int new_bodyLength, byte[] new_body, int new_seq, byte[] new_info, int new_type = 2) : base(new_bodyLength, new_body, new_type)
        
        {
            _seq = new_seq;
            _info = new_info;
        }
    }
}
