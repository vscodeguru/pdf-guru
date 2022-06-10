
namespace PdfSharp.Drawing
{
    internal interface IXGraphicsRenderer
    {
        void Close();

        void DrawLine(XPen pen, double x1, double y1, double x2, double y2);

        void DrawLines(XPen pen, XPoint[] points);

        void DrawBezier(XPen pen, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4);

        void DrawBeziers(XPen pen, XPoint[] points);

        void DrawCurve(XPen pen, XPoint[] points, double tension);

        void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle);

        void DrawRectangle(XPen pen, XBrush brush, double x, double y, double width, double height);

        void DrawRectangles(XPen pen, XBrush brush, XRect[] rects);

        void DrawRoundedRectangle(XPen pen, XBrush brush, double x, double y, double width, double height, double ellipseWidth, double ellipseHeight);

        void DrawEllipse(XPen pen, XBrush brush, double x, double y, double width, double height);

        void DrawPolygon(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode);

        void DrawPie(XPen pen, XBrush brush, double x, double y, double width, double height, double startAngle, double sweepAngle);

        void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points, double tension, XFillMode fillmode);

        void DrawPath(XPen pen, XBrush brush, XGraphicsPath path);

        void DrawString(string s, XFont font, XBrush brush, XRect layoutRectangle, XStringFormat format);

        void DrawImage(XImage image, double x, double y, double width, double height);
        void DrawImage(XImage image, XRect destRect, XRect srcRect, XGraphicsUnit srcUnit);

        void Save(XGraphicsState state);

        void Restore(XGraphicsState state);

        void BeginContainer(XGraphicsContainer container, XRect dstrect, XRect srcrect, XGraphicsUnit unit);

        void EndContainer(XGraphicsContainer container);

        void AddTransform(XMatrix transform, XMatrixOrder matrixOrder);

        void SetClip(XGraphicsPath path, XCombineMode combineMode);

        void ResetClip();

        void WriteComment(string comment);

    }
}
