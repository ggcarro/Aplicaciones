using System;
using System.Threading;

namespace Cliente
{

    class Client
    {
        public void Run()
        {
            // Hilo cliente
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