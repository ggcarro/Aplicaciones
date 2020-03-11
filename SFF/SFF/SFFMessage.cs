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
        private int _bodyLenght;
        private byte[] _body;
   
        public PacketBodyType Type
        {
            get => _type;
            set => _type = value;
        }

        public int BodyLenght
        {
            get => _bodyLenght;
            set => _bodyLenght = value;
            
        }

        public byte[] Body {

            get => _body;
            set => _body = value;
        }

        public Packet(int new_type, int new_bodyLenght, byte[] new_body)
        {
            _type = (PacketBodyType)new_type;
            _bodyLenght = new_bodyLenght;
            _body = new_body;
        }
    }
}
