using System;

namespace PdfSharp.Pdf
{
    public enum PdfFontEncoding
    {
        WinAnsi = 0,

        Unicode = 1,

        [Obsolete("Use WinAnsi or Unicode")]
        Automatic = 1,      

    }
}
