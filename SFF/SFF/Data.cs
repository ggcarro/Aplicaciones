using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class Data
    {
        private int _seq;
        private byte[] _information;

        public int Seq
        {
            get => _seq;
            set => _seq = value;
        }
        public byte[] Information
        {
            get => _information;
            set => _information = value;
        }

        public Data(byte[] new_data, int new_seq)
        {
            _information = new_data;
            _seq = new_seq;
        }
    }
}
