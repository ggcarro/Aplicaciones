using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    class AckDiscontBinaryCodec : BinaryCodec<AckDiscon>
    {
        PacketBinaryCodec codec;
        public override void WriteBinaryData(BinaryWriter writer, AckDiscon message)
        {
            codec.WriteBinaryData(writer, message);
        }

        public override AckDiscon ReadBinaryData(BinaryReader reader)
        {
            Packet packet = codec.ReadBinaryData(reader);

            return new AckDiscon(packet.BodyLength, packet.Body);
        }
    }
}
