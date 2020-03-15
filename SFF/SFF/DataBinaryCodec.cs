using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class DataBinaryCodec : BinaryCodec<Data>
    {
        PacketBinaryCodec codec;
        public override void WriteBinaryData(BinaryWriter writer, Data message)
        {
            codec.WriteBinaryData(writer, message);
            writer.Write(message.Seq);
        }

        public override Data ReadBinaryData(BinaryReader reader)
        {
            Packet packet = codec.ReadBinaryData(reader);
            int new_seq = reader.ReadInt32();

            return new Data(packet.BodyLength, packet.Body, new_seq);
        }
    }
}
