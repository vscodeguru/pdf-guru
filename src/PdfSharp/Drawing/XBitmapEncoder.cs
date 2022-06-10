using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Internal;
#if CORE_WITH_GDI
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
#endif

namespace PdfSharp.Drawing
{
    public abstract class XBitmapEncoder
    {
        internal XBitmapEncoder()
        {
        }

        public static XBitmapEncoder GetPngEncoder()
        {
            return new XPngBitmapEncoder();
        }

        public XBitmapSource Source
        {
            get { return _source; }
            set { _source = value; }
        }
        XBitmapSource _source;

        public abstract void Save(Stream stream);
    }

    internal sealed class XPngBitmapEncoder : XBitmapEncoder
    {
        internal XPngBitmapEncoder()
        { }

        public override void Save(Stream stream)
        {
            if (Source == null)
                throw new InvalidOperationException("No image source.");
#if CORE_WITH_GDI || GDI
            if (Source.AssociatedGraphics != null)
            {
                Source.DisassociateWithGraphics();
                Debug.Assert(Source.AssociatedGraphics == null);
            }
            try
            {
                Lock.EnterGdiPlus();
                Source._gdiImage.Save(stream, ImageFormat.Png);
            }
            finally { Lock.ExitGdiPlus(); }
#endif
        }
    }
}
