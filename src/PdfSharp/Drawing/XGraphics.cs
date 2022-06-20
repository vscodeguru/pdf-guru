using System;
using System.Diagnostics;
using PdfSharp.Pdf;
using PdfSharp.Drawing.Pdf;
using PdfSharp.Pdf.Advanced;
namespace PdfSharp.Drawing     
{
    [Flags]
    enum InternalGraphicsMode
    {
        DrawingGdiGraphics,
        DrawingPdfContent,
        DrawingBitmap,
    }

    public sealed class XGraphics : IDisposable
    {
        XGraphics(PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit pageUnit, XPageDirection pageDirection)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (page.Owner == null)
                throw new ArgumentException("You cannot draw on a page that is not owned by a PdfDocument object.", "page");

            if (page.RenderContent != null)
                throw new InvalidOperationException("An XGraphics object already exists for this page and must be disposed before a new one can be created.");

            if (page.Owner.IsReadOnly)
                throw new InvalidOperationException("Cannot create XGraphics for a page of a document that cannot be modified. Use PdfDocumentOpenMode.Modify.");

            _gsStack = new GraphicsStateStack(this);
            PdfContent content = null;
            switch (options)
            {
                case XGraphicsPdfPageOptions.Replace:
                    page.Contents.Elements.Clear();
                    goto case XGraphicsPdfPageOptions.Append;

                case XGraphicsPdfPageOptions.Prepend:
                    content = page.Contents.PrependContent();
                    break;

                case XGraphicsPdfPageOptions.Append:
                    content = page.Contents.AppendContent();
                    break;
            }
            page.RenderContent = content;

#if CORE
            TargetContext = XGraphicTargetContext.CORE;
#endif
            _renderer = new PdfSharp.Drawing.Pdf.XGraphicsPdfRenderer(page, this, options);
            _pageSizePoints = new XSize(page.Width, page.Height);
            switch (pageUnit)
            {
                case XGraphicsUnit.Point:
                    _pageSize = new XSize(page.Width, page.Height);
                    break;

                case XGraphicsUnit.Inch:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Inch, XUnit.FromPoint(page.Height).Inch);
                    break;

                case XGraphicsUnit.Millimeter:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Millimeter, XUnit.FromPoint(page.Height).Millimeter);
                    break;

                case XGraphicsUnit.Centimeter:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Centimeter, XUnit.FromPoint(page.Height).Centimeter);
                    break;

                case XGraphicsUnit.Presentation:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Presentation, XUnit.FromPoint(page.Height).Presentation);
                    break;

                default:
                    throw new NotImplementedException("unit");
            }
            _pageUnit = pageUnit;
            _pageDirection = pageDirection;

            Initialize();
        }

        XGraphics(XForm form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            _form = form;
            form.AssociateGraphics(this);

            _gsStack = new GraphicsStateStack(this);
#if CORE
            TargetContext = XGraphicTargetContext.CORE;
            _drawGraphics = false;
            if (form.Owner != null)
                _renderer = new XGraphicsPdfRenderer(form, this);
            _pageSize = form.Size;
            Initialize();
#endif

        }

        public static XGraphics CreateMeasureContext(XSize size, XGraphicsUnit pageUnit, XPageDirection pageDirection)
        {
#if CORE
            PdfDocument dummy = new PdfDocument();
            PdfPage page = dummy.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append, pageUnit, pageDirection);
            return gfx;
#endif

        }



        public static XGraphics FromPdfPage(PdfPage page)
        {
            return new XGraphics(page, XGraphicsPdfPageOptions.Append, XGraphicsUnit.Point, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsUnit unit)
        {
            return new XGraphics(page, XGraphicsPdfPageOptions.Append, unit, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XPageDirection pageDirection)
        {
            return new XGraphics(page, XGraphicsPdfPageOptions.Append, XGraphicsUnit.Point, pageDirection);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options)
        {
            return new XGraphics(page, options, XGraphicsUnit.Point, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options, XPageDirection pageDirection)
        {
            return new XGraphics(page, options, XGraphicsUnit.Point, pageDirection);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit unit)
        {
            return new XGraphics(page, options, unit, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit unit, XPageDirection pageDirection)
        {
            return new XGraphics(page, options, unit, pageDirection);
        }

        public static XGraphics FromPdfForm(XPdfForm form)
        {
            if (form.Gfx != null)
                return form.Gfx;

            return new XGraphics(form);
        }

        public static XGraphics FromForm(XForm form)
        {
            if (form.Gfx != null)
                return form.Gfx;

            return new XGraphics(form);
        }

        public static XGraphics FromImage(XImage image)
        {
            return FromImage(image, XGraphicsUnit.Point);
        }

        public static XGraphics FromImage(XImage image, XGraphicsUnit unit)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            XBitmapImage bmImage = image as XBitmapImage;
            if (bmImage != null)
            {
#if CORE
                return null;
#endif
            }
            return null;
        }

        void Initialize()
        {
            _pageOrigin = new XPoint();

            double pageHeight = _pageSize.Height;
            PdfPage targetPage = PdfPage;
            XPoint trimOffset = new XPoint();
            if (targetPage != null && targetPage.TrimMargins.AreSet)
            {
                pageHeight += targetPage.TrimMargins.Top.Point + targetPage.TrimMargins.Bottom.Point;
                trimOffset = new XPoint(targetPage.TrimMargins.Left.Point, targetPage.TrimMargins.Top.Point);
            }

            XMatrix matrix = new XMatrix();
#if CORE
            Debug.Assert(TargetContext == XGraphicTargetContext.CORE);
#endif
            if (_pageDirection != XPageDirection.Downwards)
                matrix.Prepend(new XMatrix(1, 0, 0, -1, 0, pageHeight));

            if (trimOffset != new XPoint())
                matrix.TranslatePrepend(trimOffset.X, -trimOffset.Y);

            DefaultViewMatrix = matrix;
            _transform = new XMatrix();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    if (_associatedImage != null)
                    {
                        _associatedImage.DisassociateWithGraphics(this);
                        _associatedImage = null;
                    }
                }

                if (_form != null)
                    _form.Finish();
                _drawGraphics = false;

                if (_renderer != null)
                {
                    _renderer.Close();
                    _renderer = null;
                }
            }
        }
        bool _disposed;

        public PdfFontEncoding MUH     
        {
            get { return _muh; }
            set { _muh = value; }
        }
        PdfFontEncoding _muh;

        internal XGraphicTargetContext TargetContext;

        public XGraphicsUnit PageUnit
        {
            get { return _pageUnit; }
        }
        readonly XGraphicsUnit _pageUnit;

        public XPageDirection PageDirection
        {
            get { return _pageDirection; }
            set
            {
                if (value != XPageDirection.Downwards)
                    throw new NotImplementedException("PageDirection must be XPageDirection.Downwards in current implementation.");
            }
        }
        readonly XPageDirection _pageDirection;

        public XPoint PageOrigin
        {
            get { return _pageOrigin; }
            set
            {
                if (value != new XPoint())
                    throw new NotImplementedException("PageOrigin cannot be modified in current implementation.");
            }
        }
        XPoint _pageOrigin;

        public XSize PageSize
        {
            get { return _pageSize; }
        }
        XSize _pageSize;
        XSize _pageSizePoints;


        public void DrawLine(XPen pen, XPoint pt1, XPoint pt2)
        {
            DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void DrawLine(XPen pen, double x1, double y1, double x2, double y2)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawLines(pen, new[] { new XPoint(x1, y1), new XPoint(x2, y2) });
        }

        public void DrawLines(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawLines(pen, points);
        }

        public void DrawLines(XPen pen, double x, double y, params double[] value)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (value == null)
                throw new ArgumentNullException("value");

            int length = value.Length;
            XPoint[] points = new XPoint[length / 2 + 1];
            points[0].X = x;
            points[0].Y = y;
            for (int idx = 0; idx < length / 2; idx++)
            {
                points[idx + 1].X = value[2 * idx];
                points[idx + 1].Y = value[2 * idx + 1];
            }
            DrawLines(pen, points);
        }



        public void DrawBezier(XPen pen, XPoint pt1, XPoint pt2, XPoint pt3, XPoint pt4)
        {
            DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void DrawBezier(XPen pen, double x1, double y1, double x2, double y2,
          double x3, double y3, double x4, double y4)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawBeziers(pen,
                  new XPoint[] { new XPoint(x1, y1), new XPoint(x2, y2), new XPoint(x3, y3), new XPoint(x4, y4) });
        }


        public void DrawBeziers(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            int count = points.Length;
            if (count == 0)
                return;

            if ((count - 1) % 3 != 0)
                throw new ArgumentException("Invalid number of points for bezier curves. Number must fulfill 4+3n.", "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawBeziers(pen, points);
        }



        public void DrawCurve(XPen pen, XPoint[] points)
        {
            DrawCurve(pen, points, 0.5);
        }


        public void DrawCurve(XPen pen, XPoint[] points, int offset, int numberOfSegments, double tension)
        {
            XPoint[] points2 = new XPoint[numberOfSegments];
            Array.Copy(points, offset, points2, 0, numberOfSegments);
            DrawCurve(pen, points2, tension);
        }

        public void DrawCurve(XPen pen, XPoint[] points, double tension)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("DrawCurve requires two or more points.", "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawCurve(pen, points, tension);
        }



        public void DrawArc(XPen pen, XRect rect, double startAngle, double sweepAngle)
        {
            DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (Math.Abs(sweepAngle) >= 360)
            {
                DrawEllipse(pen, x, y, width, height);
            }
            else
            {
                if (_drawGraphics)
                {

                }

                if (_renderer != null)
                    _renderer.DrawArc(pen, x, y, width, height, startAngle, sweepAngle);
            }
        }


        public void DrawRectangle(XPen pen, XRect rect)
        {
            DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(XPen pen, double x, double y, double width, double height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRectangle(pen, null, x, y, width, height);
        }


        public void DrawRectangle(XBrush brush, XRect rect)
        {
            DrawRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(XBrush brush, double x, double y, double width, double height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRectangle(null, brush, x, y, width, height);
        }


        public void DrawRectangle(XPen pen, XBrush brush, XRect rect)
        {
            DrawRectangle(pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(XPen pen, XBrush brush, double x, double y, double width, double height)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRectangle(pen, brush, x, y, width, height);
        }

        public void DrawRectangles(XPen pen, XRect[] rectangles)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (rectangles == null)
                throw new ArgumentNullException("rectangles");

            DrawRectangles(pen, null, rectangles);
        }


        public void DrawRectangles(XBrush brush, XRect[] rectangles)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (rectangles == null)
                throw new ArgumentNullException("rectangles");

            DrawRectangles(null, brush, rectangles);
        }

        public void DrawRectangles(XPen pen, XBrush brush, XRect[] rectangles)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            if (rectangles == null)
                throw new ArgumentNullException("rectangles");

            int count = rectangles.Length;
            if (_drawGraphics)
            {

            }

            if (_renderer != null)
            {
                for (int idx = 0; idx < count; idx++)
                {
                    XRect rect = rectangles[idx];
                    _renderer.DrawRectangle(pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }



        public void DrawRoundedRectangle(XPen pen, XRect rect, XSize ellipseSize)
        {
            DrawRoundedRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
        }

        public void DrawRoundedRectangle(XPen pen, double x, double y, double width, double height, double ellipseWidth, double ellipseHeight)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            DrawRoundedRectangle(pen, null, x, y, width, height, ellipseWidth, ellipseHeight);
        }



        public void DrawRoundedRectangle(XBrush brush, XRect rect, XSize ellipseSize)
        {
            DrawRoundedRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
        }

        public void DrawRoundedRectangle(XBrush brush, double x, double y, double width, double height, double ellipseWidth, double ellipseHeight)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            DrawRoundedRectangle(null, brush, x, y, width, height, ellipseWidth, ellipseHeight);
        }

        public void DrawRoundedRectangle(XPen pen, XBrush brush, XRect rect, XSize ellipseSize)
        {
            DrawRoundedRectangle(pen, brush, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
        }

        public void DrawRoundedRectangle(XPen pen, XBrush brush, double x, double y, double width, double height,
            double ellipseWidth, double ellipseHeight)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRoundedRectangle(pen, brush, x, y, width, height, ellipseWidth, ellipseHeight);
        }


        public void DrawEllipse(XPen pen, XRect rect)
        {
            DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(XPen pen, double x, double y, double width, double height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawEllipse(pen, null, x, y, width, height);
        }


        public void DrawEllipse(XBrush brush, XRect rect)
        {
            DrawEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(XBrush brush, double x, double y, double width, double height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawEllipse(null, brush, x, y, width, height);
        }


        public void DrawEllipse(XPen pen, XBrush brush, XRect rect)
        {
            DrawEllipse(pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(XPen pen, XBrush brush, double x, double y, double width, double height)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawEllipse(pen, brush, x, y, width, height);
        }


        public void DrawPolygon(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPolygon(pen, null, points, XFillMode.Alternate);     
        }


        public void DrawPolygon(XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPolygon(null, brush, points, fillmode);
        }



        public void DrawPolygon(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPolygon(pen, brush, points, fillmode);
        }



        public void DrawPie(XPen pen, XRect rect, double startAngle, double sweepAngle)
        {
            DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPie(pen, null, x, y, width, height, startAngle, sweepAngle);
        }



        public void DrawPie(XBrush brush, XRect rect, double startAngle, double sweepAngle)
        {
            DrawPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(XBrush brush, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (brush == null)
                throw new ArgumentNullException("brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {


            }

            if (_renderer != null)
                _renderer.DrawPie(null, brush, x, y, width, height, startAngle, sweepAngle);
        }


        public void DrawPie(XPen pen, XBrush brush, XRect rect, double startAngle, double sweepAngle)
        {
            DrawPie(pen, brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(XPen pen, XBrush brush, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPie(pen, brush, x, y, width, height, startAngle, sweepAngle);
        }



        public void DrawClosedCurve(XPen pen, XPoint[] points)
        {
            DrawClosedCurve(pen, null, points, XFillMode.Alternate, 0.5);
        }



        public void DrawClosedCurve(XPen pen, XPoint[] points, double tension)
        {
            DrawClosedCurve(pen, null, points, XFillMode.Alternate, tension);
        }


        public void DrawClosedCurve(XBrush brush, XPoint[] points)
        {
            DrawClosedCurve(null, brush, points, XFillMode.Alternate, 0.5);
        }


        public void DrawClosedCurve(XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            DrawClosedCurve(null, brush, points, fillmode, 0.5);
        }



        public void DrawClosedCurve(XBrush brush, XPoint[] points, XFillMode fillmode, double tension)
        {
            DrawClosedCurve(null, brush, points, fillmode, tension);
        }


        public void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points)
        {
            DrawClosedCurve(pen, brush, points, XFillMode.Alternate, 0.5);
        }


        public void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            DrawClosedCurve(pen, brush, points, fillmode, 0.5);
        }


        public void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode, double tension)
        {
            if (pen == null && brush == null)
            {
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            }

            int count = points.Length;
            if (count == 0)
                return;
            if (count < 2)
                throw new ArgumentException("Not enough points.", "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawClosedCurve(pen, brush, points, tension, fillmode);
        }

        public void DrawPath(XPen pen, XGraphicsPath path)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPath(pen, null, path);
        }

        public void DrawPath(XBrush brush, XGraphicsPath path)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPath(null, brush, path);
        }

        public void DrawPath(XPen pen, XBrush brush, XGraphicsPath path)
        {
            if (pen == null && brush == null)
            {
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            }
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPath(pen, brush, path);
        }

        public void DrawString(string s, XFont font, XBrush brush, XPoint point)
        {
            DrawString(s, font, brush, new XRect(point.X, point.Y, 0, 0), XStringFormats.Default);
        }

        public void DrawString(string s, XFont font, XBrush brush, XPoint point, XStringFormat format)
        {
            DrawString(s, font, brush, new XRect(point.X, point.Y, 0, 0), format);
        }

        public void DrawString(string s, XFont font, XBrush brush, double x, double y)
        {
            DrawString(s, font, brush, new XRect(x, y, 0, 0), XStringFormats.Default);
        }

        public void DrawString(string s, XFont font, XBrush brush, double x, double y, XStringFormat format)
        {
            DrawString(s, font, brush, new XRect(x, y, 0, 0), format);
        }


        public void DrawString(string s, XFont font, XBrush brush, XRect layoutRectangle)
        {
            DrawString(s, font, brush, layoutRectangle, XStringFormats.Default);
        }

        public void DrawString(string text, XFont font, XBrush brush, XRect layoutRectangle, XStringFormat format)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (font == null)
                throw new ArgumentNullException("font");
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (format != null && format.LineAlignment == XLineAlignment.BaseLine && layoutRectangle.Height != 0)
                throw new InvalidOperationException("DrawString: With XLineAlignment.BaseLine the height of the layout rectangle must be 0.");

            if (text.Length == 0)
                return;

            if (format == null)
                format = XStringFormats.Default;
            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawString(text, font, brush, layoutRectangle, format);
        }

        public XSize MeasureString(string text, XFont font, XStringFormat stringFormat)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (font == null)
                throw new ArgumentNullException("font");
            if (stringFormat == null)
                throw new ArgumentNullException("stringFormat");

            return FontHelper.MeasureString(text, font, stringFormat);




            XSize size = FontHelper.MeasureString(text, font, XStringFormats.Default);
            return size;


        }

        public XSize MeasureString(string text, XFont font)
        {
            return MeasureString(text, font, XStringFormats.Default);
        }


        public void DrawImage(XImage image, XPoint point)
        {
            DrawImage(image, point.X, point.Y);
        }

        public void DrawImage(XImage image, double x, double y)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            CheckXPdfFormConsistence(image);

            double width = image.PointWidth;
            double height = image.PointHeight;

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawImage(image, x, y, image.PointWidth, image.PointHeight);
        }


        public void DrawImage(XImage image, XRect rect)
        {
            DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawImage(XImage image, double x, double y, double width, double height)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            CheckXPdfFormConsistence(image);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawImage(image, x, y, width, height);
        }


        public void DrawImage(XImage image, XRect destRect, XRect srcRect, XGraphicsUnit srcUnit)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            CheckXPdfFormConsistence(image);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawImage(image, destRect, srcRect, srcUnit);
        }

        void DrawMissingImageRect(XRect rect)
        {


        }

        void CheckXPdfFormConsistence(XImage image)
        {
            XForm xForm = image as XForm;
            if (xForm != null)
            {
                xForm.Finish();

                if (_renderer != null && (_renderer as XGraphicsPdfRenderer) != null)
                {
                    if (xForm.Owner != null && xForm.Owner != ((XGraphicsPdfRenderer)_renderer).Owner)
                        throw new InvalidOperationException(
                            "A XPdfForm object is always bound to the document it was created for and cannot be drawn in the context of another document.");

                    if (xForm == ((XGraphicsPdfRenderer)_renderer)._form)
                        throw new InvalidOperationException(
                            "A XPdfForm cannot be drawn on itself.");
                }
            }
        }

        public void DrawBarCode(BarCodes.BarCode barcode, XPoint position)
        {
            barcode.Render(this, XBrushes.Black, null, position);
        }

        public void DrawBarCode(BarCodes.BarCode barcode, XBrush brush, XPoint position)
        {
            barcode.Render(this, brush, null, position);
        }

        public void DrawBarCode(BarCodes.BarCode barcode, XBrush brush, XFont font, XPoint position)
        {
            barcode.Render(this, brush, font, position);
        }

        public void DrawMatrixCode(BarCodes.MatrixCode matrixcode, XPoint position)
        {
            matrixcode.Render(this, XBrushes.Black, position);
        }

        public void DrawMatrixCode(BarCodes.MatrixCode matrixcode, XBrush brush, XPoint position)
        {
            matrixcode.Render(this, brush, position);
        }

        public XGraphicsState Save()
        {
            XGraphicsState xState = null;
#if CORE || NETFX_CORE
            if (TargetContext == XGraphicTargetContext.CORE || TargetContext == XGraphicTargetContext.NONE)
            {
                xState = new XGraphicsState();
                InternalGraphicsState iState = new InternalGraphicsState(this, xState);
                iState.Transform = _transform;
                _gsStack.Push(iState);
            }
            else
            {
                Debug.Assert(false, "XGraphicTargetContext must be XGraphicTargetContext.CORE.");
            }
#endif

            if (_renderer != null)
                _renderer.Save(xState);

            return xState;
        }

        public void Restore(XGraphicsState state)
        {
            if (state == null)
                throw new ArgumentNullException("state");

#if CORE
            if (TargetContext == XGraphicTargetContext.CORE)
            {
                _gsStack.Restore(state.InternalState);
                _transform = state.InternalState.Transform;
            }
#endif


            if (_renderer != null)
                _renderer.Restore(state);
        }

        public void Restore()
        {
            if (_gsStack.Count == 0)
                throw new InvalidOperationException("Cannot restore without preceding save operation.");
            Restore(_gsStack.Current.State);
        }

        public XGraphicsContainer BeginContainer()
        {
            return BeginContainer(new XRect(0, 0, 1, 1), new XRect(0, 0, 1, 1), XGraphicsUnit.Point);
        }


        public XGraphicsContainer BeginContainer(XRect dstrect, XRect srcrect, XGraphicsUnit unit)
        {
            if (unit != XGraphicsUnit.Point)
                throw new ArgumentException("The current implementation supports XGraphicsUnit.Point only.", "unit");

            XGraphicsContainer xContainer = null;
#if CORE
            if (TargetContext == XGraphicTargetContext.CORE)
                xContainer = new XGraphicsContainer();
#endif

            InternalGraphicsState iState = new InternalGraphicsState(this, xContainer);
            iState.Transform = _transform;

            _gsStack.Push(iState);

            if (_renderer != null)
                _renderer.BeginContainer(xContainer, dstrect, srcrect, unit);

            XMatrix matrix = new XMatrix();
            double scaleX = dstrect.Width / srcrect.Width;
            double scaleY = dstrect.Height / srcrect.Height;
            matrix.TranslatePrepend(-srcrect.X, -srcrect.Y);
            matrix.ScalePrepend(scaleX, scaleY);
            matrix.TranslatePrepend(dstrect.X / scaleX, dstrect.Y / scaleY);
            AddTransform(matrix, XMatrixOrder.Prepend);

            return xContainer;
        }

        public void EndContainer(XGraphicsContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _gsStack.Restore(container.InternalState);

            _transform = container.InternalState.Transform;

            if (_renderer != null)
                _renderer.EndContainer(container);
        }

        public int GraphicsStateLevel
        {
            get { return _gsStack.Count; }
        }

        public XSmoothingMode SmoothingMode
        {
            get
            {
                return _smoothingMode;
            }
            set
            {
                _smoothingMode = value;

            }
        }
        XSmoothingMode _smoothingMode;

        public void TranslateTransform(double dx, double dy)
        {
            AddTransform(XMatrix.CreateTranslation(dx, dy), XMatrixOrder.Prepend);
        }

        public void TranslateTransform(double dx, double dy, XMatrixOrder order)
        {
            XMatrix matrix = new XMatrix();
            matrix.TranslatePrepend(dx, dy);
            AddTransform(matrix, order);
        }

        public void ScaleTransform(double scaleX, double scaleY)
        {
            AddTransform(XMatrix.CreateScaling(scaleX, scaleY), XMatrixOrder.Prepend);
        }

        public void ScaleTransform(double scaleX, double scaleY, XMatrixOrder order)
        {
            XMatrix matrix = new XMatrix();
            matrix.ScalePrepend(scaleX, scaleY);
            AddTransform(matrix, order);
        }

        public void ScaleTransform(double scaleXY)
        {
            ScaleTransform(scaleXY, scaleXY);
        }

        public void ScaleTransform(double scaleXY, XMatrixOrder order)
        {
            ScaleTransform(scaleXY, scaleXY, order);
        }

        public void ScaleAtTransform(double scaleX, double scaleY, double centerX, double centerY)
        {
            AddTransform(XMatrix.CreateScaling(scaleX, scaleY, centerX, centerY), XMatrixOrder.Prepend);
        }

        public void ScaleAtTransform(double scaleX, double scaleY, XPoint center)
        {
            AddTransform(XMatrix.CreateScaling(scaleX, scaleY, center.X, center.Y), XMatrixOrder.Prepend);
        }

        public void RotateTransform(double angle)
        {
            AddTransform(XMatrix.CreateRotationRadians(angle * Const.Deg2Rad), XMatrixOrder.Prepend);
        }

        public void RotateTransform(double angle, XMatrixOrder order)
        {
            XMatrix matrix = new XMatrix();
            matrix.RotatePrepend(angle);
            AddTransform(matrix, order);
        }

        public void RotateAtTransform(double angle, XPoint point)
        {
            AddTransform(XMatrix.CreateRotationRadians(angle * Const.Deg2Rad, point.X, point.Y), XMatrixOrder.Prepend);
        }

        public void RotateAtTransform(double angle, XPoint point, XMatrixOrder order)
        {
            AddTransform(XMatrix.CreateRotationRadians(angle * Const.Deg2Rad, point.X, point.Y), order);
        }

        public void ShearTransform(double shearX, double shearY)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad), XMatrixOrder.Prepend);
        }

        public void ShearTransform(double shearX, double shearY, XMatrixOrder order)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad), order);
        }

        public void SkewAtTransform(double shearX, double shearY, double centerX, double centerY)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad, centerX, centerY), XMatrixOrder.Prepend);
        }

        public void SkewAtTransform(double shearX, double shearY, XPoint center)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad, center.X, center.Y), XMatrixOrder.Prepend);
        }

        public void MultiplyTransform(XMatrix matrix)
        {
            AddTransform(matrix, XMatrixOrder.Prepend);
        }

        public void MultiplyTransform(XMatrix matrix, XMatrixOrder order)
        {
            AddTransform(matrix, order);
        }

        public XMatrix Transform
        {
            get { return _transform; }
        }

        void AddTransform(XMatrix transform, XMatrixOrder order)
        {
            XMatrix matrix = _transform;
            matrix.Multiply(transform, order);
            _transform = matrix;
            matrix = DefaultViewMatrix;
            matrix.Multiply(_transform, XMatrixOrder.Prepend);
#if CORE
            if (TargetContext == XGraphicTargetContext.CORE)
            {
                GetType();
            }
#endif

            if (_renderer != null)
                _renderer.AddTransform(transform, XMatrixOrder.Prepend);
        }


        public void IntersectClip(XRect rect)
        {
            XGraphicsPath path = new XGraphicsPath();
            path.AddRectangle(rect);
            IntersectClip(path);
        }

        public void IntersectClip(XGraphicsPath path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.SetClip(path, XCombineMode.Intersect);
        }

        public void WriteComment(string comment)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");

            if (_drawGraphics)
            {
            }

            if (_renderer != null)
                _renderer.WriteComment(comment);
        }

        public XGraphicsInternals Internals
        {
            get { return _internals ?? (_internals = new XGraphicsInternals(this)); }
        }
        XGraphicsInternals _internals;

        public SpaceTransformer Transformer
        {
            get { return _transformer ?? (_transformer = new SpaceTransformer(this)); }
        }
        SpaceTransformer _transformer;

        internal void DisassociateImage()
        {
            if (_associatedImage == null)
                throw new InvalidOperationException("No image associated.");

            Dispose();
        }

        internal InternalGraphicsMode InternalGraphicsMode
        {
            get { return _internalGraphicsMode; }
            set { _internalGraphicsMode = value; }
        }
        InternalGraphicsMode _internalGraphicsMode;

        internal XImage AssociatedImage
        {
            get { return _associatedImage; }
            set { _associatedImage = value; }
        }
        XImage _associatedImage;

        internal XMatrix DefaultViewMatrix;

        bool _drawGraphics;

        readonly XForm _form;


        IXGraphicsRenderer _renderer;

        XMatrix _transform;

        readonly GraphicsStateStack _gsStack;

        public PdfPage PdfPage
        {
            get
            {
                XGraphicsPdfRenderer renderer = _renderer as PdfSharp.Drawing.Pdf.XGraphicsPdfRenderer;
                return renderer != null ? renderer._page : null;
            }
        }


        public class XGraphicsInternals
        {
            internal XGraphicsInternals(XGraphics gfx)
            {
                _gfx = gfx;
            }
            readonly XGraphics _gfx;

        }

        public class SpaceTransformer
        {
            internal SpaceTransformer(XGraphics gfx)
            {
                _gfx = gfx;
            }
            readonly XGraphics _gfx;

            public XRect WorldToDefaultPage(XRect rect)
            {
                XPoint[] points = new XPoint[4];
                points[0] = new XPoint(rect.X, rect.Y);
                points[1] = new XPoint(rect.X + rect.Width, rect.Y);
                points[2] = new XPoint(rect.X, rect.Y + rect.Height);
                points[3] = new XPoint(rect.X + rect.Width, rect.Y + rect.Height);

                XMatrix matrix = _gfx.Transform;
                matrix.TransformPoints(points);

                double height = _gfx.PageSize.Height;
                points[0].Y = height - points[0].Y;
                points[1].Y = height - points[1].Y;
                points[2].Y = height - points[2].Y;
                points[3].Y = height - points[3].Y;

                double xmin = Math.Min(Math.Min(points[0].X, points[1].X), Math.Min(points[2].X, points[3].X));
                double xmax = Math.Max(Math.Max(points[0].X, points[1].X), Math.Max(points[2].X, points[3].X));
                double ymin = Math.Min(Math.Min(points[0].Y, points[1].Y), Math.Min(points[2].Y, points[3].Y));
                double ymax = Math.Max(Math.Max(points[0].Y, points[1].Y), Math.Max(points[2].Y, points[3].Y));

                return new XRect(xmin, ymin, xmax - xmin, ymax - ymin);
            }
        }
    }
}
