using System;

namespace EchoVocabulary
{
    public class EchoMessage
    {
        private string _date;
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Date
        {
            get { return _date; }
            set { _date = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss.fff"); }
        }

        public EchoMessage(string new_date, string new_message)
        {
            _date = new_date;
            _message = new_message;
        }


    }
}
