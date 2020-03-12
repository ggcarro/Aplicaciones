using System;

namespace SFFVocabulary
{
    public enum PacketBodyType
    {
        NewFile = 1,
        AckNewFile = 10,
        Data = 2,
        AckData = 20,
        Discon = 3,
        AckDiscon = 30
    }

    public class Packet  //Será necesario añadir el constructor de la clase y propiedades de acceso a los campos
    {
        private PacketBodyType _type;
        private int _bodyLength;
        private byte[] _body;
   
        public PacketBodyType Type
        {
            get => _type;
            set => _type = value;
        }

        public int BodyLength
        {
            get => _bodyLength;
            set => _bodyLength = value;
            
        }

        public byte[] Body {

            get => _body;
            set => _body = value;
        }

        public Packet(int new_bodyLength, byte[] new_body, int new_type)
        {
            _type = (PacketBodyType)new_type;
            _bodyLength = new_bodyLength;
            _body = new_body;
        }
    }
}
