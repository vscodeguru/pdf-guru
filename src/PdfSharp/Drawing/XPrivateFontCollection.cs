using System;
using System.Collections.Generic;
using System.IO;

namespace PdfSharp.Drawing
{
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

        [Obsolete("Use the GDI build of PDFsharp and use Add(Stream stream)")]

        public static void AddFont(string filename)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Use the GDI build of PDFsharp and use Add(Stream stream)")]

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
}
