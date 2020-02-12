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

    public interface IEchoMessageCodec
    {
        byte[] Encode(EchoMessage message);
        EchoMessage Decode(Stream source);
    }

    public class BinaryEchoMessageCodec : IEchoMessageCodec
    {

        public byte[] Encode(EchoMessage message)
        {
            byte[] byteBuffer;

            string _date = message.Date;
            string _message = message.Message;

            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(_date);
            writer.Write(_message);

            writer.Flush();
            byteBuffer = ms.ToArray();

            return byteBuffer;
        }

        public EchoMessage Decode(Stream source)   //¿Deberiamos usar try/catch para lanzar una excepcion por si no codifica bien?
        {
            BinaryReader reader = new BinaryReader(source);

            string read_date = reader.ReadString();
            string read_message = reader.ReadString();

            EchoMessage decoded_message = new EchoMessage(read_date, read_message);
            return decoded_message;
        }
    }
}
