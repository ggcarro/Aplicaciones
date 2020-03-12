using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class NewFile : Packet
    {
        private string __fileName;

        public string FileName
        {
            get => __fileName;
            set => __fileName = value;
        }

        public NewFile(int new_bodyLength, byte[] new_body, string new_fileName, int new_type = 1) : base(new_bodyLength, new_body, new_type)
        {
            __fileName = new_fileName;
        }
    }
}
