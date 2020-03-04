using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SNFVocabulary
{
    public class BinarySNFMessageCodec
    {
        public byte[] Encode(SNFMessage message)
        {
            byte[] byteBuffer;

            int _seq = message.Seq;
            int _ack = message.Ack;
            byte[] _data = message.Data;
            string _fileName = message.FileName;

            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(_seq);
            writer.Write(_ack);

            writer.Flush();
            byteBuffer = ms.ToArray();

            return byteBuffer;
        }
        public byte[] EncodeFull(SNFMessage message)
        {
            byte[] byteBuffer;

            int _seq = message.Seq;
            int _ack = message.Ack;
            byte[] _data = message.Data;
            string _fileName = message.FileName;

            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(_seq);
            writer.Write(_ack);
            writer.Write(_data);
            writer.Write(_fileName);
            

            writer.Flush();
            byteBuffer = ms.ToArray();

            return byteBuffer;
        }

        public SNFMessage Decode(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(ms);

            int read_seq = reader.ReadInt32();
            int read_ack = reader.ReadInt32();

            SNFMessage decoded_message = new SNFMessage(read_seq, read_ack);
            return decoded_message;

        }

        public SNFMessage DecodeFull(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(ms);

            int read_seq = reader.ReadInt32();
            int read_ack = reader.ReadInt32();
            byte[] read_data = reader.ReadBytes(1024);
            string read_fileName = reader.ReadString();

            SNFMessage decoded_message = new SNFMessage(read_seq, read_ack, read_data, read_fileName);
            return decoded_message;

        }
    }
}
