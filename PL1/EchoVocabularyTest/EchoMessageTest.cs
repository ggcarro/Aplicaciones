using System;
using System.IO;
using EchoVocabulary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnityTest1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()   // Prueba con contenido normal 1
        {
            EchoMessage data = new EchoMessage("06/16/1998 9:15:45.345", "Test1 works!");
            BinaryEchoMessageCodec codification = new BinaryEchoMessageCodec();

            byte[] test1 = codification.Encode(data);
            MemoryStream ms = new MemoryStream(test1);

            EchoMessage decoded = codification.Decode(ms);

            Assert.IsTrue(data.Message.CompareTo(decoded.Message) == 0);
        }

        [TestMethod]
        public void TestMethod2()   // Prueba con contenido normal 2
        {
            EchoMessage data = new EchoMessage("06/05/1998 4:38:14.624", "Test2 also works!");
            BinaryEchoMessageCodec codification = new BinaryEchoMessageCodec();

            byte[] test1 = codification.Encode(data);
            MemoryStream ms = new MemoryStream(test1);

            EchoMessage decoded = codification.Decode(ms);

            Assert.IsTrue(data.Message.CompareTo(decoded.Message) == 0);
        }

        [TestMethod]
        public void TestMethod3()   // Prueba con mensaje de datos vacio
        {
            EchoMessage data = new EchoMessage("09/28/1996 4:38:14.624", "");
            BinaryEchoMessageCodec codification = new BinaryEchoMessageCodec();

            byte[] test1 = codification.Encode(data);
            MemoryStream ms = new MemoryStream(test1);

            EchoMessage decoded = codification.Decode(ms);

            Assert.IsTrue(data.Message.CompareTo(decoded.Message) == 0);
        }

        [TestMethod]
        public void TestMethod4()   // Prueba con formato de fecha distinto
        {
            EchoMessage data = new EchoMessage("3/19/1965", "Date shortcut?");
            BinaryEchoMessageCodec codification = new BinaryEchoMessageCodec();

            byte[] test1 = codification.Encode(data);
            MemoryStream ms = new MemoryStream(test1);

            EchoMessage decoded = codification.Decode(ms);

            Assert.IsTrue(data.Date.CompareTo(decoded.Date) == 0);
        }

        [TestMethod]
        public void TestMethod5()   // Prueba con mensaje de caracteres especiales
        {
            EchoMessage data = new EchoMessage("09/28/1996 4:38:14.624", "ñÓá#@€÷¬");
            BinaryEchoMessageCodec codification = new BinaryEchoMessageCodec();

            byte[] test1 = codification.Encode(data);
            MemoryStream ms = new MemoryStream(test1);

            EchoMessage decoded = codification.Decode(ms);

            Assert.IsTrue(data.Message.CompareTo(decoded.Message) == 0);
        }
    }
}





