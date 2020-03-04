using System;
using System.Net;
using System.Net.Sockets;
using SNFVocabulary;

namespace ServerUDP
{
    class Server
    {
        static void Main(string[] args)
        {
            UdpClient client = null;
            SNFMessage receiveMsg;
            int ack=0;

            try
            {
                // Enlazar el socket en un puerto
                client = new UdpClient(23456);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                return;
            }

            // Dirección desde donde recibir (cualquier direccion)
            // Se modificará tras la recepción (info Paquete)
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Server Started");
            // El servidor se ejecuta infinitamente

            for (; ; )
            {
                try
                {
   
                   // Recibir
                    byte[] rcvBuffer = client.Receive(ref remoteIPEndPoint);
                    BinarySNFMessageCodec codec = new BinarySNFMessageCodec();
                    receiveMsg = codec.Decode(rcvBuffer);

                    //Comprobacion de que es el mensaje esperado
                    if (ack == receiveMsg.Ack)
                    {
                        ack++;
                        SNFMessage confirmationMsg = new SNFMessage(receiveMsg.Seq, receiveMsg.Seq);
                        byte[] sendBuffer = codec.Encode(confirmationMsg);
                        client.Send(sendBuffer, sendBuffer.Length, remoteIPEndPoint);
                    }
                    else
                        Console.WriteLine("Paquete descartado {0} {1} {2}", receiveMsg.Seq, receiveMsg.Ack, ack);
                    
             }
             catch (SocketException se)
             {
                 Console.WriteLine(se.ErrorCode + ": " + se.Message);
                 return;
             }
            }
        }
    }
}