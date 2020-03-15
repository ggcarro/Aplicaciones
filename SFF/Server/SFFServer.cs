using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Server
{
    class SFFServer
    {
        Packet sendMessage;
        Packet receiveMessage;
        PacketBinaryCodec codec = new PacketBinaryCodec();
        IPEndPoint remoteIPEndPoint;
        UdpClient udpClient;
        FileStream fs;

        string pathFile;
        string dirWork = "C:\\Users\\UO258767\\Desktop\\Server\\";

        public SFFServer()
        {
            try
            {
                udpClient = new UdpClient(23456);
            }
            catch (SocketException se)
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
                    Console.WriteLine("Type {0}", receiveMessage.Type);

                    PacketHandler handle;
                    handle.HandlePacket(receiveMessage.Type); //Comprobar el tipo de mensaje y responder

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


        public void send(int length, byte[] body, int type)
        {
            sendMessage = new Packet(length, body, type);
            byte[] sendBuffer = codec.Encode(sendMessage);
            udpClient.Send(sendBuffer, sendBuffer.Length, remoteIPEndPoint);
        }

        void check()
        {
            if (receiveMessage.Seq == 1)
            {
                pathFile = dirWork + receiveMessage.FileName;

                fs = new FileStream(pathFile, FileMode.Create);
            }
            if (receiveMessage.Ack == ack && receiveMessage.Seq == seq)
            {
                ack++;
                send();
                write();
                seq++;

            }
            else if (receiveMessage.Seq == -1)
            {
                Console.WriteLine("The communication is over. Client off.");
                fs.Close();
                ack = -1;
                seq = -1;
                send();
                ack = 0;
                seq = 1;
            }
            else
            {
                Console.WriteLine("Packet unexpected");
                send();
            }


        }

        void write()
        {
            fs.Write(receiveMessage.Data);
        }

    }
}

