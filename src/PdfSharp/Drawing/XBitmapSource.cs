
using System.Diagnostics;
using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    public abstract class XBitmapSource : XImage
    {
        public override int PixelWidth
        {
            get
            {
#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public override int PixelHeight
        {
            get
            {
#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }
    }
}
