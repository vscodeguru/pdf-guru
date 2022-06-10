using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using PdfSharp.Internal;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Drawing.Pdf
{
    internal sealed class PdfGraphicsState : ICloneable
    {
        public PdfGraphicsState(XGraphicsPdfRenderer renderer)
        {
            _renderer = renderer;
        }
        readonly XGraphicsPdfRenderer _renderer;

        public PdfGraphicsState Clone()
        {
            PdfGraphicsState state = (PdfGraphicsState)MemberwiseClone();
            return state;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        internal int Level;

        internal InternalGraphicsState InternalState;

        public void PushState()
        {
            _renderer.Append("q/n");
        }

        public void PopState()
        {
            _renderer.Append("Q/n");
        }

        double _realizedLineWith = -1;
        int _realizedLineCap = -1;
        int _realizedLineJoin = -1;
        double _realizedMiterLimit = -1;
        XDashStyle _realizedDashStyle = (XDashStyle)(-1);
        string _realizedDashPattern;
        XColor _realizedStrokeColor = XColor.Empty;
        bool _realizedStrokeOverPrint;

        public void RealizePen(XPen pen, PdfColorMode colorMode)
        {
            const string frmt2 = Config.SignificantFigures2;
            const string format = Config.SignificantFigures3;
            XColor color = pen.Color;
            bool overPrint = pen.Overprint;
            color = ColorSpaceHelper.EnsureColorMode(colorMode, color);

            if (_realizedLineWith != pen._width)
            {
                _renderer.AppendFormatArgs("{0:" + format + "} w\n", pen._width);
                _realizedLineWith = pen._width;
            }

            if (_realizedLineCap != (int)pen._lineCap)
            {
                _renderer.AppendFormatArgs("{0} J\n", (int)pen._lineCap);
                _realizedLineCap = (int)pen._lineCap;
            }

            if (_realizedLineJoin != (int)pen._lineJoin)
            {
                _renderer.AppendFormatArgs("{0} j\n", (int)pen._lineJoin);
                _realizedLineJoin = (int)pen._lineJoin;
            }

            if (_realizedLineCap == (int)XLineJoin.Miter)
            {
                if (_realizedMiterLimit != (int)pen._miterLimit && (int)pen._miterLimit != 0)
                {
                    _renderer.AppendFormatInt("{0} M\n", (int)pen._miterLimit);
                    _realizedMiterLimit = (int)pen._miterLimit;
                }
            }

            if (_realizedDashStyle != pen._dashStyle || pen._dashStyle == XDashStyle.Custom)
            {
                double dot = pen.Width;
                double dash = 3 * dot;

                XDashStyle dashStyle = pen.DashStyle;
                if (dot == 0)
                    dashStyle = XDashStyle.Solid;

                switch (dashStyle)
                {
                    case XDashStyle.Solid:
                        _renderer.Append("[]0 d\n");
                        break;

                    case XDashStyle.Dash:
                        _renderer.AppendFormatArgs("[{0:" + frmt2 + "} {1:" + frmt2 + "}]0 d\n", dash, dot);
                        break;

                    case XDashStyle.Dot:
                        _renderer.AppendFormatArgs("[{0:" + frmt2 + "}]0 d\n", dot);
                        break;

                    case XDashStyle.DashDot:
                        _renderer.AppendFormatArgs("[{0:" + frmt2 + "} {1:" + frmt2 + "} {1:" + frmt2 + "} {1:" + frmt2 + "}]0 d\n", dash, dot);
                        break;

                    case XDashStyle.DashDotDot:
                        _renderer.AppendFormatArgs("[{0:" + frmt2 + "} {1:" + frmt2 + "} {1:" + frmt2 + "} {1:" + frmt2 + "} {1:" + frmt2 + "} {1:" + frmt2 + "}]0 d\n", dash, dot);
                        break;

                    case XDashStyle.Custom:
                        {
                            StringBuilder pdf = new StringBuilder("[", 256);
                            int len = pen._dashPattern == null ? 0 : pen._dashPattern.Length;
                            for (int idx = 0; idx < len; idx++)
                            {
                                if (idx > 0)
                                    pdf.Append(' ');
                                pdf.Append(PdfEncoders.ToString(pen._dashPattern[idx] * pen._width));
                            }
                            if (len > 0 && len % 2 == 1)
                            {
                                pdf.Append(' ');
                                pdf.Append(PdfEncoders.ToString(0.2 * pen._width));
                            }
                            pdf.AppendFormat(CultureInfo.InvariantCulture, "]{0:" + format + "} d\n", pen._dashOffset * pen._width);
                            string pattern = pdf.ToString();

                            {
                                _realizedDashPattern = pattern;
                                _renderer.Append(pattern);
                            }
                        }
                        break;
                }
                _realizedDashStyle = dashStyle;
            }

            if (colorMode != PdfColorMode.Cmyk)
            {
                if (_realizedStrokeColor.Rgb != color.Rgb)
                {
                    _renderer.Append(PdfEncoders.ToString(color, PdfColorMode.Rgb));
                    _renderer.Append(" RG\n");
                }
            }
            else
            {
                if (!ColorSpaceHelper.IsEqualCmyk(_realizedStrokeColor, color))
                {
                    _renderer.Append(PdfEncoders.ToString(color, PdfColorMode.Cmyk));
                    _renderer.Append(" K\n");
                }
            }

            if (_renderer.Owner.Version >= 14 && (_realizedStrokeColor.A != color.A || _realizedStrokeOverPrint != overPrint))
            {
                PdfExtGState extGState = _renderer.Owner.ExtGStateTable.GetExtGStateStroke(color.A, overPrint);
                string gs = _renderer.Resources.AddExtGState(extGState);
                _renderer.AppendFormatString("{0} gs\n", gs);

                if (_renderer._page != null && color.A < 1)
                    _renderer._page.TransparencyUsed = true;
            }
            _realizedStrokeColor = color;
            _realizedStrokeOverPrint = overPrint;
        }

        XColor _realizedFillColor = XColor.Empty;
        bool _realizedNonStrokeOverPrint;

        public void RealizeBrush(XBrush brush, PdfColorMode colorMode, int renderingMode, double fontEmSize)
        {
            XSolidBrush solidBrush = brush as XSolidBrush;
            if (solidBrush != null)
            {
                XColor color = solidBrush.Color;
                bool overPrint = solidBrush.Overprint;

                if (renderingMode == 0)
                {
                    RealizeFillColor(color, overPrint, colorMode);
                }
                else if (renderingMode == 2)
                {
                    RealizeFillColor(color, false, colorMode);
                    RealizePen(new XPen(color, fontEmSize * Const.BoldEmphasis), colorMode);
                }
                else
                    throw new InvalidOperationException("Only rendering modes 0 and 2 are currently supported.");
            }
            else
            {
                if (renderingMode != 0)
                    throw new InvalidOperationException("Rendering modes other than 0 can only be used with solid color brushes.");

                XLinearGradientBrush gradientBrush = brush as XLinearGradientBrush;
                if (gradientBrush != null)
                {
                    Debug.Assert(UnrealizedCtm.IsIdentity, "Must realize ctm first.");
                    XMatrix matrix = _renderer.DefaultViewMatrix;
                    matrix.Prepend(EffectiveCtm);
                    PdfShadingPattern pattern = new PdfShadingPattern(_renderer.Owner);
                    pattern.SetupFromBrush(gradientBrush, matrix, _renderer);
                    string name = _renderer.Resources.AddPattern(pattern);
                    _renderer.AppendFormatString("/Pattern cs\n", name);
                    _renderer.AppendFormatString("{0} scn\n", name);

                    _realizedFillColor = XColor.Empty;
                }
            }
        }

        private void RealizeFillColor(XColor color, bool overPrint, PdfColorMode colorMode)
        {
            color = ColorSpaceHelper.EnsureColorMode(colorMode, color);

            if (colorMode != PdfColorMode.Cmyk)
            {
                if (_realizedFillColor.IsEmpty || _realizedFillColor.Rgb != color.Rgb)
                {
                    _renderer.Append(PdfEncoders.ToString(color, PdfColorMode.Rgb));
                    _renderer.Append(" rg\n");
                }
            }
            else
            {
                Debug.Assert(colorMode == PdfColorMode.Cmyk);

                if (_realizedFillColor.IsEmpty || !ColorSpaceHelper.IsEqualCmyk(_realizedFillColor, color))
                {
                    _renderer.Append(PdfEncoders.ToString(color, PdfColorMode.Cmyk));
                    _renderer.Append(" k\n");
                }
            }

            if (_renderer.Owner.Version >= 14 && (_realizedFillColor.A != color.A || _realizedNonStrokeOverPrint != overPrint))
            {

                PdfExtGState extGState = _renderer.Owner.ExtGStateTable.GetExtGStateNonStroke(color.A, overPrint);
                string gs = _renderer.Resources.AddExtGState(extGState);
                _renderer.AppendFormatString("{0} gs\n", gs);

                if (_renderer._page != null && color.A < 1)
                    _renderer._page.TransparencyUsed = true;
            }
            _realizedFillColor = color;
            _realizedNonStrokeOverPrint = overPrint;
        }

        internal void RealizeNonStrokeTransparency(double transparency, PdfColorMode colorMode)
        {
            XColor color = _realizedFillColor;
            color.A = transparency;
            RealizeFillColor(color, _realizedNonStrokeOverPrint, colorMode);
        }

        internal PdfFont _realizedFont;
        string _realizedFontName = String.Empty;
        double _realizedFontSize;
        int _realizedRenderingMode;            
        double _realizedCharSpace;            

        public void RealizeFont(XFont font, XBrush brush, int renderingMode)
        {
            const string format = Config.SignificantFigures3;

            RealizeBrush(brush, _renderer._colorMode, renderingMode, font.Size);  

            if (_realizedRenderingMode != renderingMode)
            {
                _renderer.AppendFormatInt("{0} Tr\n", renderingMode);
                _realizedRenderingMode = renderingMode;
            }

            if (_realizedRenderingMode == 0)
            {
                if (_realizedCharSpace != 0)
                {
                    _renderer.Append("0 Tc\n");
                    _realizedCharSpace = 0;
                }
            }
            else     
            {
                double charSpace = font.Size * Const.BoldEmphasis;
                if (_realizedCharSpace != charSpace)
                {
                    _renderer.AppendFormatDouble("{0:" + format + "} Tc\n", charSpace);
                    _realizedCharSpace = charSpace;
                }
            }

            _realizedFont = null;
            string fontName = _renderer.GetFontName(font, out _realizedFont);
            if (fontName != _realizedFontName || _realizedFontSize != font.Size)
            {
                if (_renderer.Gfx.PageDirection == XPageDirection.Downwards)
                    _renderer.AppendFormatFont("{0} {1:" + format + "} Tf\n", fontName, font.Size);
                else
                    _renderer.AppendFormatFont("{0} {1:" + format + "} Tf\n", fontName, font.Size);
                _realizedFontName = fontName;
                _realizedFontSize = font.Size;
            }
        }

        public XPoint RealizedTextPosition;

        public bool ItalicSimulationOn;

        public XMatrix RealizedCtm;

        public XMatrix UnrealizedCtm;

        public XMatrix EffectiveCtm;

        public XMatrix InverseEffectiveCtm;

        public XMatrix WorldTransform;

        public void AddTransform(XMatrix value, XMatrixOrder matrixOrder)
        {
            XMatrix transform = value;
            if (_renderer.Gfx.PageDirection == XPageDirection.Downwards)
            {
                transform.M12 = -value.M12;
                transform.M21 = -value.M21;
            }
            UnrealizedCtm.Prepend(transform);

            WorldTransform.Prepend(value);
        }

        public void RealizeCtm()
        {
            if (!UnrealizedCtm.IsIdentity)
            {
                Debug.Assert(!UnrealizedCtm.IsIdentity, "mrCtm is unnecessarily set.");

                const string format = Config.SignificantFigures7;

                double[] matrix = UnrealizedCtm.GetElements();
                _renderer.AppendFormatArgs("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} cm\n",
                    matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);

                RealizedCtm.Prepend(UnrealizedCtm);
                UnrealizedCtm = new XMatrix();
                EffectiveCtm = RealizedCtm;
                InverseEffectiveCtm = EffectiveCtm;
                InverseEffectiveCtm.Invert();
            }
        }
        public void SetAndRealizeClipRect(XRect clipRect)
        {
            XGraphicsPath clipPath = new XGraphicsPath();
            clipPath.AddRectangle(clipRect);
            RealizeClipPath(clipPath);
        }

        public void SetAndRealizeClipPath(XGraphicsPath clipPath)
        {
            RealizeClipPath(clipPath);
        }

        void RealizeClipPath(XGraphicsPath clipPath)
        {
#if CORE
            DiagnosticsHelper.HandleNotImplemented("RealizeClipPath");
#endif
            _renderer.BeginGraphicMode();
            RealizeCtm();
#if CORE
            _renderer.AppendPath(clipPath._corePath);
#endif
            _renderer.Append(clipPath.FillMode == XFillMode.Winding ? "W n\n" : "W* n\n");
        }

    }
}
