using System;
using System.Diagnostics;
using System.Globalization;
using GdiFont = System.Drawing.Font;
using GdiFontStyle = System.Drawing.FontStyle;
using PdfSharp.Fonts;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Internal;

namespace PdfSharp.Drawing
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal sealed class XGlyphTypeface
    {
        const string KeyPrefix = "tk:";    

#if CORE || GDI
        XGlyphTypeface(string key, XFontFamily fontFamily, XFontSource fontSource, XStyleSimulations styleSimulations, GdiFont gdiFont)
        {
            _key = key;
            _fontFamily = fontFamily;
            _fontSource = fontSource;

            _fontface = OpenTypeFontface.CetOrCreateFrom(fontSource);
            Debug.Assert(ReferenceEquals(_fontSource.Fontface, _fontface));

            _gdiFont = gdiFont;

            _styleSimulations = styleSimulations;
            Initialize();
        }
#endif
        public static XGlyphTypeface GetOrCreateFrom(string familyName, FontResolvingOptions fontResolvingOptions)
        {
            string typefaceKey = ComputeKey(familyName, fontResolvingOptions);
            XGlyphTypeface glyphTypeface;
            try
            {
                Lock.EnterFontFactory();
                if (GlyphTypefaceCache.TryGetGlyphTypeface(typefaceKey, out glyphTypeface))
                {
                    return glyphTypeface;
                }

                FontResolverInfo fontResolverInfo = FontFactory.ResolveTypeface(familyName, fontResolvingOptions, typefaceKey);
                if (fontResolverInfo == null)
                {
                    throw new InvalidOperationException("No appropriate font found.");
                }

#if CORE || GDI
                GdiFont gdiFont = null;
#endif
                XFontFamily fontFamily;
                PlatformFontResolverInfo platformFontResolverInfo = fontResolverInfo as PlatformFontResolverInfo;
                if (platformFontResolverInfo != null)
                {
#if CORE || GDI
                    gdiFont = platformFontResolverInfo.GdiFont;
                    fontFamily = XFontFamily.GetOrCreateFromGdi(gdiFont);
#endif
                }
                else
                {
                    fontFamily = XFontFamily.GetOrCreateFontFamily(familyName);
                }

                XFontSource fontSource = FontFactory.GetFontSourceByFontName(fontResolverInfo.FaceName);
                Debug.Assert(fontSource != null);

#if CORE || GDI
                glyphTypeface = new XGlyphTypeface(typefaceKey, fontFamily, fontSource, fontResolverInfo.StyleSimulations, gdiFont);
#endif
                GlyphTypefaceCache.AddGlyphTypeface(glyphTypeface);
            }
            finally { Lock.ExitFontFactory(); }
            return glyphTypeface;
        }

#if CORE || GDI
        public static XGlyphTypeface GetOrCreateFromGdi(GdiFont gdiFont)
        {
            string typefaceKey = ComputeKey(gdiFont);
            XGlyphTypeface glyphTypeface;
            if (GlyphTypefaceCache.TryGetGlyphTypeface(typefaceKey, out glyphTypeface))
            {
                return glyphTypeface;
            }

            XFontFamily fontFamily = XFontFamily.GetOrCreateFromGdi(gdiFont);
            XFontSource fontSource = XFontSource.GetOrCreateFromGdi(typefaceKey, gdiFont);

            XStyleSimulations styleSimulations = XStyleSimulations.None;
            if (gdiFont.Bold && !fontSource.Fontface.os2.IsBold)
                styleSimulations |= XStyleSimulations.BoldSimulation;
            if (gdiFont.Italic && !fontSource.Fontface.os2.IsItalic)
                styleSimulations |= XStyleSimulations.ItalicSimulation;

            glyphTypeface = new XGlyphTypeface(typefaceKey, fontFamily, fontSource, styleSimulations, gdiFont);
            GlyphTypefaceCache.AddGlyphTypeface(glyphTypeface);

            return glyphTypeface;
        }
#endif

        public XFontFamily FontFamily
        {
            get { return _fontFamily; }
        }
        readonly XFontFamily _fontFamily;

        internal OpenTypeFontface Fontface
        {
            get { return _fontface; }
        }
        readonly OpenTypeFontface _fontface;

        public XFontSource FontSource
        {
            get { return _fontSource; }
        }
        readonly XFontSource _fontSource;

        void Initialize()
        {
            _familyName = _fontface.name.Name;
            if (string.IsNullOrEmpty(_faceName) || _faceName.StartsWith("?"))
                _faceName = _familyName;
            _styleName = _fontface.name.Style;
            _displayName = _fontface.name.FullFontName;
            if (string.IsNullOrEmpty(_displayName))
            {
                _displayName = _familyName;
                if (string.IsNullOrEmpty(_styleName))
                    _displayName += " (" + _styleName + ")";
            }

            _isBold = _fontface.os2.IsBold;
            _isItalic = _fontface.os2.IsItalic;
        }

        internal string FaceName
        {
            get { return _faceName; }
        }
        string _faceName;

        public string FamilyName
        {
            get { return _familyName; }
        }
        string _familyName;

        public string StyleName
        {
            get { return _styleName; }
        }
        string _styleName;

        public string DisplayName
        {
            get { return _displayName; }
        }
        string _displayName;

        public bool IsBold
        {
            get { return _isBold; }
        }
        bool _isBold;

        public bool IsItalic
        {
            get { return _isItalic; }
        }
        bool _isItalic;

        public XStyleSimulations StyleSimulations
        {
            get { return _styleSimulations; }
        }
        XStyleSimulations _styleSimulations;

        string GetFaceNameSuffix()
        {
            if (IsBold)
                return IsItalic ? ",BoldItalic" : ",Bold";
            return IsItalic ? ",Italic" : "";
        }

        internal string GetBaseName()
        {
            string name = DisplayName;
            int ich = name.IndexOf("bold", StringComparison.OrdinalIgnoreCase);
            if (ich > 0)
                name = name.Substring(0, ich) + name.Substring(ich + 4, name.Length - ich - 4);
            ich = name.IndexOf("italic", StringComparison.OrdinalIgnoreCase);
            if (ich > 0)
                name = name.Substring(0, ich) + name.Substring(ich + 6, name.Length - ich - 6);
            name = name.Trim();
            name += GetFaceNameSuffix();
            return name;
        }

        internal static string ComputeKey(string familyName, FontResolvingOptions fontResolvingOptions)
        {
            string simulationSuffix = "";
            if (fontResolvingOptions.OverrideStyleSimulations)
            {
                switch (fontResolvingOptions.StyleSimulations)
                {
                    case XStyleSimulations.BoldSimulation: simulationSuffix = "|b+/i-"; break;
                    case XStyleSimulations.ItalicSimulation: simulationSuffix = "|b-/i+"; break;
                    case XStyleSimulations.BoldItalicSimulation: simulationSuffix = "|b+/i+"; break;
                    case XStyleSimulations.None: break;
                    default: throw new ArgumentOutOfRangeException("fontResolvingOptions");
                }
            }
            string key = KeyPrefix + familyName.ToLowerInvariant()
                + (fontResolvingOptions.IsItalic ? "/i" : "/n")        
                + (fontResolvingOptions.IsBold ? "/700" : "/400") + "/5"  
                + simulationSuffix;
            return key;
        }

        internal static string ComputeKey(string familyName, bool isBold, bool isItalic)
        {
            return ComputeKey(familyName, new FontResolvingOptions(FontHelper.CreateStyle(isBold, isItalic)));
        }

#if CORE || GDI
        internal static string ComputeKey(GdiFont gdiFont)
        {
            string name1 = gdiFont.Name;
            string name2 = gdiFont.OriginalFontName;
            string name3 = gdiFont.SystemFontName;

            string name = name1;
            GdiFontStyle style = gdiFont.Style;

            string key = KeyPrefix + name.ToLowerInvariant() + ((style & GdiFontStyle.Italic) == GdiFontStyle.Italic ? "/i" : "/n") + ((style & GdiFontStyle.Bold) == GdiFontStyle.Bold ? "/700" : "/400") + "/5";  
            return key;
        }
#endif


        public string Key
        {
            get { return _key; }
        }
        readonly string _key;

#if CORE || GDI
        internal GdiFont GdiFont
        {
            get { return _gdiFont; }
        }

        private readonly GdiFont _gdiFont;
#endif
        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0} - {1} ({2})", FamilyName, StyleName, FaceName); }
        }
    }
}
