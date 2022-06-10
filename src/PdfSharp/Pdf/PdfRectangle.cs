using System;
using System.Diagnostics;
using System.Globalization;
using PdfSharp.Drawing;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public sealed class PdfRectangle : PdfItem
    {
        public PdfRectangle()
        { }

        internal PdfRectangle(double x1, double y1, double x2, double y2)
        {
            _x1 = x1;
            _y1 = y1;
            _x2 = x2;
            _y2 = y2;
        }

        public PdfRectangle(XPoint pt1, XPoint pt2)
        {
            _x1 = pt1.X;
            _y1 = pt1.Y;
            _x2 = pt2.X;
            _y2 = pt2.Y;
        }

        public PdfRectangle(XPoint pt, XSize size)
        {
            _x1 = pt.X;
            _y1 = pt.Y;
            _x2 = pt.X + size.Width;
            _y2 = pt.Y + size.Height;
        }

        public PdfRectangle(XRect rect)
        {
            _x1 = rect.X;
            _y1 = rect.Y;
            _x2 = rect.X + rect.Width;
            _y2 = rect.Y + rect.Height;
        }

        internal PdfRectangle(PdfItem item)
        {
            if (item == null || item is PdfNull)
                return;

            if (item is PdfReference)
                item = ((PdfReference)item).Value;

            PdfArray array = item as PdfArray;
            if (array == null)
                throw new InvalidOperationException(PSSR.UnexpectedTokenInPdfFile);

            _x1 = array.Elements.GetReal(0);
            _y1 = array.Elements.GetReal(1);
            _x2 = array.Elements.GetReal(2);
            _y2 = array.Elements.GetReal(3);
        }

        public new PdfRectangle Clone()
        {
            return (PdfRectangle)Copy();
        }

        protected override object Copy()
        {
            PdfRectangle rect = (PdfRectangle)base.Copy();
            return rect;
        }

        public bool IsEmpty
        {
            get { return _x1 == 0 && _y1 == 0 && _x2 == 0 && _y2 == 0; }
        }

        public override bool Equals(object obj)
        {
            PdfRectangle rectangle = obj as PdfRectangle;
            if (rectangle != null)
            {
                PdfRectangle rect = rectangle;
                return rect._x1 == _x1 && rect._y1 == _y1 && rect._x2 == _x2 && rect._y2 == _y2;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (int)(((((uint)_x1) ^ ((((uint)_y1) << 13) |
              (((uint)_y1) >> 0x13))) ^ ((((uint)_x2) << 0x1a) |
              (((uint)_x2) >> 6))) ^ ((((uint)_y2) << 7) |
              (((uint)_y2) >> 0x19)));
        }

        public static bool operator ==(PdfRectangle left, PdfRectangle right)
        {
            if ((object)left != null)
            {
                if ((object)right != null)
                    return left._x1 == right._x1 && left._y1 == right._y1 && left._x2 == right._x2 && left._y2 == right._y2;
                return false;
            }
            return (object)right == null;
        }

        public static bool operator !=(PdfRectangle left, PdfRectangle right)
        {
            return !(left == right);
        }

        public double X1
        {
            get { return _x1; }
        }
        readonly double _x1;

        public double Y1
        {
            get { return _y1; }
        }
        readonly double _y1;

        public double X2
        {
            get { return _x2; }
        }
        readonly double _x2;

        public double Y2
        {
            get { return _y2; }
        }
        readonly double _y2;

        public double Width
        {
            get { return _x2 - _x1; }
        }

        public double Height
        {
            get { return _y2 - _y1; }
        }

        public XPoint Location
        {
            get { return new XPoint(_x1, _y1); }
        }

        public XSize Size
        {
            get { return new XSize(_x2 - _x1, _y2 - _y1); }
        }

        public bool Contains(XPoint pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(double x, double y)
        {
            return _x1 <= x && x <= _x2 && _y1 <= y && y <= _y2;
        }


        public bool Contains(XRect rect)
        {
            return _x1 <= rect.X && (rect.X + rect.Width) <= _x2 &&
              _y1 <= rect.Y && (rect.Y + rect.Height) <= _y2;
        }

        public bool Contains(PdfRectangle rect)
        {
            return _x1 <= rect._x1 && rect._x2 <= _x2 &&
              _y1 <= rect._y1 && rect._y2 <= _y2;
        }

        public XRect ToXRect()
        {
            return new XRect(_x1, _y1, Width, Height);
        }

        public override string ToString()
        {
            const string format = Config.SignificantFigures3;
            return PdfEncoders.Format("[{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "}]", _x1, _y1, _x2, _y2);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture,
                    "X1={0:" + format + "}, Y1={1:" + format + "}, X2={2:" + format + "}, Y2={3:" + format + "}", _x1, _y1, _x2, _y2);
            }
        }
        public static readonly PdfRectangle Empty = new PdfRectangle();
    }
}
