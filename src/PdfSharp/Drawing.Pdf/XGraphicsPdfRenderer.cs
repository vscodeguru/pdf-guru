
using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Internal;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Internal;
using PdfSharp.Pdf.Advanced;

namespace PdfSharp.Drawing.Pdf
{
    internal class XGraphicsPdfRenderer : IXGraphicsRenderer
    {
        public XGraphicsPdfRenderer(PdfPage page, XGraphics gfx, XGraphicsPdfPageOptions options)
        {
            _page = page;
            _colorMode = page._document.Options.ColorMode;
            _options = options;
            _gfx = gfx;
            _content = new StringBuilder();
            page.RenderContent._pdfRenderer = this;
            _gfxState = new PdfGraphicsState(this);
        }

        public XGraphicsPdfRenderer(XForm form, XGraphics gfx)
        {
            _form = form;
            _colorMode = form.Owner.Options.ColorMode;
            _gfx = gfx;
            _content = new StringBuilder();
            form.PdfRenderer = this;
            _gfxState = new PdfGraphicsState(this);
        }

        string GetContent()
        {
            EndPage();
            return _content.ToString();
        }

        public XGraphicsPdfPageOptions PageOptions
        {
            get { return _options; }
        }

        public void Close()
        {
            if (_page != null)
            {
                PdfContent content2 = _page.RenderContent;
                content2.CreateStream(PdfEncoders.RawEncoding.GetBytes(GetContent()));

                _gfx = null;
                _page.RenderContent._pdfRenderer = null;
                _page.RenderContent = null;
                _page = null;
            }
            else if (_form != null)
            {
                _form._pdfForm.CreateStream(PdfEncoders.RawEncoding.GetBytes(GetContent()));
                _gfx = null;
                _form.PdfRenderer = null;
                _form = null;
            }
        }

        public void DrawLine(XPen pen, double x1, double y1, double x2, double y2)
        {
            DrawLines(pen, new XPoint[] { new XPoint(x1, y1), new XPoint(x2, y2) });
        }

        public void DrawLines(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count == 0)
                return;

            Realize(pen);

            const string format = Config.SignificantFigures4;
            AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx++)
                AppendFormatPoint("{0:" + format + "} {1:" + format + "} l\n", points[idx].X, points[idx].Y);
            _content.Append("S\n");
        }

        public void DrawBezier(XPen pen, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            DrawBeziers(pen, new XPoint[] { new XPoint(x1, y1), new XPoint(x2, y2), new XPoint(x3, y3), new XPoint(x4, y4) });
        }

        public void DrawBeziers(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count == 0)
                return;

            if ((count - 1) % 3 != 0)
                throw new ArgumentException("Invalid number of points for bezier curves. Number must fulfil 4+3n.", "points");

            Realize(pen);

            const string format = Config.SignificantFigures4;
            AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
                AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
                    points[idx].X, points[idx].Y,
                    points[idx + 1].X, points[idx + 1].Y,
                    points[idx + 2].X, points[idx + 2].Y);

            AppendStrokeFill(pen, null, XFillMode.Alternate, false);
        }

        public void DrawCurve(XPen pen, XPoint[] points, double tension)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count == 0)
                return;
            if (count < 2)
                throw new ArgumentException("Not enough points", "points");

            tension /= 3;

            Realize(pen);

            const string format = Config.SignificantFigures4;
            AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", points[0].X, points[0].Y);
            if (count == 2)
            {
                AppendCurveSegment(points[0], points[0], points[1], points[1], tension);
            }
            else
            {
                AppendCurveSegment(points[0], points[0], points[1], points[2], tension);
                for (int idx = 1; idx < count - 2; idx++)
                    AppendCurveSegment(points[idx - 1], points[idx], points[idx + 1], points[idx + 2], tension);
                AppendCurveSegment(points[count - 3], points[count - 2], points[count - 1], points[count - 1], tension);
            }
            AppendStrokeFill(pen, null, XFillMode.Alternate, false);
        }

        public void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            Realize(pen);

            AppendPartialArc(x, y, width, height, startAngle, sweepAngle, PathStart.MoveTo1st, new XMatrix());
            AppendStrokeFill(pen, null, XFillMode.Alternate, false);
        }

        public void DrawRectangle(XPen pen, XBrush brush, double x, double y, double width, double height)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush");

            const string format = Config.SignificantFigures3;

            Realize(pen, brush);
            AppendFormatRect("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} re\n", x, y + height, width, height);

            if (pen != null && brush != null)
                _content.Append("B\n");
            else if (pen != null)
                _content.Append("S\n");
            else
                _content.Append("f\n");
        }

        public void DrawRectangles(XPen pen, XBrush brush, XRect[] rects)
        {
            int count = rects.Length;
            for (int idx = 0; idx < count; idx++)
            {
                XRect rect = rects[idx];
                DrawRectangle(pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        public void DrawRoundedRectangle(XPen pen, XBrush brush, double x, double y, double width, double height, double ellipseWidth, double ellipseHeight)
        {
            XGraphicsPath path = new XGraphicsPath();
            path.AddRoundedRectangle(x, y, width, height, ellipseWidth, ellipseHeight);
            DrawPath(pen, brush, path);
        }

        public void DrawEllipse(XPen pen, XBrush brush, double x, double y, double width, double height)
        {
            Realize(pen, brush);

            XRect rect = new XRect(x, y, width, height);
            double δx = rect.Width / 2;
            double δy = rect.Height / 2;
            double fx = δx * Const.κ;
            double fy = δy * Const.κ;
            double x0 = rect.X + δx;
            double y0 = rect.Y + δy;

            const string format = Config.SignificantFigures4;
            AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", x0 + δx, y0);
            AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
              x0 + δx, y0 + fy, x0 + fx, y0 + δy, x0, y0 + δy);
            AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
              x0 - fx, y0 + δy, x0 - δx, y0 + fy, x0 - δx, y0);
            AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
              x0 - δx, y0 - fy, x0 - fx, y0 - δy, x0, y0 - δy);
            AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
              x0 + fx, y0 - δy, x0 + δx, y0 - fy, x0 + δx, y0);
            AppendStrokeFill(pen, brush, XFillMode.Winding, true);
        }

        public void DrawPolygon(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            Realize(pen, brush);

            int count = points.Length;
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            const string format = Config.SignificantFigures4;
            AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx++)
                AppendFormatPoint("{0:" + format + "} {1:" + format + "} l\n", points[idx].X, points[idx].Y);

            AppendStrokeFill(pen, brush, fillmode, true);
        }

        public void DrawPie(XPen pen, XBrush brush, double x, double y, double width, double height,
          double startAngle, double sweepAngle)
        {
            Realize(pen, brush);

            const string format = Config.SignificantFigures4;
            AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", x + width / 2, y + height / 2);
            AppendPartialArc(x, y, width, height, startAngle, sweepAngle, PathStart.LineTo1st, new XMatrix());
            AppendStrokeFill(pen, brush, XFillMode.Alternate, true);
        }

        public void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points, double tension, XFillMode fillmode)
        {
            int count = points.Length;
            if (count == 0)
                return;
            if (count < 2)
                throw new ArgumentException("Not enough points.", "points");

            tension /= 3;

            Realize(pen, brush);

            const string format = Config.SignificantFigures4;
            AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", points[0].X, points[0].Y);
            if (count == 2)
            {
                AppendCurveSegment(points[0], points[0], points[1], points[1], tension);
            }
            else
            {
                AppendCurveSegment(points[count - 1], points[0], points[1], points[2], tension);
                for (int idx = 1; idx < count - 2; idx++)
                    AppendCurveSegment(points[idx - 1], points[idx], points[idx + 1], points[idx + 2], tension);
                AppendCurveSegment(points[count - 3], points[count - 2], points[count - 1], points[0], tension);
                AppendCurveSegment(points[count - 2], points[count - 1], points[0], points[1], tension);
            }
            AppendStrokeFill(pen, brush, fillmode, true);
        }

        public void DrawPath(XPen pen, XBrush brush, XGraphicsPath path)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen");

#if CORE
            Realize(pen, brush);
            AppendPath(path._corePath);
            AppendStrokeFill(pen, brush, path.FillMode, false);
#endif
        }

        public void DrawString(string s, XFont font, XBrush brush, XRect rect, XStringFormat format)
        {
            double x = rect.X;
            double y = rect.Y;

            double lineSpace = font.GetHeight();
            double cyAscent = lineSpace * font.CellAscent / font.CellSpace;
            double cyDescent = lineSpace * font.CellDescent / font.CellSpace;
            double width = _gfx.MeasureString(s, font).Width;

            bool italicSimulation = (font.GlyphTypeface.StyleSimulations & XStyleSimulations.ItalicSimulation) != 0;
            bool boldSimulation = (font.GlyphTypeface.StyleSimulations & XStyleSimulations.BoldSimulation) != 0;
            bool strikeout = (font.Style & XFontStyle.Strikeout) != 0;
            bool underline = (font.Style & XFontStyle.Underline) != 0;

            Realize(font, brush, boldSimulation ? 2 : 0);

            switch (format.Alignment)
            {
                case XStringAlignment.Near:
                    break;

                case XStringAlignment.Center:
                    x += (rect.Width - width) / 2;
                    break;

                case XStringAlignment.Far:
                    x += rect.Width - width;
                    break;
            }
            if (Gfx.PageDirection == XPageDirection.Downwards)
            {
                switch (format.LineAlignment)
                {
                    case XLineAlignment.Near:
                        y += cyAscent;
                        break;

                    case XLineAlignment.Center:
                        y += (cyAscent * 3 / 4) / 2 + rect.Height / 2;
                        break;

                    case XLineAlignment.Far:
                        y += -cyDescent + rect.Height;
                        break;

                    case XLineAlignment.BaseLine:
                        break;
                }
            }
            else
            {
                switch (format.LineAlignment)
                {
                    case XLineAlignment.Near:
                        y += cyDescent;
                        break;

                    case XLineAlignment.Center:
                        y += -(cyAscent * 3 / 4) / 2 + rect.Height / 2;
                        break;

                    case XLineAlignment.Far:
                        y += -cyAscent + rect.Height;
                        break;

                    case XLineAlignment.BaseLine:
                        break;
                }
            }

            PdfFont realizedFont = _gfxState._realizedFont;
            Debug.Assert(realizedFont != null);
            realizedFont.AddChars(s);

            const string format2 = Config.SignificantFigures4;
            OpenTypeDescriptor descriptor = realizedFont.FontDescriptor._descriptor;

            string text = null;
            if (font.Unicode)
            {
                StringBuilder sb = new StringBuilder();
                bool isSymbolFont = descriptor.FontFace.cmap.symbol;
                for (int idx = 0; idx < s.Length; idx++)
                {
                    char ch = s[idx];
                    if (isSymbolFont)
                    {
                        ch = (char)(ch | (descriptor.FontFace.os2.usFirstCharIndex & 0xFF00));    
                    }
                    int glyphID = descriptor.CharCodeToGlyphIndex(ch);
                    sb.Append((char)glyphID);
                }
                s = sb.ToString();

                byte[] bytes = PdfEncoders.RawUnicodeEncoding.GetBytes(s);
                bytes = PdfEncoders.FormatStringLiteral(bytes, true, false, true, null);
                text = PdfEncoders.RawEncoding.GetString(bytes, 0, bytes.Length);
            }
            else
            {
                byte[] bytes = PdfEncoders.WinAnsiEncoding.GetBytes(s);
                text = PdfEncoders.ToStringLiteral(bytes, false, null);
            }

            XPoint pos = new XPoint(x, y);
            pos = WorldToView(pos);

            double verticalOffset = 0;
            if (boldSimulation)
            {
            }

#if ITALIC_SIMULATION
            if (italicSimulation)
            {
                if (_gfxState.ItalicSimulationOn)
                {
                    AdjustTdOffset(ref pos, verticalOffset, true);
                    AppendFormatArgs("{0:" + format2 + "} {1:" + format2 + "} Td\n{2} Tj\n", pos.X, pos.Y, text);
                }
                else
                {
                    XMatrix m = new XMatrix(1, 0, Const.ItalicSkewAngleSinus, 1, pos.X, pos.Y);
                    AppendFormatArgs("{0:" + format2 + "} {1:" + format2 + "} {2:" + format2 + "} {3:" + format2 + "} {4:" + format2 + "} {5:" + format2 + "} Tm\n{6} Tj\n",
                        m.M11, m.M12, m.M21, m.M22, m.OffsetX, m.OffsetY, text);
                    _gfxState.ItalicSimulationOn = true;
                    AdjustTdOffset(ref pos, verticalOffset, false);
                }
            }
            else
            {
                if (_gfxState.ItalicSimulationOn)
                {
                    XMatrix m = new XMatrix(1, 0, 0, 1, pos.X, pos.Y);
                    AppendFormatArgs("{0:" + format2 + "} {1:" + format2 + "} {2:" + format2 + "} {3:" + format2 + "} {4:" + format2 + "} {5:" + format2 + "} Tm\n{6} Tj\n",
                        m.M11, m.M12, m.M21, m.M22, m.OffsetX, m.OffsetY, text);
                    _gfxState.ItalicSimulationOn = false;
                    AdjustTdOffset(ref pos, verticalOffset, false);
                }
                else
                {
                    AdjustTdOffset(ref pos, verticalOffset, false);
                    AppendFormatArgs("{0:" + format2 + "} {1:" + format2 + "} Td {2} Tj\n", pos.X, pos.Y, text);
                }
            }
#endif
            if (underline)
            {
                double underlinePosition = lineSpace * realizedFont.FontDescriptor._descriptor.UnderlinePosition / font.CellSpace;
                double underlineThickness = lineSpace * realizedFont.FontDescriptor._descriptor.UnderlineThickness / font.CellSpace;
                double underlineRectY = Gfx.PageDirection == XPageDirection.Downwards
                    ? y - underlinePosition
                    : y + underlinePosition - underlineThickness;
                DrawRectangle(null, brush, x, underlineRectY, width, underlineThickness);
            }

            if (strikeout)
            {
                double strikeoutPosition = lineSpace * realizedFont.FontDescriptor._descriptor.StrikeoutPosition / font.CellSpace;
                double strikeoutSize = lineSpace * realizedFont.FontDescriptor._descriptor.StrikeoutSize / font.CellSpace;
                double strikeoutRectY = Gfx.PageDirection == XPageDirection.Downwards
                    ? y - strikeoutPosition
                    : y + strikeoutPosition - strikeoutSize;
                DrawRectangle(null, brush, x, strikeoutRectY, width, strikeoutSize);
            }
        }

        public void DrawImage(XImage image, double x, double y, double width, double height)
        {
            const string format = Config.SignificantFigures4;

            string name = Realize(image);
            if (!(image is XForm))
            {
                if (_gfx.PageDirection == XPageDirection.Downwards)
                {
                    AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm {4} Do Q\n",
                        x, y + height, width, height, name);
                }
                else
                {
                    AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm {4} Do Q\n",
                        x, y, width, height, name);
                }
            }
            else
            {
                BeginPage();

                XForm form = (XForm)image;
                form.Finish();

                PdfFormXObject pdfForm = Owner.FormTable.GetForm(form);

                double cx = width / image.PointWidth;
                double cy = height / image.PointHeight;

                if (cx != 0 && cy != 0)
                {
                    XPdfForm xForm = image as XPdfForm;
                    if (_gfx.PageDirection == XPageDirection.Downwards)
                    {
                        double xDraw = x;
                        double yDraw = y;
                        if (xForm != null)
                        {
                            xDraw -= xForm.Page.MediaBox.X1;
                            yDraw += xForm.Page.MediaBox.Y1;
                        }
                        AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm 100 Tz {4} Do Q\n",
                            xDraw, yDraw + height, cx, cy, name);
                    }
                    else
                    {
                        AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm {4} Do Q\n",
                            x, y, cx, cy, name);
                    }
                }
            }
        }

        public void DrawImage(XImage image, XRect destRect, XRect srcRect, XGraphicsUnit srcUnit)
        {
            const string format = Config.SignificantFigures4;

            double x = destRect.X;
            double y = destRect.Y;
            double width = destRect.Width;
            double height = destRect.Height;

            string name = Realize(image);
            if (!(image is XForm))
            {
                if (_gfx.PageDirection == XPageDirection.Downwards)
                {
                    AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm {4} Do\nQ\n",
                        x, y + height, width, height, name);
                }
                else
                {
                    AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm {4} Do Q\n",
                        x, y, width, height, name);
                }
            }
            else
            {
                BeginPage();

                XForm form = (XForm)image;
                form.Finish();

                PdfFormXObject pdfForm = Owner.FormTable.GetForm(form);

                double cx = width / image.PointWidth;
                double cy = height / image.PointHeight;

                if (cx != 0 && cy != 0)
                {
                    XPdfForm xForm = image as XPdfForm;
                    if (_gfx.PageDirection == XPageDirection.Downwards)
                    {
                        double xDraw = x;
                        double yDraw = y;
                        if (xForm != null)
                        {
                            xDraw -= xForm.Page.MediaBox.X1;
                            yDraw += xForm.Page.MediaBox.Y1;
                        }
                        AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm {4} Do Q\n",
                            xDraw, yDraw + height, cx, cy, name);
                    }
                    else
                    {
                        AppendFormatImage("q {2:" + format + "} 0 0 {3:" + format + "} {0:" + format + "} {1:" + format + "} cm {4} Do Q\n",
                            x, y, cx, cy, name);
                    }
                }
            }
        }

        public void Save(XGraphicsState state)
        {
            BeginGraphicMode();
            RealizeTransform();
            _gfxState.InternalState = state.InternalState;
            SaveState();
        }

        public void Restore(XGraphicsState state)
        {
            BeginGraphicMode();
            RestoreState(state.InternalState);
        }

        public void BeginContainer(XGraphicsContainer container, XRect dstrect, XRect srcrect, XGraphicsUnit unit)
        {
            BeginGraphicMode();
            RealizeTransform();
            _gfxState.InternalState = container.InternalState;
            SaveState();
        }

        public void EndContainer(XGraphicsContainer container)
        {
            BeginGraphicMode();
            RestoreState(container.InternalState);
        }

        public XMatrix Transform
        {
            get
            {
                if (_gfxState.UnrealizedCtm.IsIdentity)
                    return _gfxState.EffectiveCtm;
                return _gfxState.UnrealizedCtm * _gfxState.RealizedCtm;
            }
        }

        public void AddTransform(XMatrix value, XMatrixOrder matrixOrder)
        {
            _gfxState.AddTransform(value, matrixOrder);
        }

        public void SetClip(XGraphicsPath path, XCombineMode combineMode)
        {
            if (path == null)
                throw new NotImplementedException("SetClip with no path.");

            if (_gfxState.Level < GraphicsStackLevelWorldSpace)
                RealizeTransform();      

            if (combineMode == XCombineMode.Replace)
            {
                if (_clipLevel != 0)
                {
                    if (_clipLevel != _gfxState.Level)
                        throw new NotImplementedException("Cannot set new clip region in an inner graphic state level.");
                    else
                        ResetClip();
                }
                _clipLevel = _gfxState.Level;
            }
            else if (combineMode == XCombineMode.Intersect)
            {
                if (_clipLevel == 0)
                    _clipLevel = _gfxState.Level;
            }
            else
            {
                Debug.Assert(false, "Invalid XCombineMode in internal function.");
            }
            _gfxState.SetAndRealizeClipPath(path);
        }

        public void ResetClip()
        {
            if (_clipLevel == 0)
                return;

            if (_clipLevel != _gfxState.Level)
                throw new NotImplementedException("Cannot reset clip region in an inner graphic state level.");

            BeginGraphicMode();

            InternalGraphicsState state = _gfxState.InternalState;
            XMatrix ctm = _gfxState.EffectiveCtm;
            RestoreState();
            SaveState();
            _gfxState.InternalState = state;
        }

        int _clipLevel;

        public void WriteComment(string comment)
        {
            comment = comment.Replace("\n", "\n% ");
            Append("% " + comment + "\n");
        }

        void AppendPartialArc(double x, double y, double width, double height, double startAngle, double sweepAngle, PathStart pathStart, XMatrix matrix)
        {
            double α = startAngle;
            if (α < 0)
                α = α + (1 + Math.Floor((Math.Abs(α) / 360))) * 360;
            else if (α > 360)
                α = α - Math.Floor(α / 360) * 360;
            Debug.Assert(α >= 0 && α <= 360);

            double β = sweepAngle;
            if (β < -360)
                β = -360;
            else if (β > 360)
                β = 360;

            if (α == 0 && β < 0)
                α = 360;
            else if (α == 360 && β > 0)
                α = 0;

            bool smallAngle = Math.Abs(β) <= 90;

            β = α + β;
            if (β < 0)
                β = β + (1 + Math.Floor((Math.Abs(β) / 360))) * 360;

            bool clockwise = sweepAngle > 0;
            int startQuadrant = Quadrant(α, true, clockwise);
            int endQuadrant = Quadrant(β, false, clockwise);

            if (startQuadrant == endQuadrant && smallAngle)
                AppendPartialArcQuadrant(x, y, width, height, α, β, pathStart, matrix);
            else
            {
                int currentQuadrant = startQuadrant;
                bool firstLoop = true;
                do
                {
                    if (currentQuadrant == startQuadrant && firstLoop)
                    {
                        double ξ = currentQuadrant * 90 + (clockwise ? 90 : 0);
                        AppendPartialArcQuadrant(x, y, width, height, α, ξ, pathStart, matrix);
                    }
                    else if (currentQuadrant == endQuadrant)
                    {
                        double ξ = currentQuadrant * 90 + (clockwise ? 0 : 90);
                        AppendPartialArcQuadrant(x, y, width, height, ξ, β, PathStart.Ignore1st, matrix);
                    }
                    else
                    {
                        double ξ1 = currentQuadrant * 90 + (clockwise ? 0 : 90);
                        double ξ2 = currentQuadrant * 90 + (clockwise ? 90 : 0);
                        AppendPartialArcQuadrant(x, y, width, height, ξ1, ξ2, PathStart.Ignore1st, matrix);
                    }

                    if (currentQuadrant == endQuadrant && smallAngle)
                        break;

                    smallAngle = true;

                    if (clockwise)
                        currentQuadrant = currentQuadrant == 3 ? 0 : currentQuadrant + 1;
                    else
                        currentQuadrant = currentQuadrant == 0 ? 3 : currentQuadrant - 1;

                    firstLoop = false;
                } while (true);
            }
        }

        int Quadrant(double φ, bool start, bool clockwise)
        {
            Debug.Assert(φ >= 0);
            if (φ > 360)
                φ = φ - Math.Floor(φ / 360) * 360;

            int quadrant = (int)(φ / 90);
            if (quadrant * 90 == φ)
            {
                if ((start && !clockwise) || (!start && clockwise))
                    quadrant = quadrant == 0 ? 3 : quadrant - 1;
            }
            else
                quadrant = clockwise ? ((int)Math.Floor(φ / 90)) % 4 : (int)Math.Floor(φ / 90);
            return quadrant;
        }

        void AppendPartialArcQuadrant(double x, double y, double width, double height, double α, double β, PathStart pathStart, XMatrix matrix)
        {
            Debug.Assert(α >= 0 && α <= 360);
            Debug.Assert(β >= 0);
            if (β > 360)
                β = β - Math.Floor(β / 360) * 360;
            Debug.Assert(Math.Abs(α - β) <= 90);

            double δx = width / 2;
            double δy = height / 2;

            double x0 = x + δx;
            double y0 = y + δy;

            bool reflect = false;
            if (α >= 180 && β >= 180)
            {
                α -= 180;
                β -= 180;
                reflect = true;
            }

            double sinα, sinβ;
            if (width == height)
            {
                α = α * Calc.Deg2Rad;
                β = β * Calc.Deg2Rad;
            }
            else
            {
                α = α * Calc.Deg2Rad;
                sinα = Math.Sin(α);
                if (Math.Abs(sinα) > 1E-10)
                    α = Math.PI / 2 - Math.Atan(δy * Math.Cos(α) / (δx * sinα));
                β = β * Calc.Deg2Rad;
                sinβ = Math.Sin(β);
                if (Math.Abs(sinβ) > 1E-10)
                    β = Math.PI / 2 - Math.Atan(δy * Math.Cos(β) / (δx * sinβ));
            }

            double κ = 4 * (1 - Math.Cos((α - β) / 2)) / (3 * Math.Sin((β - α) / 2));
            sinα = Math.Sin(α);
            double cosα = Math.Cos(α);
            sinβ = Math.Sin(β);
            double cosβ = Math.Cos(β);

            const string format = Config.SignificantFigures3;
            XPoint pt1, pt2, pt3;
            if (!reflect)
            {
                switch (pathStart)
                {
                    case PathStart.MoveTo1st:
                        pt1 = matrix.Transform(new XPoint(x0 + δx * cosα, y0 + δy * sinα));
                        AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", pt1.X, pt1.Y);
                        break;

                    case PathStart.LineTo1st:
                        pt1 = matrix.Transform(new XPoint(x0 + δx * cosα, y0 + δy * sinα));
                        AppendFormatPoint("{0:" + format + "} {1:" + format + "} l\n", pt1.X, pt1.Y);
                        break;

                    case PathStart.Ignore1st:
                        break;
                }
                pt1 = matrix.Transform(new XPoint(x0 + δx * (cosα - κ * sinα), y0 + δy * (sinα + κ * cosα)));
                pt2 = matrix.Transform(new XPoint(x0 + δx * (cosβ + κ * sinβ), y0 + δy * (sinβ - κ * cosβ)));
                pt3 = matrix.Transform(new XPoint(x0 + δx * cosβ, y0 + δy * sinβ));
                AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
                  pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
            }
            else
            {
                switch (pathStart)
                {
                    case PathStart.MoveTo1st:
                        pt1 = matrix.Transform(new XPoint(x0 - δx * cosα, y0 - δy * sinα));
                        AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", pt1.X, pt1.Y);
                        break;

                    case PathStart.LineTo1st:
                        pt1 = matrix.Transform(new XPoint(x0 - δx * cosα, y0 - δy * sinα));
                        AppendFormatPoint("{0:" + format + "} {1:" + format + "} l\n", pt1.X, pt1.Y);
                        break;

                    case PathStart.Ignore1st:
                        break;
                }
                pt1 = matrix.Transform(new XPoint(x0 - δx * (cosα - κ * sinα), y0 - δy * (sinα + κ * cosα)));
                pt2 = matrix.Transform(new XPoint(x0 - δx * (cosβ + κ * sinβ), y0 - δy * (sinβ - κ * cosβ)));
                pt3 = matrix.Transform(new XPoint(x0 - δx * cosβ, y0 - δy * sinβ));
                AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
                    pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
            }
        }

        void AppendCurveSegment(XPoint pt0, XPoint pt1, XPoint pt2, XPoint pt3, double tension3)
        {
            const string format = Config.SignificantFigures4;
            AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n",
                pt1.X + tension3 * (pt2.X - pt0.X), pt1.Y + tension3 * (pt2.Y - pt0.Y),
                pt2.X - tension3 * (pt3.X - pt1.X), pt2.Y - tension3 * (pt3.Y - pt1.Y),
                pt2.X, pt2.Y);
        }



#if CORE
        internal void AppendPath(CoreGraphicsPath path)
        {
            AppendPath(path.PathPoints, path.PathTypes);
        }
#endif

#if CORE || GDI
        void AppendPath(XPoint[] points, Byte[] types)
        {
            const string format = Config.SignificantFigures4;
            int count = points.Length;
            if (count == 0)
                return;

            for (int idx = 0; idx < count; idx++)
            {
                const byte PathPointTypeStart = 0;  
                const byte PathPointTypeLine = 1;  
                const byte PathPointTypeBezier = 3;      
                const byte PathPointTypePathTypeMask = 0x07;      
                const byte PathPointTypeCloseSubpath = 0x80;   
                byte type = types[idx];
                switch (type & PathPointTypePathTypeMask)
                {
                    case PathPointTypeStart:
                        AppendFormatPoint("{0:" + format + "} {1:" + format + "} m\n", points[idx].X, points[idx].Y);
                        break;

                    case PathPointTypeLine:
                        AppendFormatPoint("{0:" + format + "} {1:" + format + "} l\n", points[idx].X, points[idx].Y);
                        if ((type & PathPointTypeCloseSubpath) != 0)
                            Append("h\n");
                        break;

                    case PathPointTypeBezier:
                        Debug.Assert(idx + 2 < count);
                        AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} c\n", points[idx].X, points[idx].Y,
                            points[++idx].X, points[idx].Y, points[++idx].X, points[idx].Y);
                        if ((types[idx] & PathPointTypeCloseSubpath) != 0)
                            Append("h\n");
                        break;
                }
            }
        }
#endif
        internal void Append(string value)
        {
            _content.Append(value);
        }

        internal void AppendFormatArgs(string format, params object[] args)
        {
            _content.AppendFormat(CultureInfo.InvariantCulture, format, args);
        }

        internal void AppendFormatString(string format, string s)
        {
            _content.AppendFormat(CultureInfo.InvariantCulture, format, s);
        }

        internal void AppendFormatFont(string format, string s, double d)
        {
            _content.AppendFormat(CultureInfo.InvariantCulture, format, s, d);
        }

        internal void AppendFormatInt(string format, int n)
        {
            _content.AppendFormat(CultureInfo.InvariantCulture, format, n);
        }

        internal void AppendFormatDouble(string format, double d)
        {
            _content.AppendFormat(CultureInfo.InvariantCulture, format, d);
        }

        internal void AppendFormatPoint(string format, double x, double y)
        {
            XPoint result = WorldToView(new XPoint(x, y));
            _content.AppendFormat(CultureInfo.InvariantCulture, format, result.X, result.Y);
        }

        internal void AppendFormatRect(string format, double x, double y, double width, double height)
        {
            XPoint point1 = WorldToView(new XPoint(x, y));
            _content.AppendFormat(CultureInfo.InvariantCulture, format, point1.X, point1.Y, width, height);
        }

        internal void AppendFormat3Points(string format, double x1, double y1, double x2, double y2, double x3, double y3)
        {
            XPoint point1 = WorldToView(new XPoint(x1, y1));
            XPoint point2 = WorldToView(new XPoint(x2, y2));
            XPoint point3 = WorldToView(new XPoint(x3, y3));
            _content.AppendFormat(CultureInfo.InvariantCulture, format, point1.X, point1.Y, point2.X, point2.Y, point3.X, point3.Y);
        }

        internal void AppendFormat(string format, XPoint point)
        {
            XPoint result = WorldToView(point);
            _content.AppendFormat(CultureInfo.InvariantCulture, format, result.X, result.Y);
        }

        internal void AppendFormat(string format, double x, double y, string s)
        {
            XPoint result = WorldToView(new XPoint(x, y));
            _content.AppendFormat(CultureInfo.InvariantCulture, format, result.X, result.Y, s);
        }

        internal void AppendFormatImage(string format, double x, double y, double width, double height, string name)
        {
            XPoint result = WorldToView(new XPoint(x, y));
            _content.AppendFormat(CultureInfo.InvariantCulture, format, result.X, result.Y, width, height, name);
        }

        void AppendStrokeFill(XPen pen, XBrush brush, XFillMode fillMode, bool closePath)
        {
            if (closePath)
                _content.Append("h ");

            if (fillMode == XFillMode.Winding)
            {
                if (pen != null && brush != null)
                    _content.Append("B\n");
                else if (pen != null)
                    _content.Append("S\n");
                else
                    _content.Append("f\n");
            }
            else
            {
                if (pen != null && brush != null)
                    _content.Append("B*\n");
                else if (pen != null)
                    _content.Append("S\n");
                else
                    _content.Append("f*\n");
            }
        }
        void BeginPage()
        {
            if (_gfxState.Level == GraphicsStackLevelInitial)
            {
                DefaultViewMatrix = new XMatrix();
                if (_gfx.PageDirection == XPageDirection.Downwards)
                {
                    PageHeightPt = Size.Height;
                    XPoint trimOffset = new XPoint();
                    if (_page != null && _page.TrimMargins.AreSet)
                    {
                        PageHeightPt += _page.TrimMargins.Top.Point + _page.TrimMargins.Bottom.Point;
                        trimOffset = new XPoint(_page.TrimMargins.Left.Point, _page.TrimMargins.Top.Point);
                    }

                    switch (_gfx.PageUnit)
                    {
                        case XGraphicsUnit.Point:
                            break;

                        case XGraphicsUnit.Presentation:
                            DefaultViewMatrix.ScalePrepend(XUnit.PresentationFactor);
                            break;

                        case XGraphicsUnit.Inch:
                            DefaultViewMatrix.ScalePrepend(XUnit.InchFactor);
                            break;

                        case XGraphicsUnit.Millimeter:
                            DefaultViewMatrix.ScalePrepend(XUnit.MillimeterFactor);
                            break;

                        case XGraphicsUnit.Centimeter:
                            DefaultViewMatrix.ScalePrepend(XUnit.CentimeterFactor);
                            break;
                    }

                    if (trimOffset != new XPoint())
                    {
                        Debug.Assert(_gfx.PageUnit == XGraphicsUnit.Point, "With TrimMargins set the page units must be Point. Ohter cases nyi.");
                        DefaultViewMatrix.TranslatePrepend(trimOffset.X, -trimOffset.Y);
                    }

                    SaveState();

                    if (!DefaultViewMatrix.IsIdentity)
                    {
                        Debug.Assert(_gfxState.RealizedCtm.IsIdentity);
                        const string format = Config.SignificantFigures7;
                        double[] cm = DefaultViewMatrix.GetElements();
                        AppendFormatArgs("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} cm ",
                                     cm[0], cm[1], cm[2], cm[3], cm[4], cm[5]);
                    }

                }
                else
                {
                    switch (_gfx.PageUnit)
                    {
                        case XGraphicsUnit.Point:
                            break;

                        case XGraphicsUnit.Presentation:
                            DefaultViewMatrix.ScalePrepend(XUnit.PresentationFactor);
                            break;

                        case XGraphicsUnit.Inch:
                            DefaultViewMatrix.ScalePrepend(XUnit.InchFactor);
                            break;

                        case XGraphicsUnit.Millimeter:
                            DefaultViewMatrix.ScalePrepend(XUnit.MillimeterFactor);
                            break;

                        case XGraphicsUnit.Centimeter:
                            DefaultViewMatrix.ScalePrepend(XUnit.CentimeterFactor);
                            break;
                    }

                    SaveState();
                    const string format = Config.SignificantFigures7;
                    double[] cm = DefaultViewMatrix.GetElements();
                    AppendFormat3Points("{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" + format + "} cm ",
                        cm[0], cm[1], cm[2], cm[3], cm[4], cm[5]);
                }
            }
        }

        void EndPage()
        {
            if (_streamMode == StreamMode.Text)
            {
                _content.Append("ET\n");
                _streamMode = StreamMode.Graphic;
            }

            while (_gfxStateStack.Count != 0)
                RestoreState();
        }

        internal void BeginGraphicMode()
        {
            if (_streamMode != StreamMode.Graphic)
            {
                if (_streamMode == StreamMode.Text)
                    _content.Append("ET\n");

                _streamMode = StreamMode.Graphic;
            }
        }

        internal void BeginTextMode()
        {
            if (_streamMode != StreamMode.Text)
            {
                _streamMode = StreamMode.Text;
                _content.Append("BT\n");
                _gfxState.RealizedTextPosition = new XPoint();
                _gfxState.ItalicSimulationOn = false;
            }
        }

        StreamMode _streamMode;

        private void Realize(XPen pen, XBrush brush)
        {
            BeginPage();
            BeginGraphicMode();
            RealizeTransform();

            if (pen != null)
                _gfxState.RealizePen(pen, _colorMode);  

            if (brush != null)
            {
                _gfxState.RealizeBrush(brush, _colorMode, 0, 0);  
            }
        }

        void Realize(XPen pen)
        {
            Realize(pen, null);
        }

        void Realize(XBrush brush)
        {
            Realize(null, brush);
        }

        void Realize(XFont font, XBrush brush, int renderingMode)
        {
            BeginPage();
            RealizeTransform();
            BeginTextMode();
            _gfxState.RealizeFont(font, brush, renderingMode);
        }

        void AdjustTdOffset(ref XPoint pos, double dy, bool adjustSkew)
        {
            pos.Y += dy;
            XPoint posSave = pos;
            pos = pos - new XVector(_gfxState.RealizedTextPosition.X, _gfxState.RealizedTextPosition.Y);
            if (adjustSkew)
            {
                pos.X -= Const.ItalicSkewAngleSinus * pos.Y;
            }
            _gfxState.RealizedTextPosition = posSave;
        }

        string Realize(XImage image)
        {
            BeginPage();
            BeginGraphicMode();
            RealizeTransform();

            _gfxState.RealizeNonStrokeTransparency(1, _colorMode);

            XForm form = image as XForm;
            return form != null ? GetFormName(form) : GetImageName(image);
        }

        void RealizeTransform()
        {
            BeginPage();

            if (_gfxState.Level == GraphicsStackLevelPageSpace)
            {
                BeginGraphicMode();
                SaveState();
            }

            if (!_gfxState.UnrealizedCtm.IsIdentity)
            {
                BeginGraphicMode();
                _gfxState.RealizeCtm();
            }
        }

        internal XPoint WorldToView(XPoint point)
        {
            Debug.Assert(_gfxState.UnrealizedCtm.IsIdentity, "Somewhere a RealizeTransform is missing.");
#if true
            XPoint pt = _gfxState.WorldTransform.Transform(point);
            return _gfxState.InverseEffectiveCtm.Transform(new XPoint(pt.X, PageHeightPt / DefaultViewMatrix.M22 - pt.Y));
#endif
        }

#if CORE || GDI
        [Conditional("DEBUG")]
        void DumpPathData(XPoint[] points, byte[] types)
        {
            int count = points.Length;
            for (int idx = 0; idx < count; idx++)
            {
                string info = PdfEncoders.Format("{0:X}   {1:####0.000} {2:####0.000}", types[idx], points[idx].X, points[idx].Y);
                Debug.WriteLine(info, "PathData");
            }
        }
#endif

        internal PdfDocument Owner
        {
            get
            {
                if (_page != null)
                    return _page.Owner;
                return _form.Owner;
            }
        }

        internal XGraphics Gfx
        {
            get { return _gfx; }
        }

        internal PdfResources Resources
        {
            get
            {
                if (_page != null)
                    return _page.Resources;
                return _form.Resources;
            }
        }

        internal XSize Size
        {
            get
            {
                if (_page != null)
                    return new XSize(_page.Width, _page.Height);
                return _form.Size;
            }
        }

        internal string GetFontName(XFont font, out PdfFont pdfFont)
        {
            if (_page != null)
                return _page.GetFontName(font, out pdfFont);
            return _form.GetFontName(font, out pdfFont);
        }

        internal string GetImageName(XImage image)
        {
            if (_page != null)
                return _page.GetImageName(image);
            return _form.GetImageName(image);
        }

        internal string GetFormName(XForm form)
        {
            if (_page != null)
                return _page.GetFormName(form);
            return _form.GetFormName(form);
        }

        internal PdfPage _page;
        internal XForm _form;
        internal PdfColorMode _colorMode;
        XGraphicsPdfPageOptions _options;
        XGraphics _gfx;
        readonly StringBuilder _content;

        const int GraphicsStackLevelInitial = 0;

        const int GraphicsStackLevelPageSpace = 1;

        const int GraphicsStackLevelWorldSpace = 2;

        void SaveState()
        {
            Debug.Assert(_streamMode == StreamMode.Graphic, "Cannot save state in text mode.");

            _gfxStateStack.Push(_gfxState);
            _gfxState = _gfxState.Clone();
            _gfxState.Level = _gfxStateStack.Count;
            Append("q\n");
        }

        void RestoreState()
        {
            Debug.Assert(_streamMode == StreamMode.Graphic, "Cannot restore state in text mode.");

            _gfxState = _gfxStateStack.Pop();
            Append("Q\n");
        }

        PdfGraphicsState RestoreState(InternalGraphicsState state)
        {
            int count = 1;
            PdfGraphicsState top = _gfxStateStack.Pop();
            while (top.InternalState != state)
            {
                Append("Q\n");
                count++;
                top = _gfxStateStack.Pop();
            }
            Append("Q\n");
            _gfxState = top;
            return top;
        }

        PdfGraphicsState _gfxState;

        readonly Stack<PdfGraphicsState> _gfxStateStack = new Stack<PdfGraphicsState>();

        public double PageHeightPt;

        public XMatrix DefaultViewMatrix;
    }
}