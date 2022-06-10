

namespace PdfSharp.Drawing
{
    public class XBitmapDecoder
    {
        internal XBitmapDecoder()
        { }

        public static XBitmapDecoder GetPngDecoder()
        {
            return new XPngBitmapDecoder();
        }
    }

    internal sealed class XPngBitmapDecoder : XBitmapDecoder
    {
        internal XPngBitmapDecoder()
        { }
    }
}
