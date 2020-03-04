using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SNFVocabulary;

namespace PL2
{
    class FileClient
    {
        private int N = 100;
        private readonly string ipServer = "127.0.0.1";
        private readonly int portServer = 23456;

        private byte[] sendPacket;
        private SNFMessage sendMessage;
        private SNFMessage receiveMessage;
        private BinarySNFMessageCodec codec = new BinarySNFMessageCodec();
        private long bytesToSend;
        private int bytesSended;
        private long fileLength;
        private int maxBytes = 512000;


        public static void Main(string[] args)
        {
            // Creamos objeto clase cliente;
            FileClient client = new FileClient();

            // Creamos socket UDP
            UdpClient udpClient = new UdpClient();

            // Asignamos TimeOut
            udpClient.Client.ReceiveTimeout = 5000;

            Console.WriteLine("Client Started");


            // Cargamos el fichero
            FileStream file = new FileStream(args[0], FileMode.Open);
            client.fileLength = file.Length;
            client.bytesToSend = file.Length;

            Console.WriteLine("File Length: {0}", client.fileLength);
            

            // Enviamos el primer mensaje   
            int seq = 0;
            try
            {
                client.send(seq, seq - 1, udpClient);
                Console.WriteLine("Send: Seq {0}, Ack {1}", client.sendMessage.Seq, client.sendMessage.Ack);

            }
            catch (Exception e)
            {
                Console.WriteLine("Something happened when sending the first message.");
                Console.WriteLine("Exception: {0}", e.Message); // Otro tipo de error
                Environment.Exit(0); // Finalizamos el programa
            }


            int nTimeOut = 0;
            possibleStates state;

            while (client.bytesToSend!=0 && nTimeOut < 10)
            {

                //Recibo
                state = client.receive(udpClient);

                // Si hay timeOut vuelvo a enviar
                if (state == possibleStates.TimeOut)
                {
                    nTimeOut++;
                    client.send(seq, seq - 1, udpClient);
                    Console.WriteLine("TimeOut {0} SS {1}, SA {2}", nTimeOut, client.sendMessage.Seq, client.sendMessage.Ack);
                }

                if (state == possibleStates.ReceiveOk)
                {
                    // Reseteamos TimeOut
                    nTimeOut = 0;

                    // Comprobamos que es el paquete deseado
                    if (client.checkMessage())
                    {
                        // Aumentamos numero de seq
                        seq++;
                        // Leemos del archivo y actualizmos variables;
                        byte[] data = new byte[client.maxBytes];
                        if (client.bytesToSend > client.maxBytes)
                        {
                            file.Read(data, 0, client.bytesSended + client.maxBytes);
                            client.bytesSended += client.maxBytes;
                            client.bytesToSend -= client.maxBytes;
                        }
                        else
                        {
                            file.Read(data, 0, (int) client.bytesToSend);
                            client.bytesSended += client.maxBytes;
                            client.bytesToSend = 0;
                        }
                        
                        

                        // Generamos numero aleatorio para simular posible pérdida
                        Random rd = new Random();
                        if (rd.Next(0, 10) > -1)
                        {
                            client.send(seq, seq - 1, udpClient, data);
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


            if (nTimeOut >= 10)
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

        private void send(int seq, int ack, UdpClient udpClient, byte[] data = null)
        {
            if( seq==0 || seq == -1)
            {
                sendMessage = new SNFMessage(seq, ack);
                sendPacket = codec.Encode(sendMessage);
                udpClient.Send(sendPacket, sendPacket.Length, ipServer, portServer);
            }
            else
            {
                sendMessage = new SNFMessage(seq, ack, data);
                sendPacket = codec.Encode(sendMessage);
                udpClient.Send(sendPacket, sendPacket.Length, ipServer, portServer);
            }
            
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
            if (sendMessage.Seq == receiveMessage.Seq && sendMessage.Ack + 1 == receiveMessage.Ack)
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
