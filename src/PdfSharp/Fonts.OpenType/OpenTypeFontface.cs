
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using PdfSharp.Drawing;
using Fixed = System.Int32;

namespace PdfSharp.Fonts.OpenType
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal sealed class OpenTypeFontface
    {
        OpenTypeFontface(OpenTypeFontface fontface)
        {
            _offsetTable = fontface._offsetTable;
            _fullFaceName = fontface._fullFaceName;
        }

        public OpenTypeFontface(byte[] data, string faceName)
        {
            _fullFaceName = faceName;
            int length = data.Length;
            Array.Copy(data, FontSource.Bytes, length);
            Read();
        }

        public OpenTypeFontface(XFontSource fontSource)
        {
            FontSource = fontSource;
            Read();
            _fullFaceName = name.FullFontName;
        }

        public static OpenTypeFontface CetOrCreateFrom(XFontSource fontSource)
        {
            OpenTypeFontface fontface;
            if (OpenTypeFontfaceCache.TryGetFontface(fontSource.Key, out fontface))
            {
                return fontface;
            }
            Debug.Assert(fontSource.Fontface != null);
            fontface = OpenTypeFontfaceCache.AddFontface(fontSource.Fontface);
            Debug.Assert(ReferenceEquals(fontSource.Fontface, fontface));
            return fontface;
        }

        public string FullFaceName
        {
            get { return _fullFaceName; }
        }
        readonly string _fullFaceName;

        public ulong CheckSum
        {
            get
            {
                if (_checkSum == 0)
                    _checkSum = FontHelper.CalcChecksum(FontSource.Bytes);
                return _checkSum;
            }
        }
        ulong _checkSum;

        public XFontSource FontSource
        {
            get { return _fontSource; }
            private set
            {
                if (value == null)
                    throw new InvalidOperationException("Font cannot be resolved.");
                _fontSource = value;
            }
        }
        XFontSource _fontSource;

        internal FontTechnology _fontTechnology;

        internal OffsetTable _offsetTable;

        internal Dictionary<string, TableDirectoryEntry> TableDictionary = new Dictionary<string, TableDirectoryEntry>();

        internal CMapTable cmap;
        internal ControlValueTable cvt;
        internal FontProgram fpgm;
        internal MaximumProfileTable maxp;
        internal NameTable name;
        internal ControlValueProgram prep;
        internal FontHeaderTable head;
        internal HorizontalHeaderTable hhea;
        internal HorizontalMetricsTable hmtx;
        internal OS2Table os2;
        internal PostScriptTable post;
        internal GlyphDataTable glyf;
        internal IndexToLocationTable loca;
        internal GlyphSubstitutionTable gsub;
        internal VerticalHeaderTable vhea;  
        internal VerticalMetricsTable vmtx;  
        public bool CanRead
        {
            get { return FontSource != null; }
        }

        public bool CanWrite
        {
            get { return FontSource == null; }
        }

        public void AddTable(OpenTypeFontTable fontTable)
        {
            if (!CanWrite)
                throw new InvalidOperationException("Font image cannot be modified.");

            if (fontTable == null)
                throw new ArgumentNullException("fontTable");

            if (fontTable._fontData == null)
            {
                fontTable._fontData = this;
            }
            else
            {
                Debug.Assert(fontTable._fontData.CanRead);
                fontTable = new IRefFontTable(this, fontTable);
            }

            TableDictionary[fontTable.DirectoryEntry.Tag] = fontTable.DirectoryEntry;
            switch (fontTable.DirectoryEntry.Tag)
            {
                case TableTagNames.CMap:
                    cmap = fontTable as CMapTable;
                    break;

                case TableTagNames.Cvt:
                    cvt = fontTable as ControlValueTable;
                    break;

                case TableTagNames.Fpgm:
                    fpgm = fontTable as FontProgram;
                    break;

                case TableTagNames.MaxP:
                    maxp = fontTable as MaximumProfileTable;
                    break;

                case TableTagNames.Name:
                    name = fontTable as NameTable;
                    break;

                case TableTagNames.Head:
                    head = fontTable as FontHeaderTable;
                    break;

                case TableTagNames.HHea:
                    hhea = fontTable as HorizontalHeaderTable;
                    break;

                case TableTagNames.HMtx:
                    hmtx = fontTable as HorizontalMetricsTable;
                    break;

                case TableTagNames.OS2:
                    os2 = fontTable as OS2Table;
                    break;

                case TableTagNames.Post:
                    post = fontTable as PostScriptTable;
                    break;

                case TableTagNames.Glyf:
                    glyf = fontTable as GlyphDataTable;
                    break;

                case TableTagNames.Loca:
                    loca = fontTable as IndexToLocationTable;
                    break;

                case TableTagNames.GSUB:
                    gsub = fontTable as GlyphSubstitutionTable;
                    break;

                case TableTagNames.Prep:
                    prep = fontTable as ControlValueProgram;
                    break;
            }
        }

        internal void Read()
        {
            const uint OTTO = 0x4f54544f;        
            const uint TTCF = 0x74746366;        
            try
            {

                uint startTag = ReadULong();
                if (startTag == TTCF)
                {
                    _fontTechnology = FontTechnology.TrueTypeCollection;
                    throw new InvalidOperationException("TrueType collection fonts are not yet supported by PDFsharp.");
                }

                _offsetTable.Version = startTag;
                _offsetTable.TableCount = ReadUShort();
                _offsetTable.SearchRange = ReadUShort();
                _offsetTable.EntrySelector = ReadUShort();
                _offsetTable.RangeShift = ReadUShort();

                Debug.Assert(_pos == 12);
                if (_offsetTable.Version == OTTO)
                    _fontTechnology = FontTechnology.PostscriptOutlines;
                else
                    _fontTechnology = FontTechnology.TrueTypeOutlines;

                for (int idx = 0; idx < _offsetTable.TableCount; idx++)
                {
                    TableDirectoryEntry entry = TableDirectoryEntry.ReadFrom(this);
                    TableDictionary.Add(entry.Tag, entry);
                }

                if (TableDictionary.ContainsKey("bhed"))
                    throw new NotSupportedException("Bitmap fonts are not supported by PDFsharp.");

                if (Seek(CMapTable.Tag) != -1)
                    cmap = new CMapTable(this);

                if (Seek(ControlValueTable.Tag) != -1)
                    cvt = new ControlValueTable(this);

                if (Seek(FontProgram.Tag) != -1)
                    fpgm = new FontProgram(this);

                if (Seek(MaximumProfileTable.Tag) != -1)
                    maxp = new MaximumProfileTable(this);

                if (Seek(NameTable.Tag) != -1)
                    name = new NameTable(this);

                if (Seek(FontHeaderTable.Tag) != -1)
                    head = new FontHeaderTable(this);

                if (Seek(HorizontalHeaderTable.Tag) != -1)
                    hhea = new HorizontalHeaderTable(this);

                if (Seek(HorizontalMetricsTable.Tag) != -1)
                    hmtx = new HorizontalMetricsTable(this);

                if (Seek(OS2Table.Tag) != -1)
                    os2 = new OS2Table(this);

                if (Seek(PostScriptTable.Tag) != -1)
                    post = new PostScriptTable(this);

                if (Seek(GlyphDataTable.Tag) != -1)
                    glyf = new GlyphDataTable(this);

                if (Seek(IndexToLocationTable.Tag) != -1)
                    loca = new IndexToLocationTable(this);

                if (Seek(GlyphSubstitutionTable.Tag) != -1)
                    gsub = new GlyphSubstitutionTable(this);

                if (Seek(ControlValueProgram.Tag) != -1)
                    prep = new ControlValueProgram(this);
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }

        public OpenTypeFontface CreateFontSubSet(Dictionary<int, object> glyphs, bool cidFont)
        {
            OpenTypeFontface fontData = new OpenTypeFontface(this);

            IndexToLocationTable locaNew = new IndexToLocationTable();
            locaNew.ShortIndex = loca.ShortIndex;
            GlyphDataTable glyfNew = new GlyphDataTable();

            if (!cidFont)
                fontData.AddTable(cmap);
            if (cvt != null)
                fontData.AddTable(cvt);
            if (fpgm != null)
                fontData.AddTable(fpgm);
            fontData.AddTable(glyfNew);
            fontData.AddTable(head);
            fontData.AddTable(hhea);
            fontData.AddTable(hmtx);
            fontData.AddTable(locaNew);
            if (maxp != null)
                fontData.AddTable(maxp);
            if (prep != null)
                fontData.AddTable(prep);

            glyf.CompleteGlyphClosure(glyphs);

            int glyphCount = glyphs.Count;
            int[] glyphArray = new int[glyphCount];
            glyphs.Keys.CopyTo(glyphArray, 0);
            Array.Sort(glyphArray);

            int size = 0;
            for (int idx = 0; idx < glyphCount; idx++)
                size += glyf.GetGlyphSize(glyphArray[idx]);
            glyfNew.DirectoryEntry.Length = size;

            int numGlyphs = maxp.numGlyphs;
            locaNew.LocaTable = new int[numGlyphs + 1];

            glyfNew.GlyphTable = new byte[glyfNew.DirectoryEntry.PaddedLength];

            int glyphOffset = 0;
            int glyphIndex = 0;
            for (int idx = 0; idx < numGlyphs; idx++)
            {
                locaNew.LocaTable[idx] = glyphOffset;
                if (glyphIndex < glyphCount && glyphArray[glyphIndex] == idx)
                {
                    glyphIndex++;
                    byte[] bytes = glyf.GetGlyphData(idx);
                    int length = bytes.Length;
                    if (length > 0)
                    {
                        Buffer.BlockCopy(bytes, 0, glyfNew.GlyphTable, glyphOffset, length);
                        glyphOffset += length;
                    }
                }
            }
            locaNew.LocaTable[numGlyphs] = glyphOffset;

            fontData.Compile();

            return fontData;
        }

        void Compile()
        {
            MemoryStream stream = new MemoryStream();
            OpenTypeFontWriter writer = new OpenTypeFontWriter(stream);

            int tableCount = TableDictionary.Count;
            int selector = _entrySelectors[tableCount];

            _offsetTable.Version = 0x00010000;
            _offsetTable.TableCount = tableCount;
            _offsetTable.SearchRange = (ushort)((1 << selector) * 16);
            _offsetTable.EntrySelector = (ushort)selector;
            _offsetTable.RangeShift = (ushort)((tableCount - (1 << selector)) * 16);
            _offsetTable.Write(writer);

            string[] tags = new string[tableCount];
            TableDictionary.Keys.CopyTo(tags, 0);
            Array.Sort(tags, StringComparer.Ordinal);

            int tablePosition = 12 + 16 * tableCount;
            for (int idx = 0; idx < tableCount; idx++)
            {
                TableDirectoryEntry entry = TableDictionary[tags[idx]];
                entry.FontTable.PrepareForCompilation();
                entry.Offset = tablePosition;
                writer.Position = tablePosition;
                entry.FontTable.Write(writer);
                int endPosition = writer.Position;
                tablePosition = endPosition;
                writer.Position = 12 + 16 * idx;
                entry.Write(writer);
            }
            writer.Stream.Flush();
            int l = (int)writer.Stream.Length;
            FontSource = XFontSource.CreateCompiledFont(stream.ToArray());
        }
        static readonly int[] _entrySelectors = { 0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

        public int Position
        {
            get { return _pos; }
            set { _pos = value; }
        }
        int _pos;

        public int Seek(string tag)
        {
            if (TableDictionary.ContainsKey(tag))
            {
                _pos = TableDictionary[tag].Offset;
                return _pos;
            }
            return -1;
        }

        public int SeekOffset(int offset)
        {
            _pos += offset;
            return _pos;
        }

        public byte ReadByte()
        {
            return _fontSource.Bytes[_pos++];
        }

        public short ReadShort()
        {
            int pos = _pos;
            _pos += 2;
            return (short)((_fontSource.Bytes[pos] << 8) | (_fontSource.Bytes[pos + 1]));
        }

        public ushort ReadUShort()
        {
            int pos = _pos;
            _pos += 2;
            return (ushort)((_fontSource.Bytes[pos] << 8) | (_fontSource.Bytes[pos + 1]));
        }

        public int ReadLong()
        {
            int pos = _pos;
            _pos += 4;
            return (_fontSource.Bytes[pos] << 24) | (_fontSource.Bytes[pos + 1] << 16) | (_fontSource.Bytes[pos + 2] << 8) | (_fontSource.Bytes[pos + 3]);
        }

        public uint ReadULong()
        {
            int pos = _pos;
            _pos += 4;
            return (uint)((_fontSource.Bytes[pos] << 24) | (_fontSource.Bytes[pos + 1] << 16) | (_fontSource.Bytes[pos + 2] << 8) | (_fontSource.Bytes[pos + 3]));
        }

        public Fixed ReadFixed()
        {
            int pos = _pos;
            _pos += 4;
            return (_fontSource.Bytes[pos] << 24) | (_fontSource.Bytes[pos + 1] << 16) | (_fontSource.Bytes[pos + 2] << 8) | (_fontSource.Bytes[pos + 3]);
        }

        public short ReadFWord()
        {
            int pos = _pos;
            _pos += 2;
            return (short)((_fontSource.Bytes[pos] << 8) | (_fontSource.Bytes[pos + 1]));
        }

        public ushort ReadUFWord()
        {
            int pos = _pos;
            _pos += 2;
            return (ushort)((_fontSource.Bytes[pos] << 8) | (_fontSource.Bytes[pos + 1]));
        }

        public long ReadLongDate()
        {
            int pos = _pos;
            _pos += 8;
            byte[] bytes = _fontSource.Bytes;
            return (((long)bytes[pos]) << 56) | (((long)bytes[pos + 1]) << 48) | (((long)bytes[pos + 2]) << 40) | (((long)bytes[pos + 3]) << 32) |
                   (((long)bytes[pos + 4]) << 24) | (((long)bytes[pos + 5]) << 16) | (((long)bytes[pos + 6]) << 8) | bytes[pos + 7];
        }

        public string ReadString(int size)
        {
            char[] chars = new char[size];
            for (int idx = 0; idx < size; idx++)
                chars[idx] = (char)_fontSource.Bytes[_pos++];
            return new string(chars);
        }

        public byte[] ReadBytes(int size)
        {
            byte[] bytes = new byte[size];
            for (int idx = 0; idx < size; idx++)
                bytes[idx] = _fontSource.Bytes[_pos++];
            return bytes;
        }

        public void Read(byte[] buffer)
        {
            Read(buffer, 0, buffer.Length);
        }

        public void Read(byte[] buffer, int offset, int length)
        {
            Buffer.BlockCopy(_fontSource.Bytes, _pos, buffer, offset, length);
            _pos += length;
        }

        public string ReadTag()
        {
            return ReadString(4);
        }

        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "OpenType fontfaces: {0}", _fullFaceName); }
        }

        internal struct OffsetTable
        {
            public uint Version;

            public int TableCount;

            public ushort SearchRange;

            public ushort EntrySelector;

            public ushort RangeShift;

            public void Write(OpenTypeFontWriter writer)
            {
                writer.WriteUInt(Version);
                writer.WriteShort(TableCount);
                writer.WriteUShort(SearchRange);
                writer.WriteUShort(EntrySelector);
                writer.WriteUShort(RangeShift);
            }
        }
    }
}
