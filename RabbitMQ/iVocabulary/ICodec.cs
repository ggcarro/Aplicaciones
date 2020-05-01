using System;
namespace iVocabulary
{
    public interface ICodec
    {
        byte[] Encode(Image image);
        Image Decode(byte[] bytes);
    }
}
