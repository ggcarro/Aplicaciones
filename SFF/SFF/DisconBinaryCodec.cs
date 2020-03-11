using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class DisconBinaryCodec : BinaryCodec<Discon>
    {
        public override void WriteBinaryData(BinaryWriter writer, Discon message)
        {
            writer.Write((int)message.EndTransmission);
        }

        public override Discon ReadBinaryData(BinaryReader reader)
        {
            int endTransmission = reader.ReadInt32();

            return new Discon(endTransmission);
        }
    }
}
