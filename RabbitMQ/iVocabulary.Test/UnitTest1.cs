using Microsoft.VisualStudio.TestTools.UnitTesting;
using iVocabulary;

namespace iVocabulary.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void textTest()
        {
            string text = "mensaje de prueba";
            int num = 1;
            Image image = new Image(text, num);

            BinaryImageCodec iCodec = new BinaryImageCodec();

            byte[] buffer = iCodec.Encode(image);

            Image proImage = iCodec.Decode(buffer);

            Assert.AreEqual(image.Text, proImage.Text);
        }


        [TestMethod]
        public void numTest()
        {
            string text = "mensaje de prueba";
            int num = 1;
            Image image = new Image(text, num);

            BinaryImageCodec iCodec = new BinaryImageCodec();

            byte[] buffer = iCodec.Encode(image);

            Image proImage = iCodec.Decode(buffer);

            Assert.AreEqual(image.Num, proImage.Num);
        }

        [TestMethod]
        public void emptyTextTest()
        {
            string text = "";
            int num = 1;
            Image image = new Image(text, num);

            BinaryImageCodec iCodec = new BinaryImageCodec();

            byte[] buffer = iCodec.Encode(image);

            Image proImage = iCodec.Decode(buffer);

            Assert.AreEqual(image.Text, proImage.Text);
        }

        [TestMethod]
        public void specialCharTest()
        {
            string text = "~ñπas∂€#ásé";
            int num = 1;
            Image image = new Image(text, num);

            BinaryImageCodec iCodec = new BinaryImageCodec();

            byte[] buffer = iCodec.Encode(image);

            Image proImage = iCodec.Decode(buffer);

            Assert.AreEqual(image.Text, proImage.Text);
        }

        [TestMethod]
        public void negativeNumTest()
        {
            string text = "prueba";
            int num = -1;
            Image image = new Image(text, num);

            BinaryImageCodec iCodec = new BinaryImageCodec();

            byte[] buffer = iCodec.Encode(image);

            Image proImage = iCodec.Decode(buffer);

            Assert.AreEqual(image.Num, proImage.Num);
        }


    }
}
