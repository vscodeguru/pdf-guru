using System;
using System.IO;

namespace PdfSharp.Pdf.Filters
{
    public class LzwDecode : Filter
    {
        public override byte[] Encode(byte[] data)
        {
            throw new NotImplementedException("PDFsharp does not support LZW encoding.");
        }

        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            if (data[0] == 0x00 && data[1] == 0x01)
                throw new Exception("LZW flavour not supported.");

            MemoryStream outputStream = new MemoryStream();

            InitializeDictionary();

            _data = data;
            _bytePointer = 0;
            _nextData = 0;
            _nextBits = 0;
            int code, oldCode = 0;
            byte[] str;

            while ((code = NextCode) != 257)
            {
                if (code == 256)
                {
                    InitializeDictionary();
                    code = NextCode;
                    if (code == 257)
                    {
                        break;
                    }
                    outputStream.Write(_stringTable[code], 0, _stringTable[code].Length);
                    oldCode = code;

                }
                else
                {
                    if (code < _tableIndex)
                    {
                        str = _stringTable[code];
                        outputStream.Write(str, 0, str.Length);
                        AddEntry(_stringTable[oldCode], str[0]);
                        oldCode = code;
                    }
                    else
                    {
                        str = _stringTable[oldCode];
                        outputStream.Write(str, 0, str.Length);
                        AddEntry(str, str[0]);
                        oldCode = code;
                    }
                }
            }

            if (outputStream.Length >= 0)
            {
                outputStream.Capacity = (int)outputStream.Length;
                return outputStream.GetBuffer();
            }
            return null;
        }

        void InitializeDictionary()
        {
            _stringTable = new byte[8192][];

            for (int i = 0; i < 256; i++)
            {
                _stringTable[i] = new byte[1];
                _stringTable[i][0] = (byte)i;
            }

            _tableIndex = 258;
            _bitsToGet = 9;
        }

        void AddEntry(byte[] oldstring, byte newstring)
        {
            int length = oldstring.Length;
            byte[] str = new byte[length + 1];
            Array.Copy(oldstring, 0, str, 0, length);
            str[length] = newstring;

            _stringTable[_tableIndex++] = str;

            if (_tableIndex == 511)
                _bitsToGet = 10;
            else if (_tableIndex == 1023)
                _bitsToGet = 11;
            else if (_tableIndex == 2047)
                _bitsToGet = 12;
        }

        int NextCode
        {
            get
            {
                try
                {
                    _nextData = (_nextData << 8) | (_data[_bytePointer++] & 0xff);
                    _nextBits += 8;

                    if (_nextBits < _bitsToGet)
                    {
                        _nextData = (_nextData << 8) | (_data[_bytePointer++] & 0xff);
                        _nextBits += 8;
                    }

                    int code = (_nextData >> (_nextBits - _bitsToGet)) & _andTable[_bitsToGet - 9];
                    _nextBits -= _bitsToGet;

                    return code;
                }
                catch
                {
                    return 257;
                }
            }
        }

        readonly int[] _andTable = { 511, 1023, 2047, 4095 };
        byte[][] _stringTable;
        byte[] _data;
        int _tableIndex, _bitsToGet = 9;
        int _bytePointer;
        int _nextData = 0;
        int _nextBits = 0;
    }
}
