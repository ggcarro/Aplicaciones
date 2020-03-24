using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Receiver
{
    class PreListening : ReceiverState
    {
        /*  En el estado pre-escucha  contemplamos la posibilidad de que llegue un paquete de inicio (se puede haber perdido el ACK)
         *  así como que llegue un paquete de datoso o uno de desconexión.
         */
         public PreListening (SFFReceiver context) : base (context)
        {
            //Console.WriteLine("PreListening State");
            RegisterHandler(PacketBodyType.NewFile, OnPacketNewFile);
            RegisterHandler(PacketBodyType.Data, OnPacketData);
            RegisterHandler(PacketBodyType.Discon, OnPacketDiscon);
        }

        protected void OnPacketNewFile(Packet receivePacket)
        {
            Console.WriteLine("New connection established");
            byte[] bytes = Encoding.ASCII.GetBytes("AckNewFile");
            Packet sendPacket = new Packet((int)PacketBodyType.AckNewFile, bytes.Length, bytes);
            _context.Send(sendPacket);
            Console.WriteLine("Ack NewFile Send");
            _context.ChangeState(this);
        }
        protected void OnPacketData(Packet receivePacket)
        {
            if (_context.CheckSeq(receivePacket))
            {
                _context.Write(receivePacket);
                Packet sendPacket = _context.Ack();
                _context.IncreaseSeq();
                _context.Send(sendPacket);
                _context.ChangeState(new Listening(_context));
            }
            else
            {
                //Packet sendPacket = new Packet((int)PacketBodyType.AckData, 0, null);
                //_context.Send(sendPacket);
                _context.ChangeState(new Listening(_context));
            }
            
        }
        protected void OnPacketDiscon(Packet receivePacket)
        {
            Packet sendPacket = new Packet((int)PacketBodyType.AckDiscon, 0, null);
            _context.Send(sendPacket);
            Console.WriteLine("Connection Finished");
            _context.Finish();
            _context.ChangeState(null);

        }

        protected override void OnUnknownPacket(Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
            _context.ChangeState(this); // ¿Esto es necesario? ¿Habría alguna forma de que saliera si no de este contexto?
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
