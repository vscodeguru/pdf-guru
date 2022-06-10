using System;
using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    public sealed class XGraphicsPath
    {
        public XGraphicsPath()
        {

        }


        public XGraphicsPath Clone()
        {
            XGraphicsPath path = (XGraphicsPath)MemberwiseClone();
            return path;
        }

        public void AddLine(XPoint pt1, XPoint pt2)
        {
            AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void AddLine(double x1, double y1, double x2, double y2)
        {
#if CORE
            _corePath.MoveOrLineTo(x1, y1);
            _corePath.LineTo(x2, y2, false);
#endif
        }

        public void AddLines(XPoint[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count == 0)
                return;
#if CORE
            _corePath.MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx++)
                _corePath.LineTo(points[idx].X, points[idx].Y, false);
#endif
        }


        public void AddBezier(XPoint pt1, XPoint pt2, XPoint pt3, XPoint pt4)
        {
            AddBezier(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void AddBezier(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
#if CORE
            _corePath.MoveOrLineTo(x1, y1);
            _corePath.BezierTo(x2, y2, x3, y3, x4, y4, false);
#endif
        }

        public void AddBeziers(XPoint[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count < 4)
                throw new ArgumentException("At least four points required for bezier curve.", "points");

            if ((count - 1) % 3 != 0)
                throw new ArgumentException("Invalid number of points for bezier curve. Number must fulfil 4+3n.",
                    "points");

#if CORE
            _corePath.MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
            {
                _corePath.BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y,
                    points[idx + 2].X, points[idx + 2].Y, false);
            }
#endif
        }


        public void AddCurve(XPoint[] points)
        {
            AddCurve(points, 0.5);
        }

        public void AddCurve(XPoint[] points, double tension)
        {
            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("AddCurve requires two or more points.", "points");
#if CORE
            _corePath.AddCurve(points, tension);
#endif

        }


        public void AddCurve(XPoint[] points, int offset, int numberOfSegments, double tension)
        {
#if CORE
            throw new NotImplementedException("AddCurve not yet implemented.");
#endif
        }

        public void AddArc(XRect rect, double startAngle, double sweepAngle)
        {
            AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddArc(double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
#if CORE
            _corePath.AddArc(x, y, width, height, startAngle, sweepAngle);
#endif
        }

        public void AddArc(XPoint point1, XPoint point2, XSize size, double rotationAngle, bool isLargeArg, XSweepDirection sweepDirection)
        {
        }
        public void AddRectangle(XRect rect)
        {
#if CORE
            _corePath.MoveTo(rect.X, rect.Y);
            _corePath.LineTo(rect.X + rect.Width, rect.Y, false);
            _corePath.LineTo(rect.X + rect.Width, rect.Y + rect.Height, false);
            _corePath.LineTo(rect.X, rect.Y + rect.Height, true);
            _corePath.CloseSubpath();
#endif
        }

        public void AddRectangle(double x, double y, double width, double height)
        {
            AddRectangle(new XRect(x, y, width, height));
        }

        public void AddRectangles(XRect[] rects)
        {
            int count = rects.Length;
            for (int idx = 0; idx < count; idx++)
            {
#if CORE
                AddRectangle(rects[idx]);
#endif
            }
        }


        public void AddRoundedRectangle(double x, double y, double width, double height, double ellipseWidth, double ellipseHeight)
        {
#if CORE
#if true
            double arcWidth = ellipseWidth / 2;
            double arcHeight = ellipseHeight / 2;
#if true   
            _corePath.MoveTo(x + width - arcWidth, y);
            _corePath.QuadrantArcTo(x + width - arcWidth, y + arcHeight, arcWidth, arcHeight, 1, true);

            _corePath.LineTo(x + width, y + height - arcHeight, false);
            _corePath.QuadrantArcTo(x + width - arcWidth, y + height - arcHeight, arcWidth, arcHeight, 4, true);

            _corePath.LineTo(x + arcWidth, y + height, false);
            _corePath.QuadrantArcTo(x + arcWidth, y + height - arcHeight, arcWidth, arcHeight, 3, true);

            _corePath.LineTo(x, y + arcHeight, false);
            _corePath.QuadrantArcTo(x + arcWidth, y + arcHeight, arcWidth, arcHeight, 2, true);

            _corePath.CloseSubpath();
 
#endif
#endif
#endif

        }

        public void AddEllipse(XRect rect)
        {
            AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void AddEllipse(double x, double y, double width, double height)
        {
#if CORE
            double w = width / 2;
            double h = height / 2;
            double xc = x + w;
            double yc = y + h;
            _corePath.MoveTo(x + w, y);
            _corePath.QuadrantArcTo(xc, yc, w, h, 1, true);
            _corePath.QuadrantArcTo(xc, yc, w, h, 4, true);
            _corePath.QuadrantArcTo(xc, yc, w, h, 3, true);
            _corePath.QuadrantArcTo(xc, yc, w, h, 2, true);
            _corePath.CloseSubpath();
#endif
        }

        public void AddPolygon(XPoint[] points)
        {
#if CORE
            int count = points.Length;
            if (count == 0)
                return;

            _corePath.MoveTo(points[0].X, points[0].Y);
            for (int idx = 0; idx < count - 1; idx++)
                _corePath.LineTo(points[idx].X, points[idx].Y, false);
            _corePath.LineTo(points[count - 1].X, points[count - 1].Y, true);
            _corePath.CloseSubpath();
#endif

        }

        public void AddPie(XRect rect, double startAngle, double sweepAngle)
        {
            AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddPie(double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddPie");
#endif

        }

        public void AddClosedCurve(XPoint[] points)
        {
            AddClosedCurve(points, 0.5);
        }

        public void AddClosedCurve(XPoint[] points, double tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            int count = points.Length;
            if (count == 0)
                return;
            if (count < 2)
                throw new ArgumentException("Not enough points.", "points");

#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddClosedCurve");
#endif

        }

        public void AddPath(XGraphicsPath path, bool connect)
        {
#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddPath");
#endif

        }


        public void AddString(string s, XFontFamily family, XFontStyle style, double emSize, XPoint origin,
            XStringFormat format)
        {
            try
            {
#if CORE
                DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddString");
#endif

            }
            catch
            {
                throw;
            }
        }


        public void AddString(string s, XFontFamily family, XFontStyle style, double emSize, XRect layoutRect,
            XStringFormat format)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            if (family == null)
                throw new ArgumentNullException("family");

            if (format == null)
                format = XStringFormats.Default;

            if (format.LineAlignment == XLineAlignment.BaseLine && layoutRect.Height != 0)
                throw new InvalidOperationException(
                    "DrawString: With XLineAlignment.BaseLine the height of the layout rectangle must be 0.");

            if (s.Length == 0)
                return;

            XFont font = new XFont(family.Name, emSize, style);
#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddString");
#endif

        }

        public void CloseFigure()
        {
#if CORE
            _corePath.CloseSubpath();
#endif

        }

        public void StartFigure()
        {

        }

        public XFillMode FillMode
        {
            get { return _fillMode; }
            set
            {
                _fillMode = value;

            }
        }

        private XFillMode _fillMode;

        public void Flatten()
        {

        }

        public void Flatten(XMatrix matrix)
        {

        }

        public void Flatten(XMatrix matrix, double flatness)
        {

        }

        public void Widen(XPen pen)
        {

        }

        public void Widen(XPen pen, XMatrix matrix)
        {

        }

        public void Widen(XPen pen, XMatrix matrix, double flatness)
        {


        }

        public XGraphicsPathInternals Internals
        {
            get { return new XGraphicsPathInternals(this); }
        }

#if CORE
        internal CoreGraphicsPath _corePath;
#endif
    }
}