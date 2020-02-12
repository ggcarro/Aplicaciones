using System;
using System.Collections.Generic;
using System.Text;

namespace EchoVocabulary
{
    public interface IEchoMessageCodec
    {
        byte[] Encode(EchoMessage message);
        EchoMessage Decode(Stream source);
    }
}
