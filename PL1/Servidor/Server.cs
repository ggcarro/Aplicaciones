using System;
using System.Net;
using System.Net.Sockets;

namespace Servidor
{



    class Server
    {
        static void Main (string[] args)
        {
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, 23456);
                listener.Start();
            }
            catch (SocketException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
                return;
            }

            // El servidor se ejecuta infinitamente
            for (; ; )
            {
                TcpClient client = null;
                NetworkStream netStream = null;

                try
                {
                    client = listener.AcceptTcpClient();
                    netStream = client.GetStream();

                    // Usar netStream para intercambiar información

                    netStream.Close();
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    netStream.Close();
                }
            }
        }
    }
}