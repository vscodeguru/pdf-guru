
using System;
using System.Diagnostics;
using System.Text;

using Fixed = System.Int32;
using FWord = System.Int16;
using UFWord = System.UInt16;

namespace PdfSharp.Fonts.OpenType
{
    internal enum PlatformId
    {
        Apple, Mac, Iso, Win
    }

    internal enum WinEncodingId
    {
        Symbol, Unicode
    }

    internal class CMap4 : OpenTypeFontTable
    {
        public WinEncodingId encodingId;    
        public ushort format;       
        public ushort length;           
        public ushort language;                       
        public ushort segCountX2;    
        public ushort searchRange;    
        public ushort entrySelector;  
        public ushort rangeShift;
        public ushort[] endCount;         
        public ushort[] startCount;         
        public short[] idDelta;          
        public ushort[] idRangeOffs;        
        public int glyphCount;             
        public ushort[] glyphIdArray;          

        public CMap4(OpenTypeFontface fontData, WinEncodingId encodingId)
            : base(fontData, "----")
        {
            this.encodingId = encodingId;
            Read();
        }

        internal void Read()
        {
            try
            {
                format = _fontData.ReadUShort();
                Debug.Assert(format == 4, "Only format 4 expected.");
                length = _fontData.ReadUShort();
                language = _fontData.ReadUShort();      
                segCountX2 = _fontData.ReadUShort();
                searchRange = _fontData.ReadUShort();
                entrySelector = _fontData.ReadUShort();
                rangeShift = _fontData.ReadUShort();

                int segCount = segCountX2 / 2;
                glyphCount = (length - (16 + 8 * segCount)) / 2;

                endCount = new ushort[segCount];
                startCount = new ushort[segCount];
                idDelta = new short[segCount];
                idRangeOffs = new ushort[segCount];

                glyphIdArray = new ushort[glyphCount];

                for (int idx = 0; idx < segCount; idx++)
                    endCount[idx] = _fontData.ReadUShort();

                _fontData.ReadUShort();

                for (int idx = 0; idx < segCount; idx++)
                    startCount[idx] = _fontData.ReadUShort();

                for (int idx = 0; idx < segCount; idx++)
                    idDelta[idx] = _fontData.ReadShort();

                for (int idx = 0; idx < segCount; idx++)
                    idRangeOffs[idx] = _fontData.ReadUShort();

                for (int idx = 0; idx < glyphCount; idx++)
                    glyphIdArray[idx] = _fontData.ReadUShort();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class CMapTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.CMap;

        public ushort version;
        public ushort numTables;

        public bool symbol;

        public CMap4 cmap4;

        public CMapTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        internal void Read()
        {
            try
            {
                int tableOffset = _fontData.Position;

                version = _fontData.ReadUShort();
                numTables = _fontData.ReadUShort();

                bool success = false;
                for (int idx = 0; idx < numTables; idx++)
                {
                    PlatformId platformId = (PlatformId)_fontData.ReadUShort();
                    WinEncodingId encodingId = (WinEncodingId)_fontData.ReadUShort();
                    int offset = _fontData.ReadLong();

                    int currentPosition = _fontData.Position;

                    if (platformId == PlatformId.Win && (encodingId == WinEncodingId.Symbol || encodingId == WinEncodingId.Unicode))
                    {
                        symbol = encodingId == WinEncodingId.Symbol;

                        _fontData.Position = tableOffset + offset;
                        cmap4 = new CMap4(_fontData, encodingId);
                        _fontData.Position = currentPosition;
                        success = true;
                        break;
                    }
                }
                if (!success)
                    throw new InvalidOperationException("Font has no usable platform or encoding ID. It cannot be used with PDFsharp.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class FontHeaderTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.Head;

        public Fixed version;     
        public Fixed fontRevision;
        public uint checkSumAdjustment;
        public uint magicNumber;    
        public ushort flags;
        public ushort unitsPerEm;                      
        public long created;
        public long modified;
        public short xMin, yMin;      
        public short xMax, yMax;      
        public ushort macStyle;
        public ushort lowestRecPPEM;
        public short fontDirectionHint;
        public short indexToLocFormat;        
        public short glyphDataFormat;     

        public FontHeaderTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                version = _fontData.ReadFixed();
                fontRevision = _fontData.ReadFixed();
                checkSumAdjustment = _fontData.ReadULong();
                magicNumber = _fontData.ReadULong();
                flags = _fontData.ReadUShort();
                unitsPerEm = _fontData.ReadUShort();
                created = _fontData.ReadLongDate();
                modified = _fontData.ReadLongDate();
                xMin = _fontData.ReadShort();
                yMin = _fontData.ReadShort();
                xMax = _fontData.ReadShort();
                yMax = _fontData.ReadShort();
                macStyle = _fontData.ReadUShort();
                lowestRecPPEM = _fontData.ReadUShort();
                fontDirectionHint = _fontData.ReadShort();
                indexToLocFormat = _fontData.ReadShort();
                glyphDataFormat = _fontData.ReadShort();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class HorizontalHeaderTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.HHea;

        public Fixed version;     
        public FWord ascender;          
        public FWord descender;          
        public FWord lineGap;                   
        public UFWord advanceWidthMax;
        public FWord minLeftSideBearing;
        public FWord minRightSideBearing;
        public FWord xMaxExtent;
        public short caretSlopeRise;
        public short caretSlopeRun;
        public short reserved1;
        public short reserved2;
        public short reserved3;
        public short reserved4;
        public short reserved5;
        public short metricDataFormat;
        public ushort numberOfHMetrics;

        public HorizontalHeaderTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                version = _fontData.ReadFixed();
                ascender = _fontData.ReadFWord();
                descender = _fontData.ReadFWord();
                lineGap = _fontData.ReadFWord();
                advanceWidthMax = _fontData.ReadUFWord();
                minLeftSideBearing = _fontData.ReadFWord();
                minRightSideBearing = _fontData.ReadFWord();
                xMaxExtent = _fontData.ReadFWord();
                caretSlopeRise = _fontData.ReadShort();
                caretSlopeRun = _fontData.ReadShort();
                reserved1 = _fontData.ReadShort();
                reserved2 = _fontData.ReadShort();
                reserved3 = _fontData.ReadShort();
                reserved4 = _fontData.ReadShort();
                reserved5 = _fontData.ReadShort();
                metricDataFormat = _fontData.ReadShort();
                numberOfHMetrics = _fontData.ReadUShort();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class HorizontalMetrics : OpenTypeFontTable
    {
        public const string Tag = "----";

        public ushort advanceWidth;
        public short lsb;

        public HorizontalMetrics(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                advanceWidth = _fontData.ReadUFWord();
                lsb = _fontData.ReadFWord();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class HorizontalMetricsTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.HMtx;

        public HorizontalMetrics[] Metrics;
        public FWord[] LeftSideBearing;

        public HorizontalMetricsTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                HorizontalHeaderTable hhea = _fontData.hhea;
                MaximumProfileTable maxp = _fontData.maxp;
                if (hhea != null && maxp != null)
                {
                    int numMetrics = hhea.numberOfHMetrics; 
                    int numLsbs = maxp.numGlyphs - numMetrics;

                    Debug.Assert(numMetrics != 0);
                    Debug.Assert(numLsbs >= 0);

                    Metrics = new HorizontalMetrics[numMetrics];
                    for (int idx = 0; idx < numMetrics; idx++)
                        Metrics[idx] = new HorizontalMetrics(_fontData);

                    if (numLsbs > 0)
                    {
                        LeftSideBearing = new FWord[numLsbs];
                        for (int idx = 0; idx < numLsbs; idx++)
                            LeftSideBearing[idx] = _fontData.ReadFWord();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class VerticalHeaderTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.VHea;

        public Fixed Version;     
        public FWord Ascender;          
        public FWord Descender;          
        public FWord LineGap;                   
        public UFWord AdvanceWidthMax;
        public FWord MinLeftSideBearing;
        public FWord MinRightSideBearing;
        public FWord xMaxExtent;
        public short caretSlopeRise;
        public short caretSlopeRun;
        public short reserved1;
        public short reserved2;
        public short reserved3;
        public short reserved4;
        public short reserved5;
        public short metricDataFormat;
        public ushort numberOfHMetrics;

        public VerticalHeaderTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                Version = _fontData.ReadFixed();
                Ascender = _fontData.ReadFWord();
                Descender = _fontData.ReadFWord();
                LineGap = _fontData.ReadFWord();
                AdvanceWidthMax = _fontData.ReadUFWord();
                MinLeftSideBearing = _fontData.ReadFWord();
                MinRightSideBearing = _fontData.ReadFWord();
                xMaxExtent = _fontData.ReadFWord();
                caretSlopeRise = _fontData.ReadShort();
                caretSlopeRun = _fontData.ReadShort();
                reserved1 = _fontData.ReadShort();
                reserved2 = _fontData.ReadShort();
                reserved3 = _fontData.ReadShort();
                reserved4 = _fontData.ReadShort();
                reserved5 = _fontData.ReadShort();
                metricDataFormat = _fontData.ReadShort();
                numberOfHMetrics = _fontData.ReadUShort();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class VerticalMetrics : OpenTypeFontTable
    {
        public const string Tag = "----";

        public ushort advanceWidth;
        public short lsb;

        public VerticalMetrics(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                advanceWidth = _fontData.ReadUFWord();
                lsb = _fontData.ReadFWord();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class VerticalMetricsTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.VMtx;

        public HorizontalMetrics[] metrics;
        public FWord[] leftSideBearing;

        public VerticalMetricsTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
            throw new NotImplementedException("VerticalMetricsTable");
        }

        public void Read()
        {
            try
            {
                HorizontalHeaderTable hhea = _fontData.hhea;
                MaximumProfileTable maxp = _fontData.maxp;
                if (hhea != null && maxp != null)
                {
                    int numMetrics = hhea.numberOfHMetrics; 
                    int numLsbs = maxp.numGlyphs - numMetrics;

                    Debug.Assert(numMetrics != 0);
                    Debug.Assert(numLsbs >= 0);

                    metrics = new HorizontalMetrics[numMetrics];
                    for (int idx = 0; idx < numMetrics; idx++)
                        metrics[idx] = new HorizontalMetrics(_fontData);

                    if (numLsbs > 0)
                    {
                        leftSideBearing = new FWord[numLsbs];
                        for (int idx = 0; idx < numLsbs; idx++)
                            leftSideBearing[idx] = _fontData.ReadFWord();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class MaximumProfileTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.MaxP;

        public Fixed version;
        public ushort numGlyphs;
        public ushort maxPoints;
        public ushort maxContours;
        public ushort maxCompositePoints;
        public ushort maxCompositeContours;
        public ushort maxZones;
        public ushort maxTwilightPoints;
        public ushort maxStorage;
        public ushort maxFunctionDefs;
        public ushort maxInstructionDefs;
        public ushort maxStackElements;
        public ushort maxSizeOfInstructions;
        public ushort maxComponentElements;
        public ushort maxComponentDepth;

        public MaximumProfileTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                version = _fontData.ReadFixed();
                numGlyphs = _fontData.ReadUShort();
                maxPoints = _fontData.ReadUShort();
                maxContours = _fontData.ReadUShort();
                maxCompositePoints = _fontData.ReadUShort();
                maxCompositeContours = _fontData.ReadUShort();
                maxZones = _fontData.ReadUShort();
                maxTwilightPoints = _fontData.ReadUShort();
                maxStorage = _fontData.ReadUShort();
                maxFunctionDefs = _fontData.ReadUShort();
                maxInstructionDefs = _fontData.ReadUShort();
                maxStackElements = _fontData.ReadUShort();
                maxSizeOfInstructions = _fontData.ReadUShort();
                maxComponentElements = _fontData.ReadUShort();
                maxComponentDepth = _fontData.ReadUShort();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class NameTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.Name;

        public string Name = String.Empty;

        public string Style = String.Empty;

        public string FullFontName = String.Empty;

        public ushort format;
        public ushort count;
        public ushort stringOffset;

        byte[] bytes;

        public NameTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                bytes = new byte[DirectoryEntry.PaddedLength];
                Buffer.BlockCopy(_fontData.FontSource.Bytes, DirectoryEntry.Offset, bytes, 0, DirectoryEntry.Length);

                format = _fontData.ReadUShort();
                count = _fontData.ReadUShort();
                stringOffset = _fontData.ReadUShort();

                for (int idx = 0; idx < count; idx++)
                {
                    NameRecord nrec = ReadNameRecord();
                    byte[] value = new byte[nrec.length];
                    Buffer.BlockCopy(_fontData.FontSource.Bytes, DirectoryEntry.Offset + stringOffset + nrec.offset, value, 0, nrec.length);

                    if (nrec.platformID == 0 || nrec.platformID == 3)
                    {
                        if (nrec.nameID == 1 && nrec.languageID == 0x0409)
                        {
                            if (String.IsNullOrEmpty(Name))
                                Name = Encoding.BigEndianUnicode.GetString(value, 0, value.Length);
                        }

                        if (nrec.nameID == 2 && nrec.languageID == 0x0409)
                        {
                            if (String.IsNullOrEmpty(Style))
                                Style = Encoding.BigEndianUnicode.GetString(value, 0, value.Length);
                        }

                        if (nrec.nameID == 4 && nrec.languageID == 0x0409)
                        {
                            if (String.IsNullOrEmpty(FullFontName))
                                FullFontName = Encoding.BigEndianUnicode.GetString(value, 0, value.Length);
                        }
                    }
                }
                Debug.Assert(!String.IsNullOrEmpty(Name));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }

        NameRecord ReadNameRecord()
        {
            NameRecord nrec = new NameRecord();
            nrec.platformID = _fontData.ReadUShort();
            nrec.encodingID = _fontData.ReadUShort();
            nrec.languageID = _fontData.ReadUShort();
            nrec.nameID = _fontData.ReadUShort();
            nrec.length = _fontData.ReadUShort();
            nrec.offset = _fontData.ReadUShort();
            return nrec;
        }

        class NameRecord
        {
            public ushort platformID;
            public ushort encodingID;
            public ushort languageID;
            public ushort nameID;
            public ushort length;
            public ushort offset;
        }
    }

    internal class OS2Table : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.OS2;

        [Flags]
        public enum FontSelectionFlags : ushort
        {
            Italic = 1 << 0,
            Bold = 1 << 5,
            Regular = 1 << 6,
        }

        public ushort version;
        public short xAvgCharWidth;
        public ushort usWeightClass;
        public ushort usWidthClass;
        public ushort fsType;
        public short ySubscriptXSize;
        public short ySubscriptYSize;
        public short ySubscriptXOffset;
        public short ySubscriptYOffset;
        public short ySuperscriptXSize;
        public short ySuperscriptYSize;
        public short ySuperscriptXOffset;
        public short ySuperscriptYOffset;
        public short yStrikeoutSize;
        public short yStrikeoutPosition;
        public short sFamilyClass;
        public byte[] panose;    
        public uint ulUnicodeRange1;   
        public uint ulUnicodeRange2;   
        public uint ulUnicodeRange3;   
        public uint ulUnicodeRange4;   
        public string achVendID;   
        public ushort fsSelection;
        public ushort usFirstCharIndex;
        public ushort usLastCharIndex;
        public short sTypoAscender;
        public short sTypoDescender;
        public short sTypoLineGap;
        public ushort usWinAscent;
        public ushort usWinDescent;
        public uint ulCodePageRange1;   
        public uint ulCodePageRange2;   
        public short sxHeight;
        public short sCapHeight;
        public ushort usDefaultChar;
        public ushort usBreakChar;
        public ushort usMaxContext;

        public OS2Table(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                version = _fontData.ReadUShort();
                xAvgCharWidth = _fontData.ReadShort();
                usWeightClass = _fontData.ReadUShort();
                usWidthClass = _fontData.ReadUShort();
                fsType = _fontData.ReadUShort();
                ySubscriptXSize = _fontData.ReadShort();
                ySubscriptYSize = _fontData.ReadShort();
                ySubscriptXOffset = _fontData.ReadShort();
                ySubscriptYOffset = _fontData.ReadShort();
                ySuperscriptXSize = _fontData.ReadShort();
                ySuperscriptYSize = _fontData.ReadShort();
                ySuperscriptXOffset = _fontData.ReadShort();
                ySuperscriptYOffset = _fontData.ReadShort();
                yStrikeoutSize = _fontData.ReadShort();
                yStrikeoutPosition = _fontData.ReadShort();
                sFamilyClass = _fontData.ReadShort();
                panose = _fontData.ReadBytes(10);
                ulUnicodeRange1 = _fontData.ReadULong();
                ulUnicodeRange2 = _fontData.ReadULong();
                ulUnicodeRange3 = _fontData.ReadULong();
                ulUnicodeRange4 = _fontData.ReadULong();
                achVendID = _fontData.ReadString(4);
                fsSelection = _fontData.ReadUShort();
                usFirstCharIndex = _fontData.ReadUShort();
                usLastCharIndex = _fontData.ReadUShort();
                sTypoAscender = _fontData.ReadShort();
                sTypoDescender = _fontData.ReadShort();
                sTypoLineGap = _fontData.ReadShort();
                usWinAscent = _fontData.ReadUShort();
                usWinDescent = _fontData.ReadUShort();

                if (version >= 1)
                {
                    ulCodePageRange1 = _fontData.ReadULong();
                    ulCodePageRange2 = _fontData.ReadULong();

                    if (version >= 2)
                    {
                        sxHeight = _fontData.ReadShort();
                        sCapHeight = _fontData.ReadShort();
                        usDefaultChar = _fontData.ReadUShort();
                        usBreakChar = _fontData.ReadUShort();
                        usMaxContext = _fontData.ReadUShort();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }

        public bool IsBold
        {
            get { return (fsSelection & (ushort)FontSelectionFlags.Bold) != 0; }
        }

        public bool IsItalic
        {
            get { return (fsSelection & (ushort)FontSelectionFlags.Italic) != 0; }
        }
    }

    internal class PostScriptTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.Post;

        public Fixed formatType;
        public float italicAngle;
        public FWord underlinePosition;
        public FWord underlineThickness;
        public ulong isFixedPitch;
        public ulong minMemType42;
        public ulong maxMemType42;
        public ulong minMemType1;
        public ulong maxMemType1;

        public PostScriptTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            Read();
        }

        public void Read()
        {
            try
            {
                formatType = _fontData.ReadFixed();
                italicAngle = _fontData.ReadFixed() / 65536f;
                underlinePosition = _fontData.ReadFWord();
                underlineThickness = _fontData.ReadFWord();
                isFixedPitch = _fontData.ReadULong();
                minMemType42 = _fontData.ReadULong();
                maxMemType42 = _fontData.ReadULong();
                minMemType1 = _fontData.ReadULong();
                maxMemType1 = _fontData.ReadULong();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class ControlValueTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.Cvt;

        FWord[] array;                       

        public ControlValueTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            DirectoryEntry.Tag = TableTagNames.Cvt;
            DirectoryEntry = fontData.TableDictionary[TableTagNames.Cvt];
            Read();
        }

        public void Read()
        {
            try
            {
                int length = DirectoryEntry.Length / 2;
                array = new FWord[length];
                for (int idx = 0; idx < length; idx++)
                    array[idx] = _fontData.ReadFWord();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class FontProgram : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.Fpgm;

        byte[] bytes;                 

        public FontProgram(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            DirectoryEntry.Tag = TableTagNames.Fpgm;
            DirectoryEntry = fontData.TableDictionary[TableTagNames.Fpgm];
            Read();
        }

        public void Read()
        {
            try
            {
                int length = DirectoryEntry.Length;
                bytes = new byte[length];
                for (int idx = 0; idx < length; idx++)
                    bytes[idx] = _fontData.ReadByte();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class ControlValueProgram : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.Prep;

        byte[] bytes;                            

        public ControlValueProgram(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            DirectoryEntry.Tag = TableTagNames.Prep;
            DirectoryEntry = fontData.TableDictionary[TableTagNames.Prep];
            Read();
        }

        public void Read()
        {
            try
            {
                int length = DirectoryEntry.Length;
                bytes = new byte[length];
                for (int idx = 0; idx < length; idx++)
                    bytes[idx] = _fontData.ReadByte();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }

    internal class GlyphSubstitutionTable : OpenTypeFontTable
    {
        public const string Tag = TableTagNames.GSUB;

        public GlyphSubstitutionTable(OpenTypeFontface fontData)
            : base(fontData, Tag)
        {
            DirectoryEntry.Tag = TableTagNames.GSUB;
            DirectoryEntry = fontData.TableDictionary[TableTagNames.GSUB];
            Read();
        }

        public void Read()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(PSSR.ErrorReadingFontData, ex);
            }
        }
    }
}
