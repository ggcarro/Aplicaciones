using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class PacketBinaryCodec : BinaryCodec<Packet>
    {
        public override void WriteBinaryData(BinaryWriter writer, Packet message)
        {
            writer.Write((int)message.Type);
            writer.Write(message.BodyLength);
            writer.Write(message.Body);
        }

        public override Packet ReadBinaryData(BinaryReader reader)
        {
            int typeRaw = reader.ReadInt32();
            int bodyLength = reader.ReadInt32();

            byte[] body = new byte[bodyLength];
            reader.Read(body, 0, bodyLength);

            return new Packet(bodyLength, body, typeRaw);
        }
    }
}
