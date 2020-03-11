using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    class AckDataBinaryCodec : BinaryCodec<AckData>
    {
        public override void WriteBinaryData(BinaryWriter writer, AckData message)
        {
            writer.Write((int)message.Seq);
            writer.Write((int)message.Ack);
        }

        public override AckData ReadBinaryData(BinaryReader reader)
        {
            int seq = reader.ReadInt32();
            int ack = reader.ReadInt32();

            return new AckData(seq, ack);
        }
    }
}
