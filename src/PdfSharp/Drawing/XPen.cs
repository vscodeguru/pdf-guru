using System;
using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    public sealed class XPen
    {
        public XPen(XColor color)
            : this(color, 1, false)
        { }

        public XPen(XColor color, double width)
            : this(color, width, false)
        { }

        internal XPen(XColor color, double width, bool immutable)
        {
            _color = color;
            _width = width;
            _lineJoin = XLineJoin.Miter;
            _lineCap = XLineCap.Flat;
            _dashStyle = XDashStyle.Solid;
            _dashOffset = 0f;
            _immutable = immutable;
        }

        public XPen(XPen pen)
        {
            _color = pen._color;
            _width = pen._width;
            _lineJoin = pen._lineJoin;
            _lineCap = pen._lineCap;
            _dashStyle = pen._dashStyle;
            _dashOffset = pen._dashOffset;
            _dashPattern = pen._dashPattern;
            if (_dashPattern != null)
                _dashPattern = (double[])_dashPattern.Clone();
        }

        public XPen Clone()
        {
            return new XPen(this);
        }

        public XColor Color
        {
            get { return _color; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _color != value;
                _color = value;
            }
        }
        internal XColor _color;

        public double Width
        {
            get { return _width; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _width != value;
                _width = value;
            }
        }
        internal double _width;

        public XLineJoin LineJoin
        {
            get { return _lineJoin; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _lineJoin != value;
                _lineJoin = value;
            }
        }
        internal XLineJoin _lineJoin;

        public XLineCap LineCap
        {
            get { return _lineCap; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _lineCap != value;
                _lineCap = value;
            }
        }
        internal XLineCap _lineCap;

        public double MiterLimit
        {
            get { return _miterLimit; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _miterLimit != value;
                _miterLimit = value;
            }
        }
        internal double _miterLimit;

        public XDashStyle DashStyle
        {
            get { return _dashStyle; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _dashStyle != value;
                _dashStyle = value;
            }
        }
        internal XDashStyle _dashStyle;

        public double DashOffset
        {
            get { return _dashOffset; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _dashOffset != value;
                _dashOffset = value;
            }
        }
        internal double _dashOffset;

        public double[] DashPattern
        {
            get
            {
                if (_dashPattern == null)
                    _dashPattern = new double[0];
                return _dashPattern;
            }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));

                int length = value.Length;
                for (int idx = 0; idx < length; idx++)
                {
                    if (value[idx] <= 0)
                        throw new ArgumentException("Dash pattern value must greater than zero.");
                }

                _dirty = true;
                _dashStyle = XDashStyle.Custom;
                _dashPattern = (double[])value.Clone();
            }
        }
        internal double[] _dashPattern;

        public bool Overprint
        {
            get { return _overprint; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _overprint = value;
            }
        }
        internal bool _overprint;

        bool _dirty = true;
        readonly bool _immutable;

    }
}