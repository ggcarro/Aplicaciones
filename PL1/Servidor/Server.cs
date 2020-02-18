using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using EchoVocabulary;

namespace Server
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
            Console.WriteLine("Server started");
            // El servidor se ejecuta infinitamente
            for (; ; )
            {
                TcpClient client = null;
                NetworkStream netStream = null;
                //BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                ASCIIEchoMessageCodec codec = new ASCIIEchoMessageCodec();

                EchoMessage receiveMsg;
                byte[] responseBuffer;

                try
                {
                    client = listener.AcceptTcpClient();
                    netStream = client.GetStream();

                    // Usar netStream para intercambiar información
                    receiveMsg = codec.Decode(netStream);
                    // Console.WriteLine("Receive Message: {0}, Date: {1}", receiveMsg.Message, receiveMsg.Date);

                    // Para comprbar el funcionamiento del TimeOut
                    // Thread.Sleep(2000);
                    
                    // Enviar el eco
                    EchoMessage responseMessage = new EchoMessage(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"), receiveMsg.Message);
                    
                    responseBuffer = codec.Encode(responseMessage);
                    netStream.Write(responseBuffer, 0, responseBuffer.Length);
                    

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