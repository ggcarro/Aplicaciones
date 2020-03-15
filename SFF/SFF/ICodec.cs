using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SFFVocabulary {
    public abstract class ICodec<T>
    {
        public abstract byte[] Encode(T message);
        public abstract T Decode(Stream source);

        public T Decode(byte[] packet)
        {
            using (Stream payload = new MemoryStream(packet, 0, packet.Length, false))
                return Decode(payload);
        }
    }
}
