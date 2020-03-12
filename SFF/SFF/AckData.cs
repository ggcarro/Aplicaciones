using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class AckData : Packet
    {
        private int _ack;

        public int Ack
        {
            get => _ack;
            set => _ack = value;
        }

        public AckData (int new_bodyLength, byte[] new_body, int new_ack, int new_type = 20) : base(new_bodyLength, new_body, new_type)

        {
            _ack = new_ack;
        }
    }
}
