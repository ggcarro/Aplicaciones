using System;

namespace SNFVocabulary
{
    public class SNFMessage
    {
        private int _seq;
        private int _ack;
        private int _num;


        public int Seq
        {
            get { return _seq; }
            set { _seq = value; }
        }

        public int Ack
        {
            get { return _ack; }
            set { _ack = value; }
        }
        public int Num
        {
            get { return _num; }
            set { _num = value; }
        }

        public SNFMessage(int new_seq, int new_ack)
        {
            _seq = new_seq;
            _ack = new_ack;
        }

        public SNFMessage(int new_seq, int new_ack, int new_num)
        {
            _seq = new_seq;
            _ack = new_ack;
            _num = new_num;
        }
        
    }
}
