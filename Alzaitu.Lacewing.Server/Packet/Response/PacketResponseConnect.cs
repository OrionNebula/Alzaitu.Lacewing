using System;
using System.IO;
using System.Text;

namespace Alzaitu.Lacewing.Server.Packet.Response
{
    [PacketType(0, 0, true, false)]
    internal class PacketResponseConnect : PacketResponse
    {
        public const string DENY_REASON = "Connection was not allowed.";

        public short PeerId { get; set; }
        
        public string WelcomeMessage { get; set; }

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
