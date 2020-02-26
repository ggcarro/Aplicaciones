using System;
using System.Net;
using System.Net.Sockets;
using SNFVocabulary;

namespace PL2
{
    class Client
    {
        private string ipServer = "192.168.222.42";
        private int portServer = 23456;
        private byte[] sendPacket;
        public BinarySNFMessageCodec codec = new BinarySNFMessageCodec();

        public static void Main(string[] args)
        {

            BinarySNFMessageCodec binaryCodec = new BinarySNFMessageCodec();
            UdpClient udpClient = new UdpClient();
            
            Client client = new Client();
            
            Console.WriteLine("Client Started");
            const int N = 100000; // Suponemos que se quiere enviar este cantidad de números

                
            SNFMessage sendMessage = null;



            for (int i = 1; i < N; i++)
            {
                // Envio información

                sendMessage = new SNFMessage(i, i - 1);

                try
                {
                    client.send(sendMessage, udpClient);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message); // Otro tipo de error
                }

                bool t;
                int n = 0;
                // Recibo información
                Console.WriteLine("recibo info");
                do
                {
                    Console.WriteLine("Intendo recibir");
                    t = false;
                    var timeToWait = TimeSpan.FromSeconds(2);
                    var asyncResult = udpClient.BeginReceive(null, null);
                    asyncResult.AsyncWaitHandle.WaitOne(timeToWait);
                    if (asyncResult.IsCompleted)
                    {
                        try
                        {
                            IPEndPoint remoteEP = null;
                            byte[] receivedData = udpClient.EndReceive(asyncResult, ref remoteEP);


                            SNFMessage receiveMessage = binaryCodec.Decode(receivedData);
                            Console.WriteLine("Ack received {0}", receiveMessage.Ack);
                            if (receiveMessage.Ack == i)
                            {
                                Console.WriteLine("OK");
                                t = true;
                            }
                            else
                            {
                                Console.WriteLine("Incorrect ACK");
                                Console.WriteLine("Try to send again the packet");
                                Console.WriteLine("Seq sent: {0} Ack sent: {1} Seq receive: {2} Ack receive: {3}", sendMessage.Seq, sendMessage.Ack, receiveMessage.Seq, receiveMessage.Ack);
                                n++;
                                client.send(sendMessage, udpClient);
                            }
                            // EndReceive worked and we have received data and remote endpoint
                        }
                        catch (Exception ex)
                        {
                            // EndReceive failed and we ended up here
                            Console.WriteLine(ex.Message);
                            client.send(sendMessage, udpClient);
                        }
                    }
                    else
                    {
                        // The operation wasn't completed before the timeout and we're off the hook
                        Console.WriteLine("Timeout");
                        Console.WriteLine("Try to send again the packet");
                        n++;
                        client.send(sendMessage, udpClient);
                    }
                }while (t == false || n > 20) ;

                if (n > 20)
                {
                    Console.WriteLine("Number max of timeouts");
                }

            }
        
        }

        public void send(SNFMessage message, UdpClient client)
        {
            sendPacket = codec.Encode(message);
            client.Send(sendPacket, sendPacket.Length, ipServer, portServer);
            Console.WriteLine("Seq sent {0}", message.Seq);
        }

       


    }
}
