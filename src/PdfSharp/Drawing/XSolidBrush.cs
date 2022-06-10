using System;
namespace PdfSharp.Drawing
{
    public sealed class XSolidBrush : XBrush
    {
        public XSolidBrush()
        { }

        public XSolidBrush(XColor color)
            : this(color, false)
        { }

        internal XSolidBrush(XColor color, bool immutable)
        {
            _color = color;
            _immutable = immutable;
        }

        public XSolidBrush(XSolidBrush brush)
        {
            _color = brush.Color;
        }

        public XColor Color
        {
            get { return _color; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XSolidBrush"));
                _color = value;
            }
        }
        internal XColor _color;

        public bool Overprint
        {
            get { return _overprint; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XSolidBrush"));
                _overprint = value;
            }
        }
        internal bool _overprint;

        readonly bool _immutable;
    }
}
