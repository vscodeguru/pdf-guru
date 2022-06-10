using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSharp.Drawing
{
    [Flags]
    internal enum XGdiFontStyle      
    {
        Regular = 0,

        Bold = 1,

        Italic = 2,

        BoldItalic = 3,

        Underline = 4,

        Strikeout = 8,
    }
}
