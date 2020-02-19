using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SNFVocabulary
{
    interface ISNFMessage
    {
        byte[] Encode(SNFMessage message);
        SNFMessage Decode(byte[] buffer);
    }
}
