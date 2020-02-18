using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using EchoVocabulary;

namespace ClientUDP
{
    class ClientUDP
    {
        private byte[] sendPacket;

        public void Run()
        {
            UdpClient client = new UdpClient();

            try
            {
                // Envíar información
                BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                EchoMessage sentMessage = new EchoMessage(DateTime.Now.ToString("MM/dd/yyyy h:mm:ss.fff"), "Echo Message");
                sendPacket = codec.Encode(sentMessage);
                client.Send(sendPacket, sendPacket.Length, "127.0.0.1", 23456);
                DateTime T1 = DateTime.Now; //T inicio


                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Recibir información
                byte[] rcvPacket = client.Receive(ref remoteIPEndPoint);
                EchoMessage receivedMessage = codec.UPDdecode(rcvPacket);

                // Comprobar que el mensaje de retorno coincide con el de ida
                if (receivedMessage.Message == sentMessage.Message)
                {
                    Console.WriteLine("The echo works!");
                }

                DateTime T2 = DateTime.ParseExact(receivedMessage.Date, "MM/dd/yyyy h:mm:ss.fff", null); //T cuando el servidor envia el mensaje respuesta
                DateTime T3 = DateTime.Now; //T cuando el cliente recive el mensaje respuesta

                TimeSpan responseT = T3.Subtract(T1);
                TimeSpan askT = T2.Subtract(T1);
                TimeSpan answerT = T3.Subtract(T2);

                if (responseT.TotalMilliseconds > 1000)
                {
                    Console.WriteLine("Timeout");
                }
                else
                {
                    // Muestra de los tiempos que tarda el servidor
                    Console.WriteLine("Sending question time was: {0} miliseconds", askT.TotalMilliseconds);
                    Console.WriteLine("Sending answer time was: {0} miliseconds", answerT.TotalMilliseconds);
                    Console.WriteLine("Total response time was: {0} miliseconds", responseT.TotalMilliseconds);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
            }
            client.Close();
        }
        class Program
        {
            static void Main(string[] args)
            {
                Console.WriteLine("Client Started");
                const int N = 100;

                // Se crean los hilos
                Thread[] threads = new Thread[N];
                for (int i = 0; i < N; i++)
                {
                    ClientUDP client = new ClientUDP();
                    threads[i] = new Thread(new ThreadStart(client.Run));
                    threads[i].Start();
                }
                // Se espera a que los hilos terminen
                for (int i = 0; i < N; i++)
                    threads[i].Join();
            }
        }
    }
}
