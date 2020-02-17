using System;
using System.Net.Sockets;
using System.Threading;

namespace Cliente
{

    class Client
    {
        public void Run()
        {
            TcpClient client = null;
            NetworkStream netStream = null;
            try
            {
                client = new TcpClient("127.0.0.1", 23456);

                netStream = client.GetStream();

                // Usar netStream para intercambiar información

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
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