using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EchoVocabulary
{
    public class ASCIIEchoMessageCodec
    {
        public byte[] Encode(EchoMessage message)
        {
            byte[] byteBuffer;

            string _date = message.Date;
            string _message = message.Message;

            MemoryStream ms = new MemoryStream();

            byteBuffer = System.Text.Encoding.ASCII.GetBytes(_date + "/a/b/a" + _message);

            return byteBuffer;
        }

        public EchoMessage Decode(Stream source)   //¿Deberiamos usar try/catch para lanzar una excepcion por si no codifica bien?
        {
            byte[] recvData = new byte[256];
            int bytes = source.Read(recvData, 0, recvData.Length);
            string msg = System.Text.Encoding.ASCII.GetString(recvData, 0, bytes);

            string[] msgfields = new string[] { "/a/b/a" };

            string read_date = msgfields[0];
            string read_message = msgfields[1];

            EchoMessage decoded_message = new EchoMessage(read_date, read_message);
            return decoded_message;
        }
    }
}
