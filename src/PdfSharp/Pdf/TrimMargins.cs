using System.Diagnostics;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("(Left={_left.Millimeter}mm, Right={_right.Millimeter}mm, Top={_top.Millimeter}mm, Bottom={_bottom.Millimeter}mm)")]
    public sealed class TrimMargins
    {
        public XUnit All
        {
            set
            {
                _left = value;
                _right = value;
                _top = value;
                _bottom = value;
            }
        }

        public XUnit Left
        {
            get { return _left; }
            set { _left = value; }
        }
        XUnit _left;

        public XUnit Right
        {
            get { return _right; }
            set { _right = value; }
        }
        XUnit _right;

        public XUnit Top
        {
            get { return _top; }
            set { _top = value; }
        }
        XUnit _top;

        public XUnit Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }
        XUnit _bottom;

        public bool AreSet
        {
            get { return _left.Value != 0 || _right.Value != 0 || _top.Value != 0 || _bottom.Value != 0; }
        }
    }
}
