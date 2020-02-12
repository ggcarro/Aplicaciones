using System;
using System.IO;

namespace EchoVocabulary
{
    
   
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
