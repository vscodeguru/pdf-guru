using System;
using System.Collections.Generic;
using System.IO;

namespace PdfSharp.Drawing
{
#if true
    public sealed class XPrivateFontCollection
    {
        XPrivateFontCollection()
        {
        }

        internal static XPrivateFontCollection Singleton
        {
            get { return _singleton; }
        }
        internal static XPrivateFontCollection _singleton = new XPrivateFontCollection();

#if GDI
 
#else
        [Obsolete("Use the GDI build of PDFsharp and use Add(Stream stream)")]
#endif
        public static void AddFont(string filename)
        {
            throw new NotImplementedException();
        }


#if GDI
        [Obsolete("Use Add(Stream stream)")]
#else
        [Obsolete("Use the GDI build of PDFsharp and use Add(Stream stream)")]
#endif
        public static void AddFont(Stream stream, string facename)
        {
            throw new NotImplementedException();
        }


        static string MakeKey(string familyName, XFontStyle style)
        {
            return MakeKey(familyName, (style & XFontStyle.Bold) != 0, (style & XFontStyle.Italic) != 0);
        }

        static string MakeKey(string familyName, bool bold, bool italic)
        {
            return familyName + "#" + (bold ? "b" : "") + (italic ? "i" : "");
        }

        readonly Dictionary<string, XGlyphTypeface> _typefaces = new Dictionary<string, XGlyphTypeface>();


    }
#endif
}
