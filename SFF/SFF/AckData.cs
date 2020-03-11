using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class AckData
    {
        private int _seq;
        private int _ack;

        public int Seq
        {
            get => _seq;
            set => _seq = value;
        }

        public int Ack
        {
            get => _ack;
            set => _ack = value;
        }

        public AckData (int new_seq, int new_ack)
        {
            _seq = new_seq;
            _ack = new_ack;
        }
    }
}
