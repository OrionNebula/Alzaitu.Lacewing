using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal unsafe struct LacewingPump : IDisposable
    {
        public bool InUse => lw_pump_in_use(ref this);

        public IntPtr Tag
        {
            get => lw_pump_tag(ref this);
            set => lw_pump_set_tag(ref this, value);
        }

        public void Post(void* fn, void* param) => lw_pump_post(ref this, fn, param);

        public void PostRemove(ref LacewingPumpWatch watch) => lw_pump_post_remove(ref this, ref watch);

        public void Remove(ref LacewingPumpWatch watch) => lw_pump_remove(ref this, ref watch);

        public void RemoveUser() => lw_pump_remove_user(ref this);

        public void AddUser() => lw_pump_add_user(ref this);

        //This segment is too advanced to be made cross-platform easily. Use LacewingEventPump
        /*
        //TODO: Add way to specify callback
        public LacewingPumpWatch* Add(string path, IntPtr tag) => RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? Add(File.OpenRead(path).SafeFileHandle, tag, null)
            : Add(fileno(fopen(path, "rwb")), tag, null, null, false); //TODO: This last bit here is really a guess as to what I think this should be doing on non-Windows machines.

        private LacewingPumpWatch* Add(SafeHandle fileHandle, IntPtr tag, lw_pump_callback_windows callback) =>
            fileHandle.IsInvalid
                ? throw new ArgumentException("Must pass a valid file handle")
                : lw_pump_add(ref this, fileHandle.DangerousGetHandle(), tag, callback);

        private LacewingPumpWatch* Add(int fileDescriptor, IntPtr tag, lw_pump_callback onReadReady,
            lw_pump_callback onWriteReady, bool edgeTriggered) => lw_pump_add(ref this, fileDescriptor, tag,
            onReadReady, onWriteReady, edgeTriggered);*/


        public void Dispose() => lw_pump_delete(ref this);

        public static LacewingPump* Create(ref LacewingEventPump eventPump) => lw_pump_new(ref eventPump);

        #region Native

        /*[DllImport("libc.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int fileno(IntPtr stream);

        [DllImport("libc.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr fopen(string path, string mode);*/

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_pump_delete(ref LacewingPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_pump_add_user(ref LacewingPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_pump_remove_user(ref LacewingPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern bool  lw_pump_in_use(ref LacewingPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_pump_remove(ref LacewingPump pump, ref LacewingPumpWatch watch);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_pump_post_remove(ref LacewingPump pump, ref LacewingPumpWatch watch);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_pump_post(ref LacewingPump pump, void* fn, void* param);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr lw_pump_tag(ref LacewingPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void lw_pump_set_tag(ref LacewingPump pump, IntPtr tag);

        /*private delegate void lw_pump_callback_windows(IntPtr tag, OVERLAPPED* overlapped, ulong bytes, int error);
        private delegate void lw_pump_callback(IntPtr tag);*/

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingPump* lw_pump_new(ref LacewingEventPump pumpdef);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingEventPump* lw_pump_get_def (ref LacewingPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void* lw_pump_tail(ref LacewingPump pump);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingPump* lw_pump_from_tail(void* tail);

        /*[DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingPumpWatch* lw_pump_add(ref LacewingPump pump, IntPtr handle, IntPtr tag, lw_pump_callback_windows callback);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingPumpWatch* lw_pump_update_callbacks(ref LacewingPump pump, ref LacewingPumpWatch pumpWatch, IntPtr tag, lw_pump_callback_windows callback);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingPumpWatch* lw_pump_add(ref LacewingPump pump, int fd, IntPtr tag, lw_pump_callback onReadReady, lw_pump_callback onWriteReady, bool edgeTriggered);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern LacewingPumpWatch* lw_pump_update_callbacks(ref LacewingPump pump, ref LacewingPumpWatch pumpWatch, IntPtr tag, lw_pump_callback onReadReady, lw_pump_callback onWriteReady, bool edgeTriggered);

        [StructLayout(LayoutKind.Explicit, Size = 20)]
        public struct OVERLAPPED
        {
            [FieldOffset(0)]
            public uint Internal;

            [FieldOffset(4)]
            public uint InternalHigh;

            [FieldOffset(8)]
            public uint Offset;

            [FieldOffset(12)]
            public uint OffsetHigh;

            [FieldOffset(8)]
            public IntPtr Pointer;

            [FieldOffset(16)]
            public IntPtr hEvent;
        }*/

        #endregion

    }
}
