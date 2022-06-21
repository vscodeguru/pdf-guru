using System;
using System.Diagnostics;
using System.Drawing;
using GdiFont = System.Drawing.Font;
using GdiFontStyle = System.Drawing.FontStyle;
using PdfSharp.Fonts;
using PdfSharp.Fonts.OpenType;

namespace PdfSharp.Drawing
{
    static class FontHelper
    {
        public static XSize MeasureString(string text, XFont font, XStringFormat stringFormat_notyetused)
        {
            XSize size = new XSize();

            OpenTypeDescriptor descriptor = FontDescriptorCache.GetOrCreateDescriptorFor(font) as OpenTypeDescriptor;
            if (descriptor != null)
            {
                size.Height = (descriptor.Ascender + descriptor.Descender) * font.Size / font.UnitsPerEm;
                Debug.Assert(descriptor.Ascender > 0);

                bool symbol = descriptor.FontFace.cmap.symbol;
                int length = text.Length;
                int width = 0;
                for (int idx = 0; idx < length; idx++)
                {
                    char ch = text[idx];
                    if (ch < 32)
                        continue;

                    if (symbol)
                    {
                        ch = (char)(ch | (descriptor.FontFace.os2.usFirstCharIndex & 0xFF00));    
                    }
                    int glyphIndex = descriptor.CharCodeToGlyphIndex(ch);
                    width += descriptor.GlyphIndexToWidth(glyphIndex);
                }
                size.Width = width * font.Size / descriptor.UnitsPerEm;

                if ((font.GlyphTypeface.StyleSimulations & XStyleSimulations.BoldSimulation) == XStyleSimulations.BoldSimulation)
                {
                    size.Width += length * font.Size * Const.BoldEmphasis;
                }
            }
            Debug.Assert(descriptor != null, "No OpenTypeDescriptor.");
            return size;
        }

        public static GdiFont CreateFont(string familyName, double emSize, GdiFontStyle style, out XFontSource fontSource)
        {
            fontSource = null;
            GdiFont font;

            font = new GdiFont(familyName, (float)emSize, style, GraphicsUnit.World);
            return font;
        }


        public static ulong CalcChecksum(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            const uint prime = 65521;      
            uint s1 = 0;
            uint s2 = 0;
            int length = buffer.Length;
            int offset = 0;
            while (length > 0)
            {
                int n = 3800;
                if (n > length)
                    n = length;
                length -= n;
                while (--n >= 0)
                {
                    s1 += buffer[offset++];
                    s2 = s2 + s1;
                }
                s1 %= prime;
                s2 %= prime;
            }
            ulong ul1 = (ulong)s2 << 16;
            ul1 = ul1 | s1;
            ulong ul2 = (ulong)buffer.Length;
            return (ul1 << 32) | ul2;
        }

        public static XFontStyle CreateStyle(bool isBold, bool isItalic)
        {
            return (isBold ? XFontStyle.Bold : 0) | (isItalic ? XFontStyle.Italic : 0);
        }
    }
}
