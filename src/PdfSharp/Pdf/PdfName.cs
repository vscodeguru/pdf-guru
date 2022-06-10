using System;
using System.Collections.Generic;
using System.Diagnostics;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfName : PdfItem
    {
        public PdfName()
        {
            _value = "/";    
        }

        public PdfName(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length == 0 || value[0] != '/')
                throw new ArgumentException(PSSR.NameMustStartWithSlash);

            _value = value;
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public string Value
        {
            get { return _value; }
        }
        readonly string _value;

        public override string ToString()
        {
            return _value;
        }

        public static bool operator ==(PdfName name, string str)
        {
            if (ReferenceEquals(name, null))
                return str == null;

            return name._value == str;
        }

        public static bool operator !=(PdfName name, string str)
        {
            if (ReferenceEquals(name, null))
                return str != null;

            return name._value != str;
        }

        public static readonly PdfName Empty = new PdfName("/");

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }

        public static PdfXNameComparer Comparer
        {
            get { return new PdfXNameComparer(); }
        }

        public class PdfXNameComparer : IComparer<PdfName>
        {
            public int Compare(PdfName l, PdfName r)
            {
#if true_
#else
                if (l != null)
                {
                    if (r != null)
                        return String.Compare(l._value, r._value, StringComparison.Ordinal);
                    return -1;
                }
                if (r != null)
                    return 1;
                return 0;
#endif
            }
        }
    }
}
