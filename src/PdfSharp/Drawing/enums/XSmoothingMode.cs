using System;

namespace PdfSharp.Drawing
{
    [Flags]
    public enum XSmoothingMode      
    {
        Invalid = -1,

        Default = 0,

        HighSpeed = 1,

        HighQuality = 2,

        None = 3,

        AntiAlias = 4,
    }
}
