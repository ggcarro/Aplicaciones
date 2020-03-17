using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SFFVocabulary;
using System.IO;

namespace Receiver
{  
    abstract class ReceiverState : State
    {
        protected SFFReceiver _context;
        protected ReceiverState(SFFReceiver context)
        {
            _context = context;
        }

        protected override Packet Receive()
        {
            return _context.Receive();
        }
        protected override void OnUnknownPacket(Exception e) { }
        protected override void OnSocketClosed() { }
        protected override void OnSocketException(SocketException se) { }
        protected override void OnCorruptPacket(EndOfStreamException ese) { }
        protected override void OnUnknownException(Exception e) { }

    }
}
