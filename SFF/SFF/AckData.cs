using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class AckData
    {
        private int _ack;

        public int Ack
        {
            get => _ack;
            set => _ack = value;
        }

        public AckData (int new_ack)
        {
            _ack = new_ack;
        }
    }
}
