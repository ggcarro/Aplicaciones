using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class AckNewFileBinaryCodec : BinaryCodec<AckNewFile>
    {
        public override void WriteBinaryData(BinaryWriter writer, AckNewFile message)
        {
            writer.Write((int)message.StartTransmission);
        }

        public override AckNewFile ReadBinaryData(BinaryReader reader)
        {
            int endTransmission = reader.ReadInt32();

            return new AckNewFile(endTransmission);
        }
    }
}
