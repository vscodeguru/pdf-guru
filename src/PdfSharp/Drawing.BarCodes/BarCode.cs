using System;
using System.ComponentModel;

namespace PdfSharp.Drawing.BarCodes
{
    public abstract class BarCode : CodeBase
    {
        public BarCode(string text, XSize size, CodeDirection direction)
            : base(text, size, direction)
        {
            Text = text;
            Size = size;
            Direction = direction;
        }

        public static BarCode FromType(CodeType type, string text, XSize size, CodeDirection direction)
        {
            switch (type)
            {
                case CodeType.Code2of5Interleaved:
                    return new Code2of5Interleaved(text, size, direction);

                case CodeType.Code3of9Standard:
                    return new Code3of9Standard(text, size, direction);

                default:
                    throw new InvalidEnumArgumentException("type", (int)type, typeof(CodeType));
            }
        }

        public static BarCode FromType(CodeType type, string text, XSize size)
        {
            return FromType(type, text, size, CodeDirection.LeftToRight);
        }

        public static BarCode FromType(CodeType type, string text)
        {
            return FromType(type, text, XSize.Empty, CodeDirection.LeftToRight);
        }

        public static BarCode FromType(CodeType type)
        {
            return FromType(type, String.Empty, XSize.Empty, CodeDirection.LeftToRight);
        }

        public virtual double WideNarrowRatio
        {
            get { return 0; }
            set { }
        }

        public TextLocation TextLocation
        {
            get { return _textLocation; }
            set { _textLocation = value; }
        }
        TextLocation _textLocation;

        public int DataLength
        {
            get { return _dataLength; }
            set { _dataLength = value; }
        }
        int _dataLength;

        public char StartChar
        {
            get { return _startChar; }
            set { _startChar = value; }
        }
        char _startChar;

        public char EndChar
        {
            get { return _endChar; }
            set { _endChar = value; }
        }
        char _endChar;

        public virtual bool TurboBit
        {
            get { return _turboBit; }
            set { _turboBit = value; }
        }
        bool _turboBit;

        internal virtual void InitRendering(BarCodeRenderInfo info)
        {
            if (Text == null)
                throw new InvalidOperationException(BcgSR.BarCodeNotSet);

            if (Size.IsEmpty)
                throw new InvalidOperationException(BcgSR.EmptyBarCodeSize);
        }

        protected internal abstract void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position);
    }
}
