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
        readonly string ipServer = "127.0.0.1";
        readonly int portServer = 23456;
        

        byte[] sendPacket;
        SNFMessage sendMessage;
        SNFMessage receiveMessage;
        BinarySNFMessageCodec codec;
        UdpClient udpClient;
        states state;

        int[] vector;
        

        public SNFClient()
        {
            // Inicializamos variables
            udpClient = new UdpClient();
            codec = new BinarySNFMessageCodec();

            vector =new int[10]{9,8,7,6,5,4,3,2,1,0};
            Console.WriteLine(vector.Length);
            seq = 1;
            ack = 0;
            nTimeOut = 0;

            
            // Asignamos TimeOut
            udpClient.Client.ReceiveTimeout = 500;

            Console.WriteLine("Everything OK");
            

        }

        public void Run()
        {
            Console.WriteLine("Client started");

            while (seq!=0 && nTimeOut<10)
            {
               
                
                // Generamos numero aleatorio para simular pérdida
                Random rd = new Random();
                if (rd.Next(0, 100) > -1 || seq==-1 || seq == 0)
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
                    
                    nTimeOut++;
                }
                if (state == states.Ok)
                {
                    if (checkMessage())
                    {
                        Console.WriteLine("Ack {0} Seq {0}",ack,seq);

                        // Aumentamos Seq
                        if (ack<(vector.Length-1) || seq == -1)
                        {
                            seq++;
                            ack++;
                        }
                        else
                        {
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
            int a = ack;
            if (ack == -2)
            {
                a = vector.Length - 1;
            }
            sendMessage = new SNFMessage(seq, ack, vector[a]);
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
