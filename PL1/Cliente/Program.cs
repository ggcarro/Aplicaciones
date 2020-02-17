using System;
using System.Net.Sockets;
using System.Threading;
using EchoVocabulary;

namespace Cliente
{

    class Client
    {
        public void Run()
        {
            TcpClient client = null;
            NetworkStream netStream = null;
            string message = "Echo Message";
            EchoMessage sentMessage = new EchoMessage(message, DateTime.Now.ToString("MM/dd/yyyy h:mm:ss.fff"));

            client.Client.ReceiveTimeout = 1000;

            try
            {
                client = new TcpClient("127.0.0.1", 23456);
                netStream = client.GetStream(); //Usar netStream para intercambiar información
                DateTime T1 = DateTime.Now; //T inicio

                //Envio de mensaje
                BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                byte[] responseBuffer = codec.Encode(sentMessage);
                netStream.Write(responseBuffer, 0, responseBuffer.Length);

                //Recepcion de mensaje
                netStream = client.GetStream();
                EchoMessage receivedMessage = codec.Decode(netStream);

                Console.WriteLine("Echo received: ", receivedMessage.Message);

                DateTime T2 = Convert.ToDateTime(receivedMessage.Date); //T cuando el servidor envia el mensaje respuesta
                DateTime T3 = DateTime.Now; //T cuando el cliente recive el mensaje respuesta

                //Comprobacion de si el servidor tarda mas de 1s en responder (usamos ms)
                int responseTime = (3600000 * T3.Hour + 60000 * T3.Minute + 1000 * T3.Second + T3.Millisecond) - (3600000 * T1.Hour + 60000 * T1.Minute + 1000 * T1.Second + T1.Millisecond);
                int askTime = (3600000 * T2.Hour + 60000 * T2.Minute + 1000 * T2.Second + T2.Millisecond) - (3600000 * T1.Hour + 60000 * T1.Minute + 1000 * T1.Second + T1.Millisecond);
                int answerTime = (3600000 * T3.Hour + 60000 * T3.Minute + 1000 * T3.Second + T3.Millisecond) - (3600000 * T2.Hour + 60000 * T2.Minute + 1000 * T2.Second + T2.Millisecond);
                Console.WriteLine("Sending question time was: {0} miliseconds", askTime);
                Console.WriteLine("Sending answer time was: {0} miliseconds", answerTime);
                Console.WriteLine("Total response time was: {0} miliseconds", responseTime);
            }


            catch(Exception e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException is SocketException)
                    {
                        SocketException se = (SocketException)e.InnerException;

                        if (se.SocketErrorCode == SocketError.TimedOut)
                            Console.WriteLine("TimeOut for this request");
                        else
                            Console.WriteLine("An error occurred"); // Otro tipo de error en el socket
                    }
                }
                else
                    Console.WriteLine("Exception: {0}", e.Message); // Otro tipo de error
            }
            finally
            {
                netStream.Close();
                client.Close();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int N = 100;

            // Se crean los hilos
            Thread[] threads = new Thread[N];
            for (int i = 0; i < N; i++)
            {
                Client client = new Client();
                threads[i] = new Thread(new ThreadStart(client.Run));
                threads[i].Start();
            }
            // Se espera a que los hilos terminen
            for (int i = 0; i < N; i++)
                threads[i].Join();
        }
    }
}