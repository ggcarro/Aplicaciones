using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Sender
{
    class InitialState : SenderState
    {
        public InitialState(SFFSender context) : base(context) { }
        public override void HandleEvents()
        {
            Console.WriteLine("Initial State");
            _context.CreateFile();
            Packet packet = _context.FileNamePacket();
            _context.Send(packet); 
            _context.SetTimer();
            _context.ChangeState(new Waiting(_context));
        }



    }
}
