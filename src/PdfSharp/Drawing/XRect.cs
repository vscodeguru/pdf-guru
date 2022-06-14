using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using PdfSharp.Internal;


namespace PdfSharp.Drawing
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, StructLayout(LayoutKind.Sequential)]    
    public struct XRect : IFormattable
    {
        public XRect(double x, double y, double width, double height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("WidthAndHeightCannotBeNegative");   
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public XRect(XPoint point1, XPoint point2)
        {
            _x = Math.Min(point1.X, point2.X);
            _y = Math.Min(point1.Y, point2.Y);
            _width = Math.Max(Math.Max(point1.X, point2.X) - _x, 0);
            _height = Math.Max(Math.Max(point1.Y, point2.Y) - _y, 0);
        }

        public XRect(XPoint point, XVector vector)
            : this(point, point + vector)
        { }

        public XRect(XPoint location, XSize size)
        {
            if (size.IsEmpty)
                this = s_empty;
            else
            {
                _x = location.X;
                _y = location.Y;
                _width = size.Width;
                _height = size.Height;
            }
        }

        public XRect(XSize size)
        {
            if (size.IsEmpty)
                this = s_empty;
            else
            {
                _x = _y = 0;
                _width = size.Width;
                _height = size.Height;
            }
        }


        public static XRect FromLTRB(double left, double top, double right, double bottom)
        {
            return new XRect(left, top, right - left, bottom - top);
        }

        public static bool operator ==(XRect rect1, XRect rect2)
        {
            return rect1.X == rect2.X && rect1.Y == rect2.Y && rect1.Width == rect2.Width && rect1.Height == rect2.Height;
        }

        public static bool operator !=(XRect rect1, XRect rect2)
        {
            return !(rect1 == rect2);
        }

        public static bool Equals(XRect rect1, XRect rect2)
        {
            if (rect1.IsEmpty)
                return rect2.IsEmpty;
            return rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y) && rect1.Width.Equals(rect2.Width) && rect1.Height.Equals(rect2.Height);
        }

        public override bool Equals(object o)
        {
            if (!(o is XRect))
                return false;
            return Equals(this, (XRect)o);
        }

        public bool Equals(XRect value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
                return 0;
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }

        public static XRect Parse(string source)
        {
            XRect empty;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
            string str = helper.NextTokenRequired();
            if (str == "Empty")
                empty = Empty;
            else
                empty = new XRect(Convert.ToDouble(str, cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo));
            helper.LastTokenRequired();
            return empty;
        }

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
            if (IsEmpty)
                return "Empty";
            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            provider = provider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", new object[] { numericListSeparator, _x, _y, _width, _height });
        }

        public static XRect Empty
        {
            get { return s_empty; }
        }

        public bool IsEmpty
        {
            get { return _width < 0; }
        }

        public XPoint Location
        {
            get { return new XPoint(_x, _y); }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");   
                _x = value.X;
                _y = value.Y;
            }
        }

        public XSize Size
        {
            get
            {
                if (IsEmpty)
                    return XSize.Empty;
                return new XSize(_width, _height);
            }
            set
            {
                if (value.IsEmpty)
                    this = s_empty;
                else
                {
                    if (IsEmpty)
                        throw new InvalidOperationException("CannotModifyEmptyRect");   
                    _width = value.Width;
                    _height = value.Height;
                }
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");   
                _x = value;
            }
        }
        double _x;

        public double Y
        {
            get { return _y; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");   
                _y = value;
            }
        }
        double _y;

        public double Width
        {
            get { return _width; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");   
                if (value < 0)
                    throw new ArgumentException("WidthCannotBeNegative");   

                _width = value;
            }
        }
        double _width;

        public double Height
        {
            get { return _height; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");   
                if (value < 0)
                    throw new ArgumentException("HeightCannotBeNegative");   
                _height = value;
            }
        }
        double _height;

        public double Left
        {
            get { return _x; }
        }

        public double Top
        {
            get { return _y; }
        }

        public double Right
        {
            get
            {
                if (IsEmpty)
                    return double.NegativeInfinity;
                return _x + _width;
            }
        }

        public double Bottom
        {
            get
            {
                if (IsEmpty)
                    return double.NegativeInfinity;
                return _y + _height;
            }
        }

        public XPoint TopLeft
        {
            get { return new XPoint(Left, Top); }
        }

        public XPoint TopRight
        {
            get { return new XPoint(Right, Top); }
        }

        public XPoint BottomLeft
        {
            get { return new XPoint(Left, Bottom); }
        }

        public XPoint BottomRight
        {
            get { return new XPoint(Right, Bottom); }
        }

        public XPoint Center
        {
            get { return new XPoint(_x + _width / 2, _y + _height / 2); }
        }

        public bool Contains(XPoint point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(double x, double y)
        {
            if (IsEmpty)
                return false;
            return ContainsInternal(x, y);
        }

        public bool Contains(XRect rect)
        {
            return !IsEmpty && !rect.IsEmpty &&
              _x <= rect._x && _y <= rect._y &&
              _x + _width >= rect._x + rect._width && _y + _height >= rect._y + rect._height;
        }

        public bool IntersectsWith(XRect rect)
        {
            return !IsEmpty && !rect.IsEmpty &&
                rect.Left <= Right && rect.Right >= Left &&
                rect.Top <= Bottom && rect.Bottom >= Top;
        }

        public void Intersect(XRect rect)
        {
            if (!IntersectsWith(rect))
                this = Empty;
            else
            {
                double left = Math.Max(Left, rect.Left);
                double top = Math.Max(Top, rect.Top);
                _width = Math.Max(Math.Min(Right, rect.Right) - left, 0.0);
                _height = Math.Max(Math.Min(Bottom, rect.Bottom) - top, 0.0);
                _x = left;
                _y = top;
            }
        }

        public static XRect Intersect(XRect rect1, XRect rect2)
        {
            rect1.Intersect(rect2);
            return rect1;
        }

        public void Union(XRect rect)
        {
            if (IsEmpty)
                this = rect;
            else if (!rect.IsEmpty)
            {
                double left = Math.Min(Left, rect.Left);
                double top = Math.Min(Top, rect.Top);
                if (rect.Width == Double.PositiveInfinity || Width == Double.PositiveInfinity)
                    _width = Double.PositiveInfinity;
                else
                {
                    double right = Math.Max(Right, rect.Right);
                    _width = Math.Max(right - left, 0.0);
                }

                if (rect.Height == Double.PositiveInfinity || _height == Double.PositiveInfinity)
                    _height = Double.PositiveInfinity;
                else
                {
                    double bottom = Math.Max(Bottom, rect.Bottom);
                    _height = Math.Max(bottom - top, 0.0);
                }
                _x = left;
                _y = top;
            }
        }

        public static XRect Union(XRect rect1, XRect rect2)
        {
            rect1.Union(rect2);
            return rect1;
        }

        public void Union(XPoint point)
        {
            Union(new XRect(point, point));
        }

        public static XRect Union(XRect rect, XPoint point)
        {
            rect.Union(new XRect(point, point));
            return rect;
        }

        public void Offset(XVector offsetVector)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CannotCallMethod");   
            _x += offsetVector.X;
            _y += offsetVector.Y;
        }

        public void Offset(double offsetX, double offsetY)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CannotCallMethod");   
            _x += offsetX;
            _y += offsetY;
        }

        public static XRect Offset(XRect rect, XVector offsetVector)
        {
            rect.Offset(offsetVector.X, offsetVector.Y);
            return rect;
        }

        public static XRect Offset(XRect rect, double offsetX, double offsetY)
        {
            rect.Offset(offsetX, offsetY);
            return rect;
        }

        public static XRect operator +(XRect rect, XPoint point)
        {
            return new XRect(rect._x + point.X, rect.Y + point.Y, rect._width, rect._height);
        }

        public static XRect operator -(XRect rect, XPoint point)
        {
            return new XRect(rect._x - point.X, rect.Y - point.Y, rect._width, rect._height);
        }

        public void Inflate(XSize size)
        {
            Inflate(size.Width, size.Height);
        }

        public void Inflate(double width, double height)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CannotCallMethod");   
            _x -= width;
            _y -= height;
            _width += width;
            _width += width;
            _height += height;
            _height += height;
            if (_width < 0 || _height < 0)
                this = s_empty;
        }

        public static XRect Inflate(XRect rect, XSize size)
        {
            rect.Inflate(size.Width, size.Height);
            return rect;
        }

        public static XRect Inflate(XRect rect, double width, double height)
        {
            rect.Inflate(width, height);
            return rect;
        }

        public static XRect Transform(XRect rect, XMatrix matrix)
        {
            XMatrix.MatrixHelper.TransformRect(ref rect, ref matrix);
            return rect;
        }

        public void Transform(XMatrix matrix)
        {
            XMatrix.MatrixHelper.TransformRect(ref this, ref matrix);
        }

        public void Scale(double scaleX, double scaleY)
        {
            if (!IsEmpty)
            {
                _x *= scaleX;
                _y *= scaleY;
                _width *= scaleX;
                _height *= scaleY;
                if (scaleX < 0)
                {
                    _x += _width;
                    _width *= -1.0;
                }
                if (scaleY < 0)
                {
                    _y += _height;
                    _height *= -1.0;
                }
            }
        }


        bool ContainsInternal(double x, double y)
        {
            return x >= _x && x - _width <= _x && y >= _y && y - _height <= _y;
        }

        static XRect CreateEmptyRect()
        {
            XRect rect = new XRect();
            rect._x = double.PositiveInfinity;
            rect._y = double.PositiveInfinity;
            rect._width = double.NegativeInfinity;
            rect._height = double.NegativeInfinity;
            return rect;
        }

        static XRect()
        {
            s_empty = CreateEmptyRect();
        }

        static readonly XRect s_empty;

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture,
                    "rect=({0:" + format + "}, {1:" + format + "}, {2:" + format + "}, {3:" + format + "})",
                    _x, _y, _width, _height);
            }
        }
    }
}
