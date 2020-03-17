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
        /*  En el estado inicial solo contemplamos la posibilidad de que llegue un paquete de inicio 
         *  de transferencia con el nombre del fichero y se aceptará la petición. Todo paquete distinto será ignorado
         */
         public InitialState(SFFReceiver context) : base (context)
        {
            RegisterHandler(PacketBodyType.NewFile, OnPacketNewFile); 
        }

        protected void OnPacketNewFile(Packet receivePacket)
        {
            Console.WriteLine("Initial State");
            _context.CreateFile(receivePacket);
            Console.WriteLine("New connection established");
            byte[] n = new byte[0];
            Packet sendPacket = new Packet((int)PacketBodyType.AckNewFile, n.Length, n);
            _context.Send(sendPacket);
            Console.WriteLine("Ack Send");
            _context.ChangeState(new PreListening(_context));
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
