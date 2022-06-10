using System;

namespace PdfSharp.Drawing
{
    [Flags]
    public enum XFontStyle      
    {
        Regular = XGdiFontStyle.Regular,

        Bold = XGdiFontStyle.Bold,

        Italic = XGdiFontStyle.Italic,

        BoldItalic = XGdiFontStyle.BoldItalic,

        Underline = XGdiFontStyle.Underline,

        Strikeout = XGdiFontStyle.Strikeout,

    }
}
