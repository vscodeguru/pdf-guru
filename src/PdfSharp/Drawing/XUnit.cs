using System;
using System.Diagnostics;
using System.Globalization;

namespace PdfSharp.Drawing
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public struct XUnit : IFormattable
    {
        internal const double PointFactor = 1;
        internal const double InchFactor = 72;
        internal const double MillimeterFactor = 72 / 25.4;
        internal const double CentimeterFactor = 72 / 2.54;
        internal const double PresentationFactor = 72 / 96.0;

        internal const double PointFactorWpf = 96 / 72.0;
        internal const double InchFactorWpf = 96;
        internal const double MillimeterFactorWpf = 96 / 25.4;
        internal const double CentimeterFactorWpf = 96 / 2.54;
        internal const double PresentationFactorWpf = 1;

        public XUnit(double point)
        {
            _value = point;
            _type = XGraphicsUnit.Point;
        }

        public XUnit(double value, XGraphicsUnit type)
        {
            if (!Enum.IsDefined(typeof(XGraphicsUnit), type))
#if !SILVERLIGHT && !NETFX_CORE && !UWP
                throw new System.ComponentModel.InvalidEnumArgumentException(nameof(type), (int)type, typeof(XGraphicsUnit));
#endif
            _value = value;
            _type = type;
        }

        public double Value
        {
            get { return _value; }
        }

        public XGraphicsUnit Type
        {
            get { return _type; }
        }

        public double Point
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value;

                    case XGraphicsUnit.Inch:
                        return _value * 72;

                    case XGraphicsUnit.Millimeter:
                        return _value * 72 / 25.4;

                    case XGraphicsUnit.Centimeter:
                        return _value * 72 / 2.54;

                    case XGraphicsUnit.Presentation:
                        return _value * 72 / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Point;
            }
        }

        public double Inch
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value / 72;

                    case XGraphicsUnit.Inch:
                        return _value;

                    case XGraphicsUnit.Millimeter:
                        return _value / 25.4;

                    case XGraphicsUnit.Centimeter:
                        return _value / 2.54;

                    case XGraphicsUnit.Presentation:
                        return _value / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Inch;
            }
        }

        public double Millimeter
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value * 25.4 / 72;

                    case XGraphicsUnit.Inch:
                        return _value * 25.4;

                    case XGraphicsUnit.Millimeter:
                        return _value;

                    case XGraphicsUnit.Centimeter:
                        return _value * 10;

                    case XGraphicsUnit.Presentation:
                        return _value * 25.4 / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Millimeter;
            }
        }

        public double Centimeter
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value * 2.54 / 72;

                    case XGraphicsUnit.Inch:
                        return _value * 2.54;

                    case XGraphicsUnit.Millimeter:
                        return _value / 10;

                    case XGraphicsUnit.Centimeter:
                        return _value;

                    case XGraphicsUnit.Presentation:
                        return _value * 2.54 / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Centimeter;
            }
        }

        public double Presentation
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value * 96 / 72;

                    case XGraphicsUnit.Inch:
                        return _value * 96;

                    case XGraphicsUnit.Millimeter:
                        return _value * 96 / 25.4;

                    case XGraphicsUnit.Centimeter:
                        return _value * 96 / 2.54;

                    case XGraphicsUnit.Presentation:
                        return _value;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Point;
            }
        }

        public string ToString(IFormatProvider formatProvider)
        {
            string valuestring = _value.ToString(formatProvider) + GetSuffix();
            return valuestring;
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            string valuestring = _value.ToString(format, formatProvider) + GetSuffix();
            return valuestring;
        }

        public override string ToString()
        {
            string valuestring = _value.ToString(CultureInfo.InvariantCulture) + GetSuffix();
            return valuestring;
        }

        string GetSuffix()
        {
            switch (_type)
            {
                case XGraphicsUnit.Point:
                    return "pt";

                case XGraphicsUnit.Inch:
                    return "in";

                case XGraphicsUnit.Millimeter:
                    return "mm";

                case XGraphicsUnit.Centimeter:
                    return "cm";

                case XGraphicsUnit.Presentation:
                    return "pu";

                default:
                    throw new InvalidCastException();
            }
        }

        public static XUnit FromPoint(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Point;
            return unit;
        }

        public static XUnit FromInch(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Inch;
            return unit;
        }

        public static XUnit FromMillimeter(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Millimeter;
            return unit;
        }

        public static XUnit FromCentimeter(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Centimeter;
            return unit;
        }

        public static XUnit FromPresentation(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Presentation;
            return unit;
        }

        public static implicit operator XUnit(string value)
        {
            XUnit unit;
            value = value.Trim();

            value = value.Replace(',', '.');

            int count = value.Length;
            int valLen = 0;
            for (; valLen < count;)
            {
                char ch = value[valLen];
                if (ch == '.' || ch == '-' || ch == '+' || char.IsNumber(ch))
                    valLen++;
                else
                    break;
            }

            try
            {
                unit._value = Double.Parse(value.Substring(0, valLen).Trim(), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                unit._value = 1;
                string message = String.Format("String '{0}' is not a valid value for structure 'XUnit'.", value);
                throw new ArgumentException(message, ex);
            }

            string typeStr = value.Substring(valLen).Trim().ToLower();
            unit._type = XGraphicsUnit.Point;
            switch (typeStr)
            {
                case "cm":
                    unit._type = XGraphicsUnit.Centimeter;
                    break;

                case "in":
                    unit._type = XGraphicsUnit.Inch;
                    break;

                case "mm":
                    unit._type = XGraphicsUnit.Millimeter;
                    break;

                case "":
                case "pt":
                    unit._type = XGraphicsUnit.Point;
                    break;

                case "pu":    
                    unit._type = XGraphicsUnit.Presentation;
                    break;

                default:
                    throw new ArgumentException("Unknown unit type: '" + typeStr + "'");
            }
            return unit;
        }

        public static implicit operator XUnit(int value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Point;
            return unit;
        }

        public static implicit operator XUnit(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Point;
            return unit;
        }

        public static implicit operator double(XUnit value)
        {
            return value.Point;
        }

        public static bool operator ==(XUnit value1, XUnit value2)
        {
            return value1._type == value2._type && value1._value == value2._value;
        }

        public static bool operator !=(XUnit value1, XUnit value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(Object obj)
        {
            if (obj is XUnit)
                return this == (XUnit)obj;
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode() ^ _type.GetHashCode();
        }

        public static XUnit Parse(string value)
        {
            XUnit unit = value;
            return unit;
        }

        public void ConvertType(XGraphicsUnit type)
        {
            if (_type == type)
                return;

            switch (type)
            {
                case XGraphicsUnit.Point:
                    _value = Point;
                    _type = XGraphicsUnit.Point;
                    break;

                case XGraphicsUnit.Inch:
                    _value = Inch;
                    _type = XGraphicsUnit.Inch;
                    break;

                case XGraphicsUnit.Centimeter:
                    _value = Centimeter;
                    _type = XGraphicsUnit.Centimeter;
                    break;

                case XGraphicsUnit.Millimeter:
                    _value = Millimeter;
                    _type = XGraphicsUnit.Millimeter;
                    break;

                case XGraphicsUnit.Presentation:
                    _value = Presentation;
                    _type = XGraphicsUnit.Presentation;
                    break;

                default:
                    throw new ArgumentException("Unknown unit type: '" + type + "'");
            }
        }

        public static readonly XUnit Zero = new XUnit();

        double _value;
        XGraphicsUnit _type;

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture, "unit=({0:" + format + "} {1})", _value, GetSuffix());
            }
        }
    }
}