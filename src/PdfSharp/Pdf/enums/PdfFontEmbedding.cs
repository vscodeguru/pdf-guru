using System;

namespace PdfSharp.Pdf
{
    public enum PdfFontEmbedding
    {
        Always,

        [Obsolete("Fonts must always be embedded.")]
        None,

        [Obsolete("Fonts must always be embedded.")]
        Default,

        [Obsolete("Fonts must always be embedded.")]
        Automatic,
    }
}
