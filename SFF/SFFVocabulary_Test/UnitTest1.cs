using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFFVocabulary;

namespace SFFVocabulary_Test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()  // Paquete data
        {
            byte[] info = { 4, 8, 12, 16, 23, 42};
            int seq = 1;
            Data data = new Data(info, seq);

            BinaryCodec<Data> codification_data = new DataBinaryCodec();
            byte[] data_coded = codification_data.Encode(data);
            Packet message = new Packet(2, data_coded.Length, data_coded);
            BinaryCodec<Packet> codification_packet = new PacketBinaryCodec();
            byte[] codification = codification_packet.Encode(message);

            Packet decodification_packet = codification_packet.Decode(codification);
            byte[] packet_decoded = decodification_packet.Body;
            BinaryCodec<Data> decodification_data = new DataBinaryCodec();
            Data data_decoded = decodification_data.Decode(packet_decoded);

            CollectionAssert.AreEqual(info, data_decoded.Information);

        }

        [TestMethod]
        public void TestMethod2()  // Paquete ackData
        {
            int ack = 2;
            AckData ackData = new AckData(ack);

            BinaryCodec<AckData> codification_ackData = new AckDataBinaryCodec();
            byte[] ackData_coded = codification_ackData.Encode(ackData);
            Packet message = new Packet(2, ackData_coded.Length, ackData_coded);
            BinaryCodec<Packet> codification_packet = new PacketBinaryCodec();
            byte[] codification = codification_packet.Encode(message);

            Packet decodification_packet = codification_packet.Decode(codification);
            byte[] packet_decoded = decodification_packet.Body;
            BinaryCodec<AckData> decodification_ackData = new AckDataBinaryCodec();
            AckData ackData_decoded = decodification_ackData.Decode(packet_decoded);

            Assert.AreEqual(ack, ackData_decoded.Ack);

        }


        [TestMethod]
        public void TestMethod3()  // Paquete newFile
        {
            string fileName = "XXX.mov";
            NewFile nfile = new NewFile(fileName);

            BinaryCodec<NewFile> codification_nfile = new NewFileBinaryCodec();
            byte[] nfile_coded = codification_nfile.Encode(nfile);
            Packet message = new Packet(2, nfile_coded.Length, nfile_coded);
            BinaryCodec<Packet> codification_packet = new PacketBinaryCodec();
            byte[] codification = codification_packet.Encode(message);

            Packet decodification_packet = codification_packet.Decode(codification);
            byte[] packet_decoded = decodification_packet.Body;
            BinaryCodec<NewFile> decodification_nfile = new NewFileBinaryCodec();
            NewFile nfile_decoded = decodification_nfile.Decode(packet_decoded);

            Assert.AreEqual(fileName, nfile_decoded.FileName);

        }

        [TestMethod]
        public void TestMethod4()  // Paquete newFile caracteres especiales
        {
            string fileName = "ρφα.mov";
            NewFile nfile = new NewFile(fileName);

            BinaryCodec<NewFile> codification_nfile = new NewFileBinaryCodec();
            byte[] nfile_coded = codification_nfile.Encode(nfile);
            Packet message = new Packet(2, nfile_coded.Length, nfile_coded);
            BinaryCodec<Packet> codification_packet = new PacketBinaryCodec();
            byte[] codification = codification_packet.Encode(message);

            Packet decodification_packet = codification_packet.Decode(codification);
            byte[] packet_decoded = decodification_packet.Body;
            BinaryCodec<NewFile> decodification_nfile = new NewFileBinaryCodec();
            NewFile nfile_decoded = decodification_nfile.Decode(packet_decoded);

            Assert.AreEqual(fileName, nfile_decoded.FileName);

        }
    }
}
