#region PDFsharp - A .NET library for processing PDF
#endregion

using PdfSharp.Drawing;

namespace PdfSharp.Internal
{
#if GDI && WPF
    /// <summary>
    /// Internal switch indicating what context has to be used if both GDI and WPF are defined.
    /// </summary>
    static class TargetContextHelper
    {
        public static XGraphicTargetContext TargetContext = XGraphicTargetContext.WPF;
    }
#endif
}