using System;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;


namespace PdfSharp.Drawing
{
    [DebuggerDisplay("clr=(A={A}, R={R}, G={G}, B={B} C={C}, M={M}, Y={Y}, K={K})")]
    public struct XColor
    {
        XColor(uint argb)
        {
            _cs = XColorSpace.Rgb;
            _a = (byte)((argb >> 24) & 0xff) / 255f;
            _r = (byte)((argb >> 16) & 0xff);
            _g = (byte)((argb >> 8) & 0xff);
            _b = (byte)(argb & 0xff);
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            _gs = 0;
            RgbChanged();
        }

        XColor(byte alpha, byte red, byte green, byte blue)
        {
            _cs = XColorSpace.Rgb;
            _a = alpha / 255f;
            _r = red;
            _g = green;
            _b = blue;
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            _gs = 0;
            RgbChanged();
        }

        XColor(double alpha, double cyan, double magenta, double yellow, double black)
        {
            _cs = XColorSpace.Cmyk;
            _a = (float)(alpha > 1 ? 1 : (alpha < 0 ? 0 : alpha));
            _c = (float)(cyan > 1 ? 1 : (cyan < 0 ? 0 : cyan));
            _m = (float)(magenta > 1 ? 1 : (magenta < 0 ? 0 : magenta));
            _y = (float)(yellow > 1 ? 1 : (yellow < 0 ? 0 : yellow));
            _k = (float)(black > 1 ? 1 : (black < 0 ? 0 : black));
            _r = 0;
            _g = 0;
            _b = 0;
            _gs = 0f;
            CmykChanged();
        }

        XColor(double cyan, double magenta, double yellow, double black)
            : this(1.0, cyan, magenta, yellow, black)
        { }

        XColor(double gray)
        {
            _cs = XColorSpace.GrayScale;
            if (gray < 0)
                _gs = 0;
            else if (gray > 1)
                _gs = 1;
            else
                _gs = (float)gray;

            _a = 1;
            _r = 0;
            _g = 0;
            _b = 0;
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            GrayChanged();
        }

        internal XColor(XKnownColor knownColor)
            : this(XKnownColorTable.KnownColorToArgb(knownColor))
        { }

        public static XColor FromArgb(int argb)
        {
            return new XColor((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)(argb));
        }

        public static XColor FromArgb(uint argb)
        {
            return new XColor((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)(argb));
        }

        public static XColor FromArgb(int red, int green, int blue)
        {
            CheckByte(red, "red");
            CheckByte(green, "green");
            CheckByte(blue, "blue");
            return new XColor(255, (byte)red, (byte)green, (byte)blue);
        }

        public static XColor FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, "alpha");
            CheckByte(red, "red");
            CheckByte(green, "green");
            CheckByte(blue, "blue");
            return new XColor((byte)alpha, (byte)red, (byte)green, (byte)blue);
        }

        public static XColor FromArgb(int alpha, XColor color)
        {
            color.A = ((byte)alpha) / 255.0;
            return color;
        }


        public static XColor FromCmyk(double cyan, double magenta, double yellow, double black)
        {
            return new XColor(cyan, magenta, yellow, black);
        }

        public static XColor FromCmyk(double alpha, double cyan, double magenta, double yellow, double black)
        {
            return new XColor(alpha, cyan, magenta, yellow, black);
        }

        public static XColor FromGrayScale(double grayScale)
        {
            return new XColor(grayScale);
        }

        public static XColor FromKnownColor(XKnownColor color)
        {
            return new XColor(color);
        }

        public static XColor FromName(string name)
        {
            return Empty;
        }

        public XColorSpace ColorSpace
        {
            get { return _cs; }
            set
            {
                if (!Enum.IsDefined(typeof(XColorSpace), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(XColorSpace));
                _cs = value;
            }
        }

        public bool IsEmpty
        {
            get { return this == Empty; }
        }

        public override bool Equals(object obj)
        {
            if (obj is XColor)
            {
                XColor color = (XColor)obj;
                if (_r == color._r && _g == color._g && _b == color._b &&
                  _c == color._c && _m == color._m && _y == color._y && _k == color._k &&
                  _gs == color._gs)
                {
                    return _a == color._a;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ((byte)(_a * 255)) ^ _r ^ _g ^ _b;
        }

        public static bool operator ==(XColor left, XColor right)
        {
            if (left._r == right._r && left._g == right._g && left._b == right._b &&
                left._c == right._c && left._m == right._m && left._y == right._y && left._k == right._k &&
                left._gs == right._gs)
            {
                return left._a == right._a;
            }
            return false;
        }

        public static bool operator !=(XColor left, XColor right)
        {
            return !(left == right);
        }

        public bool IsKnownColor
        {
            get { return XKnownColorTable.IsKnownColor(Argb); }
        }

        public double GetHue()
        {
            if ((_r == _g) && (_g == _b))
                return 0;

            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value7 = 0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            double value6 = value4 - value5;
            if (value1 == value4)
                value7 = (value2 - value3) / value6;
            else if (value2 == value4)
                value7 = 2f + ((value3 - value1) / value6);
            else if (value3 == value4)
                value7 = 4f + ((value1 - value2) / value6);

            value7 *= 60;
            if (value7 < 0)
                value7 += 360;
            return value7;
        }

        public double GetSaturation()
        {
            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value7 = 0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            if (value4 == value5)
                return value7;

            double value6 = (value4 + value5) / 2;
            if (value6 <= 0.5)
                return (value4 - value5) / (value4 + value5);
            return (value4 - value5) / ((2f - value4) - value5);
        }

        public double GetBrightness()
        {
            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            return (value4 + value5) / 2;
        }

        void RgbChanged()
        {
            _cs = XColorSpace.Rgb;
            int c = 255 - _r;
            int m = 255 - _g;
            int y = 255 - _b;
            int k = Math.Min(c, Math.Min(m, y));
            if (k == 255)
                _c = _m = _y = 0;
            else
            {
                float black = 255f - k;
                _c = (c - k) / black;
                _m = (m - k) / black;
                _y = (y - k) / black;
            }
            _k = _gs = k / 255f;
        }

        void CmykChanged()
        {
            _cs = XColorSpace.Cmyk;
            float black = _k * 255;
            float factor = 255f - black;
            _r = (byte)(255 - Math.Min(255f, _c * factor + black));
            _g = (byte)(255 - Math.Min(255f, _m * factor + black));
            _b = (byte)(255 - Math.Min(255f, _y * factor + black));
            _gs = (float)(1 - Math.Min(1.0, 0.3f * _c + 0.59f * _m + 0.11 * _y + _k));
        }

        void GrayChanged()
        {
            _cs = XColorSpace.GrayScale;
            _r = (byte)(_gs * 255);
            _g = (byte)(_gs * 255);
            _b = (byte)(_gs * 255);
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 1 - _gs;
        }

        public double A
        {
            get { return _a; }
            set
            {
                if (value < 0)
                    _a = 0;
                else if (value > 1)
                    _a = 1;
                else
                    _a = (float)value;
            }
        }

        public byte R
        {
            get { return _r; }
            set { _r = value; RgbChanged(); }
        }

        public byte G
        {
            get { return _g; }
            set { _g = value; RgbChanged(); }
        }

        public byte B
        {
            get { return _b; }
            set { _b = value; RgbChanged(); }
        }

        internal uint Rgb
        {
            get { return ((uint)_r << 16) | ((uint)_g << 8) | _b; }
        }

        internal uint Argb
        {
            get { return ((uint)(_a * 255) << 24) | ((uint)_r << 16) | ((uint)_g << 8) | _b; }
        }

        public double C
        {
            get { return _c; }
            set
            {
                if (value < 0)
                    _c = 0;
                else if (value > 1)
                    _c = 1;
                else
                    _c = (float)value;
                CmykChanged();
            }
        }

        public double M
        {
            get { return _m; }
            set
            {
                if (value < 0)
                    _m = 0;
                else if (value > 1)
                    _m = 1;
                else
                    _m = (float)value;
                CmykChanged();
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                if (value < 0)
                    _y = 0;
                else if (value > 1)
                    _y = 1;
                else
                    _y = (float)value;
                CmykChanged();
            }
        }

        public double K
        {
            get { return _k; }
            set
            {
                if (value < 0)
                    _k = 0;
                else if (value > 1)
                    _k = 1;
                else
                    _k = (float)value;
                CmykChanged();
            }
        }

        public double GS
        {
            get { return _gs; }
            set
            {
                if (value < 0)
                    _gs = 0;
                else if (value > 1)
                    _gs = 1;
                else
                    _gs = (float)value;
                GrayChanged();
            }
        }

        public static XColor Empty;

        public string RgbCmykG
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture,
                  "{0};{1};{2};{3};{4};{5};{6};{7};{8}", _r, _g, _b, _c, _m, _y, _k, _gs, _a);
            }
            set
            {
                string[] values = value.Split(';');
                _r = byte.Parse(values[0], CultureInfo.InvariantCulture);
                _g = byte.Parse(values[1], CultureInfo.InvariantCulture);
                _b = byte.Parse(values[2], CultureInfo.InvariantCulture);
                _c = float.Parse(values[3], CultureInfo.InvariantCulture);
                _m = float.Parse(values[4], CultureInfo.InvariantCulture);
                _y = float.Parse(values[5], CultureInfo.InvariantCulture);
                _k = float.Parse(values[6], CultureInfo.InvariantCulture);
                _gs = float.Parse(values[7], CultureInfo.InvariantCulture);
                _a = float.Parse(values[8], CultureInfo.InvariantCulture);
            }
        }

        static void CheckByte(int val, string name)
        {
            if (val < 0 || val > 0xFF)
                throw new ArgumentException(PSSR.InvalidValue(val, name, 0, 255));
        }

        XColorSpace _cs;

        float _a;   

        byte _r;    
        byte _g;     
        byte _b;    

        float _c;   
        float _m;    
        float _y;   
        float _k;   

        float _gs;    
    }
}
