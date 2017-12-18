using System.Runtime.InteropServices;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal unsafe struct LacewingEventPump
    {
        public void PostEventLoopExit() => lw_eventpump_post_eventloop_exit(ref this);

        public LacewingError* StartSleepyTicking(OnTickNeeded onTickNeeded) =>
            lw_eventpump_start_sleepy_ticking(ref this, onTickNeeded);

        public LacewingError* StartEventLoop() => lw_eventpump_start_eventloop(ref this);

        public LacewingError* Tick() => lw_eventpump_tick(ref this);

        public static LacewingEventPump* Create() => lw_eventpump_new();

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingEventPump* lw_eventpump_new();

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingError*  lw_eventpump_tick(ref LacewingEventPump eventPump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingError*  lw_eventpump_start_eventloop(ref LacewingEventPump eventPump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingError*  lw_eventpump_start_sleepy_ticking(ref LacewingEventPump eventPump, OnTickNeeded onTickNeeded);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_eventpump_post_eventloop_exit(ref LacewingEventPump eventPump);

        #endregion
    }

    internal delegate void OnTickNeeded(ref LacewingEventPump eventPump);
}
