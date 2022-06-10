using System;

namespace PdfSharp.Drawing.BarCodes
{
    public class Code3of9Standard : ThickThinBarCode
    {
        public Code3of9Standard()
            : base("", XSize.Empty, CodeDirection.LeftToRight)
        { }

        public Code3of9Standard(string code)
            : base(code, XSize.Empty, CodeDirection.LeftToRight)
        { }

        public Code3of9Standard(string code, XSize size)
            : base(code, size, CodeDirection.LeftToRight)
        { }

        public Code3of9Standard(string code, XSize size, CodeDirection direction)
            : base(code, size, direction)
        { }

        private static bool[] ThickThinLines(char ch)
        {
            return Lines["0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*".IndexOf(ch)];
        }
        static readonly bool[][] Lines =
        {
            new bool[] {false, false, false, true, true, false, true, false, false},
            new bool[] {true, false, false, true, false, false, false, false, true},
            new bool[] {false, false, true, true, false, false, false, false, true},
            new bool[] {true, false, true, true, false, false, false, false, false},
            new bool[] {false, false, false, true, true, false, false, false, true},
            new bool[] {true, false, false, true, true, false, false, false, false},
            new bool[] {false, false, true, true, true, false, false, false, false},
            new bool[] {false, false, false, true, false, false, true, false, true},
            new bool[] {true, false, false, true, false, false, true, false, false},
            new bool[] {false, false, true, true, false, false, true, false, false},
            new bool[] {true, false, false, false, false, true, false, false, true},
            new bool[] {false, false, true, false, false, true, false, false, true},
            new bool[] {true, false, true, false, false, true, false, false, false},
            new bool[] {false, false, false, false, true, true, false, false, true},
            new bool[] {true, false, false, false, true, true, false, false, false},
            new bool[] {false, false, true, false, true, true, false, false, false},
            new bool[] {false, false, false, false, false, true, true, false, true},
            new bool[] {true, false, false, false, false, true, true, false, false},
            new bool[] {false, false, true, false, false, true, true, false, false},
            new bool[] {false, false, false, false, true, true, true, false, false},
            new bool[] {true, false, false, false, false, false, false, true, true},
            new bool[] {false, false, true, false, false, false, false, true, true},
            new bool[] {true, false, true, false, false, false, false, true, false},
            new bool[] {false, false, false, false, true, false, false, true, true},
            new bool[] {true, false, false, false, true, false, false, true, false},
            new bool[] {false, false, true, false, true, false, false, true, false},
            new bool[] {false, false, false, false, false, false, true, true, true},
            new bool[] {true, false, false, false, false, false, true, true, false},
            new bool[] {false, false, true, false, false, false, true, true, false},
            new bool[] {false, false, false, false, true, false, true, true, false},
            new bool[] {true, true, false, false, false, false, false, false, true},
            new bool[] {false, true, true, false, false, false, false, false, true},
            new bool[] {true, true, true, false, false, false, false, false, false},
            new bool[] {false, true, false, false, true, false, false, false, true},
            new bool[] {true, true, false, false, true, false, false, false, false},
            new bool[] {false, true, true, false, true, false, false, false, false},
            new bool[] {false, true, false, false, false, false, true, false, true},
            new bool[] {true, true, false, false, false, false, true, false, false},
            new bool[] {false, true, true, false, false, false, true, false, false},
            new bool[] {false, true, false, true, false, true, false, false, false},
            new bool[] {false, true, false, true, false, false, false, true, false},
            new bool[] {false, true, false, false, false, true, false, true, false},
            new bool[] {false, false, false, true, false, true, false, true, false},
            new bool[] {false, true, false, false, true, false, true, false, false},
        };


        internal override void CalcThinBarWidth(BarCodeRenderInfo info)
        {
            double thinLineAmount = 13 + 6 * WideNarrowRatio + (3 * WideNarrowRatio + 7) * Text.Length;
            info.ThinBarWidth = Size.Width / thinLineAmount;
        }

        protected override void CheckCode(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (text.Length == 0)
                throw new ArgumentException(BcgSR.Invalid3Of9Code(text));

            foreach (char ch in text)
            {
                if ("0123456789ABCDEFGHIJKLMNOP'QRSTUVWXYZ-. $/+%*".IndexOf(ch) < 0)
                    throw new ArgumentException(BcgSR.Invalid3Of9Code(text));
            }
        }

        protected internal override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            XGraphicsState state = gfx.Save();

            BarCodeRenderInfo info = new BarCodeRenderInfo(gfx, brush, font, position);
            InitRendering(info);
            info.CurrPosInString = 0;
            info.CurrPos = position - CalcDistance(AnchorType.TopLeft, Anchor, Size);

            if (TurboBit)
                RenderTurboBit(info, true);
            RenderStart(info);
            while (info.CurrPosInString < Text.Length)
            {
                RenderNextChar(info);
                RenderGap(info, false);
            }
            RenderStop(info);
            if (TurboBit)
                RenderTurboBit(info, false);
            if (TextLocation != TextLocation.None)
                RenderText(info);

            gfx.Restore(state);
        }

        private void RenderNextChar(BarCodeRenderInfo info)
        {
            RenderChar(info, Text[info.CurrPosInString]);
            ++info.CurrPosInString;
        }

        private void RenderChar(BarCodeRenderInfo info, char ch)
        {
            bool[] thickThinLines = ThickThinLines(ch);
            int idx = 0;
            while (idx < 9)
            {
                RenderBar(info, thickThinLines[idx]);
                if (idx < 8)
                    RenderGap(info, thickThinLines[idx + 1]);
                idx += 2;
            }
        }

        private void RenderStart(BarCodeRenderInfo info)
        {
            RenderChar(info, '*');
            RenderGap(info, false);
        }

        private void RenderStop(BarCodeRenderInfo info)
        {
            RenderChar(info, '*');
        }
    }
}
