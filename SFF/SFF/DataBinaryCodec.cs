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
            writer.Write(message.Information.Length);
            writer.Write(message.Information);
            writer.Write(message.Seq);
        }

        public override Data ReadBinaryData(BinaryReader reader)
        {
            int new_length = reader.ReadInt32();
            byte[] new_data = reader.ReadBytes(new_length);
            int new_seq = reader.ReadInt32();

            return new Data(new_data, new_seq);
        }
    }
}
