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
        int _seq;
        int _senderPort;
        int _lost = 0;
        int _timeOut;
        int minTimeOut = 500;
        int _fails = 0;
        int _maxFails = 10;
        bool _continue;
        long _bytesLeft;
        Packet _lastPacket;
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
            _bytesLeft = new FileInfo(_filename).Length;


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

        public void AddFail()
        {
            _fails++;
        }
        public void ChangeState(SenderState senderState)
        {
            _state = senderState;
        }
        public bool CheckFails()
        {
            return (_fails < _maxFails);
        }
        public bool CheckSeq(Packet packet)
        {
            ICodec<AckData> dCodec = new AckDataBinaryCodec();
            AckData ackData = dCodec.Decode(packet.Body);
            if (ackData.Ack == _seq)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ContinueTX()
        {
            return _continue;
        }
        public void CreateFile()
        {
            _fileStream = new FileStream(_filename, FileMode.Open, FileAccess.Read);
            
        }
        public Packet Discon()
        {
            return new Packet((int)PacketBodyType.Discon, 0, null);
        }
        public Packet FileNamePacket()
        {
            NewFile file = new NewFile(_filename);
            ICodec<NewFile> fcodec = new NewFileBinaryCodec();
            byte[] fBuffer = fcodec.Encode(file);
            return new Packet((int)PacketBodyType.NewFile, fBuffer.Length, fBuffer);

        }
        public void Finish()
        {
            _fileStream.Close();
            _client.Close();
        }
        public void IncreaseSeq()
        {
            _seq++;
        }
        public Packet LastPacket()
        {
            return _lastPacket;
        }
        public Packet Receive()
        {
            byte[] receiveBuffer = _client.Receive(ref _remoteIPEndPoint);
            return _codec.Decode(receiveBuffer);
        }
        public Packet ReadData()
        {
            //Ahora estoy leyendo siempre 512, hay que modificarlo para que si quedan menos lea solo los que queden
            int bytes=512;
            if (_bytesLeft > 512)
            {
                _bytesLeft -= 512;
                _continue = true;
            }
            else
            {
                bytes = (int) _bytesLeft;
                _bytesLeft = 0;
                _continue = false;
            }
            byte[] buffer = new byte[bytes];
            _fileStream.Read(buffer, 0, buffer.Length);
            Data data = new Data(buffer, _seq);
            ICodec<Data> _dCodec = new DataBinaryCodec();
            byte[] body = _dCodec.Encode(data);
            return new Packet((int)PacketBodyType.Data, body.Length, body);

        }
        public void ResetFails()
        {
            _fails = 0;
        }
        public void Send(Packet packet)
        {
            _lastPacket = packet;
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
        public void SetTimer()
        {
            _client.Client.ReceiveTimeout = _timeOut;
            _timeOut = minTimeOut;
        }
        public void Timer()
        {
            _timeOut = _timeOut * 2;
            _client.Client.ReceiveTimeout = _timeOut;
        }
    }
}
