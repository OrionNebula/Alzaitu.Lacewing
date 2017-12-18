using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Alzaitu.Lacewing.Server
{
    public sealed class LacewingServer : IDisposable
    {
        public const int DEFAULT_PORT = 6121;

        /// <summary>
        /// Message of the day.
        /// </summary>
        public string Motd { get; set; } =
            $"Welcome! Server is running Alzaitu.Lacewing version {typeof(LacewingServer).Assembly.GetName().Version} ({RuntimeInformation.OSDescription} {Environment.Version})";

        public int Port { get; }

        public ServerClientCollection Clients { get; } = new ServerClientCollection();

        private readonly TcpListener _listener;

        public LacewingServer(int port = DEFAULT_PORT)
        {
            Port = port;
            _listener = TcpListener.Create(port);
        }

        public void Start()
        {
            _listener.Start();
            _listener.BeginAcceptSocket(ClientAcceptLoop, null);
        }

        private void ClientAcceptLoop(IAsyncResult result)
        {
            var socket = _listener.EndAcceptSocket(result);
            _listener.BeginAcceptSocket(ClientAcceptLoop, result.AsyncState);

            var client = new ServerClient(socket, this);
            Clients.Add(client);
            client.Run();
        }

        public void Dispose()
        {
            _listener.Stop();
        }
    }
}
