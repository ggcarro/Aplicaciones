using System;
namespace iVocabulary
{
    public class Image
    {
        
        string _text;
        int _num;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public int Num
        {
            get { return _num; }
            set { _num = value; }
        }

        public Image(string text, int num)
        {
            _text = text;
            _num = num;
        }
        
    }
}
