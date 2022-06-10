using System;
using System.Diagnostics;
using System.Globalization;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfInteger : PdfNumber, IConvertible
    {
        public PdfInteger()
        { }

        public PdfInteger(int value)
        {
            _value = value;
        }

        public int Value
        {
            get { return _value; }
        }
        readonly int _value;

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(_value);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return _value;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return new DateTime();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return _value;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(_value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return _value;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(_value);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(_value);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return _value.ToString(provider);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(_value);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(_value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return _value;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return _value;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(_value);
        }

    }
}