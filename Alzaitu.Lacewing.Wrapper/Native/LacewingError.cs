using System;
using System.Runtime.InteropServices;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal unsafe struct LacewingError : IDisposable
    {
        public uint Size => lw_error_size(ref this);

        public IntPtr Tag
        {
            get => new IntPtr(lw_error_tag(ref this));
            set => lw_error_set_tag(ref this, value.ToPointer());
        }

        public ref LacewingError Clone() => ref *lw_error_clone(ref this);

        public void Add(string format, params object[] args) => lw_error_addf(ref this, string.Format(format, args));

        public void Add(long errorCode) => lw_error_add(ref this, errorCode);

        public void Dispose() => lw_error_delete(ref this);

        public override string ToString() => Marshal.PtrToStringUTF8(lw_error_tostring(ref this));

        public static LacewingError* Create() => lw_error_new();

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern LacewingError* lw_error_new();

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void lw_error_delete(ref LacewingError error);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void lw_error_add(ref LacewingError error, long unknown);

        [DllImport("liblacewing.dll")]
        private static extern void lw_error_addf(ref LacewingError error, string format);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern uint lw_error_size(ref LacewingError error);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern IntPtr lw_error_tostring(ref LacewingError error);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern LacewingError* lw_error_clone(ref LacewingError error);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void* lw_error_tag(ref LacewingError error);

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private static extern void lw_error_set_tag(ref LacewingError error, void* tag);

        #endregion
    }
}
