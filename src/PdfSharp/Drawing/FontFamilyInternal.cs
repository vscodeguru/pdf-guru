using PdfSharp.Internal;
using System.Diagnostics;
using System.Globalization;
using GdiFontFamily = System.Drawing.FontFamily;

namespace PdfSharp.Drawing
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class FontFamilyInternal
    {
        FontFamilyInternal(string familyName, bool createPlatformObjects)
        {
            _sourceName = _name = familyName;

            if (createPlatformObjects)
            {
                _gdiFontFamily = new GdiFontFamily(familyName);
                _name = _gdiFontFamily.Name;
            }

        }

        FontFamilyInternal(GdiFontFamily gdiFontFamily)
        {
            _sourceName = _name = gdiFontFamily.Name;
            _gdiFontFamily = gdiFontFamily;
        }


        internal static FontFamilyInternal GetOrCreateFromName(string familyName, bool createPlatformObject)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal family = FontFamilyCache.GetFamilyByName(familyName);
                if (family == null)
                {
                    family = new FontFamilyInternal(familyName, createPlatformObject);
                    family = FontFamilyCache.CacheOrGetFontFamily(family);
                }
                return family;
            }
            finally { Lock.ExitFontFactory(); }
        }


        internal static FontFamilyInternal GetOrCreateFromGdi(GdiFontFamily gdiFontFamily)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal fontFamily = new FontFamilyInternal(gdiFontFamily);
                fontFamily = FontFamilyCache.CacheOrGetFontFamily(fontFamily);
                return fontFamily;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public string SourceName
        {
            get { return _sourceName; }
        }
        readonly string _sourceName;

        public string Name
        {
            get { return _name; }
        }
        readonly string _name;

        public GdiFontFamily GdiFamily
        {
            get { return _gdiFontFamily; }
        }
        readonly GdiFontFamily _gdiFontFamily;



        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "FontFamily: '{0}'", Name); }
        }
    }
}
