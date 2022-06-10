using System;

namespace PdfSharp.Pdf.IO
{
    [Flags]
    internal enum PdfWriterOptions
    {
        Regular = 0x000000,

        OmitStream = 0x000001,

        OmitInflation = 0x000002,
    }
}
