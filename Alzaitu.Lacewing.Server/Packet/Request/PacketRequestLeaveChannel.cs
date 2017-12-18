using System.IO;
using Alzaitu.Lacewing.Server.Packet.Serialization;

namespace Alzaitu.Lacewing.Server.Packet.Request
{
    [PacketType(0, 3, false, true)]
    internal class PacketRequestLeaveChannel : PacketRequest
    {
        [ProtocolPosition(0)]
        public short ChannelId { get; private set; }

        protected override void ReadImpl(BinaryReader rdr, long size) => ChannelId = rdr.ReadInt16();

        //public override long GetSize() => sizeof(short);
    }
}
