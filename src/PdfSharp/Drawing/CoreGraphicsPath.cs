using System;
using System.Collections.Generic;
using System.Diagnostics;
using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    internal class CoreGraphicsPath
    {
        const byte PathPointTypeStart = 0;   
        const byte PathPointTypeLine = 1;   
        const byte PathPointTypeBezier = 3;       
        const byte PathPointTypePathTypeMask = 0x07;       
        const byte PathPointTypeCloseSubpath = 0x80;    

        public CoreGraphicsPath()
        { }

        public CoreGraphicsPath(CoreGraphicsPath path)
        {
            _points = new List<XPoint>(path._points);
            _types = new List<byte>(path._types);
        }

        public void MoveOrLineTo(double x, double y)
        {
            if (_types.Count == 0 || (_types[_types.Count - 1] & PathPointTypeCloseSubpath) == PathPointTypeCloseSubpath)
                MoveTo(x, y);
            else
                LineTo(x, y, false);
        }

        public void MoveTo(double x, double y)
        {
            _points.Add(new XPoint(x, y));
            _types.Add(PathPointTypeStart);
        }

        public void LineTo(double x, double y, bool closeSubpath)
        {
            if (_points.Count > 0 && _points[_points.Count - 1].Equals(new XPoint(x, y)))
                return;

            _points.Add(new XPoint(x, y));
            _types.Add((byte)(PathPointTypeLine | (closeSubpath ? PathPointTypeCloseSubpath : 0)));
        }

        public void BezierTo(double x1, double y1, double x2, double y2, double x3, double y3, bool closeSubpath)
        {
            _points.Add(new XPoint(x1, y1));
            _types.Add(PathPointTypeBezier);
            _points.Add(new XPoint(x2, y2));
            _types.Add(PathPointTypeBezier);
            _points.Add(new XPoint(x3, y3));
            _types.Add((byte)(PathPointTypeBezier | (closeSubpath ? PathPointTypeCloseSubpath : 0)));
        }

        public void QuadrantArcTo(double x, double y, double width, double height, int quadrant, bool clockwise)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException("width");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height");

            double w = Const.κ * width;
            double h = Const.κ * height;
            double x1, y1, x2, y2, x3, y3;
            switch (quadrant)
            {
                case 1:
                    if (clockwise)
                    {
                        x1 = x + w;
                        y1 = y - height;
                        x2 = x + width;
                        y2 = y - h;
                        x3 = x + width;
                        y3 = y;
                    }
                    else
                    {
                        x1 = x + width;
                        y1 = y - h;
                        x2 = x + w;
                        y2 = y - height;
                        x3 = x;
                        y3 = y - height;
                    }
                    break;

                case 2:
                    if (clockwise)
                    {
                        x1 = x - width;
                        y1 = y - h;
                        x2 = x - w;
                        y2 = y - height;
                        x3 = x;
                        y3 = y - height;
                    }
                    else
                    {
                        x1 = x - w;
                        y1 = y - height;
                        x2 = x - width;
                        y2 = y - h;
                        x3 = x - width;
                        y3 = y;
                    }
                    break;

                case 3:
                    if (clockwise)
                    {
                        x1 = x - w;
                        y1 = y + height;
                        x2 = x - width;
                        y2 = y + h;
                        x3 = x - width;
                        y3 = y;
                    }
                    else
                    {
                        x1 = x - width;
                        y1 = y + h;
                        x2 = x - w;
                        y2 = y + height;
                        x3 = x;
                        y3 = y + height;
                    }
                    break;

                case 4:
                    if (clockwise)
                    {
                        x1 = x + width;
                        y1 = y + h;
                        x2 = x + w;
                        y2 = y + height;
                        x3 = x;
                        y3 = y + height;
                    }
                    else
                    {
                        x1 = x + w;
                        y1 = y + height;
                        x2 = x + width;
                        y2 = y + h;
                        x3 = x + width;
                        y3 = y;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("quadrant");
            }
            BezierTo(x1, y1, x2, y2, x3, y3, false);
        }

        public void CloseSubpath()
        {
            int count = _types.Count;
            if (count > 0)
                _types[count - 1] |= PathPointTypeCloseSubpath;
        }

        XFillMode FillMode
        {
            get { return _fillMode; }
            set { _fillMode = value; }
        }
        XFillMode _fillMode;

        public void AddArc(double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            XMatrix matrix = XMatrix.Identity;
            List<XPoint> points = GeometryHelper.BezierCurveFromArc(x, y, width, height, startAngle, sweepAngle, PathStart.MoveTo1st, ref matrix);
            int count = points.Count;
            Debug.Assert((count + 2) % 3 == 0);

            MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
                BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y, points[idx + 2].X, points[idx + 2].Y, false);
        }

        public void AddArc(XPoint point1, XPoint point2, XSize size, double rotationAngle, bool isLargeArg, XSweepDirection sweepDirection)
        {
            List<XPoint> points = GeometryHelper.BezierCurveFromArc(point1, point2, size, rotationAngle, isLargeArg,
                sweepDirection == XSweepDirection.Clockwise, PathStart.MoveTo1st);
            int count = points.Count;
            Debug.Assert((count + 2) % 3 == 0);

            MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
                BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y, points[idx + 2].X, points[idx + 2].Y, false);
        }

        public void AddCurve(XPoint[] points, double tension)
        {
            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("AddCurve requires two or more points.", "points");

            tension /= 3;
            MoveOrLineTo(points[0].X, points[0].Y);
            if (count == 2)
            {
                ToCurveSegment(points[0], points[0], points[1], points[1], tension);
            }
            else
            {
                ToCurveSegment(points[0], points[0], points[1], points[2], tension);
                for (int idx = 1; idx < count - 2; idx++)
                {
                    ToCurveSegment(points[idx - 1], points[idx], points[idx + 1], points[idx + 2], tension);
                }
                ToCurveSegment(points[count - 3], points[count - 2], points[count - 1], points[count - 1], tension);
            }
        }

        void ToCurveSegment(XPoint pt0, XPoint pt1, XPoint pt2, XPoint pt3, double tension3)
        {
            BezierTo(
                pt1.X + tension3 * (pt2.X - pt0.X), pt1.Y + tension3 * (pt2.Y - pt0.Y),
                pt2.X - tension3 * (pt3.X - pt1.X), pt2.Y - tension3 * (pt3.Y - pt1.Y),
                pt2.X, pt2.Y,
                false);
        }

        public XPoint[] PathPoints { get { return _points.ToArray(); } }

        public byte[] PathTypes { get { return _types.ToArray(); } }

        readonly List<XPoint> _points = new List<XPoint>();
        readonly List<byte> _types = new List<byte>();
    }
}
