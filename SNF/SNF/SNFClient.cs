using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SNFVocabulary;

namespace Client
{
    public class SNFClient
    {
        int seq;
        int ack;
        int N = 100;
        int nTimeOut;
        readonly string ipServer = "192.168.222.42";
        readonly int portServer = 23456;
        readonly string filePath = "C:/Users/UO258767/Desktop/Moto.mov";
        string fileName;

        byte[] sendPacket;
        SNFMessage sendMessage;
        SNFMessage receiveMessage;
        BinarySNFMessageCodec codec;
        UdpClient udpClient;
        states state;

        FileStream file;
        byte[] data;
        
        

        public SNFClient()
        {
            // Inicializamos variables
            udpClient = new UdpClient();
            codec = new BinarySNFMessageCodec();
            
            file = new FileStream(filePath, FileMode.Open);
            String[] path = filePath.Split('/');
            fileName = path[path.Length - 1];
            seq = 1;
            ack = 0;
            data = new byte[8192];
            nTimeOut = 0;

            
            // Asignamos TimeOut
            udpClient.Client.ReceiveTimeout = 5000;

            Console.WriteLine("Everything OK");
            

        }

        public void Run()
        {
            Console.WriteLine("Client started");
            int c=0;
            bool b = true;
            while (seq!=0 && nTimeOut<10)
            {
                if (b == true)
                {
                    c = file.Read(data, 0, data.Length);
                }
                
                // Generamos numero aleatorio para simular pérdida
                Random rd = new Random();
                if (rd.Next(0, 100) > 2 || seq==-1 || seq == 0)
                {
                    
                    send();
                    Console.WriteLine("SS {0} SA {1}", sendMessage.Seq, sendMessage.Ack);
                }
                else
                {
                    Console.WriteLine("Packet Lost SS {0} SA {1}", sendMessage.Seq, sendMessage.Ack);
                }

                state = receive();

                if (state == states.TimeOut)
                {
                    b = false;
                    nTimeOut++;
                }
                if (state == states.Ok)
                {
                    if (checkMessage())
                    {
                        // Aumentamos Seq
                        if (c>0 || seq == -1)
                        {
                            seq++;
                            ack++;
                            b = true;
                        }
                        else
                        {
                            b = false;
                            seq = -1;
                            ack = -2;
                        }
                        // Reseteamos intentos nTimeOut;
                        nTimeOut = 0;
                        


                    }
                    else
                    {
                        Console.WriteLine("Packet not expected SS {0} SA {1} RS {2} RA {3}", sendMessage.Seq, sendMessage.Ack, receiveMessage.Seq, receiveMessage.Ack);
                    }
                }
            }

            if (nTimeOut >= 10)
            {
                Console.WriteLine("Max number of TimeOuts");
            }
            else
            {
                Console.WriteLine("Everything was successfully delivered");
            }

            udpClient.Close();
            Console.WriteLine("Client finish");
            Environment.Exit(0);

        }

        void send()
        {
            sendMessage = new SNFMessage(seq, ack, data, fileName);
            sendPacket = codec.EncodeFull(sendMessage);
            udpClient.Send(sendPacket, sendPacket.Length, ipServer, portServer);
        }

        states receive()
        {
            try
            {
                // Recibir datos
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedData = udpClient.Receive(ref remoteEP);
                receiveMessage = codec.Decode(receivedData);
                return states.Ok;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    // Timeout
                    Console.WriteLine("TimeOut Error");
                    return states.TimeOut;
                }

                else
                {
                    // Otro error
                    return states.Error;
                    
                }

            }
        }

        bool checkMessage()
        {
            if (sendMessage.Seq == receiveMessage.Seq && sendMessage.Ack + 1 == receiveMessage.Ack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        enum states
        {
            Ok,
            TimeOut,
            Error
        }
    }
}
