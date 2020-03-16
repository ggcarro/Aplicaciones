using System;
using System.Collections.Generic;
using System.Text;

namespace SFFVocabulary
{
    public class NewFile
    {
        private string __fileName;

        public string FileName
        {
            get => __fileName;
            set => __fileName = value;
        }

        public NewFile(string new_fileName)
        {
            __fileName = new_fileName;
        }
    }
}
