using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    class AckDataBinaryCodec : BinaryCodec<AckData>
    {
        PacketBinaryCodec codec;
        public override void WriteBinaryData(BinaryWriter writer, AckData message)
        {
            codec.WriteBinaryData(writer, message);
            writer.Write(message.Ack);
        }

        public override AckData ReadBinaryData(BinaryReader reader)
        {

            return new AckData(packet.BodyLength, new_body);
        }
    }
}
