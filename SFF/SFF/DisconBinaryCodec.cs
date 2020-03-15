using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class DisconBinaryCodec : BinaryCodec<Discon>
    {
        PacketBinaryCodec codec;
        public override void WriteBinaryData(BinaryWriter writer, Discon message)
        {
            codec.WriteBinaryData(writer, message);
        }

        public override Discon ReadBinaryData(BinaryReader reader)
        {
            Packet packet = codec.ReadBinaryData(reader);

            return new Discon(packet.BodyLength, packet.Body);
        }
    }
}
