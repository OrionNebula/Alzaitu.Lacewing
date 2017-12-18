using System;
using Alzaitu.Lacewing.Server;

namespace Alzaitu.Lacewing.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var srv = new LacewingServer
            {
            })
            {
                srv.Start();
                while (true) ;
            }
        }
    }
}
