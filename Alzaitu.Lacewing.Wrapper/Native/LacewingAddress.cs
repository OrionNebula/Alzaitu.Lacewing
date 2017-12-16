using System;
using System.Runtime.InteropServices;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal unsafe struct LacewingAddress : IDisposable
    {
        public long Port
        {
            get => lw_addr_port(ref this);
            set => lw_addr_set_port(ref this, value);
        }

        public AddressType Type
        {
            get => lw_addr_type(ref this);
            set => lw_addr_set_type(ref this, value);
        }

        public IntPtr Tag
        {
            get => new IntPtr(lw_addr_tag(ref this));
            set => lw_addr_set_tag(ref this, value.ToPointer());
        }

        public bool IsIPv6 => lw_addr_ipv6(ref this);

        public bool IsReady() => lw_addr_ready(ref this);

        public ref LacewingError Resolve() => ref *lw_addr_resolve(ref this);

        public ref LacewingAddress Clone() => ref *lw_addr_clone(ref this);

        public override string ToString() => lw_addr_tostring(ref this);

        public bool Equals(ref LacewingAddress address) => lw_addr_equal(ref this, ref address);

        public void Dispose() => lw_addr_delete(ref this);

        public static ref LacewingAddress Create(string hostname, string service) => ref lw_addr_new(hostname, service);

        public static ref LacewingAddress Create(string hostname, long port) => ref lw_addr_new_port(hostname, port);

        public static ref LacewingAddress Create(string hostname, string service, AddressHint hints) => ref lw_addr_new_hint(hostname, service, hints);

        public static ref LacewingAddress Create(string hostname, long port, AddressHint hints) => ref lw_addr_new_port_hint(hostname, port, hints);

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern ref LacewingAddress lw_addr_new(string hostname, string service);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern ref LacewingAddress lw_addr_new_port(string hostname, long port);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern ref LacewingAddress lw_addr_new_hint(string hostname, string service, AddressHint hints);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern ref LacewingAddress lw_addr_new_port_hint(string hostname, long port, AddressHint hints);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern LacewingAddress* lw_addr_clone(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void lw_addr_delete(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern long lw_addr_port(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern long lw_addr_set_port(ref LacewingAddress address, long port);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AddressType lw_addr_type(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void lw_addr_set_type(ref LacewingAddress address, AddressType type);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern bool lw_addr_ready(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern LacewingError* lw_addr_resolve(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern bool lw_addr_ipv6(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern bool lw_addr_equal(ref LacewingAddress one, ref LacewingAddress two);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern string lw_addr_tostring(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void* lw_addr_tag(ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void lw_addr_set_tag(ref LacewingAddress address, void* tag);

        #endregion
    }

    internal enum AddressHint : long
    {
        Ipv6 = 4
    }

    internal enum AddressType : int
    {
        Tcp = 1,
        Udp = 2,
    }
}
