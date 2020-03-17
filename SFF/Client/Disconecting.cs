using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Sender 
{
    class Disconnecting : SenderState
    {
        public Disconnecting(SFFSender context) : base(context) {
            RegisterHandler(PacketBodyType.AckDiscon, OnAckDiscon);
        }
         
        public void OnAckDiscon(Packet receivepacket) 
        {
            _context.Finish();
            _context.ChangeState(null);

        }

        protected override void OnTimeOut()
        {
            _context.AddFail();
            if (_context.CheckFails())
            {
                _context.Finish();
                _context.ChangeState(null);
            }
            else{
                Packet packet = _context.LastPacket();
                _context.Send(packet);
                _context.Timer();
                _context.ChangeState(this);
            }

        }
        protected override void OnUnknownPacket(Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
            _context.ChangeState(this);
        }

        protected override void OnSocketException(SocketException se)
        {
            Console.WriteLine("Exception: {0}", se);
            _context.ChangeState(this);
        }
        protected override void OnCorruptPacket(EndOfStreamException ese)
        {
            Console.WriteLine("Exception: {0}", ese);
            _context.ChangeState(this);
        }
        protected override void OnUnknownException(Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
            _context.ChangeState(this);
         }



    }
}
