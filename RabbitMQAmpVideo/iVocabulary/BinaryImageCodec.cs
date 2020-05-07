using System;
using System.IO;
using System.Text;
using OpenCvSharp;

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
                image.Mat.ImWrite("temp.jpg");
                byte[] data = File.ReadAllBytes("temp.jpg");
                writer.Write(data.Length);
                writer.Write(data);
                writer.Write(image.Seq);
                writer.Write(image.SleepTime);

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
                int  dataLength = reader.ReadInt32();
                byte[] data = reader.ReadBytes(dataLength);
                int seq = reader.ReadInt32();
                int sleepTime = reader.ReadInt32();
                File.WriteAllBytes("temp2.jpg", data);
                Mat mat = new Mat("temp2.jpg");
                image = new Image(mat, seq, sleepTime);
            }
            
            return image;

        }

        
    }
}
