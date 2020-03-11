using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFFVocabulary;

namespace SFFVocabulary_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {/*
            byte[] info = new byte[123456];
            Data data = new Data(1, 1, info);

            BinaryCodec<Data> codification_data = new DataBinaryCodec();
            byte[] data_coded = codification_data.Encode(data);
            Packet message = new Packet(2, data_coded.Length, data_coded);
            BinaryCodec<Packet> codification_packet = new PacketBinaryCodec();
            byte[] codification = codification_packet.Encode(message);

            Packet decodification_packet = codification_packet.Decode(codification);
            byte[] packet_decoded = decodification_packet.Body;
            BinaryCodec<Data> decodification_data = new DataBinaryCodec();
            Data data_decoded = decodification_data.Decode(packet_decoded);

            CollectionAssert.AreEqual(info, data_decoded.Info);
            */
        }
    }
}
