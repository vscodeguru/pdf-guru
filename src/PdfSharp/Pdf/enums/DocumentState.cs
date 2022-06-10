using System;

namespace PdfSharp.Pdf
{
    [Flags]
    enum DocumentState
    {
        Created = 0x0001,

        Imported = 0x0002,

        Disposed = 0x8000,
    }
}
