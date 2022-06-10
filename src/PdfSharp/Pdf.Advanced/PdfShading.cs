using System;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Pdf;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfShading : PdfDictionary
    {
        public PdfShading(PdfDocument document)
            : base(document)
        { }

        internal void SetupFromBrush(XLinearGradientBrush brush, XGraphicsPdfRenderer renderer)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            PdfColorMode colorMode = _document.Options.ColorMode;
            XColor color1 = ColorSpaceHelper.EnsureColorMode(colorMode, brush._color1);
            XColor color2 = ColorSpaceHelper.EnsureColorMode(colorMode, brush._color2);

            PdfDictionary function = new PdfDictionary();

            Elements[Keys.ShadingType] = new PdfInteger(2);
            if (colorMode != PdfColorMode.Cmyk)
                Elements[Keys.ColorSpace] = new PdfName("/DeviceRGB");
            else
                Elements[Keys.ColorSpace] = new PdfName("/DeviceCMYK");

            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (brush._useRect)
            {
                XPoint pt1 = renderer.WorldToView(brush._rect.TopLeft);
                XPoint pt2 = renderer.WorldToView(brush._rect.BottomRight);

                switch (brush._linearGradientMode)
                {
                    case XLinearGradientMode.Horizontal:
                        x1 = pt1.X;
                        y1 = pt1.Y;
                        x2 = pt2.X;
                        y2 = pt1.Y;
                        break;

                    case XLinearGradientMode.Vertical:
                        x1 = pt1.X;
                        y1 = pt1.Y;
                        x2 = pt1.X;
                        y2 = pt2.Y;
                        break;

                    case XLinearGradientMode.ForwardDiagonal:
                        x1 = pt1.X;
                        y1 = pt1.Y;
                        x2 = pt2.X;
                        y2 = pt2.Y;
                        break;

                    case XLinearGradientMode.BackwardDiagonal:
                        x1 = pt2.X;
                        y1 = pt1.Y;
                        x2 = pt1.X;
                        y2 = pt2.Y;
                        break;
                }
            }
            else
            {
                XPoint pt1 = renderer.WorldToView(brush._point1);
                XPoint pt2 = renderer.WorldToView(brush._point2);

                x1 = pt1.X;
                y1 = pt1.Y;
                x2 = pt2.X;
                y2 = pt2.Y;
            }

            const string format = Config.SignificantFigures3;
            Elements[Keys.Coords] = new PdfLiteral("[{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "}]", x1, y1, x2, y2);

            Elements[Keys.Function] = function;
            string clr1 = "[" + PdfEncoders.ToString(color1, colorMode) + "]";
            string clr2 = "[" + PdfEncoders.ToString(color2, colorMode) + "]";

            function.Elements["/FunctionType"] = new PdfInteger(2);
            function.Elements["/C0"] = new PdfLiteral(clr1);
            function.Elements["/C1"] = new PdfLiteral(clr2);
            function.Elements["/Domain"] = new PdfLiteral("[0 1]");
            function.Elements["/N"] = new PdfInteger(1);
        }

        internal sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string ShadingType = "/ShadingType";

            [KeyInfo(KeyType.NameOrArray | KeyType.Required)]
            public const string ColorSpace = "/ColorSpace";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Background = "/Background";

            [KeyInfo(KeyType.Rectangle | KeyType.Optional)]
            public const string BBox = "/BBox";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string AntiAlias = "/AntiAlias";

            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string Coords = "/Coords";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Domain = "/Domain";

            [KeyInfo(KeyType.Function | KeyType.Required)]
            public const string Function = "/Function";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Extend = "/Extend";

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
