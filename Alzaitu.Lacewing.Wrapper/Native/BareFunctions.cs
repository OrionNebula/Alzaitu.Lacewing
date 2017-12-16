using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alzaitu.Lacewing.Wrapper.Native
{
    internal static unsafe class BareFunctions
    {
        /// <summary>
        /// Version of the liblacewing library.
        /// </summary>
        public static string Version => Marshal.PtrToStringUTF8(lw_version());

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern IntPtr lw_version();

        #endregion
    }
}
