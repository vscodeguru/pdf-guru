namespace PdfSharp.Drawing
{
    public sealed class XFontMetrics
    {
        internal XFontMetrics(string name, int unitsPerEm, int ascent, int descent, int leading, int lineSpacing,
            int capHeight, int xHeight, int stemV, int stemH, int averageWidth, int maxWidth ,
            int underlinePosition, int underlineThickness, int strikethroughPosition, int strikethroughThickness)
        {
            _name = name;
            _unitsPerEm = unitsPerEm;
            _ascent = ascent;
            _descent = descent;
            _leading = leading;
            _lineSpacing = lineSpacing;
            _capHeight = capHeight;
            _xHeight = xHeight;
            _stemV = stemV;
            _stemH = stemH;
            _averageWidth = averageWidth;
            _maxWidth = maxWidth;
            _underlinePosition = underlinePosition;
            _underlineThickness = underlineThickness;
            _strikethroughPosition = strikethroughPosition;
            _strikethroughThickness = strikethroughThickness;
        }

        public string Name
        {
            get { return _name; }
        }
        readonly string _name;

        public int UnitsPerEm
        {
            get { return _unitsPerEm; }
        }
        readonly int _unitsPerEm;

        public int Ascent
        {
            get { return _ascent; }
        }
        readonly int _ascent;

        public int Descent
        {
            get { return _descent; }
        }
        readonly int _descent;

        public int AverageWidth
        {
            get { return _averageWidth; }
        }
        readonly int _averageWidth;

        public int CapHeight
        {
            get { return _capHeight; }
        }
        readonly int _capHeight;

        public int Leading
        {
            get { return _leading; }
        }
        readonly int _leading;

        public int LineSpacing
        {
            get { return _lineSpacing; }
        }
        readonly int _lineSpacing;

        public int MaxWidth
        {
            get { return _maxWidth; }
        }
        readonly int _maxWidth;

        public int StemH
        {
            get { return _stemH; }
        }
        readonly int _stemH;

        public int StemV
        {
            get { return _stemV; }
        }
        readonly int _stemV;

        public int XHeight
        {
            get { return _xHeight; }
        }
        readonly int _xHeight;

        public int UnderlinePosition
        {
            get { return _underlinePosition; }
        }
        readonly int _underlinePosition;

        public int UnderlineThickness
        {
            get { return _underlineThickness; }
        }
        readonly int _underlineThickness;

        public int StrikethroughPosition
        {
            get { return _strikethroughPosition; }
        }
        readonly int _strikethroughPosition;

        public int StrikethroughThickness
        {
            get { return _strikethroughThickness; }
        }
        readonly int _strikethroughThickness;
    }
}
