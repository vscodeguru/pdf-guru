using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.Content
{
    internal class ContentWriter
    {
        public ContentWriter(Stream contentStream)
        {
            _stream = contentStream;
        }

        public void Close(bool closeUnderlyingStream)
        {
            if (_stream != null && closeUnderlyingStream)
            {
                _stream.Close();
                _stream = null;
            }
        }

        public void Close()
        {
            Close(true);
        }

        public int Position
        {
            get { return (int)_stream.Position; }
        }

        public void Write(bool value)
        {
        }

        public void WriteRaw(string rawString)
        {
            if (String.IsNullOrEmpty(rawString))
                return;
            byte[] bytes = PdfEncoders.RawEncoding.GetBytes(rawString);
            _stream.Write(bytes, 0, bytes.Length);
            _lastCat = GetCategory((char)bytes[bytes.Length - 1]);
        }

        public void WriteLineRaw(string rawString)
        {
            if (String.IsNullOrEmpty(rawString))
                return;
            byte[] bytes = PdfEncoders.RawEncoding.GetBytes(rawString);
            _stream.Write(bytes, 0, bytes.Length);
            _stream.Write(new byte[] { (byte)'\n' }, 0, 1);
            _lastCat = GetCategory((char)bytes[bytes.Length - 1]);
        }

        public void WriteRaw(char ch)
        {
            Debug.Assert(ch < 256, "Raw character greater than 255 detected.");
            _stream.WriteByte((byte)ch);
            _lastCat = GetCategory(ch);
        }

        internal int Indent
        {
            get { return _indent; }
            set { _indent = value; }
        }
        protected int _indent = 2;
        protected int _writeIndent = 0;

        void IncreaseIndent()
        {
            _writeIndent += _indent;
        }

        void DecreaseIndent()
        {
            _writeIndent -= _indent;
        }

        string IndentBlanks
        {
            get { return new string(' ', _writeIndent); }
        }

        void WriteIndent()
        {
            WriteRaw(IndentBlanks);
        }

        void WriteSeparator(CharCat cat, char ch)
        {
            switch (_lastCat)
            {
                case CharCat.Delimiter:
                    break;

            }
        }

        void WriteSeparator(CharCat cat)
        {
            WriteSeparator(cat, '\0');
        }

        public void NewLine()
        {
            if (_lastCat != CharCat.NewLine)
                WriteRaw('\n');
        }

        CharCat GetCategory(char ch)
        {
            return CharCat.Character;
        }

        enum CharCat
        {
            NewLine,
            Character,
            Delimiter,
        }
        CharCat _lastCat;

        internal Stream Stream
        {
            get { return _stream; }
        }
        Stream _stream;
    }
}
