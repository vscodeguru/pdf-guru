using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    [Serializable, StructLayout(LayoutKind.Sequential)]   
    public struct XSize : IFormattable
    {
        public XSize(double width, double height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("WidthAndHeightCannotBeNegative");   

            _width = width;
            _height = height;
        }

        public static bool operator ==(XSize size1, XSize size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
        }

        public static bool operator !=(XSize size1, XSize size2)
        {
            return !(size1 == size2);
        }

        public static bool Equals(XSize size1, XSize size2)
        {
            if (size1.IsEmpty)
                return size2.IsEmpty;
            return size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height);
        }

        public override bool Equals(object o)
        {
            if (!(o is XSize))
                return false;
            return Equals(this, (XSize)o);
        }

        public bool Equals(XSize value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
                return 0;
            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        public static XSize Parse(string source)
        {
            XSize empty;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
            string str = helper.NextTokenRequired();
            if (str == "Empty")
                empty = Empty;
            else
                empty = new XSize(Convert.ToDouble(str, cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo));
            helper.LastTokenRequired();
            return empty;
        }

        public XPoint ToXPoint()
        {
            return new XPoint(_width, _height);
        }

        public XVector ToXVector()
        {
            return new XVector(_width, _height);
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
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[] { numericListSeparator, _width, _height });
        }

        public static XSize Empty
        {
            get { return s_empty; }
        }
        static readonly XSize s_empty;

        public bool IsEmpty
        {
            get { return _width < 0; }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptySize");   
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
                    throw new InvalidOperationException("CannotModifyEmptySize");    
                if (value < 0)
                    throw new ArgumentException("HeightCannotBeNegative");   
                _height = value;
            }
        }
        double _height;

        public static explicit operator XVector(XSize size)
        {
            return new XVector(size._width, size._height);
        }

        public static explicit operator XPoint(XSize size)
        {
            return new XPoint(size._width, size._height);
        }

        private static XSize CreateEmptySize()
        {
            XSize size = new XSize();
            size._width = double.NegativeInfinity;
            size._height = double.NegativeInfinity;
            return size;
        }

        static XSize()
        {
            s_empty = CreateEmptySize();
        }

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture,
                    "size=({2}{0:" + format + "}, {1:" + format + "})",
                    _width, _height, IsEmpty ? "Empty " : "");
            }
        }
    }
}
