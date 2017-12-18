using Alzaitu.Lacewing.Server.Packet.Request;
using Alzaitu.Lacewing.Server.Packet.Response;

namespace Alzaitu.Lacewing.Server.StateMachine
{
    internal class PostConnectState : State<ServerClient>
    {
        public override State<ServerClient> StepForward(ServerClient context)
        {
            var packet = context.ReadPacket();
            switch (packet)
            {
                case PacketRequestSetName requestSetName:
                    context.Name = requestSetName.Name;
                    context.WritePacket(new PacketResponseSetName
                    {
                        Success = true,
                        Name = context.Name,
                        Variant = requestSetName.Variant
                    });
                    return this;
                case PacketRequestJoinChannel requestJoinChannel:
                    break;
            }

            return null;
        }
    }
}
