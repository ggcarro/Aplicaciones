using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SFFVocabulary;
using System.IO;

namespace Server
{  
    abstract class ServerState : State
    {
        protected SFFServer _context;
        protected ServerState(SFFServer context)
        {
            _context = context;
        }

        protected override Packet Receive()
        { 
            return _context.receive();
        }
        protected override void OnUnknownPacket(KeyNotFoundException e) { }
        protected override void OnTimeOut() { }
        protected override void OnSocketClosed() { }
        protected override void OnSocketException(SocketException se) { }
        protected override void OnCorruptPacket(EndOfStreamException ese) { }
        protected override void OnUnknownException(Exception e) { }

    }
}
