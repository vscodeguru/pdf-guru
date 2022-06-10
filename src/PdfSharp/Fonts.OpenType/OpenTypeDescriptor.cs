using System;
using System.Diagnostics;
using System.Text;
using PdfSharp.Pdf.Internal;
using PdfSharp.Drawing;


namespace PdfSharp.Fonts.OpenType
{
    internal sealed class OpenTypeDescriptor : FontDescriptor
    {
        public OpenTypeDescriptor(string fontDescriptorKey, string name, XFontStyle stlye, OpenTypeFontface fontface, XPdfFontOptions options)
            : base(fontDescriptorKey)
        {
            FontFace = fontface;
            FontName = name;
            Initialize();
        }

        public OpenTypeDescriptor(string fontDescriptorKey, XFont font)
            : base(fontDescriptorKey)
        {
            try
            {
                FontFace = font.GlyphTypeface.Fontface;
                FontName = font.Name;
                Initialize();
            }
            catch
            {
                GetType();
                throw;
            }
        }

        internal OpenTypeDescriptor(string fontDescriptorKey, string idName, byte[] fontData)
            : base(fontDescriptorKey)
        {
            try
            {
                FontFace = new OpenTypeFontface(fontData, idName);
                if (idName.Contains("XPS-Font-") && FontFace.name != null && FontFace.name.Name.Length != 0)
                {
                    string tag = String.Empty;
                    if (idName.IndexOf('+') == 6)
                        tag = idName.Substring(0, 6);
                    idName = tag + "+" + FontFace.name.Name;
                    if (FontFace.name.Style.Length != 0)
                        idName += "," + FontFace.name.Style;
                }
                FontName = idName;
                Initialize();
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }

        internal OpenTypeFontface FontFace;

        void Initialize()
        {
            ItalicAngle = FontFace.post.italicAngle;

            XMin = FontFace.head.xMin;
            YMin = FontFace.head.yMin;
            XMax = FontFace.head.xMax;
            YMax = FontFace.head.yMax;

            UnderlinePosition = FontFace.post.underlinePosition;
            UnderlineThickness = FontFace.post.underlineThickness;

            Debug.Assert(FontFace.os2 != null, "TrueType font has no OS/2 table.");

            StrikeoutPosition = FontFace.os2.yStrikeoutPosition;
            StrikeoutSize = FontFace.os2.yStrikeoutSize;

            StemV = 0;

            UnitsPerEm = FontFace.head.unitsPerEm;

            bool os2SeemsToBeEmpty = FontFace.os2.sTypoAscender == 0 && FontFace.os2.sTypoDescender == 0 && FontFace.os2.sTypoLineGap == 0;
            bool dontUseWinLineMetrics = (FontFace.os2.fsSelection & 128) != 0;
            if (!os2SeemsToBeEmpty && dontUseWinLineMetrics)
            {
                int typoAscender = FontFace.os2.sTypoAscender;
                int typoDescender = FontFace.os2.sTypoDescender;
                int typoLineGap = FontFace.os2.sTypoLineGap;

                Ascender = typoAscender + typoLineGap;
                Descender = -typoDescender;
                LineSpacing = typoAscender + typoLineGap - typoDescender;
            }
            else
            {
                int ascender = FontFace.hhea.ascender;
                int descender = Math.Abs(FontFace.hhea.descender);
                int lineGap = Math.Max((short)0, FontFace.hhea.lineGap);

                if (!os2SeemsToBeEmpty)
                {
                    int winAscent = FontFace.os2.usWinAscent;
                    int winDescent = Math.Abs(FontFace.os2.usWinDescent);

                    Ascender = winAscent;
                    Descender = winDescent;
                    LineSpacing = Math.Max(lineGap + ascender + descender, winAscent + winDescent);
                }
                else
                {
                    Ascender = ascender;
                    Descender = descender;
                    LineSpacing = ascender + descender + lineGap;
                }
            }

            Debug.Assert(Descender >= 0);

            int cellHeight = Ascender + Descender;
            int internalLeading = cellHeight - UnitsPerEm;      
            int externalLeading = LineSpacing - cellHeight;
            Leading = externalLeading;

            if (FontFace.os2.version >= 2 && FontFace.os2.sCapHeight != 0)
                CapHeight = FontFace.os2.sCapHeight;
            else
                CapHeight = Ascender;

            if (FontFace.os2.version >= 2 && FontFace.os2.sxHeight != 0)
                XHeight = FontFace.os2.sxHeight;
            else
                XHeight = (int)(0.66 * Ascender);

#if !EDF_CORE
            Encoding ansi = PdfEncoders.WinAnsiEncoding;  

#endif

            Encoding unicode = Encoding.Unicode;
            byte[] bytes = new byte[256];

            bool symbol = FontFace.cmap.symbol;
            Widths = new int[256];
            for (int idx = 0; idx < 256; idx++)
            {
                bytes[idx] = (byte)idx;
                char ch = (char)idx;
                string s = ansi.GetString(bytes, idx, 1);
                if (s.Length != 0)
                {
                    if (s[0] != ch)
                        ch = s[0];
                }

                if (symbol)
                {
                    ch = (char)(ch | (FontFace.os2.usFirstCharIndex & 0xFF00));    
                }
                int glyphIndex = CharCodeToGlyphIndex(ch);
                Widths[idx] = GlyphIndexToPdfWidth(glyphIndex);
            }
        }
        public int[] Widths;

        public override bool IsBoldFace
        {
            get
            {
                return FontFace.os2.IsBold;
            }
        }

        public override bool IsItalicFace
        {
            get { return FontFace.os2.IsItalic; }
        }

        internal int DesignUnitsToPdf(double value)
        {
            return (int)Math.Round(value * 1000.0 / FontFace.head.unitsPerEm);
        }

        public int CharCodeToGlyphIndex(char value)
        {
            try
            {
                CMap4 cmap = FontFace.cmap.cmap4;
                int segCount = cmap.segCountX2 / 2;
                int seg;
                for (seg = 0; seg < segCount; seg++)
                {
                    if (value <= cmap.endCount[seg])
                        break;
                }
                Debug.Assert(seg < segCount);

                if (value < cmap.startCount[seg])
                    return 0;

                if (cmap.idRangeOffs[seg] == 0)
                    return (value + cmap.idDelta[seg]) & 0xFFFF;

                int idx = cmap.idRangeOffs[seg] / 2 + (value - cmap.startCount[seg]) - (segCount - seg);
                Debug.Assert(idx >= 0 && idx < cmap.glyphCount);

                if (cmap.glyphIdArray[idx] == 0)
                    return 0;

                return (cmap.glyphIdArray[idx] + cmap.idDelta[seg]) & 0xFFFF;
            }
            catch
            {
                GetType();
                throw;
            }
        }

        public int GlyphIndexToPdfWidth(int glyphIndex)
        {
            try
            {
                int numberOfHMetrics = FontFace.hhea.numberOfHMetrics;
                int unitsPerEm = FontFace.head.unitsPerEm;

                if (glyphIndex >= numberOfHMetrics)
                    glyphIndex = numberOfHMetrics - 1;

                int width = FontFace.hmtx.Metrics[glyphIndex].advanceWidth;

                if (unitsPerEm == 1000)
                    return width;
                return width * 1000 / unitsPerEm;  
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }

        public int PdfWidthFromCharCode(char ch)
        {
            int idx = CharCodeToGlyphIndex(ch);
            int width = GlyphIndexToPdfWidth(idx);
            return width;
        }

        public double GlyphIndexToEmfWidth(int glyphIndex, double emSize)
        {
            try
            {
                int numberOfHMetrics = FontFace.hhea.numberOfHMetrics;
                int unitsPerEm = FontFace.head.unitsPerEm;

                if (glyphIndex >= numberOfHMetrics)
                    glyphIndex = numberOfHMetrics - 1;

                int width = FontFace.hmtx.Metrics[glyphIndex].advanceWidth;

                return width * emSize / unitsPerEm;  
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }

        public int GlyphIndexToWidth(int glyphIndex)
        {
            try
            {
                int numberOfHMetrics = FontFace.hhea.numberOfHMetrics;

                if (glyphIndex >= numberOfHMetrics)
                    glyphIndex = numberOfHMetrics - 1;

                int width = FontFace.hmtx.Metrics[glyphIndex].advanceWidth;
                return width;
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }
    }
}
