
using System.Drawing;
using GdiFont = System.Drawing.Font;

namespace PdfSharp.Fonts
{
    internal class PlatformFontResolverInfo : FontResolverInfo
    {
#if CORE || GDI
        public PlatformFontResolverInfo(string faceName, bool mustSimulateBold, bool mustSimulateItalic, GdiFont gdiFont)
            : base(faceName, mustSimulateBold, mustSimulateItalic)
        {
            _gdiFont = gdiFont;
        }
#endif

#if CORE || GDI
        public Font GdiFont
        {
            get { return _gdiFont; }
        }
        readonly Font _gdiFont;
#endif
    }
}
