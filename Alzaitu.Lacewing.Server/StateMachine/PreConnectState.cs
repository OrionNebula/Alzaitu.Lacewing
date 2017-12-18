using Alzaitu.Lacewing.Server.Packet.Request;
using Alzaitu.Lacewing.Server.Packet.Response;

namespace Alzaitu.Lacewing.Server.StateMachine
{
    internal class PreConnectState : State<ServerClient>
    {
        public override State<ServerClient> StepForward(ServerClient context)
        {
            var packet = context.ReadPacket(true);
            switch (packet)
            {
                case PacketRequestConnect connectPacket:
                    if (connectPacket.Version != PacketRequestConnect.CURRENT_VERSION)
                    {
                        context.WritePacket(new PacketResponseConnect
                        {
                            Success = false,
                        });
                        return null;
                    }

                    context.WritePacket(new PacketResponseConnect
                    {
                        PeerId = 0,
                        Success = true,
                        WelcomeMessage = context.Server.Motd,
                    });
                    return new PostConnectState();
            }

            return null;
        }
    }
}
