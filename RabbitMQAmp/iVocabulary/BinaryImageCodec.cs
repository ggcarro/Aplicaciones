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
                writer.Write(image.Filename);
                writer.Write(image.Data.Length);
                writer.Write(image.Data);

                writer.Flush();

                buffer = stream.ToArray();
            }

            return buffer;

        }
        public Image Decode(byte[] buffer)
        {
            Image image = new Image();
            using (MemoryStream stream = new MemoryStream(buffer))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                string filename = reader.ReadString();
                int  dataLength = reader.ReadInt32();
                byte[] data = reader.ReadBytes(dataLength);


                image = new Image(filename, data);
            }
            
            return image;

        }

        
    }
}
