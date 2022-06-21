namespace PdfSharp.Drawing.BarCodes
{
    public class CodeOmr : BarCode
    {
        public CodeOmr(string text, XSize size, CodeDirection direction)
            : base(text, size, direction)
        { }

        protected internal override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
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

            XPoint pt = position - CodeBase.CalcDistance(AnchorType.TopLeft, Anchor, Size);
            uint value;
            uint.TryParse(Text, out value);
            value |= 1;
            _synchronizeCode = true;

            if (_synchronizeCode)
            {
                XRect rect = new XRect(pt.X, pt.Y, _makerThickness, Size.Height);
                gfx.DrawRectangle(brush, rect);
                pt.X += 2 * _makerDistance;
            }
            for (int idx = 0; idx < 32; idx++)
            {
                if ((value & 1) == 1)
                {
                    XRect rect = new XRect(pt.X + idx * _makerDistance, pt.Y, _makerThickness, Size.Height);
                    gfx.DrawRectangle(brush, rect);
                }
                value = value >> 1;
            }
            gfx.Restore(state);
        }

        public bool SynchronizeCode
        {
            get { return _synchronizeCode; }
            set { _synchronizeCode = value; }
        }
        bool _synchronizeCode;

        public double MakerDistance
        {
            get { return _makerDistance; }
            set { _makerDistance = value; }
        }
        double _makerDistance = 12;   

        public double MakerThickness
        {
            get { return _makerThickness; }
            set { _makerThickness = value; }
        }
        double _makerThickness = 1;

        protected override void CheckCode(string text)
        { }
    }
}
