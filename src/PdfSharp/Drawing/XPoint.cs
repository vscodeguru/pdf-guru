using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using PdfSharp.Internal;

#if !EDF_CORE
namespace PdfSharp.Drawing
#endif
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]    
    public struct XPoint : IFormattable
    {
        public XPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }


        public static bool operator ==(XPoint point1, XPoint point2)
        {
            return point1._x == point2._x && point1._y == point2._y;
        }

        public static bool operator !=(XPoint point1, XPoint point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(XPoint point1, XPoint point2)
        {
            return point1.X.Equals(point2.X) && point1.Y.Equals(point2.Y);
        }

        public override bool Equals(object o)
        {
            if (!(o is XPoint))
                return false;
            return Equals(this, (XPoint)o);
        }

        public bool Equals(XPoint value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static XPoint Parse(string source)
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
            string str = helper.NextTokenRequired();
            XPoint point = new XPoint(Convert.ToDouble(str, cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo));
            helper.LastTokenRequired();
            return point;
        }

        public static XPoint[] ParsePoints(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            string[] values = value.Split(' ');
            int count = values.Length;
            XPoint[] points = new XPoint[count];
            for (int idx = 0; idx < count; idx++)
                points[idx] = Parse(values[idx]);
            return points;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        double _x;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        double _y;

        public override string ToString()
        {
            return ConvertToString(null, null);
        }

        public string ToString(IFormatProvider provider)
        {
            return ConvertToString(null, provider);
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return ConvertToString(format, provider);
        }

        internal string ConvertToString(string format, IFormatProvider provider)
        {
            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            provider = provider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[] { numericListSeparator, _x, _y });
        }

        public void Offset(double offsetX, double offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        public static XPoint operator +(XPoint point, XVector vector)
        {
            return new XPoint(point._x + vector.X, point._y + vector.Y);
        }

        public static XPoint operator +(XPoint point, XSize size)    
        {
            return new XPoint(point._x + size.Width, point._y + size.Height);
        }

        public static XPoint Add(XPoint point, XVector vector)
        {
            return new XPoint(point._x + vector.X, point._y + vector.Y);
        }

        public static XPoint operator -(XPoint point, XVector vector)
        {
            return new XPoint(point._x - vector.X, point._y - vector.Y);
        }

        public static XPoint Subtract(XPoint point, XVector vector)
        {
            return new XPoint(point._x - vector.X, point._y - vector.Y);
        }

        public static XVector operator -(XPoint point1, XPoint point2)
        {
            return new XVector(point1._x - point2._x, point1._y - point2._y);
        }

        [Obsolete("Use XVector instead of XSize as second parameter.")]
        public static XPoint operator -(XPoint point, XSize size)    
        {
            return new XPoint(point._x - size.Width, point._y - size.Height);
        }

        public static XVector Subtract(XPoint point1, XPoint point2)
        {
            return new XVector(point1._x - point2._x, point1._y - point2._y);
        }

        public static XPoint operator *(XPoint point, XMatrix matrix)
        {
            return matrix.Transform(point);
        }

        public static XPoint Multiply(XPoint point, XMatrix matrix)
        {
            return matrix.Transform(point);
        }

        public static XPoint operator *(XPoint point, double value)
        {
            return new XPoint(point._x * value, point._y * value);
        }

        public static XPoint operator *(double value, XPoint point)
        {
            return new XPoint(value * point._x, value * point._y);
        }

        public static explicit operator XSize(XPoint point)
        {
            return new XSize(Math.Abs(point._x), Math.Abs(point._y));
        }

        public static explicit operator XVector(XPoint point)
        {
            return new XVector(point._x, point._y);
        }

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture, "point=({0:" + format + "}, {1:" + format + "})", _x, _y);
            }
        }
    }
}
