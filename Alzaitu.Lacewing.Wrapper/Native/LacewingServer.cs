using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal unsafe struct LacewingServer : IDisposable
    {
        public const long DEFAULT_PORT = 6121;

        public bool Hosting => lw_server_hosting(ref this);

        public long Port => lw_server_port(ref this);

        public bool CertLoaded => lw_server_cert_loaded(ref this);

        public bool CanNpn => lw_server_can_npn(ref this);

        public uint ClientCount => lw_server_num_clients(ref this);

        public IntPtr Tag
        {
            get => lw_server_tag(ref this);
            set => lw_server_set_tag(ref this, value);
        }

        public void AddNpn(string protocol) => lw_server_add_npn(ref this, protocol);

        public bool LoadSysCert(string storeName, string commonName, string location) =>
            lw_server_load_sys_cert(ref this, storeName, commonName, location);

        public bool LoadCertFile(string filename, string passphrase) =>
            lw_server_load_cert_file(ref this, filename, passphrase);

        public void Unhost() => lw_server_unhost(ref this);

        public void HostFilter(ref LacewingFilter filter) => lw_server_host_filter(ref this, ref filter);

        public void Host(long port = DEFAULT_PORT) => lw_server_host(ref this, port);

        public void Dispose() => lw_server_delete(ref this);

        public static LacewingServer* Create(ref LacewingEventPump pump) => lw_server_new(ref pump);

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingServer*  lw_server_new(ref LacewingEventPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_server_delete(ref LacewingServer server);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_server_host(ref LacewingServer server, long port);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_server_host_filter(ref LacewingServer server, ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_server_unhost(ref LacewingServer server);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool lw_server_hosting(ref LacewingServer server);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern long lw_server_port(ref LacewingServer server);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool  lw_server_load_cert_file(ref LacewingServer server, string filename, string passphrase);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool  lw_server_load_sys_cert(ref LacewingServer server, string store_name, string common_name, string location);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool  lw_server_cert_loaded(ref LacewingServer server);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool  lw_server_can_npn(ref LacewingServer server);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_server_add_npn(ref LacewingServer server, string protocol);

        //        private static extern const char* lw_server_client_npn(lw_server_client);
        //private static extern LacewingAddress*  lw_server_client_addr(lw_server_client);
        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern uint  lw_server_num_clients(ref LacewingServer server);
        //private static extern lw_server_client  lw_server_client_first(ref LacewingServer server);
        //private static extern lw_server_client  lw_server_client_next(lw_server_client);
        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr lw_server_tag(ref LacewingServer server);
        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_server_set_tag(ref LacewingServer server, IntPtr tag);

        #endregion

    }
}
