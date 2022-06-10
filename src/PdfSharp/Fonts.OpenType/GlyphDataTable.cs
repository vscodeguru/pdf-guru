
using System;
using System.Collections.Generic;

namespace PdfSharp.Fonts.OpenType
{
    internal class GlyphDataTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.Glyf;

        internal byte[] GlyphTable;

        public GlyphDataTable()
            : base(null, Tag)
        {
            DirectoryEntry.Tag = TableTagNames.Glyf;
        }

        public GlyphDataTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            DirectoryEntry.Tag = TableTagNames.Glyf;
            Read();
        }

        public void Read()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] GetGlyphData(int glyph)
        {
            IndexToLocationTable loca = _fontData.loca;
            int start = GetOffset(glyph);
            int next = GetOffset(glyph + 1);
            int count = next - start;
            byte[] bytes = new byte[count];
            Buffer.BlockCopy(_fontData.FontSource.Bytes, start, bytes, 0, count);
            return bytes;
        }

        public int GetGlyphSize(int glyph)
        {
            IndexToLocationTable loca = _fontData.loca;
            return GetOffset(glyph + 1) - GetOffset(glyph);
        }

        public int GetOffset(int glyph)
        {
            return DirectoryEntry.Offset + _fontData.loca.LocaTable[glyph];
        }

        public void CompleteGlyphClosure(Dictionary<int, object> glyphs)
        {
            int count = glyphs.Count;
            int[] glyphArray = new int[glyphs.Count];
            glyphs.Keys.CopyTo(glyphArray, 0);
            if (!glyphs.ContainsKey(0))
                glyphs.Add(0, null);
            for (int idx = 0; idx < count; idx++)
                AddCompositeGlyphs(glyphs, glyphArray[idx]);
        }

        void AddCompositeGlyphs(Dictionary<int, object> glyphs, int glyph)
        {
            int start = GetOffset(glyph);
            if (start == GetOffset(glyph + 1))
                return;
            _fontData.Position = start;
            int numContours = _fontData.ReadShort();
            if (numContours >= 0)
                return;
            _fontData.SeekOffset(8);
            for (; ; )
            {
                int flags = _fontData.ReadUFWord();
                int cGlyph = _fontData.ReadUFWord();
                if (!glyphs.ContainsKey(cGlyph))
                    glyphs.Add(cGlyph, null);
                if ((flags & MORE_COMPONENTS) == 0)
                    return;
                int offset = (flags & ARG_1_AND_2_ARE_WORDS) == 0 ? 2 : 4;
                if ((flags & WE_HAVE_A_SCALE) != 0)
                    offset += 2;
                else if ((flags & WE_HAVE_AN_X_AND_Y_SCALE) != 0)
                    offset += 4;
                if ((flags & WE_HAVE_A_TWO_BY_TWO) != 0)
                    offset += 8;
                _fontData.SeekOffset(offset);
            }
        }

        public override void PrepareForCompilation()
        {
            base.PrepareForCompilation();

            if (DirectoryEntry.Length == 0)
                DirectoryEntry.Length = GlyphTable.Length;
            DirectoryEntry.CheckSum = CalcChecksum(GlyphTable);
        }

        public override void Write(OpenTypeFontWriter writer)
        {
            writer.Write(GlyphTable, 0, DirectoryEntry.PaddedLength);
        }

        const int ARG_1_AND_2_ARE_WORDS = 1;
        const int WE_HAVE_A_SCALE = 8;
        const int MORE_COMPONENTS = 32;
        const int WE_HAVE_AN_X_AND_Y_SCALE = 64;
        const int WE_HAVE_A_TWO_BY_TWO = 128;
    }
}
