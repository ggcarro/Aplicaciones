using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class Discon
    {
        private int _endTransmission = -2;

        public int EndTransmission
        {
            get => _endTransmission;
            set => _endTransmission = value;
        }

        public Discon (int new_endTransmission)
        {
            _endTransmission = new_endTransmission;
        }
    }
}
