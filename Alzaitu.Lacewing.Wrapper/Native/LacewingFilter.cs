using System;
using System.Runtime.InteropServices;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal unsafe struct LacewingFilter : IDisposable
    {
        public LacewingAddress* Remote
        {
            get => lw_filter_remote(ref this);
            set => lw_filter_set_remote(ref this, ref *value);
        }

        public LacewingAddress* Local
        {
            get => lw_filter_local(ref this);
            set => lw_filter_set_local(ref this, ref *value);
        }

        public bool Reuse
        {
            get => lw_filter_reuse(ref this);
            set => lw_filter_set_reuse(ref this, value);
        }

        public bool IPv6
        {
            get => lw_filter_ipv6(ref this);
            set => lw_filter_set_ipv6(ref this, value);
        }

        public IntPtr Tag
        {
            get => new IntPtr(lw_filter_tag(ref this));
            set => lw_filter_set_tag(ref this, value.ToPointer());
        }

        public LacewingFilter* Clone() => lw_filter_clone(ref this);

        public void Dispose() => lw_filter_delete(ref this);

        public static LacewingFilter* Create() => lw_filter_new();

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingFilter* lw_filter_new();

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_delete(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingFilter* lw_filter_clone(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingAddress* lw_filter_remote(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_set_remote(ref LacewingFilter filter, ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingAddress* lw_filter_local(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_set_local(ref LacewingFilter filter, ref LacewingAddress address);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern long lw_filter_local_port(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_set_local_port(ref LacewingFilter filter, long port);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern long lw_filter_remote_port(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_set_remote_port(ref LacewingFilter filter, long port);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool lw_filter_reuse(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_set_reuse(ref LacewingFilter filter, bool reuse);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool lw_filter_ipv6(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_set_ipv6(ref LacewingFilter filter, bool isIpv6);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void* lw_filter_tag(ref LacewingFilter filter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_filter_set_tag(ref LacewingFilter filter, void* tag);

        #endregion
    }
}
