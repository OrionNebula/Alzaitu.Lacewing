using System;
using System.IO;
using System.Text;

namespace Alzaitu.Lacewing.Server.Packet.Request
{
    [PacketType(0, 2)]
    internal class PacketRequestJoinChannel : PacketRequest
    {
        public ChannelJoinFlags Flags { get; private set; }

        public string ChannelName { get; private set; }

        protected override void ReadImpl(BinaryReader rdr, long size)
        {
            Flags = (ChannelJoinFlags) rdr.ReadByte();
            ChannelName = Encoding.UTF8.GetString(rdr.ReadBytes((int) size - sizeof(byte)));
        }

        public override long GetSize() => sizeof(byte) + Encoding.UTF8.GetByteCount(ChannelName);

        [Flags]
        public enum ChannelJoinFlags : byte
        {
            /// <summary>
            /// If the user is creating this channel, hide it from the channel list.
            /// </summary>
            HideIfCreating = 1,
            /// <summary>
            /// If the user is creating this channel, close it when they leave.
            /// </summary>
            CloseOnExit = 2
        }
    }
}
