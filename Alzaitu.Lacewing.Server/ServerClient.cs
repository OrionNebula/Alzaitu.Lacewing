using Alzaitu.Lacewing.Server.StateMachine;
using System.Net.Sockets;
using System.Threading;

namespace Alzaitu.Lacewing.Server
{
    public sealed class ServerClient
    {
        private readonly StateMachine<ServerClient> _stateMachine;

        internal Socket ClientSocket { get; }
        internal NetworkStream ClientStream { get; }

        public string Name { get; internal set; }

        public ClientFlags Flags { get; internal set; }

        public LacewingServer Server { get; }

        internal ServerClient(Socket socket, LacewingServer server)
        {
            Server = server;
            ClientSocket = socket;
            ClientStream = new NetworkStream(socket);
            _stateMachine = new StateMachine<ServerClient>(this)
            {
                CurrentState = new PreConnectState()
            };
        }

        internal void Run()
        {
            new Thread(StateThread).Start();
        }

        private void StateThread()
        {
            _stateMachine.Run();
            Disconnect();
        }

        internal Packet.Packet ReadPacket(bool initialByte = false) => Packet.Packet.ReadPacket(ClientStream, initialByte);

        internal void WritePacket(Packet.Packet packet) => packet.Write(ClientStream);

        internal void Disconnect()
        {
            ClientStream.Dispose();
            ClientSocket.Dispose();
        }
    }
}
