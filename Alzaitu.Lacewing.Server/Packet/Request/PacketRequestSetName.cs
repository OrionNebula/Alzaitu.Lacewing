using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Alzaitu.Lacewing.Server.Packet.Serialization;

namespace Alzaitu.Lacewing.Server.Packet.Request
{
    [PacketType(0, 1, false, true)]
    internal class PacketRequestSetName : PacketRequest
    {
        [ProtocolPosition(0)]
        public string Name { get; private set; }

        protected override void ReadImpl(BinaryReader rdr, long size) => Name = Encoding.UTF8.GetString(rdr.ReadBytes((int)size));

        //public override long GetSize() => Encoding.UTF8.GetByteCount(Name);
    }
}
