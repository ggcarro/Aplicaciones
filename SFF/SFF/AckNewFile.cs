using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class AckNewFile
    {
        private int _startTransmission = -10;

        public int StartTransmission
        {
            get => _startTransmission;
            set => _startTransmission = value;
        }

        public AckNewFile (int new_startTransmission)
        {
            _startTransmission = new_startTransmission;
        }
    }
}
