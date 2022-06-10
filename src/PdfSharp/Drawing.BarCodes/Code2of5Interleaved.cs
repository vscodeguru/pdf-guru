namespace PdfSharp.Drawing.BarCodes
{
    public class Code2of5Interleaved : ThickThinBarCode
    {
        public Code2of5Interleaved()
            : base("", XSize.Empty, CodeDirection.LeftToRight)
        {}

        public Code2of5Interleaved(string code)
            : base(code, XSize.Empty, CodeDirection.LeftToRight)
        {}

        public Code2of5Interleaved(string code, XSize size)
            : base(code, size, CodeDirection.LeftToRight)
        {}

        public Code2of5Interleaved(string code, XSize size, CodeDirection direction)
            : base(code, size, direction)
        {}

        static bool[] ThickAndThinLines(int digit)
        {
            return Lines[digit];
        }
        static bool[][] Lines = 
        {
            new bool[] {false, false, true, true, false},
            new bool[] {true, false, false, false, true},
            new bool[] {false, true, false, false, true},
            new bool[] {true, true, false, false, false},
            new bool[] {false, false, true, false, true},
            new bool[] {true, false, true, false, false},
            new bool[] {false, true, true, false, false},
            new bool[] {false, false, false, true, true},
            new bool[] {true, false, false, true, false},
            new bool[] {false, true, false, true, false},
        };

        protected internal override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            XGraphicsState state = gfx.Save();

            BarCodeRenderInfo info = new BarCodeRenderInfo(gfx, brush, font, position);
            InitRendering(info);
            info.CurrPosInString = 0;
            info.CurrPos = position - CodeBase.CalcDistance(AnchorType.TopLeft, Anchor, Size);

            if (TurboBit)
                RenderTurboBit(info, true);
            RenderStart(info);
            while (info.CurrPosInString < Text.Length)
                RenderNextPair(info);
            RenderStop(info);
            if (TurboBit)
                RenderTurboBit(info, false);
            if (TextLocation != TextLocation.None)
                RenderText(info);

            gfx.Restore(state);
        }

        internal override void CalcThinBarWidth(BarCodeRenderInfo info)
        {
            double thinLineAmount = 6 + WideNarrowRatio + (2 * WideNarrowRatio + 3) * Text.Length;
            info.ThinBarWidth = Size.Width / thinLineAmount;
        }

        private void RenderStart(BarCodeRenderInfo info)
        {
            RenderBar(info, false);
            RenderGap(info, false);
            RenderBar(info, false);
            RenderGap(info, false);
        }

        private void RenderStop(BarCodeRenderInfo info)
        {
            RenderBar(info, true);
            RenderGap(info, false);
            RenderBar(info, false);
        }

        private void RenderNextPair(BarCodeRenderInfo info)
        {
            int digitForLines = int.Parse(Text[info.CurrPosInString].ToString());
            int digitForGaps = int.Parse(Text[info.CurrPosInString + 1].ToString());
            bool[] linesArray = Lines[digitForLines];
            bool[] gapsArray = Lines[digitForGaps];
            for (int idx = 0; idx < 5; ++idx)
            {
                RenderBar(info, linesArray[idx]);
                RenderGap(info, gapsArray[idx]);
            }
            info.CurrPosInString += 2;
        }

        protected override void CheckCode(string text)
        {
        }
    }
}
