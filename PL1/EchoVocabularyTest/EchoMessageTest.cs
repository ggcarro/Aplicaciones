using System;
using System.IO;

namespace EchoVocabulary
{
    public class EchoMessage
    {
        private string _date;

        public string Message { get; set; }

        //get { return _message; }
        //set { _message = value;

        public string Date
        {
            get { return _date; }
            set { _date = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss.fff"); }
        }

        public EchoMessage(string new_date, string new_message)
        {
            Date = new_date;
            Message = new_message;
        }

    }
}
