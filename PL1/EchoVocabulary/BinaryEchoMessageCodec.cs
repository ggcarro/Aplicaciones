using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EchoVocabulary
{
    public class BinaryEchoMessageCodec
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

        public EchoMessage Decode(Stream source)   
        {
            BinaryReader reader = new BinaryReader(source);

            string read_date = reader.ReadString();
            string read_message = reader.ReadString();

            EchoMessage decoded_message = new EchoMessage(read_date, read_message);
            return decoded_message;
        }

        public EchoMessage UPDdecode (byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            return Decode(ms);

        }
    }
}
