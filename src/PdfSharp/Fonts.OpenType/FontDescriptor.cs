using PdfSharp.Drawing;

namespace PdfSharp.Fonts.OpenType
{
    internal class FontDescriptor
    {
        protected FontDescriptor(string key)
        {
            _key = key;
        }

        public string Key
        {
            get { return _key; }
        }
        readonly string _key;







        public string FontName
        {
            get { return _fontName; }
            protected set { _fontName = value; }
        }
        string _fontName;

        public string Weight
        {
            get { return _weight; }
            private set { _weight = value; }     
        }
        string _weight;

        public virtual bool IsBoldFace
        {
            get { return false; }
        }

        public float ItalicAngle
        {
            get { return _italicAngle; }
            protected set { _italicAngle = value; }
        }
        float _italicAngle;

        public virtual bool IsItalicFace
        {
            get { return false; }
        }

        public int XMin
        {
            get { return _xMin; }
            protected set { _xMin = value; }
        }
        int _xMin;

        public int YMin
        {
            get { return _yMin; }
            protected set { _yMin = value; }
        }
        int _yMin;

        public int XMax
        {
            get { return _xMax; }
            protected set { _xMax = value; }
        }
        int _xMax;

        public int YMax
        {
            get { return _yMax; }
            protected set { _yMax = value; }
        }
        int _yMax;

        public bool IsFixedPitch
        {
            get { return _isFixedPitch; }
            private set { _isFixedPitch = value; }     
        }
        bool _isFixedPitch;

        public int UnderlinePosition
        {
            get { return _underlinePosition; }
            protected set { _underlinePosition = value; }
        }
        int _underlinePosition;

        public int UnderlineThickness
        {
            get { return _underlineThickness; }
            protected set { _underlineThickness = value; }
        }
        int _underlineThickness;

        public int StrikeoutPosition
        {
            get { return _strikeoutPosition; }
            protected set { _strikeoutPosition = value; }
        }
        int _strikeoutPosition;

        public int StrikeoutSize
        {
            get { return _strikeoutSize; }
            protected set { _strikeoutSize = value; }
        }
        int _strikeoutSize;

        public string Version
        {
            get { return _version; }
            private set { _version = value; }     
        }
        string _version;

        public string EncodingScheme
        {
            get { return _encodingScheme; }
            private set { _encodingScheme = value; }     
        }
        string _encodingScheme;

        public int UnitsPerEm
        {
            get { return _unitsPerEm; }
            protected set { _unitsPerEm = value; }
        }
        int _unitsPerEm;

        public int CapHeight
        {
            get { return _capHeight; }
            protected set { _capHeight = value; }
        }
        int _capHeight;

        public int XHeight
        {
            get { return _xHeight; }
            protected set { _xHeight = value; }
        }
        int _xHeight;

        public int Ascender
        {
            get { return _ascender; }
            protected set { _ascender = value; }
        }
        int _ascender;

        public int Descender
        {
            get { return _descender; }
            protected set { _descender = value; }
        }
        int _descender;

        public int Leading
        {
            get { return _leading; }
            protected set { _leading = value; }
        }
        int _leading;

        public int Flags
        {
            get { return _flags; }
            private set { _flags = value; }     
        }
        int _flags;

        public int StemV
        {
            get { return _stemV; }
            protected set { _stemV = value; }
        }
        int _stemV;

        public int LineSpacing
        {
            get { return _lineSpacing; }
            protected set { _lineSpacing = value; }
        }
        int _lineSpacing;


        internal static string ComputeKey(XFont font)
        {
            return font.GlyphTypeface.Key;
        }

        internal static string ComputeKey(string name, XFontStyle style)
        {
            return ComputeKey(name,
                (style & XFontStyle.Bold) == XFontStyle.Bold,
                (style & XFontStyle.Italic) == XFontStyle.Italic);
        }

        internal static string ComputeKey(string name, bool isBold, bool isItalic)
        {
            string key = name.ToLowerInvariant() + '/'
                + (isBold ? "b" : "") + (isItalic ? "i" : "");
            return key;
        }

        internal static string ComputeKey(string name)
        {
            string key = name.ToLowerInvariant();
            return key;
        }
    }
}