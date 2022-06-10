using System;
using System.Threading;

namespace PdfSharp.Internal
{
    internal static class Lock
    {
        public static void EnterGdiPlus()
        {
            Monitor.Enter(GdiPlus);
            _gdiPlusLockCount++;
        }

        public static void ExitGdiPlus()
        {
            _gdiPlusLockCount--;
            Monitor.Exit(GdiPlus);
        }

        static readonly object GdiPlus = new object();
        static int _gdiPlusLockCount;

        public static void EnterFontFactory()
        {
            Monitor.Enter(FontFactory);
            _fontFactoryLockCount++;
        }

        public static void ExitFontFactory()
        {
            _fontFactoryLockCount--;
            Monitor.Exit(FontFactory);
        }
        static readonly object FontFactory = new object();
        [ThreadStatic]
        static int _fontFactoryLockCount;
    }
}
