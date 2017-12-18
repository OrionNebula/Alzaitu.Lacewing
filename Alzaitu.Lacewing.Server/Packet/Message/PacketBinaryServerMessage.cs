using System.IO;

namespace Alzaitu.Lacewing.Server.Packet.Message
{
    [PacketType(1, true, true)]
    internal class PacketBinaryServerMessage : Packet
    {
        public byte SubChannel { get; set; }
        public byte[] Message { get; set; }

        protected override void WriteImpl(BinaryWriter wrt)
        {
            wrt.Write(SubChannel);
            wrt.Write(Message);
        }

        protected override void ReadImpl(BinaryReader rdr, long size)
        {
            SubChannel = rdr.ReadByte();
            Message = rdr.ReadBytes((int)size - sizeof(byte));
        }

        public override long GetSize() => sizeof(byte) + Message.LongLength;
    }
}
