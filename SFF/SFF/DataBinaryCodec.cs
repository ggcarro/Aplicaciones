using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class DataBinaryCodec : BinaryCodec<Data>
    {
        public override void WriteBinaryData(BinaryWriter writer, Data message)
        {
            writer.Write((int)message.Seq);
            writer.Write((int)message.Ack);
            writer.Write((byte[])message.Info);
        }

        public override Data ReadBinaryData(BinaryReader reader)
        {
            int seq = reader.ReadInt32();
            int ack = reader.ReadInt32();
            byte[] info = reader.ReadBytes(8);

            return new Data(seq, ack, info);
        }
    }
}
