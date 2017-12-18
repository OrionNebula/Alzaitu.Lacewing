using System;
using System.IO;
using System.Text;

namespace Alzaitu.Lacewing.Server.Packet.Request
{
    [PacketType(0, 0, false, true)]
    internal class PacketRequestConnect : PacketRequest
    {
        public const string CURRENT_VERSION = "revision 3";

        public string Version { get; private set; }

        protected override void ReadImpl(BinaryReader rdr, long size)
        {
            Version = size == 0 ? null : Encoding.UTF8.GetString(rdr.ReadBytes((int)size));
        }

        public override long GetSize() => Version == null ? 0 : Encoding.UTF8.GetByteCount(Version);
    }
}
