using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SFFVocabulary;


namespace Receiver
{
    class SFFReceiver
    {
        int _senderPort = 23456;
        int _receiverPort = 23457;
        int _seq;
        const int _lost = 10;  
        UdpClient _client;
        IPEndPoint _remoteIPEndPoint;
        ICodec<Packet> _codec;
        FileStream _fileStream;
        State _state;
        Random _random;

        public SFFReceiver(int port)
        {
            _client = new UdpClient(23456);
            _remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _codec = new PacketBinaryCodec();

        }

        public void Run()
        {
            Console.WriteLine("Waiting for connection ...");
            _state = new InitialState(this);
            try
            {
                while (_state != null) {
                    _state.HandleEvents();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in server: {0}", e.Message);
            }
        }
        // Se añadiran las funciones que haya que implementar... (Las estoy ordenando alfabéticamente)
        public Packet Ack()
        {
            AckData ackData = new AckData(_seq);
            ICodec<AckData> ackCodec = new AckDataBinaryCodec();
            Byte[] body = ackCodec.Encode(ackData);
            return new Packet((int)PacketBodyType.AckData, body.Length, body);
        }

        public void ChangeState(ReceiverState receiverState)
        {
            _state = receiverState;
        }

        public bool CheckSeq(Packet packet)
        {
            ICodec<Data> dCodec = new DataBinaryCodec();
            Data data = dCodec.Decode(packet.Body);
            if (data.Seq == _seq)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateFile(Packet packet)
        {
            ICodec<NewFile> nfCodec = new NewFileBinaryCodec();
            NewFile newFile = nfCodec.Decode(packet.Body);
            string Path = "C:/Users/UO258767/Desktop/" + newFile.FileName;
            _fileStream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);
            _seq = 1;  
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

        public Packet Receive()
        {
            byte[] receiveBuffer = _client.Receive(ref _remoteIPEndPoint);
            Packet packet =_codec.Decode(receiveBuffer);
            Console.WriteLine("Receive Packet -- Type: {0}", packet.Type);
            return packet;
        }

        public void Send(Packet packet)
        {
            Console.WriteLine("Send Packet -- Type: {0}", packet.Type);
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
            catch(SocketException se)
            {
                Console.WriteLine("Exception: {0}", se);
            }
        }
        
        public void Write(Packet packet)
        {
            ICodec<Data> dCodec = new DataBinaryCodec();
            Data data = dCodec.Decode(packet.Body);
            _fileStream.Write(data.Information, 0, data.Information.Length);
        }
    }
}

