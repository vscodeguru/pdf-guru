using System;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using GdiFontFamily = System.Drawing.FontFamily;
using GdiFont = System.Drawing.Font;
using GdiFontStyle = System.Drawing.FontStyle;
using PdfSharp.Fonts;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Internal;
using PdfSharp.Pdf;

namespace PdfSharp.Drawing
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public sealed class XFont
    {
        public XFont(string familyName, double emSize)
            : this(familyName, emSize, XFontStyle.Regular, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(string familyName, double emSize, XFontStyle style)
            : this(familyName, emSize, style, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(string familyName, double emSize, XFontStyle style, XPdfFontOptions pdfOptions)
        {
            _familyName = familyName;
            _emSize = emSize;
            _style = style;
            _pdfOptions = pdfOptions;
            Initialize();
        }

        internal XFont(string familyName, double emSize, XFontStyle style, XPdfFontOptions pdfOptions, XStyleSimulations styleSimulations)
        {
            _familyName = familyName;
            _emSize = emSize;
            _style = style;
            _pdfOptions = pdfOptions;
            OverrideStyleSimulations = true;
            StyleSimulations = styleSimulations;
            Initialize();
        }

        public XFont(GdiFontFamily fontFamily, double emSize, XFontStyle style)
            : this(fontFamily, emSize, style, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(GdiFontFamily fontFamily, double emSize, XFontStyle style, XPdfFontOptions pdfOptions)
        {
            _familyName = fontFamily.Name;
            _gdiFontFamily = fontFamily;
            _emSize = emSize;
            _style = style;
            _pdfOptions = pdfOptions;
            InitializeFromGdi();
        }

        public XFont(GdiFont font)
            : this(font, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(GdiFont font, XPdfFontOptions pdfOptions)
        {
            if (font.Unit != GraphicsUnit.World)
                throw new ArgumentException("Font must use GraphicsUnit.World.");
            _gdiFont = font;
            Debug.Assert(font.Name == font.FontFamily.Name);
            _familyName = font.Name;
            _emSize = font.Size;
            _style = FontStyleFrom(font);
            _pdfOptions = pdfOptions;
            InitializeFromGdi();
        }


        void Initialize()
        {
            FontResolvingOptions fontResolvingOptions = OverrideStyleSimulations
                ? new FontResolvingOptions(_style, StyleSimulations)
                : new FontResolvingOptions(_style);

            if (StringComparer.OrdinalIgnoreCase.Compare(_familyName, GlobalFontSettings.DefaultFontName) == 0)
            {
                _familyName = "Calibri";

            }

            _glyphTypeface = XGlyphTypeface.GetOrCreateFrom(_familyName, fontResolvingOptions);

            CreateDescriptorAndInitializeFontMetrics();
        }

        void InitializeFromGdi()
        {
            try
            {
                Lock.EnterFontFactory();
                if (_gdiFontFamily != null)
                {
                    _gdiFont = new Font(_gdiFontFamily, (float)_emSize, (GdiFontStyle)_style, GraphicsUnit.World);
                }

                if (_gdiFont != null)
                {

                    _familyName = _gdiFont.FontFamily.Name;
                }
                else
                {
                    Debug.Assert(false);
                }

                if (_glyphTypeface == null)
                    _glyphTypeface = XGlyphTypeface.GetOrCreateFromGdi(_gdiFont);

                CreateDescriptorAndInitializeFontMetrics();
            }
            finally { Lock.ExitFontFactory(); }
        }


        void CreateDescriptorAndInitializeFontMetrics()    
        {
            Debug.Assert(_fontMetrics == null, "InitializeFontMetrics() was already called.");
            _descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptorFor(this);   
            _fontMetrics = new XFontMetrics(_descriptor.FontName, _descriptor.UnitsPerEm, _descriptor.Ascender, _descriptor.Descender,
                _descriptor.Leading, _descriptor.LineSpacing, _descriptor.CapHeight, _descriptor.XHeight, _descriptor.StemV, 0, 0, 0,
                _descriptor.UnderlinePosition, _descriptor.UnderlineThickness, _descriptor.StrikeoutPosition, _descriptor.StrikeoutSize);

            XFontMetrics fm = Metrics;

            UnitsPerEm = _descriptor.UnitsPerEm;
            CellAscent = _descriptor.Ascender;
            CellDescent = _descriptor.Descender;
            CellSpace = _descriptor.LineSpacing;

            Debug.Assert(fm.UnitsPerEm == _descriptor.UnitsPerEm);
        }




        [Browsable(false)]
        public XFontFamily FontFamily
        {
            get { return _glyphTypeface.FontFamily; }
        }

        public string Name
        {
            get { return _glyphTypeface.FontFamily.Name; }
        }

        internal string FaceName
        {
            get { return _glyphTypeface.FaceName; }
        }

        public double Size
        {
            get { return _emSize; }
        }
        readonly double _emSize;

        [Browsable(false)]
        public XFontStyle Style
        {
            get { return _style; }
        }
        readonly XFontStyle _style;

        public bool Bold
        {
            get { return (_style & XFontStyle.Bold) == XFontStyle.Bold; }
        }

        public bool Italic
        {
            get { return (_style & XFontStyle.Italic) == XFontStyle.Italic; }
        }

        public bool Strikeout
        {
            get { return (_style & XFontStyle.Strikeout) == XFontStyle.Strikeout; }
        }

        public bool Underline
        {
            get { return (_style & XFontStyle.Underline) == XFontStyle.Underline; }
        }

        internal bool IsVertical
        {
            get { return _isVertical; }
            set { _isVertical = value; }
        }
        bool _isVertical;


        public XPdfFontOptions PdfOptions
        {
            get { return _pdfOptions ?? (_pdfOptions = new XPdfFontOptions()); }
        }
        XPdfFontOptions _pdfOptions;

        internal bool Unicode
        {
            get { return _pdfOptions != null && _pdfOptions.FontEncoding == PdfFontEncoding.Unicode; }
        }

        public int CellSpace
        {
            get { return _cellSpace; }
            internal set { _cellSpace = value; }
        }
        int _cellSpace;

        public int CellAscent
        {
            get { return _cellAscent; }
            internal set { _cellAscent = value; }
        }
        int _cellAscent;

        public int CellDescent
        {
            get { return _cellDescent; }
            internal set { _cellDescent = value; }
        }
        int _cellDescent;

        public XFontMetrics Metrics
        {
            get
            {
                Debug.Assert(_fontMetrics != null, "InitializeFontMetrics() not yet called.");
                return _fontMetrics;
            }
        }
        XFontMetrics _fontMetrics;

        public double GetHeight()
        {
            double value = CellSpace * _emSize / UnitsPerEm;
            return value;
        }

        [Obsolete("Use GetHeight() without parameter.")]
        public double GetHeight(XGraphics graphics)
        {
            throw new InvalidOperationException("Honestly: Use GetHeight() without parameter!");

        }

        [Browsable(false)]
        public int Height
        {
            get { return (int)Math.Ceiling(GetHeight()); }
        }

        internal XGlyphTypeface GlyphTypeface
        {
            get { return _glyphTypeface; }
        }
        XGlyphTypeface _glyphTypeface;


        internal OpenTypeDescriptor Descriptor
        {
            get { return _descriptor; }
            private set { _descriptor = value; }
        }
        OpenTypeDescriptor _descriptor;


        internal string FamilyName
        {
            get { return _familyName; }
        }
        string _familyName;


        internal int UnitsPerEm
        {
            get { return _unitsPerEm; }
            private set { _unitsPerEm = value; }
        }
        internal int _unitsPerEm;

        internal bool OverrideStyleSimulations;

        internal XStyleSimulations StyleSimulations;


        public GdiFontFamily GdiFontFamily
        {
            get { return _gdiFontFamily; }
        }
        readonly GdiFontFamily _gdiFontFamily;

        internal GdiFont GdiFont
        {
            get { return _gdiFont; }
        }
        Font _gdiFont;

        internal static XFontStyle FontStyleFrom(GdiFont font)
        {
            return
              (font.Bold ? XFontStyle.Bold : 0) |
              (font.Italic ? XFontStyle.Italic : 0) |
              (font.Strikeout ? XFontStyle.Strikeout : 0) |
              (font.Underline ? XFontStyle.Underline : 0);
        }

        public static implicit operator XFont(GdiFont font)
        {
            return new XFont(font);
        }

        internal string Selector
        {
            get { return _selector; }
            set { _selector = value; }
        }
        string _selector;

        string DebuggerDisplay
        {
            get { return String.Format(CultureInfo.InvariantCulture, "font=('{0}' {1:0.##})", Name, Size); }
        }
    }
}
