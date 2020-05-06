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
            
            MemoryStream stream = image.MatData.ToMemoryStream();
            buffer = stream.ToArray();
            
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
                Mat mat = Cv2.ImDecode(data, ImreadModes.Color);

                image = new Image(mat);
            }
            
            return image;

        }

        
    }
}
