using System;
using System.Net;
using System.Net.Sockets;
using SNFVocabulary;

namespace PL2
{
    class Client
    {
        private string ipServer = "127.0.0.1";
        private int portServer = 23456;
        private byte[] sendPacket;
        private BinarySNFMessageCodec codec = new BinarySNFMessageCodec();

        public static void Main(string[] args)
        {
            

            UdpClient udpClient = new UdpClient();
            Client client = new Client();
            try
            {
                Console.WriteLine("Client Started");
                const int N = 100; // Suponemos que se quiere enviar este cantidad de números

                
                SNFMessage sendMessage = null;



                for (int i = 1; i < N; i++)
                {
                    // Envio información
                    sendMessage = new SNFMessage(i, i - 1);
                    client.send(sendMessage,udpClient);




                    // Recibo información
                    client.receive(udpClient);

                    // Compruebo ACK
                    
                }
            }
            catch(SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
            }
            udpClient.Close();
        
        }

        public void send(SNFMessage message, UdpClient client)
        {
            sendPacket = codec.Encode(message);
            client.Send(sendPacket, sendPacket.Length, ipServer, portServer);
            Console.WriteLine("Seq sent {0}", message.Seq);
        }

        public SNFMessage receive(UdpClient client)
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivePacket = client.Receive(ref remoteIPEndPoint);
            return codec.Decode(receivePacket);
        }
    }
}
