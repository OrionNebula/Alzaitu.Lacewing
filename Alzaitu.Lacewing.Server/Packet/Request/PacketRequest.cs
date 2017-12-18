using System.IO;

namespace Alzaitu.Lacewing.Server.Packet.Request
{
    internal abstract class PacketRequest : Packet
    {
        protected abstract override void ReadImpl(BinaryReader rdr, long size);

        protected sealed override void WriteImpl(BinaryWriter wrt) => base.WriteImpl(wrt);
    }
}
