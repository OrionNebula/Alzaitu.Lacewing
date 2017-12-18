using System;
using System.IO;
using System.Text;
using Alzaitu.Lacewing.Server.Packet.Serialization;

namespace Alzaitu.Lacewing.Server.Packet.Response
{
    [PacketType(0, 0, true, false)]
    internal class PacketResponseConnect : PacketResponse
    {
        public const string DENY_REASON = "Connection was not allowed.";

        [ProtocolPosition(0, emitOnFailure: false)]
        public short PeerId { get; set; }
        
        [ProtocolPosition(1, emitOnFailure: false)]
        public string WelcomeMessage { get; set; }

        [ProtocolPosition(0, emitOnSuccess: false)]
        public string DenyReason { get; set; }

        protected override void WriteResponse(BinaryWriter wrt)
        {
            if (!Success)
                wrt.Write(Encoding.UTF8.GetBytes(DENY_REASON));
            else
            {
                wrt.Write(PeerId);
                wrt.Write(Encoding.UTF8.GetBytes(WelcomeMessage));
            }
        }

        public override long GetResponseSize() => !Success
                                              ? Encoding.UTF8.GetByteCount(DENY_REASON)
                                              : sizeof(short) + Encoding.UTF8.GetByteCount(WelcomeMessage);
    }
}
