using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Sender
{
    class Sending : SenderState
    {
        public Sending(SFFSender context) : base(context) {
            RegisterHandler(PacketBodyType.AckData, OnAckData);
        }
         
        public void OnAckData(Packet receivepacket)
        {
            if (_context.CheckSeq(receivepacket))
            {
                _context.IncreaseSeq();

                if (_context.ContinueTX())
                {
                    Packet sendPacket = _context.ReadData();
                    _context.Send(sendPacket);
                    _context.SetTimer();
                    _context.ResetFails();
                    _context.ChangeState(this);
                }
                else
                {
                    Packet sendPacket = _context.Discon();
                    _context.Send(sendPacket);
                    _context.SetTimer();
                    _context.ResetFails();
                    _context.ChangeState(new Disconecting(_context));
                }
                

            }
            else
            {
                Packet packet = _context.LastPacket();
                _context.Send(packet);
                _context.Timer();
                _context.ChangeState(this);
            }

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
        protected override void OnUnknownPacket(KeyNotFoundException e)
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
