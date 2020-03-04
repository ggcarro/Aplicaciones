using System;
using System.IO;
using SNFVocabulary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnityTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()   // Prueba con contenido normal 1
        {
            SNFMessage data = new SNFMessage(1, 1);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.Encode(data);

            SNFMessage decoded = codification.Decode(test1);

            Assert.IsTrue(data.Seq.CompareTo(decoded.Seq) == 0);
        }

        [TestMethod]
        public void TestMethod2()   // Prueba con contenido normal 2
        {
            SNFMessage data = new SNFMessage(2, 1);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.Encode(data);

            SNFMessage decoded = codification.Decode(test1);

            Assert.IsTrue(data.Seq.CompareTo(decoded.Seq) == 0);
        }

        [TestMethod]
        public void TestMethod3()   // Prueba con mensaje de datos negativos
        {
            SNFMessage data = new SNFMessage(-41, -45);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.Encode(data);

            SNFMessage decoded = codification.Decode(test1);

            Assert.IsTrue(data.Seq.CompareTo(decoded.Seq) == 0);
        }

        [TestMethod]
        public void TestMethod4()   // Prueba con mensaje de datos nulos
        {
            SNFMessage data = new SNFMessage(0, 0);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.Encode(data);

            SNFMessage decoded = codification.Decode(test1);

            Assert.IsTrue(data.Seq.CompareTo(decoded.Seq) == 0);
        }

        [TestMethod]
        public void TestMethod5()   // Prueba con mensaje de iguales
        {
            SNFMessage data = new SNFMessage(1, 1);
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.Encode(data);

            SNFMessage decoded = codification.Decode(test1);

            Assert.IsTrue(data.Ack.CompareTo(decoded.Seq) == 0);
        }

        [TestMethod]
        public void TestMethod6()   // Prueba EncodeFull 1
        {
            byte[] b = new byte[8192];

            SNFMessage data = new SNFMessage(1, 1, b, "nombre");
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.EncodeFull(data);

            SNFMessage decoded = codification.DecodeFull(test1);

            Assert.IsTrue(data.Seq.CompareTo(decoded.Seq) == 0);
        }

        [TestMethod]
        public void TestMethod7()   // Prueba EncodeFull 2
        {
            byte[] b = new byte[8192];

            SNFMessage data = new SNFMessage(1, 1, b, "nombre");
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.EncodeFull(data);

            SNFMessage decoded = codification.DecodeFull(test1);

            Assert.IsTrue(data.FileName.CompareTo(decoded.FileName) == 0);
        }

        [TestMethod]
        public void TestMethod8()   // Prueba con FileName con caracteres especiales
        {
            byte[] b = new byte[8192];

            SNFMessage data = new SNFMessage(1, 1, b, "no—o·Îe");
            BinarySNFMessageCodec codification = new BinarySNFMessageCodec();

            byte[] test1 = codification.EncodeFull(data);

            SNFMessage decoded = codification.DecodeFull(test1);

            Assert.IsTrue(data.FileName.CompareTo(decoded.FileName) == 0);
        }
    }
}