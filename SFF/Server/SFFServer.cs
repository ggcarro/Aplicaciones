using System;
using System.Net;
using System.Net.Sockets;
using SFFVocabulary;


namespace Server
{
    class SFFServer
    {
        int _port;
        UdpClient client;
        IPEndPoint remoteIPEndPoint;
        State _state;

        public SFFServer(int port)
        {
            _port = port;
            client = new UdpClient(_port);
            remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

        }

        public void Run()
        {
            Console.WriteLine("Waiting for connection ...");
            _state = new InitialState(this);
            try
            {
                while(_state != null){
                    _state.HandleEvents();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in server: {0}", e.Message);
            }
        }
        public Packet receive()
        {
            byte[] rcvBuffer = client.Receive(ref remoteIPEndPoint);
            PacketBinaryCodec codec = new PacketBinaryCodec();
            return codec.Decode(rcvBuffer);
        }
        // Se añadiran las funciones que haya que implementar...
    }
}

