using System;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Internal
{
    static class ColorSpaceHelper
    {
        public static XColor EnsureColorMode(PdfColorMode colorMode, XColor color)
        {
#if true
            if (colorMode == PdfColorMode.Rgb && color.ColorSpace != XColorSpace.Rgb)
                return XColor.FromArgb((int)(color.A * 255), color.R, color.G, color.B);

            if (colorMode == PdfColorMode.Cmyk && color.ColorSpace != XColorSpace.Cmyk)
                return XColor.FromCmyk(color.A, color.C, color.M, color.Y, color.K);

            return color;

#endif
        }

        public static XColor EnsureColorMode(PdfDocument document, XColor color)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            return EnsureColorMode(document.Options.ColorMode, color);
        }

        public static bool IsEqualCmyk(XColor x, XColor y)
        {
            if (x.ColorSpace != XColorSpace.Cmyk || y.ColorSpace != XColorSpace.Cmyk)
                return false;
            return x.C == y.C && x.M == y.M && x.Y == y.Y && x.K == y.K;
        }
    }
}