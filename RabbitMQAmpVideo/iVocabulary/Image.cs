using System;
using System.IO;
using OpenCvSharp;

namespace iVocabulary
{
    public class Image
    {

        Mat _mat;
        int _seq;


        public Mat Mat
        {
            get { return _mat; }
            set { _mat = value; }

        }

        public int Seq
        {
            get { return _seq; }
            set { _seq = value; }

        }
        public Image(Mat mat, int seq)
        {
            _mat = mat;
            _seq = seq;
        }

        public Image()
        {
            _seq = -5;
        }

    }
}
