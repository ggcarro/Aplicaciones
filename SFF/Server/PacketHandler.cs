using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class PacketHandler
    {
        private Dictionary<SFFVocabulary.PacketBodyType, IHandler> _map = new Dictionary<SFFVocabulary.PacketBodyType, IHandler>();


        PacketHandler()
        {
            _map.Add(SFFVocabulary.PacketBodyType.NewFile, NewFileHandler());
            _map.Add(SFFVocabulary.PacketBodyType.Data, DataHandler());
            _map.Add(SFFVocabulary.PacketBodyType.Discon, DisconHandler());
        }


        public void HandleMessage(SFFVocabulary.PacketBodyType type, string arguments)
        {
            try
            {
                IHandler handler = _map[type];
                handler.HandleMessage(arguments);
            }
            catch (KeyNotFoundException)
            {
                // Gestionar mensaje desconocido
            }
        }

    }
}
