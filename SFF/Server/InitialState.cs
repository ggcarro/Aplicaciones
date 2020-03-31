using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Receiver
{
    class InitialState : ReceiverState
    {
         public InitialState(SFFReceiver context) : base (context)
        {
            RegisterHandler(PacketBodyType.NewFile, OnPacketNewFile); 
        }

        protected void OnPacketNewFile(Packet receivePacket)
        {
            _context.CreateFile(receivePacket);
            Console.WriteLine("New connection established");
            byte[] bytes = Encoding.ASCII.GetBytes("AckNewFile");
            Packet sendPacket = new Packet((int)PacketBodyType.AckNewFile, bytes.Length, bytes);
            _context.Send(sendPacket);
            Console.WriteLine("Ack Send");
            _context.ChangeState(new PreListening(_context));
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
