using System;

namespace PdfSharp.Drawing.BarCodes
{
    public abstract class ThickThinBarCode : BarCode        
    {
        public ThickThinBarCode(string code, XSize size, CodeDirection direction)
            : base(code, size, direction)
        { }

        internal override void InitRendering(BarCodeRenderInfo info)
        {
            base.InitRendering(info);
            CalcThinBarWidth(info);
            info.BarHeight = Size.Height;
            if (TextLocation != TextLocation.None)
                info.BarHeight *= 4.0 / 5;

            switch (Direction)
            {
                case CodeDirection.RightToLeft:
                    info.Gfx.RotateAtTransform(180, info.Position);
                    break;

                case CodeDirection.TopToBottom:
                    info.Gfx.RotateAtTransform(90, info.Position);
                    break;

                case CodeDirection.BottomToTop:
                    info.Gfx.RotateAtTransform(-90, info.Position);
                    break;
            }
        }

        public override double WideNarrowRatio
        {
            get { return _wideNarrowRatio; }
            set
            {
                if (value > 3 || value < 2)
                    throw new ArgumentOutOfRangeException("value", BcgSR.Invalid2of5Relation);
                _wideNarrowRatio = value;
            }
        }
        double _wideNarrowRatio = 2.6;

        internal void RenderBar(BarCodeRenderInfo info, bool isThick)
        {
            double barWidth = GetBarWidth(info, isThick);
            double height = Size.Height;
            double xPos = info.CurrPos.X;
            double yPos = info.CurrPos.Y;

            switch (TextLocation)
            {
                case TextLocation.AboveEmbedded:
                    height -= info.Gfx.MeasureString(Text, info.Font).Height;
                    yPos += info.Gfx.MeasureString(Text, info.Font).Height;
                    break;
                case TextLocation.BelowEmbedded:
                    height -= info.Gfx.MeasureString(Text, info.Font).Height;
                    break;
            }

            XRect rect = new XRect(xPos, yPos, barWidth, height);
            info.Gfx.DrawRectangle(info.Brush, rect);
            info.CurrPos.X += barWidth;
        }

        internal void RenderGap(BarCodeRenderInfo info, bool isThick)
        {
            info.CurrPos.X += GetBarWidth(info, isThick);
        }

        internal void RenderTurboBit(BarCodeRenderInfo info, bool startBit)
        {
            if (startBit)
                info.CurrPos.X -= 0.5 + GetBarWidth(info, true);
            else
                info.CurrPos.X += 0.5;  

            RenderBar(info, true);

            if (startBit)
                info.CurrPos.X += 0.5;  
        }

        internal void RenderText(BarCodeRenderInfo info)
        {
            if (info.Font == null)
                info.Font = new XFont("Courier New", Size.Height / 6);
            XPoint center = info.Position + CalcDistance(Anchor, AnchorType.TopLeft, Size);

            switch (TextLocation)
            {
                case TextLocation.Above:
                    center = new XPoint(center.X, center.Y - info.Gfx.MeasureString(Text, info.Font).Height);
                    info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), XStringFormats.TopCenter);
                    break;
                case TextLocation.AboveEmbedded:
                    info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), XStringFormats.TopCenter);
                    break;
                case TextLocation.Below:
                    center = new XPoint(center.X, info.Gfx.MeasureString(Text, info.Font).Height + center.Y);
                    info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), XStringFormats.BottomCenter);
                    break;
                case TextLocation.BelowEmbedded:
                    info.Gfx.DrawString(Text, info.Font, info.Brush, new XRect(center, Size), XStringFormats.BottomCenter);
                    break;
            }
        }

        internal double GetBarWidth(BarCodeRenderInfo info, bool isThick)
        {
            if (isThick)
                return info.ThinBarWidth * _wideNarrowRatio;
            return info.ThinBarWidth;
        }

        internal abstract void CalcThinBarWidth(BarCodeRenderInfo info);
    }
}
