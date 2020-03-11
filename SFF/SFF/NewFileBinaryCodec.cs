using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SFFVocabulary
{
    public class NewFileBinaryCodec : BinaryCodec<NewFile>
    {
        public override void WriteBinaryData(BinaryWriter writer, NewFile message)
        {
            writer.Write((string)message.FileName);
        }

        public override NewFile ReadBinaryData(BinaryReader reader)
        {
            string fileName = reader.ReadString();

            return new NewFile(fileName);
        }
    }
}
