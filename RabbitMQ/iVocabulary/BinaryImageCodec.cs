using System;
using System.IO;
using System.Text;

namespace iVocabulary
{
    public class BinaryImageCodec : ICodec
    {
        public byte[] Encode(Image image)
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(image.Text);
                writer.Write(image.Num);

                writer.Flush();

                buffer = stream.ToArray();
            }

            return buffer;

        }
        public Image Decode(byte[] buffer)
        {
            Image image = new Image("", -1);
            using (MemoryStream stream = new MemoryStream(buffer))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                string te = reader.ReadString();
                int num = reader.ReadInt32();

                image = new Image(te, num);
            }
            
            return image;

        }

        
    }
}
