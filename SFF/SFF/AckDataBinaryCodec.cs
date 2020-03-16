using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class AckDataBinaryCodec : BinaryCodec<AckData>
    {
        public override void WriteBinaryData(BinaryWriter writer, AckData message)
        {
            writer.Write(message.Ack);
        }

        public override AckData ReadBinaryData(BinaryReader reader)
        {
            int new_ack = reader.ReadInt32();

            return new AckData(new_ack);
        }
    }
}
