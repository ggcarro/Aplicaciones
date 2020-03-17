using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Sender
{
    abstract class SenderState:State
    {
        protected SFFSender _context;

        protected override Packet Receive()
        { 
            return _context.Receive();
        }

        protected SenderState(SFFSender context)
        {
            _context = context;
        }
        protected override void OnUnknownPacket(Exception e) { }
        protected override void OnTimeOut() { }
        protected override void OnSocketException(SocketException se) { }
        protected override void OnCorruptPacket(EndOfStreamException ese) { }
        protected override void OnUnknownException(Exception e) { }
    }
}
