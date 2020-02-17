using System;
using System.Net;
using System.Net.Sockets;
using EchoVocabulary;

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
                BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                EchoMessage receiveMsg;
                byte[] responseBuffer;

                try
                {
                    client = listener.AcceptTcpClient();
                    netStream = client.GetStream();

                    // Usar netStream para intercambiar información
                    receiveMsg = codec.Decode(netStream);

                    // Enviar el eco
                    EchoMessage responseMessage = new EchoMessage(receiveMsg.Message, DateTime.Now.ToString("MM/dd/yyyy h:mm:ss.fff"));
                    responseBuffer = codec.Encode(responseMessage);
                    netStream.Write(responseBuffer, 0, responseBuffer.Length);

                    // Comprobar que el mensaje de retorno coincide con el de ida
                    if (receiveMsg.Message == responseMessage.Message)
                        Console.WriteLine("The echo works!");

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