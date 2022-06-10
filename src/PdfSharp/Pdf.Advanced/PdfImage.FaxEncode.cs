
using System;
using System.Diagnostics;

namespace PdfSharp.Pdf.Advanced
{
    partial class PdfImage
    {
        internal readonly static uint[] WhiteTerminatingCodes =
        {
              0x35, 8,   
              0x07, 6, 
              0x07, 4, 
              0x08, 4, 
              0x0b, 4, 
              0x0c, 4, 
              0x0e, 4, 
              0x0f, 4, 
              0x13, 5, 
              0x14, 5, 
              0x07, 5,      
              0x08, 5, 
              0x08, 6, 
              0x03, 6, 
              0x34, 6, 
              0x35, 6, 
              0x2a, 6,     
              0x2b, 6, 
              0x27, 7, 
              0x0c, 7, 
              0x08, 7,    
              0x17, 7, 
              0x03, 7, 
              0x04, 7, 
              0x28, 7, 
              0x2b, 7, 
              0x13, 7, 
              0x24, 7, 
              0x18, 7, 
              0x02, 8, 
              0x03, 8,   
              0x1a, 8, 
              0x1b, 8,   
              0x12, 8, 
              0x13, 8, 
              0x14, 8, 
              0x15, 8, 
              0x16, 8, 
              0x17, 8, 
              0x28, 8, 
              0x29, 8,   
              0x2a, 8, 
              0x2b, 8, 
              0x2c, 8, 
              0x2d, 8, 
              0x04, 8, 
              0x05, 8, 
              0x0a, 8, 
              0x0b, 8,   
              0x52, 8, 
              0x53, 8,   
              0x54, 8, 
              0x55, 8, 
              0x24, 8, 
              0x25, 8, 
              0x58, 8, 
              0x59, 8, 
              0x5a, 8, 
              0x5b, 8, 
              0x4a, 8, 
              0x4b, 8,   
              0x32, 8, 
              0x33, 8, 
              0x34, 8,   
        };

        internal readonly static uint[] BlackTerminatingCodes =
        {
              0x37, 10,     
              0x02,  3, 
              0x03,  2, 
              0x02,  2, 
              0x03,  3, 
              0x03,  4, 
              0x02,  4, 
              0x03,  5, 
              0x05,  6, 
              0x04,  6, 
              0x04,  7, 
              0x05,  7, 
              0x07,  7, 
              0x04,  8, 
              0x07,  8, 
              0x18,  9, 
              0x17, 10,     
              0x18, 10, 
              0x08, 10, 
              0x67, 11, 
              0x68, 11, 
              0x6c, 11, 
              0x37, 11, 
              0x28, 11, 
              0x17, 11, 
              0x18, 11, 
              0xca, 12, 
              0xcb, 12, 
              0xcc, 12, 
              0xcd, 12, 
              0x68, 12,   
              0x69, 12, 
              0x6a, 12,   
              0x6b, 12, 
              0xd2, 12, 
              0xd3, 12, 
              0xd4, 12, 
              0xd5, 12, 
              0xd6, 12, 
              0xd7, 12, 
              0x6c, 12, 
              0x6d, 12, 
              0xda, 12, 
              0xdb, 12, 
              0x54, 12, 
              0x55, 12, 
              0x56, 12, 
              0x57, 12, 
              0x64, 12,   
              0x65, 12, 
              0x52, 12, 
              0x53, 12, 
              0x24, 12, 
              0x37, 12, 
              0x38, 12, 
              0x27, 12, 
              0x28, 12, 
              0x58, 12, 
              0x59, 12, 
              0x2b, 12, 
              0x2c, 12, 
              0x5a, 12, 
              0x66, 12, 
              0x67, 12,   
        };

        internal readonly static uint[] WhiteMakeUpCodes =
        {
              0x1b,  5,             
              0x12,  5,  
              0x17,  6,  
              0x37,  7,  
              0x36,  8,  
              0x37,  8,  
              0x64,  8,  
              0x65,  8,  
              0x68,  8,  
              0x67,  8,  
              0xcc,  9,        
              0xcd,  9,  
              0xd2,  9,  
              0xd3,  9,  
              0xd4,  9,  
              0xd5,  9,  
              0xd6,  9,       
              0xd7,  9,  
              0xd8,  9,  
              0xd9,  9,  
              0xda,  9,  
              0xdb,  9,  
              0x98,  9,  
              0x99,  9,  
              0x9a,  9,  
              0x18,  6,     
              0x9b,  9,  
              0x08, 11,  
              0x0c, 11,  
              0x0d, 11,  
              0x12, 12,  
              0x13, 12,  
              0x14, 12,    
              0x15, 12,  
              0x16, 12,  
              0x17, 12,  
              0x1c, 12,  
              0x1d, 12,  
              0x1e, 12,  
              0x1f, 12,  
              0x01, 12,     
        };

        internal readonly static uint[] BlackMakeUpCodes =
        {
              0x0f, 10,         
              0xc8, 12,   
              0xc9, 12,   
              0x5b, 12,   
              0x33, 12,   
              0x34, 12,   
              0x35, 12,   
              0x6c, 13,  
              0x6d, 13,  
              0x4a, 13,  
              0x4b, 13,  
              0x4c, 13,  
              0x4d, 13,  
              0x72, 13,  
              0x73, 13,  
              0x74, 13,  
              0x75, 13,    
              0x76, 13,  
              0x77, 13,  
              0x52, 13,  
              0x53, 13,  
              0x54, 13,  
              0x55, 13,  
              0x5a, 13,  
              0x5b, 13,  
              0x64, 13,  
              0x65, 13,  
              0x08, 11,  
              0x0c, 11,  
              0x0d, 11,  
              0x12, 12,  
              0x13, 12,  
              0x14, 12,     
              0x15, 12,  
              0x16, 12,  
              0x17, 12,  
              0x1c, 12,  
              0x1d, 12,  
              0x1e, 12,  
              0x1f, 12,  
              0x01, 12,      
        };

        internal readonly static uint[] HorizontalCodes = { 0x1, 3 };   
        internal readonly static uint[] PassCodes = { 0x1, 4, };   
        internal readonly static uint[] VerticalCodes =
        {
              0x03, 7,    
              0x03, 6,    
              0x03, 3,   
              0x1,  1,   
              0x2,  3,   
              0x02, 6,    
              0x02, 7,    
        };

        readonly static uint[] _zeroRuns =
        {
              8, 7, 6, 6, 5, 5, 5, 5, 4, 4, 4, 4, 4, 4, 4, 4,	    
              3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,	    
              2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,	    
              2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
        };

        readonly static uint[] _oneRuns =
        {
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,	    
              2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,	    
              2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,	    
              3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,	    
              4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 7, 8,	    
        };

        private static uint CountOneBits(BitReader reader, uint bitsLeft)
        {
            uint found = 0;
            for (;;)
            {
                uint bits;
                int @byte = reader.PeekByte(out bits);
                uint hits = _oneRuns[@byte];
                if (hits < bits)
                {
                    if (hits > 0)
                        reader.SkipBits(hits);
                    found += hits;
                    return found >= bitsLeft ? bitsLeft : found;
                }
                found += bits;
                if (found >= bitsLeft)
                    return bitsLeft;
                reader.NextByte();
            }
        }

        private static uint CountZeroBits(BitReader reader, uint bitsLeft)
        {
            uint found = 0;
            for (;;)
            {
                uint bits;
                int @byte = reader.PeekByte(out bits);
                uint hits = _zeroRuns[@byte];
                if (hits < bits)
                {
                    if (hits > 0)
                        reader.SkipBits(hits);
                    found += hits;
                    return found >= bitsLeft ? bitsLeft : found;
                }
                found += bits;
                if (found >= bitsLeft)
                    return bitsLeft;
                reader.NextByte();
            }
        }

        private static uint FindDifference(BitReader reader, uint bitStart, uint bitEnd, bool searchOne)
        {
            reader.SetPosition(bitStart);
            return (bitStart + (searchOne ? CountOneBits(reader, bitEnd - bitStart) : CountZeroBits(reader, bitEnd - bitStart)));
        }

        private static uint FindDifferenceWithCheck(BitReader reader, uint bitStart, uint bitEnd, bool searchOne)
        {
            return ((bitStart < bitEnd) ? FindDifference(reader, bitStart, bitEnd, searchOne) : bitEnd);
        }

        static void FaxEncode2DRow(BitWriter writer, uint bytesFileOffset, byte[] imageBits, uint currentRow, uint referenceRow, uint width, uint height, uint bytesPerLineBmp)
        {
            uint bytesOffsetRead = bytesFileOffset + (height - 1 - currentRow) * bytesPerLineBmp;
            BitReader reader = new BitReader(imageBits, bytesOffsetRead, width);
            BitReader readerReference;
            if (referenceRow != 0xffffffff)
            {
                uint bytesOffsetReadReference = bytesFileOffset + (height - 1 - referenceRow) * bytesPerLineBmp;
                readerReference = new BitReader(imageBits, bytesOffsetReadReference, width);
            }
            else
            {
                byte[] tmpImageBits = new byte[bytesPerLineBmp];
                for (int i = 0; i < bytesPerLineBmp; ++i)
                    tmpImageBits[i] = 255;
                readerReference = new BitReader(tmpImageBits, 0, width);
            }

            uint a0 = 0;
            uint a1 = !reader.GetBit(0) ? 0 : FindDifference(reader, 0, width, true);
            uint b1 = !readerReference.GetBit(0) ? 0 : FindDifference(readerReference, 0, width, true);
            uint a2, b2;
            for (;;)
            {
                b2 = FindDifferenceWithCheck(readerReference, b1, width, readerReference.GetBit(b1));
                if (b2 >= a1)
                {
                    int d = (int)b1 - (int)a1;
                    if (!(-3 <= d && d <= 3))
                    {
                        a2 = FindDifferenceWithCheck(reader, a1, width, reader.GetBit(a1));
                        writer.WriteTableLine(HorizontalCodes, 0);

                        if (a0 + a1 == 0 || reader.GetBit(a0))
                        {
                            WriteSample(writer, a1 - a0, true);
                            WriteSample(writer, a2 - a1, false);
                        }
                        else
                        {
                            WriteSample(writer, a1 - a0, false);
                            WriteSample(writer, a2 - a1, true);
                        }
                        a0 = a2;
                    }
                    else
                    {
                        writer.WriteTableLine(VerticalCodes, (uint)(d + 3));
                        a0 = a1;
                    }
                }
                else
                {
                    writer.WriteTableLine(PassCodes, 0);
                    a0 = b2;
                }
                if (a0 >= width)
                    break;
                bool bitA0 = reader.GetBit(a0);
                a1 = FindDifference(reader, a0, width, bitA0);
                b1 = FindDifference(readerReference, a0, width, !bitA0);
                b1 = FindDifferenceWithCheck(readerReference, b1, width, bitA0);
            }
        }

        private static int DoFaxEncoding(ref byte[] imageData, byte[] imageBits, uint bytesFileOffset, uint width, uint height)
        {
            try
            {
                uint bytesPerLineBmp = ((width + 31) / 32) * 4;
                BitWriter writer = new BitWriter(ref imageData);
                for (uint y = 0; y < height; ++y)
                {
                    uint bytesOffsetRead = bytesFileOffset + (height - 1 - y) * bytesPerLineBmp;
                    BitReader reader = new BitReader(imageBits, bytesOffsetRead, width);
                    for (uint bitsRead = 0; bitsRead < width;)
                    {
                        uint white = CountOneBits(reader, width - bitsRead);
                        WriteSample(writer, white, true);
                        bitsRead += white;
                        if (bitsRead < width)
                        {
                            uint black = CountZeroBits(reader, width - bitsRead);
                            WriteSample(writer, black, false);
                            bitsRead += black;
                        }
                    }
                }
                writer.FlushBuffer();
                return writer.BytesWritten();
            }
            catch (Exception )
            {
                return 0;
            }
        }

        internal static int DoFaxEncodingGroup4(ref byte[] imageData, byte[] imageBits, uint bytesFileOffset, uint width, uint height)
        {
            try
            {
                uint bytesPerLineBmp = ((width + 31) / 32) * 4;
                BitWriter writer = new BitWriter(ref imageData);
                for (uint y = 0; y < height; ++y)
                {
                    FaxEncode2DRow(writer, bytesFileOffset, imageBits, y, (y != 0) ? y - 1 : 0xffffffff, width, height, bytesPerLineBmp);
                }
                writer.FlushBuffer();
                return writer.BytesWritten();
            }
            catch (Exception ex)
            {
                ex.GetType();
                return 0;
            }
        }

        private static void WriteSample(BitWriter writer, uint count, bool white)
        {
            uint[] terminatingCodes = white ? WhiteTerminatingCodes : BlackTerminatingCodes;
            uint[] makeUpCodes = white ? WhiteMakeUpCodes : BlackMakeUpCodes;

            while (count >= 2624)
            {
                writer.WriteTableLine(makeUpCodes, 39);   
                count -= 2560;
            }
            if (count > 63)
            {
                uint line = count / 64 - 1;
                writer.WriteTableLine(makeUpCodes, line);
                count -= (line + 1) * 64;
            }
            writer.WriteTableLine(terminatingCodes, count);
        }
    }

    class BitReader
    {
        readonly byte[] _imageBits;
        uint _bytesOffsetRead;
        readonly uint _bytesFileOffset;
        byte _buffer;
        uint _bitsInBuffer;
        readonly uint _bitsTotal;         

        internal BitReader(byte[] imageBits, uint bytesFileOffset, uint bits)
        {
            _imageBits = imageBits;
            _bytesFileOffset = bytesFileOffset;
            _bitsTotal = bits;
            _bytesOffsetRead = bytesFileOffset;
            _buffer = imageBits[_bytesOffsetRead];
            _bitsInBuffer = 8;
        }

        internal void SetPosition(uint position)
        {
            _bytesOffsetRead = _bytesFileOffset + (position >> 3);
            _buffer = _imageBits[_bytesOffsetRead];
            _bitsInBuffer = 8 - (position & 0x07);
        }

        internal bool GetBit(uint position)
        {
            if (position >= _bitsTotal)
                return false;
            SetPosition(position);
            uint dummy;
            return (PeekByte(out dummy) & 0x80) > 0;
        }

        internal byte PeekByte(out uint bits)
        {
            if (_bitsInBuffer == 8)
            {
                bits = 8;
                return _buffer;
            }
            bits = _bitsInBuffer;
            return (byte)(_buffer << (int)(8 - _bitsInBuffer));
        }

        internal void NextByte()
        {
            _buffer = _imageBits[++_bytesOffsetRead];
            _bitsInBuffer = 8;
        }

        internal void SkipBits(uint bits)
        {
            Debug.Assert(bits <= _bitsInBuffer, "Buffer underrun");
            if (bits == _bitsInBuffer)
            {
                NextByte();
                return;
            }
            _bitsInBuffer -= bits;
        }
    }

    class BitWriter
    {
        internal BitWriter(ref byte[] imageData)
        {
            _imageData = imageData;
        }

        internal void FlushBuffer()
        {
            if (_bitsInBuffer > 0)
            {
                uint bits = 8 - _bitsInBuffer;
                WriteBits(0, bits);
            }
        }

        static readonly uint[] masks = { 0, 1, 3, 7, 15, 31, 63, 127, 255 };

        internal void WriteBits(uint value, uint bits)
        {
#if true

            if (bits + _bitsInBuffer > 8)
            {
                uint bitsNow = 8 - _bitsInBuffer;
                uint bitsRemainder = bits - bitsNow;
                WriteBits(value >> (int)(bitsRemainder), bitsNow);   
#if USE_GOTO
#else
        WriteBits(value, bitsRemainder);
        return;
#endif
            }

            _buffer = (_buffer << (int)bits) + (value & masks[bits]);
            _bitsInBuffer += bits;

            if (_bitsInBuffer == 8)
            {
                _imageData[_bytesOffsetWrite] = (byte)_buffer;
                _bitsInBuffer = 0;
                ++_bytesOffsetWrite;
            }
#endif
        }

        internal void WriteTableLine(uint[] table, uint line)
        {
            uint value = table[line * 2];
            uint bits = table[line * 2 + 1];
            WriteBits(value, bits);
        }

        [Obsolete]
        internal void WriteEOL()
        {
            WriteTableLine(PdfImage.WhiteMakeUpCodes, 40);
        }

        internal int BytesWritten()
        {
            FlushBuffer();
            return _bytesOffsetWrite;
        }

        int _bytesOffsetWrite;
        readonly byte[] _imageData;
        uint _buffer;
        uint _bitsInBuffer;
    }
}
