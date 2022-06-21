using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    public abstract class XBitmapSource : XImage
    {
        public override int PixelWidth
        {
            get
            {
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width;
                }
                finally { Lock.ExitGdiPlus(); }
            }
        }

        public override int PixelHeight
        {
            get
            {
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height;
                }
                finally { Lock.ExitGdiPlus(); }
            }
        }
    }
}
