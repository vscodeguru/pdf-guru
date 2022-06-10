namespace PdfSharp.Pdf.Annotations
{
    [System.Flags]
    public enum PdfAnnotationFlags
    {
        Invisible = 1 << (1 - 1),

        Hidden = 1 << (2 - 1),

        Print = 1 << (3 - 1),

        NoZoom = 1 << (4 - 1),

        NoRotate = 1 << (5 - 1),

        NoView = 1 << (6 - 1),

        ReadOnly = 1 << (7 - 1),

        Locked = 1 << (8 - 1),

        ToggleNoView = 1 << (9 - 1),
    }
}
