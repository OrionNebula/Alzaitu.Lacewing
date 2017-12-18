using System.IO;

namespace Alzaitu.Lacewing.Server.Packet.Request
{
    [PacketType(0, 3)]
    internal class PacketRequestLeaveChannel : PacketRequest
    {
        public short ChannelId { get; private set; }

        protected override void ReadImpl(BinaryReader rdr, long size) => ChannelId = rdr.ReadInt16();

        public override long GetSize() => sizeof(short);
    }
}
