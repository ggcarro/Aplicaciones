using System;
using System.IO;
using OpenCvSharp;

namespace iVocabulary
{
    public class Image
    {

        Mat _mat; 
        

        public Image(Mat mat)
        {
            _mat = mat;
        }

        public Image()
        {

        }

        public Image(string path) // FALTA IMPLEMENTAR COMPROBACIÓN DE QUE EXISTE
        {
            String[] pa = path.Split("/");
        }

        public Mat MatData
        {
            get{ return _mat; }
            set{ _mat = value; }
            
        }
    }
}
