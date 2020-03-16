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
            writer.Write(message.FileName);
        }

        public override NewFile ReadBinaryData(BinaryReader reader)
        {
            String new_fileName = reader.ReadString();
            return new NewFile(new_fileName);
        }
    }
}
