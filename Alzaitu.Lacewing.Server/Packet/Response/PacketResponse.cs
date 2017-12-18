using System.IO;

namespace Alzaitu.Lacewing.Server.Packet.Response
{
    internal abstract class PacketResponse : Packet
    {
        public bool Success { get; set; }

        protected sealed override void ReadImpl(BinaryReader rdr, long size) => base.ReadImpl(rdr, size);

        protected sealed override void WriteImpl(BinaryWriter wrt)
        {
            wrt.Write(Success);
            WriteResponse(wrt);
        }

        protected abstract void WriteResponse(BinaryWriter wrt);

        public sealed override long GetSize()
        {
            return sizeof(bool) + GetResponseSize();
        }

        public abstract long GetResponseSize();
    }
}
