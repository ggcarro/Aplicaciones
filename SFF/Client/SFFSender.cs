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
        int _seq;
        int _lost = 20;
        int _timeOut;
        int minTimeOut = 500;
        int _fails = 0;
        int _maxFails = 10;
        int _portReceiver;
        bool _continue;
        long _bytesLeft;
        Packet _lastPacket;
        UdpClient _client;
        IPEndPoint _remoteIPEndPoint;
        PacketBinaryCodec _codec;
        State _state;
        string _filename;
        string _ipServer;
        FileStream _fileStream;
        Random _random = new Random();



        public SFFSender(string ip, int port, string filename)
        {
            _client = new UdpClient();
            _codec = new PacketBinaryCodec();
            _filename = filename;
            _bytesLeft = new FileInfo(_filename).Length;
            _ipServer = ip;
            _portReceiver = port;
            _timeOut = minTimeOut;



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
            return (_fails > _maxFails);
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
            byte[] bytes = Encoding.ASCII.GetBytes("Discon");
            return new Packet((int)PacketBodyType.Discon, bytes.Length, bytes);
        }
        public Packet FileNamePacket()
        {
            String[] path = _filename.Split('/');
            string fileName = path[path.Length - 1];
            NewFile file = new NewFile(fileName);
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
            _remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBuffer = _client.Receive(ref _remoteIPEndPoint);
            return _codec.Decode(receiveBuffer);
        }
        public Packet ReadData()
        {
            int bytes=512;
            if (_bytesLeft > 512)
            {
                _bytesLeft -= 512;
                _continue = true;
            }
            else
            {
                Console.WriteLine("Ultimo paquete");
                bytes = (int)_bytesLeft;
                _bytesLeft -= _bytesLeft;
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
                int rd = _random.Next(1, 100);
                if (rd > _lost || packet.Type== PacketBodyType.NewFile)
                {
                    byte[] sendBuffer = _codec.Encode(packet);
                    Console.WriteLine("Send Packet -- Seq: {0}, Type: {1}", _seq, packet.Type);
                    _client.Send(sendBuffer, sendBuffer.Length, _ipServer, _portReceiver);
                }
                else
                {
                    Console.WriteLine("Packet lost -- Seq: {0}", _seq);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine("Exception: {0}", se);
            }
        }
        public void SetTimer()
        {
            _timeOut = minTimeOut;
            _client.Client.ReceiveTimeout = _timeOut;
        }
        public void Timer(bool increase=true)
        {
            if (increase)
            {
                _timeOut = _timeOut * 2;
            }
            Console.WriteLine("TimeOut: {0}", _timeOut);
            _client.Client.ReceiveTimeout = _timeOut;
        }
    }
}
