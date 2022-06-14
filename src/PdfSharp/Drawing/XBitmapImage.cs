

using System.Drawing;
using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    public sealed class XBitmapImage : XBitmapSource
    {
        internal XBitmapImage(int width, int height)
        {
#if GDI || CORE_WITH_GDI
            try
            {
                Lock.EnterGdiPlus();
                _gdiImage = new Bitmap(width, height);
            }
            finally { Lock.ExitGdiPlus(); }
#endif

#if CORE || GDI && !WPF     
            Initialize();
#endif
        }

        public static XBitmapSource CreateBitmap(int width, int height)
        {
            return new XBitmapImage(width, height);
        }
    }
}
