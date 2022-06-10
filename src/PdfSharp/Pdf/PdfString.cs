using System;
using System.Diagnostics;
using System.Text;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf
{
    [Flags]
    public enum PdfStringEncoding
    {
        RawEncoding = PdfStringFlags.RawEncoding,

        StandardEncoding = PdfStringFlags.StandardEncoding,

        PDFDocEncoding = PdfStringFlags.PDFDocEncoding,
        WinAnsiEncoding = PdfStringFlags.WinAnsiEncoding,

        MacRomanEncoding = PdfStringFlags.MacExpertEncoding,

        MacExpertEncoding = PdfStringFlags.MacExpertEncoding,

        Unicode = PdfStringFlags.Unicode,
    }

    [Flags]
    enum PdfStringFlags
    {
        RawEncoding = 0x00,
        StandardEncoding = 0x01,      
        PDFDocEncoding = 0x02,
        WinAnsiEncoding = 0x03,
        MacRomanEncoding = 0x04,      
        MacExpertEncoding = 0x05,      
        Unicode = 0x06,
        EncodingMask = 0x0F,

        HexLiteral = 0x80,
    }

    [DebuggerDisplay("({Value})")]
    public sealed class PdfString : PdfItem
    {
        public PdfString()
        {
        }

        public PdfString(string value)
        {
#if true
            if (!IsRawEncoding(value))
                _flags = PdfStringFlags.Unicode;
            _value = value;
#endif
        }

        public PdfString(string value, PdfStringEncoding encoding)
        {
            switch (encoding)
            {
                case PdfStringEncoding.RawEncoding:
                    CheckRawEncoding(value);
                    break;

                case PdfStringEncoding.StandardEncoding:
                    break;

                case PdfStringEncoding.PDFDocEncoding:
                    break;

                case PdfStringEncoding.WinAnsiEncoding:
                    CheckRawEncoding(value);
                    break;

                case PdfStringEncoding.MacRomanEncoding:
                    break;

                case PdfStringEncoding.Unicode:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("encoding");
            }
            _value = value;
            _flags = (PdfStringFlags)encoding;
        }

        internal PdfString(string value, PdfStringFlags flags)
        {
            _value = value;
            _flags = flags;
        }

        public int Length
        {
            get { return _value == null ? 0 : _value.Length; }
        }

        public PdfStringEncoding Encoding
        {
            get { return (PdfStringEncoding)(_flags & PdfStringFlags.EncodingMask); }
        }

        public bool HexLiteral
        {
            get { return (_flags & PdfStringFlags.HexLiteral) != 0; }
        }

        internal PdfStringFlags Flags
        {
            get { return _flags; }
        }
        readonly PdfStringFlags _flags;

        public string Value
        {
            get { return _value ?? ""; }
        }
        string _value;

        internal byte[] EncryptionValue
        {
            get { return _value == null ? new byte[0] : PdfEncoders.RawEncoding.GetBytes(_value); }
            set { _value = PdfEncoders.RawEncoding.GetString(value, 0, value.Length); }
        }

        public override string ToString()
        {
#if true
            PdfStringEncoding encoding = (PdfStringEncoding)(_flags & PdfStringFlags.EncodingMask);
            string pdf = (_flags & PdfStringFlags.HexLiteral) == 0 ?
                PdfEncoders.ToStringLiteral(_value, encoding, null) :
                PdfEncoders.ToHexStringLiteral(_value, encoding, null);
            return pdf;
#endif
        }

        public string ToStringFromPdfDocEncoded()
        {
            int length = _value.Length;
            char[] bytes = new char[length];
            for (int idx = 0; idx < length; idx++)
            {
                char ch = _value[idx];
                if (ch <= 255)
                {
                    bytes[idx] = Encode[ch];
                }
                else
                {
                    throw new InvalidOperationException("DocEncoded string contains char greater 255.");
                }
            }
            StringBuilder sb = new StringBuilder(length);
            for (int idx = 0; idx < length; idx++)
                sb.Append((char)bytes[idx]);
            return sb.ToString();
        }
        static readonly char[] Encode =
        {
            '\x00', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07', '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D', '\x0E', '\x0F',
            '\x10', '\x11', '\x12', '\x13', '\x14', '\x15', '\x16', '\x17', '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D', '\x1E', '\x1F',
            '\x20', '\x21', '\x22', '\x23', '\x24', '\x25', '\x26', '\x27', '\x28', '\x29', '\x2A', '\x2B', '\x2C', '\x2D', '\x2E', '\x2F',
            '\x30', '\x31', '\x32', '\x33', '\x34', '\x35', '\x36', '\x37', '\x38', '\x39', '\x3A', '\x3B', '\x3C', '\x3D', '\x3E', '\x3F',
            '\x40', '\x41', '\x42', '\x43', '\x44', '\x45', '\x46', '\x47', '\x48', '\x49', '\x4A', '\x4B', '\x4C', '\x4D', '\x4E', '\x4F',
            '\x50', '\x51', '\x52', '\x53', '\x54', '\x55', '\x56', '\x57', '\x58', '\x59', '\x5A', '\x5B', '\x5C', '\x5D', '\x5E', '\x5F',
            '\x60', '\x61', '\x62', '\x63', '\x64', '\x65', '\x66', '\x67', '\x68', '\x69', '\x6A', '\x6B', '\x6C', '\x6D', '\x6E', '\x6F',
            '\x70', '\x71', '\x72', '\x73', '\x74', '\x75', '\x76', '\x77', '\x78', '\x79', '\x7A', '\x7B', '\x7C', '\x7D', '\x7E', '\x7F',
            '\x2022', '\x2020', '\x2021', '\x2026', '\x2014', '\x2013', '\x0192', '\x2044', '\x2039', '\x203A', '\x2212', '\x2030', '\x201E', '\x201C', '\x201D', '\x2018',
            '\x2019', '\x201A', '\x2122', '\xFB01', '\xFB02', '\x0141', '\x0152', '\x0160', '\x0178', '\x017D', '\x0131', '\x0142', '\x0153', '\x0161', '\x017E', '\xFFFD',
            '\x20AC', '\xA1', '\xA2', '\xA3', '\xA4', '\xA5', '\xA6', '\xA7', '\xA8', '\xA9', '\xAA', '\xAB', '\xAC', '\xAD', '\xAE', '\xAF',
            '\xB0', '\xB1', '\xB2', '\xB3', '\xB4', '\xB5', '\xB6', '\xB7', '\xB8', '\xB9', '\xBA', '\xBB', '\xBC', '\xBD', '\xBE', '\xBF',
            '\xC0', '\xC1', '\xC2', '\xC3', '\xC4', '\xC5', '\xC6', '\xC7', '\xC8', '\xC9', '\xCA', '\xCB', '\xCC', '\xCD', '\xCE', '\xCF',
            '\xD0', '\xD1', '\xD2', '\xD3', '\xD4', '\xD5', '\xD6', '\xD7', '\xD8', '\xD9', '\xDA', '\xDB', '\xDC', '\xDD', '\xDE', '\xDF',
            '\xE0', '\xE1', '\xE2', '\xE3', '\xE4', '\xE5', '\xE6', '\xE7', '\xE8', '\xE9', '\xEA', '\xEB', '\xEC', '\xED', '\xEE', '\xEF',
            '\xF0', '\xF1', '\xF2', '\xF3', '\xF4', '\xF5', '\xF6', '\xF7', '\xF8', '\xF9', '\xFA', '\xFB', '\xFC', '\xFD', '\xFE', '\xFF',
        };

        static void CheckRawEncoding(string s)
        {
            if (String.IsNullOrEmpty(s))
                return;

            int length = s.Length;
            for (int idx = 0; idx < length; idx++)
            {
                Debug.Assert(s[idx] < 256, "RawString contains invalid character.");
            }
        }

        static bool IsRawEncoding(string s)
        {
            if (String.IsNullOrEmpty(s))
                return true;

            int length = s.Length;
            for (int idx = 0; idx < length; idx++)
            {
                if (!(s[idx] < 256))
                    return false;
            }
            return true;
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }
    }
}
