using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class AckDiscon
    {
        private int _ackEndTransmission = -20;

        public int AckEndTransmission
        {
            get => _ackEndTransmission;
            set => _ackEndTransmission = value;
        }

        public AckDiscon(int new_ackEndTransmission)
        {
            _ackEndTransmission = new_ackEndTransmission;
        }
    }
}
