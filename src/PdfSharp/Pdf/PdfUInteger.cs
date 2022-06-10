using System;
using System.Diagnostics;
using System.Globalization;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfUInteger : PdfNumber, IConvertible
    {
        public PdfUInteger()
        { }

        public PdfUInteger(uint value)
        {
            _value = value;
        }

        public uint Value
        {
            get { return _value; }
        }
        readonly uint _value;

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(_value);
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            return _value;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return new DateTime();
        }

        public float ToSingle(IFormatProvider provider)
        {
            return _value;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(_value);
        }

        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(_value);
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(_value);
        }

        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(_value);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return _value.ToString(provider);
        }

        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(_value);
        }

        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(_value);
        }

        public long ToInt64(IFormatProvider provider)
        {
            return _value;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return _value;
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(_value);
        }

    }
}