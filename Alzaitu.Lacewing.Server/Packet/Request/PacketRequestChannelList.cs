using System.IO;

namespace Alzaitu.Lacewing.Server.Packet.Request
{
    [PacketType(0, 4)]
    internal class PacketRequestChannelList : PacketRequest
    {
        protected override void ReadImpl(BinaryReader rdr, long size) { }

        public override long GetSize() => 0;
    }
}
