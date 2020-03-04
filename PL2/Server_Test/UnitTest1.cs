using Microsoft.VisualStudio.TestTools.UnitTesting;
using SNFVocabulary;
using System.IO;

namespace Server_Test
{
    [TestClass]
    public class UnitTest1      // Pruebas de codificacion
    {
        [TestMethod]
        public void TestMethod1()   // Prueba con contenido normal 1
        {
            SNFMessage test1 = new SNFMessage(1234, 1);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test = codification.Encode(test1);
            
            SNFMessage decoded = codification.Decode(test);

            Assert.IsTrue(test1.Seq.CompareTo(decoded.Seq) == 0);
        }

        [TestMethod]
        public void TestMethod2()   // Prueba con contenido normal 2
        {
            SNFMessage test2 = new SNFMessage(1, 1234);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test = codification.Encode(test2);

            SNFMessage decoded = codification.Decode(test);

            Assert.IsTrue(test2.Ack.CompareTo(decoded.Ack) == 0);
        }

        [TestMethod]
        public void TestMethod3()   // Prueba con un seq = ack
        {
            SNFMessage test3 = new SNFMessage(1, 1);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test = codification.Encode(test3);

            SNFMessage decoded = codification.Decode(test);

            Assert.IsTrue(test3.Seq.CompareTo(decoded.Ack) == 0);
        }

        [TestMethod]
        public void TestMethod4()   // Prueba con un ack negativo
        {
            SNFMessage test4 = new SNFMessage(1234, -10);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test = codification.Encode(test4);

            SNFMessage decoded = codification.Decode(test);

            Assert.IsTrue(test4.Ack.CompareTo(decoded.Ack) == 0);
        }
    }
}
