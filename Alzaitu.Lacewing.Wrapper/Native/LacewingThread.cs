using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal unsafe struct LacewingThread : IDisposable
    {
        public bool Started => lw_thread_started(ref this);

        public IntPtr Tag
        {
            get => lw_thread_tag(ref this);
            set => lw_thread_set_tag(ref this, value);
        }

        public void* Join() => lw_thread_join(ref this);

        public void Start() => lw_thread_start(ref this, null);

        public void Dispose() => lw_thread_delete(ref this);

        public static LacewingThread* Create(string name, void* proc) => lw_thread_new(name, proc);

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingThread* lw_thread_new(string name, void* proc);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_thread_delete(ref LacewingThread thread);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_thread_start(ref LacewingThread thread, void* parameter);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool lw_thread_started(ref LacewingThread thread);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void* lw_thread_join(ref LacewingThread thread);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr lw_thread_tag(ref LacewingThread thread);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_thread_set_tag(ref LacewingThread thread, IntPtr tag);

        #endregion
    }
}
