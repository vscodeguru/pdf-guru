using System;
using System.Diagnostics;
using System.Collections.Generic;


using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    static class GeometryHelper
    {


        public static List<XPoint> BezierCurveFromArc(double x, double y, double width, double height, double startAngle, double sweepAngle,
            PathStart pathStart, ref XMatrix matrix)
        {
            List<XPoint> points = new List<XPoint>();

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
                AppendPartialArcQuadrant(points, x, y, width, height, α, β, pathStart, matrix);
            else
            {
                int currentQuadrant = startQuadrant;
                bool firstLoop = true;
                do
                {
                    if (currentQuadrant == startQuadrant && firstLoop)
                    {
                        double ξ = currentQuadrant * 90 + (clockwise ? 90 : 0);
                        AppendPartialArcQuadrant(points, x, y, width, height, α, ξ, pathStart, matrix);
                    }
                    else if (currentQuadrant == endQuadrant)
                    {
                        double ξ = currentQuadrant * 90 + (clockwise ? 0 : 90);
                        AppendPartialArcQuadrant(points, x, y, width, height, ξ, β, PathStart.Ignore1st, matrix);
                    }
                    else
                    {
                        double ξ1 = currentQuadrant * 90 + (clockwise ? 0 : 90);
                        double ξ2 = currentQuadrant * 90 + (clockwise ? 90 : 0);
                        AppendPartialArcQuadrant(points, x, y, width, height, ξ1, ξ2, PathStart.Ignore1st, matrix);
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
            return points;
        }

        static int Quadrant(double φ, bool start, bool clockwise)
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

        static void AppendPartialArcQuadrant(List<XPoint> points, double x, double y, double width, double height, double α, double β, PathStart pathStart, XMatrix matrix)
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

            double cosα, cosβ, sinα, sinβ;
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
            cosα = Math.Cos(α);
            sinβ = Math.Sin(β);
            cosβ = Math.Cos(β);

            if (!reflect)
            {
                switch (pathStart)
                {
                    case PathStart.MoveTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 + δx * cosα, y0 + δy * sinα)));
                        break;

                    case PathStart.LineTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 + δx * cosα, y0 + δy * sinα)));
                        break;

                    case PathStart.Ignore1st:
                        break;
                }
                points.Add(matrix.Transform(new XPoint(x0 + δx * (cosα - κ * sinα), y0 + δy * (sinα + κ * cosα))));
                points.Add(matrix.Transform(new XPoint(x0 + δx * (cosβ + κ * sinβ), y0 + δy * (sinβ - κ * cosβ))));
                points.Add(matrix.Transform(new XPoint(x0 + δx * cosβ, y0 + δy * sinβ)));
            }
            else
            {
                switch (pathStart)
                {
                    case PathStart.MoveTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 - δx * cosα, y0 - δy * sinα)));
                        break;

                    case PathStart.LineTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 - δx * cosα, y0 - δy * sinα)));
                        break;

                    case PathStart.Ignore1st:
                        break;
                }
                points.Add(matrix.Transform(new XPoint(x0 - δx * (cosα - κ * sinα), y0 - δy * (sinα + κ * cosα))));
                points.Add(matrix.Transform(new XPoint(x0 - δx * (cosβ + κ * sinβ), y0 - δy * (sinβ - κ * cosβ))));
                points.Add(matrix.Transform(new XPoint(x0 - δx * cosβ, y0 - δy * sinβ)));
            }
        }

        public static List<XPoint> BezierCurveFromArc(XPoint point1, XPoint point2, XSize size,
            double rotationAngle, bool isLargeArc, bool clockwise, PathStart pathStart)
        {
            double δx = size.Width;
            double δy = size.Height;
            Debug.Assert(δx * δy > 0);
            double factor = δy / δx;
            bool isCounterclockwise = !clockwise;

            XMatrix matrix = new XMatrix();
            matrix.RotateAppend(-rotationAngle);
            matrix.ScaleAppend(δy / δx, 1);
            XPoint pt1 = matrix.Transform(point1);
            XPoint pt2 = matrix.Transform(point2);

            XPoint midPoint = new XPoint((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
            XVector vect = pt2 - pt1;
            double halfChord = vect.Length / 2;

            XVector vectRotated;

            if (isLargeArc == isCounterclockwise)
                vectRotated = new XVector(-vect.Y, vect.X);
            else
                vectRotated = new XVector(vect.Y, -vect.X);

            vectRotated.Normalize();

            double centerDistance = Math.Sqrt(δy * δy - halfChord * halfChord);
            if (double.IsNaN(centerDistance))
                centerDistance = 0;

            XPoint center = midPoint + centerDistance * vectRotated;

            double α = Math.Atan2(pt1.Y - center.Y, pt1.X - center.X);
            double β = Math.Atan2(pt2.Y - center.Y, pt2.X - center.X);

            if (isLargeArc == (Math.Abs(β - α) < Math.PI))
            {
                if (α < β)
                    α += 2 * Math.PI;
                else
                    β += 2 * Math.PI;
            }

            matrix.Invert();
            double sweepAngle = β - α;

            return BezierCurveFromArc(center.X - δx * factor, center.Y - δy, 2 * δx * factor, 2 * δy,
              α / Calc.Deg2Rad, sweepAngle / Calc.Deg2Rad, pathStart, ref matrix);
        }
    }
}