using System.Diagnostics;

namespace PdfSharp.Fonts.OpenType
{
    internal class TableDirectoryEntry
    {
        public TableDirectoryEntry()
        { }

        public TableDirectoryEntry(string tag)
        {
            Debug.Assert(tag.Length == 4);
            Tag = tag;
        }

        public string Tag;

        public uint CheckSum;

        public int Offset;

        public int Length;

        public int PaddedLength
        {
            get { return (Length + 3) & ~3; }
        }

        public OpenTypeFontTable FontTable;

        public static TableDirectoryEntry ReadFrom(OpenTypeFontface fontData)
        {
            TableDirectoryEntry entry = new TableDirectoryEntry();
            entry.Tag = fontData.ReadTag();
            entry.CheckSum = fontData.ReadULong();
            entry.Offset = fontData.ReadLong();
            entry.Length = (int)fontData.ReadULong();
            return entry;
        }

        public void Read(OpenTypeFontface fontData)
        {
            Tag = fontData.ReadTag();
            CheckSum = fontData.ReadULong();
            Offset = fontData.ReadLong();
            Length = (int)fontData.ReadULong();
        }

        public void Write(OpenTypeFontWriter writer)
        {
            Debug.Assert(Tag.Length == 4);
            Debug.Assert(Offset != 0);
            Debug.Assert(Length != 0);
            writer.WriteTag(Tag);
            writer.WriteUInt(CheckSum);
            writer.WriteInt(Offset);
            writer.WriteUInt((uint)Length);
        }
    }
}
