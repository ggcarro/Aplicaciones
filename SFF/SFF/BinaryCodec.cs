using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SFFVocabulary
{
        public abstract class BinaryCodec<T> : ICodec<T>
        {
            public abstract void WriteBinaryData(BinaryWriter writer, T message);
            public abstract T ReadBinaryData(BinaryReader reader);

            public override byte[] Encode(T message)
            {
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    WriteBinaryData(writer, message);

                    writer.Flush();
                    return ms.ToArray();
                }
            }

            public override T Decode(Stream source)
            {
                using (BinaryReader reader = new BinaryReader(source, Encoding.UTF8, true))
                {
                    return ReadBinaryData(reader);
                }
            }
        }
}
