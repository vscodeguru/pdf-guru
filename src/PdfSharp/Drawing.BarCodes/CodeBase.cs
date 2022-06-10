namespace PdfSharp.Drawing.BarCodes
{
    public abstract class CodeBase
    {
        public CodeBase(string text, XSize size, CodeDirection direction)
        {
            _text = text;
            _size = size;
            _direction = direction;
        }

        public XSize Size
        {
            get { return _size; }
            set { _size = value; }
        }
        XSize _size;

        public string Text
        {
            get { return _text; }
            set
            {
                CheckCode(value);
                _text = value;
            }
        }
        string _text;

        public AnchorType Anchor
        {
            get { return _anchor; }
            set { _anchor = value; }
        }
        AnchorType _anchor;

        public CodeDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        CodeDirection _direction;

        protected abstract void CheckCode(string text);

        public static XVector CalcDistance(AnchorType oldType, AnchorType newType, XSize size)
        {
            if (oldType == newType)
                return new XVector();

            XVector result;
            Delta delta = Deltas[(int)oldType, (int)newType];
            result = new XVector(size.Width / 2 * delta.X, size.Height / 2 * delta.Y);
            return result;
        }

        struct Delta
        {
            public Delta(int x, int y)
            {
                X = x;
                Y = y;
            }
            public readonly int X;
            public readonly int Y;
        }
        static readonly Delta[,] Deltas = new Delta[9, 9]
        {
              { new Delta(0, 0),   new Delta(1, 0),   new Delta(2, 0),  new Delta(0, 1),   new Delta(1, 1),   new Delta(2, 1),  new Delta(0, 2),  new Delta(1, 2),  new Delta(2, 2) },
              { new Delta(-1, 0),  new Delta(0, 0),   new Delta(1, 0),  new Delta(-1, 1),  new Delta(0, 1),   new Delta(1, 1),  new Delta(-1, 2), new Delta(0, 2),  new Delta(1, 2) },
              { new Delta(-2, 0),  new Delta(-1, 0),  new Delta(0, 0),  new Delta(-2, 1),  new Delta(-1, 1),  new Delta(0, 1),  new Delta(-2, 2), new Delta(-1, 2), new Delta(0, 2) },
              { new Delta(0, -1),  new Delta(1, -1),  new Delta(2, -1), new Delta(0, 0),   new Delta(1, 0),   new Delta(2, 0),  new Delta(0, 1),  new Delta(1, 1),  new Delta(2, 1) },
              { new Delta(-1, -1), new Delta(0, -1),  new Delta(1, -1), new Delta(-1, 0),  new Delta(0, 0),   new Delta(1, 0),  new Delta(-1, 1), new Delta(0, 1),  new Delta(1, 1) },
              { new Delta(-2, -1), new Delta(-1, -1), new Delta(0, -1), new Delta(-2, 0),  new Delta(-1, 0),  new Delta(0, 0),  new Delta(-2, 1), new Delta(-1, 1), new Delta(0, 1) },
              { new Delta(0, -2),  new Delta(1, -2),  new Delta(2, -2), new Delta(0, -1),  new Delta(1, -1),  new Delta(2, -1), new Delta(0, 0),  new Delta(1, 0),  new Delta(2, 0) },
              { new Delta(-1, -2), new Delta(0, -2),  new Delta(1, -2), new Delta(-1, -1), new Delta(0, -1),  new Delta(1, -1), new Delta(-1, 0), new Delta(0, 0),  new Delta(1, 0) },
              { new Delta(-2, -2), new Delta(-1, -2), new Delta(0, -2), new Delta(-2, -1), new Delta(-1, -1), new Delta(0, -1), new Delta(-2, 0), new Delta(-1, 0), new Delta(0, 0) },
        };
    }
}