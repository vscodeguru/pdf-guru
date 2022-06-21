using System;
using System.ComponentModel;

namespace PdfSharp.Drawing
{
    public sealed class XLinearGradientBrush : XBrush
    {


        public XLinearGradientBrush(XPoint point1, XPoint point2, XColor color1, XColor color2)
        {
            _point1 = point1;
            _point2 = point2;
            _color1 = color1;
            _color2 = color2;
        }


        public XLinearGradientBrush(XRect rect, XColor color1, XColor color2, XLinearGradientMode linearGradientMode)
        {
            if (!Enum.IsDefined(typeof(XLinearGradientMode), linearGradientMode))
                throw new InvalidEnumArgumentException("linearGradientMode", (int)linearGradientMode, typeof(XLinearGradientMode));

            if (rect.Width == 0 || rect.Height == 0)
                throw new ArgumentException("Invalid rectangle.", "rect");

            _useRect = true;
            _color1 = color1;
            _color2 = color2;
            _rect = rect;
            _linearGradientMode = linearGradientMode;
        }

        public XMatrix Transform
        {
            get { return _matrix; }
            set { _matrix = value; }
        }

        public void TranslateTransform(double dx, double dy)
        {
            _matrix.TranslatePrepend(dx, dy);
        }

        public void TranslateTransform(double dx, double dy, XMatrixOrder order)
        {
            _matrix.Translate(dx, dy, order);
        }

        public void ScaleTransform(double sx, double sy)
        {
            _matrix.ScalePrepend(sx, sy);
        }

        public void ScaleTransform(double sx, double sy, XMatrixOrder order)
        {
            _matrix.Scale(sx, sy, order);
        }

        public void RotateTransform(double angle)
        {
            _matrix.RotatePrepend(angle);
        }

        public void RotateTransform(double angle, XMatrixOrder order)
        {
            _matrix.Rotate(angle, order);
        }

        public void MultiplyTransform(XMatrix matrix)
        {
            _matrix.Prepend(matrix);
        }

        public void MultiplyTransform(XMatrix matrix, XMatrixOrder order)
        {
            _matrix.Multiply(matrix, order);
        }

        public void ResetTransform()
        {
            _matrix = new XMatrix();
        }

        internal bool _useRect;
        internal XPoint _point1, _point2;
        internal XColor _color1, _color2;
        internal XRect _rect;
        internal XLinearGradientMode _linearGradientMode;

        internal XMatrix _matrix;
    }
}