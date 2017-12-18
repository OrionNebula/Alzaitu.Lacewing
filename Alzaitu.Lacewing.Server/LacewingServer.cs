namespace Alzaitu.Lacewing.Server
{
    public class LacewingServer
    {
        public const int DEFAULT_PORT = 6121;

        /// <summary>
        /// Message of the day.
        /// </summary>
        public string Motd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; }

        public LacewingServer(int port = DEFAULT_PORT)
        {
            Port = port;
        }
    }
}
