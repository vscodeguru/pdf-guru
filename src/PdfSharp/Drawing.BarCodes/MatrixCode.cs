using System;

namespace PdfSharp.Drawing.BarCodes
{
    public abstract class MatrixCode : CodeBase
    {
        public MatrixCode(string text, string encoding, int rows, int columns, XSize size)
            : base(text, size, CodeDirection.LeftToRight)
        {
            _encoding = encoding;
            if (String.IsNullOrEmpty(_encoding))
                _encoding = new String('a', Text.Length);

            if (columns < rows)
            {
                _rows = columns;
                _columns = rows;
            }
            else
            {
                _columns = columns;
                _rows = rows;
            }

            Text = text;
        }

        public string Encoding
        {
            get { return _encoding; }
            set
            {
                _encoding = value;
                _matrixImage = null;
            }
        }
        string _encoding;

        public int Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                _matrixImage = null;
            }
        }
        int _columns;

        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                _matrixImage = null;
            }
        }
        int _rows;

        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                _matrixImage = null;
            }
        }

        internal XImage MatrixImage
        {
            get { return _matrixImage; }
            set { _matrixImage = value; }
        }
        XImage _matrixImage;

        protected internal abstract void Render(XGraphics gfx, XBrush brush, XPoint center);

        protected override void CheckCode(string text)
        { }
    }
}
