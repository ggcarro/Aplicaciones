using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class Data
    {
        private int _seq;
        private int _ack;
        private byte[] _info;

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

        public byte[] Info
        {
            get => _info;
            set => _info = value;
        }

        public Data(int new_seq, int new_ack, byte[] new_info)
        {
            _seq = new_seq;
            _ack = new_ack;
            _info = new_info;
        }
    }
}
