using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class AckNewFileBinaryCodec : BinaryCodec<AckNewFile>
    {
        PacketBinaryCodec codec;
        public override void WriteBinaryData(BinaryWriter writer, AckNewFile message)
        {
            codec.WriteBinaryData(writer, message);
        }

        public override AckNewFile ReadBinaryData(BinaryReader reader)
        {
            Packet packet = codec.ReadBinaryData(reader);

            return new AckNewFile(packet.BodyLength, packet.Body);
        }
    }
}
