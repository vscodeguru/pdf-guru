using System.Drawing;
using GdiFont = System.Drawing.Font;

namespace PdfSharp.Fonts
{
    internal class PlatformFontResolverInfo : FontResolverInfo
    {
        public PlatformFontResolverInfo(string faceName, bool mustSimulateBold, bool mustSimulateItalic, GdiFont gdiFont)
            : base(faceName, mustSimulateBold, mustSimulateItalic)
        {
            _gdiFont = gdiFont;
        }
        public Font GdiFont
        {
            get { return _gdiFont; }
        }
        readonly Font _gdiFont;
    }
}
