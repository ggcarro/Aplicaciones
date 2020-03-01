using System;
using System.Net;
using System.Net.Sockets;
using SNFVocabulary;

namespace PL2
{
    class Client
    {
        private int N = 100;
        private readonly string ipServer = "127.0.0.1";
        private readonly int portServer = 23456;

        private byte[] sendPacket;
        private SNFMessage sendMessage;
        private SNFMessage receiveMessage;
        private BinarySNFMessageCodec codec = new BinarySNFMessageCodec();
         

        public static void Main(string[] args)
        {
            // Creamos objeto clase cliente;
            Client client = new Client();
            
            // Creamos socket UDP
            UdpClient udpClient = new UdpClient();

            // Asignamos TimeOut
            udpClient.Client.ReceiveTimeout = 5000;

            Console.WriteLine("Client Started");
            

            // Enviamos el primer mensaje    
            try
            {
                client.send(1, 0, udpClient);
                Console.WriteLine("Send: Seq {0}, Ack {1}", client.sendMessage.Seq, client.sendMessage.Ack);

            }
            catch (Exception e)
            {
                Console.WriteLine("Something happened when sending the first message.");
                Console.WriteLine("Exception: {0}", e.Message); // Otro tipo de error
                Environment.Exit(0); // Finalizamos el programa
            }

            int i = 1;
            int nTimeOut = 0;
            possibleStates state;
            bool timeOut;

            while(i<client.N && nTimeOut<10)
            {
                
                //Recibo
                state=client.receive(udpClient);

                // Si hay timeOut vuelvo a enviar
                if (state == possibleStates.TimeOut)
                {
                    nTimeOut++;
                    client.send(i, i-1, udpClient);
                    Console.WriteLine("TimeOut {0} SS {1}, SA {2}", nTimeOut, client.sendMessage.Seq, client.sendMessage.Ack);
                }

                if(state == possibleStates.ReceiveOk)
                {
                    // Reseteamos TimeOut
                    nTimeOut = 0;

                    // Comprobamos que es el paquete deseado
                    if (client.checkMessage())
                    {
                        // Aumentamos numero de seq
                        i++;

                        // Generamos numero aleatorio para simular posible pérdida
                        Random rd = new Random();
                        if (rd.Next(0, 10) > 2)
                        {
                            client.send(i, i - 1, udpClient);
                            Console.WriteLine("Send: SS {0}, AS {1}", client.sendMessage.Seq, client.sendMessage.Ack);
                        }
                        else
                        {
                            Console.WriteLine("Lost Packet SS: {0}, AS {1}", client.sendMessage.Seq, client.sendMessage.Ack);
                        }
                    }

                    else
                    {
                        Console.WriteLine("Packet not Expected:  SS {0} AS {1} SR {2} AR {3}", client.sendMessage.Seq, client.sendMessage.Ack, client.receiveMessage.Seq, client.receiveMessage.Ack);
                        // ¿enviamos el que el cliente desea?
                    }
                }
            }


            // Enviamos fin y no hace falta esperar a confirmación.
            client.send(-1, 0, udpClient);
            

            if(nTimeOut >= 10)
            {
                Console.WriteLine("Maximum number of timeout attempts");
                Console.WriteLine("Check the conexion with the server");
            }

            else
            {
                Console.WriteLine("It seems that everything worked correctly");
            }

            udpClient.Close();
            Environment.Exit(0);

        }

        private void send(int seq, int ack, UdpClient udpClient)
        {
            sendMessage = new SNFMessage(seq, ack);
            sendPacket = codec.Encode(sendMessage);
            udpClient.Send(sendPacket, sendPacket.Length, ipServer, portServer);
        }

        private possibleStates receive(UdpClient udpClient)
        {
            try
            {
                // Recibir datos
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedData = udpClient.Receive(ref remoteEP);
                receiveMessage = codec.Decode(receivedData);
                return possibleStates.ReceiveOk;
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                {
                    // Timeout
                    Console.WriteLine("TimeOut Error");
                    return possibleStates.TimeOut;
                }

                else
                {
                    // Otro error
                    Console.WriteLine("Server seems to be disconnected");
                    return possibleStates.Error;
                }

            }
        
            
        }

        
        private bool checkMessage()
        {
            if(sendMessage.Seq==receiveMessage.Seq && sendMessage.Ack + 1 == receiveMessage.Ack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private enum possibleStates
        {
            ReceiveOk,
            TimeOut,
            Error,
        }


    }
}
