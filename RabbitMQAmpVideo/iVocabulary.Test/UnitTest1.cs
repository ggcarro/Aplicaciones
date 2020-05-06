using Microsoft.VisualStudio.TestTools.UnitTesting;
using iVocabulary;
using OpenCvSharp;
using System.IO;
using System.Linq;

namespace iVocabulary.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void matTest()
        {
            string text = @"..\..\..\Images\input.jpg";
            int seq = 56;
            var mat = new Mat(text);

            Cv2.ImWrite("temp.jpg", mat);
            byte[] data = File.ReadAllBytes("temp.jpg");
            byte[] data2 = File.ReadAllBytes(text);

            /*Image image = new Image(mat, seq);

            BinaryImageCodec iCodec = new BinaryImageCodec();

            byte[] buffer = iCodec.Encode(image);

            Image proImage = iCodec.Decode(buffer);*/

            CollectionAssert.AreEqual(data2, data2);
        }

        [TestMethod]
        public void seqTest()
        {
            string text = @"..\..\..\Images\input.jpg";
            int seq = 56;
            Mat mat = new Mat(text);

            Image image = new Image(mat, seq);

            BinaryImageCodec iCodec = new BinaryImageCodec();

            byte[] buffer = iCodec.Encode(image);

            Image proImage = iCodec.Decode(buffer);
            Assert.AreEqual(image.Seq, proImage.Seq);
        }

    }
}
