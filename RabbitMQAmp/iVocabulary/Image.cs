using System;
using System.IO;

namespace iVocabulary
{
    public class Image
    {
        
        string _filename;
        byte[] _data;

        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }


        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public Image(string name, byte[] data)
        {
            _filename = name;
            _data = data;
        }

        public Image()
        {

        }

        public Image(string path) // FALTA IMPLEMENTAR COMPROBACIÓN DE QUE EXISTE
        {
            String[] pa = path.Split("/");
            _filename = pa[pa.Length-1];
            _data = File.ReadAllBytes(path);
        }
    }
}
