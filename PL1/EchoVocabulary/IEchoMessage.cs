using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace EchoVocabulary
{
    interface IEchoMessage
    {

        byte[] Encode(EchoMessage message);
        EchoMessage Decode(Stream source);

    }
}
