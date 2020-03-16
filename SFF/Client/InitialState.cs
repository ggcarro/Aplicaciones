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
        /*  En el estado inicial solo contemplamos la posibilidad de que llegue un paquete de inicio 
         *  de transferencia con el nombre del fichero y se aceptará la petición. Todo paquete distinto será ignorado
         */
        public InitialState(SFFSender context) : base(context) { }
        public override void HandleEvents()
        {
            _context.CreateFile();
            Packet packet = _context.FileNamePacket();
            _context.Send(packet); 
            _context.SetTimer();
            _context.ChangeState(new Waiting(_context));
        }



    }
}
