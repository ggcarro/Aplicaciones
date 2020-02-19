using System;
using System.Net;
using System.Net.Sockets;
using EchoVocabulary;

namespace ServerUDP
{
    class Server
    {
        static void Main(string[] args)
        {
            UdpClient client = null;
            EchoMessage receiveMsg;
        

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
                    BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                    //ASCIIEchoMessageCodec codec = new ASCIIEchoMessageCodec();
                    receiveMsg = codec.UPDdecode(rcvBuffer); //

                    EchoMessage responseMsg = new EchoMessage(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"), receiveMsg.Message);
                    byte[] sendBuffer = codec.Encode(responseMsg);

                    // Enviar
                    client.Send(sendBuffer, sendBuffer.Length, remoteIPEndPoint);
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
