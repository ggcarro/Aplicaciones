using System;
using System.IO;

namespace iVocabulary
{
    public class BinaryImageCodec : ICodec
    {
        public byte[] Encode(Image image)
        {
            byte[] buffer;
            MemoryStream ms = new MemoryStream();
            BinaryWriter wr = new BinaryWriter(ms);

            wr.Write(image.Text);
            wr.Write(image.Num);

            wr.Flush();

            buffer = ms.ToArray();

            return buffer;

        }
        public Image Decode(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream();
            BinaryReader br = new BinaryReader(ms);

            string text = br.ReadString();
            int num = br.ReadInt32();

            Image image = new Image(text, num);
            return image;

        }

        
    }
}
