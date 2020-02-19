using System;
using System.Net.Sockets;
using System.Threading;
using EchoVocabulary;

namespace Client
{

    class Client
    {
        public void Run()
        {
            TcpClient client = null;
            NetworkStream netStream = null;
            string message = "Echo Message";
            EchoMessage sentMessage = new EchoMessage(DateTime.Now.ToString("MM/dd/yyyy h:mm:ss.fff"), message);


            
            try
            {
                client = new TcpClient("192.168.222.42", 23456);
                client.ReceiveTimeout = 1000;   // Iniciacion del timeout del socket
                netStream = client.GetStream(); //Usar netStream para intercambiar información
                DateTime T1 = DateTime.Now; //T inicio
                
                //Envio de mensaje
                BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                //ASCIIEchoMessageCodec codec = new ASCIIEchoMessageCodec();

                byte[] responseBuffer = codec.Encode(sentMessage);
                netStream.Write(responseBuffer, 0, responseBuffer.Length);

                //Recepcion de mensaje
                netStream = client.GetStream();
                EchoMessage receivedMessage = codec.Decode(netStream);

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

                // Muestra de los tiempos que tarda el servidor
                Console.WriteLine("Sending Time: {0} ms, Answer Time: {1} ms, Total Response: {2} ms", askT.TotalMilliseconds, answerT.TotalMilliseconds, responseT.TotalMilliseconds);
                
            
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
            Console.WriteLine("Client Started");
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