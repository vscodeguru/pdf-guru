using System;
using System.Diagnostics;
using System.Text;
using PdfSharp.Fonts;

namespace PdfSharp.Pdf.Advanced
{
    public class PdfFont : PdfDictionary
    {
        public PdfFont(PdfDocument document)
            : base(document)
        { }

        internal PdfFontDescriptor FontDescriptor
        {
            get
            {
                Debug.Assert(_fontDescriptor != null);
                return _fontDescriptor;
            }
            set { _fontDescriptor = value; }
        }
        PdfFontDescriptor _fontDescriptor;

        internal PdfFontEncoding FontEncoding;

        public bool IsSymbolFont
        {
            get { return _fontDescriptor.IsSymbolFont; }
        }

        internal void AddChars(string text)
        {
            if (_cmapInfo != null)
                _cmapInfo.AddChars(text);
        }

        internal void AddGlyphIndices(string glyphIndices)
        {
            if (_cmapInfo != null)
                _cmapInfo.AddGlyphIndices(glyphIndices);
        }

        internal CMapInfo CMapInfo
        {
            get { return _cmapInfo; }
            set { _cmapInfo = value; }
        }
        internal CMapInfo _cmapInfo;

        internal PdfToUnicodeMap ToUnicodeMap
        {
            get { return _toUnicode; }
            set { _toUnicode = value; }
        }
        internal PdfToUnicodeMap _toUnicode;


        internal static string CreateEmbeddedFontSubsetName(string name)
        {
            StringBuilder s = new StringBuilder(64);
            byte[] bytes = Guid.NewGuid().ToByteArray();
            for (int idx = 0; idx < 6; idx++)
                s.Append((char)('A' + bytes[idx] % 26));
            s.Append('+');
            if (name.StartsWith("/"))
                s.Append(name.Substring(1));
            else
                s.Append(name);
            return s.ToString();
        }

        public class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Font")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string Subtype = "/Subtype";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string BaseFont = "/BaseFont";

            [KeyInfo(KeyType.Dictionary | KeyType.MustBeIndirect, typeof(PdfFontDescriptor))]
            public const string FontDescriptor = "/FontDescriptor";
        }
    }
}