using System;
using System.Diagnostics;


namespace PdfSharp.Fonts.OpenType
{
    internal class OpenTypeFontTable : ICloneable
    {
        public OpenTypeFontTable(OpenTypeFontface fontData, string tag)
        {
            _fontData = fontData;
            if (fontData != null && fontData.TableDictionary.ContainsKey(tag))
                DirectoryEntry = fontData.TableDictionary[tag];
            else
                DirectoryEntry = new TableDirectoryEntry(tag);
            DirectoryEntry.FontTable = this;
        }

        public object Clone()
        {
            return DeepCopy();
        }

        protected virtual OpenTypeFontTable DeepCopy()
        {
            OpenTypeFontTable fontTable = (OpenTypeFontTable)MemberwiseClone();
            fontTable.DirectoryEntry.Offset = 0;
            fontTable.DirectoryEntry.FontTable = fontTable;
            return fontTable;
        }

        public OpenTypeFontface FontData
        {
            get { return _fontData; }
        }
        internal OpenTypeFontface _fontData;

        public TableDirectoryEntry DirectoryEntry;

        public virtual void PrepareForCompilation()
        { }

        public virtual void Write(OpenTypeFontWriter writer)
        { }

        public static uint CalcChecksum(byte[] bytes)
        {
            Debug.Assert((bytes.Length & 3) == 0);
            uint byte3, byte2, byte1, byte0;
            byte3 = byte2 = byte1 = byte0 = 0;
            int length = bytes.Length;
            for (int idx = 0; idx < length;)
            {
                byte3 += bytes[idx++];
                byte2 += bytes[idx++];
                byte1 += bytes[idx++];
                byte0 += bytes[idx++];
            }
            return (byte3 << 24) + (byte2 << 16) + (byte1 << 8) + byte0;
        }
    }
}
