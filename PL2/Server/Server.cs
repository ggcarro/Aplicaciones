using System;
using System.Net;
using System.Net.Sockets;
using SNFVocabulary;
using System.IO;

namespace ServerUDP
{
    class Server
    {
        private const string Path = "C:\\Users\\maria\\Desktop\\Test.txt"; //Depende de dónde se ejecute el servidor
        private SNFMessage receiveMessage;
        private SNFMessage sendMessage;
        BinarySNFMessageCodec codec = new BinarySNFMessageCodec();
        IPEndPoint remoteIPEndPoint;
        private int ack;
        private int seq;
        private UdpClient udpClient;
        StreamWriter sw = new StreamWriter(Path);

        static void Main(string[] args)
        {
            Server server = new Server();

            server.ack = 0;
            server.seq = 1;
            server.sendMessage = new SNFMessage(0, 0);

            try
            {
                // Enlazar el socket en un puerto
                server.udpClient = new UdpClient(23456);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                return;
            }

            // Dirección desde donde recibir (cualquier direccion)
            // Se modificará tras la recepción (info Paquete)
            server.remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Server Started");
            // El servidor se ejecuta

            for (; ; )
            {
                try
                {
                    // Recibir
                    server.receive();
                    Console.WriteLine("SR {0} AR {1} SE {2} AE {3}", server.receiveMessage.Seq, server.receiveMessage.Ack, server.seq, server.ack);


                    //Comprobacion de que es el mensaje esperado y envio
                    server.check();

                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.ErrorCode + ": " + se.Message);
                    return;
                }
            }
        }

        private void receive()
        {
            byte[] rcvBuffer = udpClient.Receive(ref remoteIPEndPoint);
            receiveMessage = codec.Decode(rcvBuffer);
        }

        private void check()
        {
            if (receiveMessage.Ack == ack && receiveMessage.Seq == seq)
            {
                ack++;
                sendMessage.Ack = ack;
                sendMessage.Seq = receiveMessage.Seq;
                send();
                Console.WriteLine("SS {0} AS {1} SR {2} AR {3}", sendMessage.Seq, sendMessage.Ack, receiveMessage.Seq, receiveMessage.Ack);
                seq++;
                write(receiveMessage.Data);
            }
            else
            {
                Console.WriteLine("Packet unexpected");
                send();
                Console.WriteLine("SS {0} AS {1} SR {2} AR {3}", sendMessage.Seq, sendMessage.Ack, receiveMessage.Seq, receiveMessage.Ack);

            }

            if (receiveMessage.Seq == -1 && receiveMessage.Ack == -2)
            {
                Console.WriteLine("The communication is over. Client off.");
                ack++;
                send();
                close();
            }
        }

        private void send()
        {
            byte[] sendBuffer = codec.Encode(sendMessage);
            udpClient.Send(sendBuffer, sendBuffer.Length, remoteIPEndPoint);

        }

        private void write(byte[] data){
            sw.WriteLine(data);
        }

        private void close()
        {
            sw.Close();
        }
    }
}
 