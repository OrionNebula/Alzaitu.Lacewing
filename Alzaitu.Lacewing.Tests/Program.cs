using System;
using Alzaitu.Lacewing.Wrapper.Native;

namespace Alzaitu.Lacewing.Tests
{
    public static class Program
    {
        public static unsafe void Main(string[] args)
        {
            Console.WriteLine("Version: {0}", BareFunctions.Version);

            var evt = LacewingEventPump.Create();
            var srv = LacewingServer.Create(ref *evt);

            srv->Host();

            evt->StartEventLoop();
        }
    }
}
