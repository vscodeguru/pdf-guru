using System;

namespace PdfSharp.Drawing.BarCodes
{
    public class CodeDataMatrix : MatrixCode
    {
        public CodeDataMatrix()
            : this("", "", 26, 26, 0, XSize.Empty)
        {}

        public CodeDataMatrix(string code, int length)
            : this(code, "", length, length, 0, XSize.Empty)
        {}

        public CodeDataMatrix(string code, int length, XSize size)
            : this(code, "", length, length, 0, size)
        {}

        public CodeDataMatrix(string code, DataMatrixEncoding dmEncoding, int length, XSize size)
            : this(code, CreateEncoding(dmEncoding, code.Length), length, length, 0, size)
        {}

        public CodeDataMatrix(string code, int rows, int columns)
            : this(code, "", rows, columns, 0, XSize.Empty)
        {}

        public CodeDataMatrix(string code, int rows, int columns, XSize size)
            : this(code, "", rows, columns, 0, size)
        {}

        public CodeDataMatrix(string code, DataMatrixEncoding dmEncoding, int rows, int columns, XSize size)
            : this(code, CreateEncoding(dmEncoding, code.Length), rows, columns, 0, size)
        {}

        public CodeDataMatrix(string code, int rows, int columns, int quietZone)
            : this(code, "", rows, columns, quietZone, XSize.Empty)
        {}

        public CodeDataMatrix(string code, string encoding, int rows, int columns, int quietZone, XSize size)
            : base(code, encoding, rows, columns, size)
        {
            QuietZone = quietZone;
        }

        public void SetEncoding(DataMatrixEncoding dmEncoding)
        {
            Encoding = CreateEncoding(dmEncoding, Text.Length);
        }

        static string CreateEncoding(DataMatrixEncoding dmEncoding, int length)
        {
            string tempencoding = "";
            switch (dmEncoding)
            {
                case DataMatrixEncoding.Ascii:
                    tempencoding = new string('a', length);
                    break;
                case DataMatrixEncoding.C40:
                    tempencoding = new string('c', length);
                    break;
                case DataMatrixEncoding.Text:
                    tempencoding = new string('t', length);
                    break;
                case DataMatrixEncoding.X12:
                    tempencoding = new string('x', length);
                    break;
                case DataMatrixEncoding.EDIFACT:
                    tempencoding = new string('e', length);
                    break;
                case DataMatrixEncoding.Base256:
                    tempencoding = new string('b', length);
                    break;
            }
            return tempencoding;
        }

        public int QuietZone
        {
            get { return _quietZone; }
            set { _quietZone = value; }
        }
        int _quietZone;

        protected internal override void Render(XGraphics gfx, XBrush brush, XPoint position)
        {
            XGraphicsState state = gfx.Save();

            switch (Direction)
            {
                case CodeDirection.RightToLeft:
                    gfx.RotateAtTransform(180, position);
                    break;

                case CodeDirection.TopToBottom:
                    gfx.RotateAtTransform(90, position);
                    break;

                case CodeDirection.BottomToTop:
                    gfx.RotateAtTransform(-90, position);
                    break;
            }

            XPoint pos = position + CalcDistance(Anchor, AnchorType.TopLeft, Size);

            if (MatrixImage == null)
                MatrixImage = DataMatrixImage.GenerateMatrixImage(Text, Encoding, Rows, Columns);

            if (QuietZone > 0)
            {
                XSize sizeWithZone = new XSize(Size.Width, Size.Height);
                sizeWithZone.Width = sizeWithZone.Width / (Columns + 2 * QuietZone) * Columns;
                sizeWithZone.Height = sizeWithZone.Height / (Rows + 2 * QuietZone) * Rows;

                XPoint posWithZone = new XPoint(pos.X, pos.Y);
                posWithZone.X += Size.Width / (Columns + 2 * QuietZone) * QuietZone;
                posWithZone.Y += Size.Height / (Rows + 2 * QuietZone) * QuietZone;

                gfx.DrawRectangle(XBrushes.White, pos.X, pos.Y, Size.Width, Size.Height);
                gfx.DrawImage(MatrixImage, posWithZone.X, posWithZone.Y, sizeWithZone.Width, sizeWithZone.Height);
            }
            else
                gfx.DrawImage(MatrixImage, pos.X, pos.Y, Size.Width, Size.Height);

            gfx.Restore(state);
        }

        protected override void CheckCode(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            DataMatrixImage mImage = new DataMatrixImage(Text, Encoding, Rows, Columns);
            mImage.Iec16022Ecc200(Columns, Rows, Encoding, Text.Length, Text, 0, 0, 0);
        }
    }
}
