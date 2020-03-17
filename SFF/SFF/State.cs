 using System;
using System.Collections.Generic;
using System.IO;
using SFFVocabulary;
using System.Net.Sockets;


namespace SFFVocabulary
{
    public abstract class State
    {

        protected abstract Packet Receive();
        protected delegate void PacketHandler(Packet packet);
        private Dictionary<PacketBodyType, PacketHandler> _map = new Dictionary<PacketBodyType, PacketHandler>();

        protected void RegisterHandler(PacketBodyType type, PacketHandler handler)
        {
            _map.Add(type, handler);
        }

        public virtual void HandleEvents()
        {
            try
            {
                Packet packet = Receive();

                PacketHandler handler = _map[packet.Type];
                handler.Invoke(packet);
            }
            catch (KeyNotFoundException e)
            {
                OnUnknownPacket(e);
            }
            catch (SocketException e)
            {
                if (e.SocketErrorCode == SocketError.TimedOut)
                    OnTimeOut();
                else if (e.SocketErrorCode == SocketError.ConnectionReset)
                    OnSocketClosed();
                else
                    OnSocketException(e);
            }
            catch (EndOfStreamException e)
            {
                OnCorruptPacket(e);
            }
            catch (Exception e)
            {
                OnUnknownException(e);
            }
        }

        protected virtual void OnUnknownPacket(Exception e) { }
        protected virtual void OnTimeOut() { }
        protected virtual void OnSocketClosed() { }
        protected virtual void OnSocketException(SocketException se) { }
        protected virtual void OnCorruptPacket(EndOfStreamException ese) { }
        protected virtual void OnUnknownException(Exception e) { }

    }
}
