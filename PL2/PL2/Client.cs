using System;
using System.Net;
using System.Net.Sockets;
using SNFVocabulary;

namespace PL2
{
    class Client
    {
        private int N = 100;
        private string ipServer = "127.0.0.1";
        private int portServer = 23456;

        private byte[] sendPacket;
        private SNFMessage sendMessage;
        private SNFMessage receiveMessage;
        private BinarySNFMessageCodec codec = new BinarySNFMessageCodec();
         

        public static void Main(string[] args)
        {
            Client client = new Client();
            
            UdpClient udpClient = new UdpClient();
            Console.WriteLine("Client Started");
            

            // Enviamos el primer mensaje    
            try
            {
                client.send(1, 0, udpClient);
                Console.WriteLine("Send: SS {0}, AS{1}", client.sendMessage.Seq, client.sendMessage.Ack);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message); // Otro tipo de error
            }
            int i = 1;
            int nTimeOut = 0;
            bool timeOut;

            while(i<client.N && nTimeOut<10)
            {
                
                //Recibo
                timeOut=client.receive(udpClient);

                // Si hay timeOut vuelvo a enviar
                if (timeOut == false)
                {
                    nTimeOut++;
                    client.send(i, i-1, udpClient);
                    Console.WriteLine("TimeOut {0} SS {1}, SA {2}", nTimeOut, client.sendMessage.Seq, client.sendMessage.Ack);
                }
                if(timeOut== true)
                {
                    nTimeOut = 0;
                    // compruebo
                    if (client.checkMessage())
                    {
                        Console.WriteLine("OK SS {0} AS {1} SR {2} AR {3}", client.sendMessage.Seq, client.sendMessage.Ack, client.receiveMessage.Seq, client.receiveMessage.Ack);
                        i++;
                        Random rd = new Random();
                        if (rd.Next(0, 10) > 5)
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

                    }
                }
            }

            
        }

        private void send(int seq, int ack, UdpClient udpClient)
        {
            sendMessage = new SNFMessage(seq, ack);
            sendPacket = codec.Encode(sendMessage);
            udpClient.Send(sendPacket, sendPacket.Length, ipServer, portServer);
        }

        private bool receive(UdpClient udpClient)
        {
            var timeToWait = TimeSpan.FromSeconds(5);
            var asyncResult = udpClient.BeginReceive(null, null);
            asyncResult.AsyncWaitHandle.WaitOne(timeToWait);
            if (asyncResult.IsCompleted)
            {
                try
                {
                    IPEndPoint remoteEP = null;
                    byte[] receivedData = udpClient.EndReceive(asyncResult, ref remoteEP);
                    receiveMessage = codec.Decode(receivedData);
                    return true;
                    
                }
                catch (Exception ex)
                {
                    // EndReceive failed and we ended up here
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                // The operation wasn't completed before the timeout and we're off the hook
                // Console.WriteLine("Timeout");
                return false;
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
        


    }
}
