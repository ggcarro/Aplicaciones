using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SFFVocabulary;

namespace Sender
{
    class SFFSender
    {
        int _receivePort;
        int _senderPort;
        int _lost = 0;
        UdpClient _client;
        IPEndPoint _remoteIPEndPoint;
        PacketBinaryCodec _codec;
        State _state;
        string _filename;
        FileStream _fileStream;
        Random _random = new Random();



        public SFFSender(string ip, int port, string filename)
        {
            _receivePort = 23456;
            _senderPort = 23455;
            _client = new UdpClient(port);
            _remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _codec = new PacketBinaryCodec();
            _filename = filename;

        }

        public void Run()
        {
            _state = new InitialState(this);
            try
            {
                while (_state != null)
                    _state.HandleEvents();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in session: {0}", e.Message);
            }
        }

        public void CreateFile()
        {
            _fileStream = new FileStream(_filename, FileMode.Open, FileAccess.Read);
        }
        public Packet Receive()
        {
            byte[] receiveBuffer = _client.Receive(ref _remoteIPEndPoint);
            return _codec.Decode(receiveBuffer);
        }
        public void Send(Packet packet)
        {
            try
            {
                if (_random.Next(1, 100) > _lost)
                {
                    byte[] sendBuffer = _codec.Encode(packet);
                    _client.Send(sendBuffer, sendBuffer.Length, _remoteIPEndPoint);
                }
                else
                {
                    Console.WriteLine("Packet lost");
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine("Exception: {0}", se);
            }
        }
    }
}
