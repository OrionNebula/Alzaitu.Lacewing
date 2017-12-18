using System.IO;
using System.Text;
using Alzaitu.Lacewing.Server.Packet.Serialization;

namespace Alzaitu.Lacewing.Server.Packet.Response
{
    [PacketType(0, 1, true, false)]
    internal class PacketResponseSetName : PacketResponse
    {
        public const string DENY_REASON = "Name was not accepted.";

        [ProtocolPosition(0, emitLengthPrefix: true)]
        public string Name { get; set; }

        [ProtocolPosition(1, emitOnSuccess: false)]
        public string DenyReason { get; set; }
        
        protected override void WriteResponse(BinaryWriter wrt)
        {
            wrt.Write((byte) Encoding.UTF8.GetByteCount(Name));
            wrt.Write(Encoding.UTF8.GetBytes(Name));
            if(!Success)
                wrt.Write(Encoding.UTF8.GetBytes(DENY_REASON));
        }

        public override long GetResponseSize() => sizeof(byte) +
                                          Encoding.UTF8.GetByteCount(Name) +
                                          (!Success ? Encoding.UTF8.GetByteCount(DENY_REASON) : 0);
    }
}
