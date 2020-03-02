using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SNFVocabulary;

namespace Server
{
    class SNFServer
    {
        int ack;
        int seq;

        SNFMessage sendMessage;
        SNFMessage receiveMessage;
        BinarySNFMessageCodec codec = new BinarySNFMessageCodec();
        IPEndPoint remoteIPEndPoint;
        UdpClient udpClient;
        
        public SNFServer()
        {
            ack = 0;
            seq = 1;

            try
            {
                udpClient = new UdpClient(23456);
            }
            catch(SocketException se)
            {
                Console.WriteLine("Error during starting server");
                Environment.Exit(0);
            }

            remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Server Started");

        }

        public void Run()
        {
            for (; ; )
            {
                try
                {
                    // Recibir
                    receive();
                    Console.WriteLine("SR {0} AR {1}", receiveMessage.Seq, receiveMessage.Ack);
                    
                    //Comprobacion de que es el mensaje esperado y envio
                    check();

                }
                catch (SocketException se)
                {
                    Console.WriteLine(se.ErrorCode + ": " + se.Message);
                    return;
                }
            }
        }

        void receive()
        {
            byte[] rcvBuffer = udpClient.Receive(ref remoteIPEndPoint);
            receiveMessage = codec.Decode(rcvBuffer);
        }
        
        void send()
        {
            sendMessage = new SNFMessage(seq, ack);
            byte[] sendBuffer = codec.Encode(sendMessage);
            udpClient.Send(sendBuffer, sendBuffer.Length, remoteIPEndPoint);
        }

        void check()
        {
            if (receiveMessage.Ack == ack && receiveMessage.Seq == seq)
            {
                ack++;
                send();
                
            }
            else
            {
                Console.WriteLine("Packet unexpected");
                send();
            }

            if (receiveMessage.Seq == -1)
            {
                Console.WriteLine("The communication is over. Client off.");
                ack=0;
                seq = 1;
                send();
            }
        }

    }
}
