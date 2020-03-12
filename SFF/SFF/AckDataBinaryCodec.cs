using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    class AckDataBinaryCodec : BinaryCodec<AckData>
    {
        //PacketBinaryCodec packet;
        public override void WriteBinaryData(BinaryWriter writer, AckData message)
        {
            //packet.WriteBinaryData(writer, message);
            writer.Write((int)message.Ack);
        }

        public override AckData ReadBinaryData(BinaryReader reader)
        {
            //Packet packet = PacketBinaryCodec.ReadBinaryData(packet);

            return new AckData(packet.BodyLength, new_body);
        }
    }
}
