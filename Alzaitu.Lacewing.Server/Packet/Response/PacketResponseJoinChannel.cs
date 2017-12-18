using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alzaitu.Lacewing.Server.Packet.Response
{
    internal class PacketResponseJoinChannel : PacketResponse
    {


        protected override void WriteResponse(BinaryWriter wrt)
        {
            throw new NotImplementedException();
        }

        public override long GetResponseSize()
        {
            throw new NotImplementedException();
        }
    }
}
