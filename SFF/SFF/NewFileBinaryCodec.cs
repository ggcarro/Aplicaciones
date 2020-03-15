using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class NewFileBinaryCodec : BinaryCodec<NewFile>
    {
        PacketBinaryCodec codec;
        public override void WriteBinaryData(BinaryWriter writer, NewFile message)
        {
            codec.WriteBinaryData(writer, message);
            writer.Write((string)message.FileName);
        }

        public override NewFile ReadBinaryData(BinaryReader reader)
        {
            Packet packet = codec.ReadBinaryData(reader);
            String new_fileName = reader.ReadString();
            return new NewFile(packet.BodyLength, packet.Body, new_fileName);
        }
    }
}
