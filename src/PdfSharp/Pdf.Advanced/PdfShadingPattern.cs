using System;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Pdf;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfShadingPattern : PdfDictionaryWithContentStream
    {
        public PdfShadingPattern(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Pattern");
            Elements[Keys.PatternType] = new PdfInteger(2);
        }

        internal void SetupFromBrush(XLinearGradientBrush brush, XMatrix matrix, XGraphicsPdfRenderer renderer)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            PdfShading shading = new PdfShading(_document);
            shading.SetupFromBrush(brush, renderer);
            Elements[Keys.Shading] = shading;
            Elements.SetMatrix(Keys.Matrix, matrix);
        }

        internal sealed new class Keys : PdfDictionaryWithContentStream.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string PatternType = "/PatternType";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string Shading = "/Shading";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Matrix = "/Matrix";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string ExtGState = "/ExtGState";

            internal static DictionaryMeta Meta
            {
                get { return _meta ?? (_meta = CreateMeta(typeof(Keys))); }
            }
            static DictionaryMeta _meta;
        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
