using System;

namespace SNFVocabulary
{
    public class SNFMessage
    {
        private int _seq;
        private int _ack;
        private byte[] _data; // Usar en caso de que el haya que mandar el fichero en sí.

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

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public SNFMessage(int new_seq, int new_ack)
        {
            _seq = new_seq;
            _ack = new_ack;
        }

        public SNFMessage(int new_seq, int new_ack, byte[] new_data)
        {
            _seq = new_seq;
            _ack = new_ack;
            _data = new_data;
        }
    }
}
