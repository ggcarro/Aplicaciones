using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    class AckDiscontBinaryCodec : BinaryCodec<AckDiscon>
    {
        public override void WriteBinaryData(BinaryWriter writer, AckDiscon message)
        {
            writer.Write((int)message.AckEndTransmission);
        }

        public override AckDiscon ReadBinaryData(BinaryReader reader)
        {
            int ack = reader.ReadInt32();

            return new AckDiscon(ack);
        }
    }
}
