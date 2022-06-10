
namespace PdfSharp.Fonts.OpenType
{
    internal class IRefFontTable : OpenTypeFontTable
    {
        public IRefFontTable(OpenTypeFontface fontData, OpenTypeFontTable fontTable)
            : base(null, fontTable.DirectoryEntry.Tag)
        {
            _fontData = fontData;
            _irefDirectoryEntry = fontTable.DirectoryEntry;
        }

        readonly TableDirectoryEntry _irefDirectoryEntry;

        public override void PrepareForCompilation()
        {
            base.PrepareForCompilation();
            DirectoryEntry.Length = _irefDirectoryEntry.Length;
            DirectoryEntry.CheckSum = _irefDirectoryEntry.CheckSum;
        }

        public override void Write(OpenTypeFontWriter writer)
        {
            writer.Write(_irefDirectoryEntry.FontTable._fontData.FontSource.Bytes, _irefDirectoryEntry.Offset, _irefDirectoryEntry.PaddedLength);
        }
    }
}
