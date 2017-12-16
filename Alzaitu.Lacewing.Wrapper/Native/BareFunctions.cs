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
        public static string Version => lw_version();

        #region Native

        [DllImport("liblacewing.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern string lw_version();

        #endregion
    }
}
