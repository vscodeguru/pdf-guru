using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Security;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.IO
{
    internal class PdfWriter
    {
        public PdfWriter(Stream pdfStream, PdfStandardSecurityHandler securityHandler)
        {
            _stream = pdfStream;
            _securityHandler = securityHandler;

        }

        public void Close(bool closeUnderlyingStream)
        {
            if (_stream != null && closeUnderlyingStream)
            {

                _stream.Close();

            }
            _stream = null;
        }

        public void Close()
        {
            Close(true);
        }

        public int Position
        {
            get { return (int)_stream.Position; }
        }

        public PdfWriterLayout Layout
        {
            get { return _layout; }
            set { _layout = value; }
        }
        PdfWriterLayout _layout;

        public PdfWriterOptions Options
        {
            get { return _options; }
            set { _options = value; }
        }
        PdfWriterOptions _options;

        public void Write(bool value)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(value ? bool.TrueString : bool.FalseString);
            _lastCat = CharCat.Character;
        }

        public void Write(PdfBoolean value)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(value.Value ? "true" : "false");
            _lastCat = CharCat.Character;
        }

        public void Write(int value)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(value.ToString(CultureInfo.InvariantCulture));
            _lastCat = CharCat.Character;
        }

        public void Write(uint value)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(value.ToString(CultureInfo.InvariantCulture));
            _lastCat = CharCat.Character;
        }

        public void Write(PdfInteger value)
        {
            WriteSeparator(CharCat.Character);
            _lastCat = CharCat.Character;
            WriteRaw(value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public void Write(PdfUInteger value)
        {
            WriteSeparator(CharCat.Character);
            _lastCat = CharCat.Character;
            WriteRaw(value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public void Write(double value)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(value.ToString(Config.SignificantFigures7, CultureInfo.InvariantCulture));
            _lastCat = CharCat.Character;
        }

        public void Write(PdfReal value)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(value.Value.ToString(Config.SignificantFigures7, CultureInfo.InvariantCulture));
            _lastCat = CharCat.Character;
        }

        public void Write(PdfString value)
        {
            WriteSeparator(CharCat.Delimiter);
            PdfStringEncoding encoding = (PdfStringEncoding)(value.Flags & PdfStringFlags.EncodingMask);
            string pdf = (value.Flags & PdfStringFlags.HexLiteral) == 0 ?
                PdfEncoders.ToStringLiteral(value.Value, encoding, SecurityHandler) :
                PdfEncoders.ToHexStringLiteral(value.Value, encoding, SecurityHandler);
            WriteRaw(pdf);
            _lastCat = CharCat.Delimiter;
        }

        public void Write(PdfName value)
        {
            WriteSeparator(CharCat.Delimiter, '/');
            string name = value.Value;

            StringBuilder pdf = new StringBuilder("/");
            for (int idx = 1; idx < name.Length; idx++)
            {
                char ch = name[idx];
                Debug.Assert(ch < 256);
                if (ch > ' ')
                    switch (ch)
                    {
                        case '%':
                        case '/':
                        case '<':
                        case '>':
                        case '(':
                        case ')':
                        case '[':
                        case ']':
                        case '#':
                            break;

                        default:
                            pdf.Append(name[idx]);
                            continue;
                    }
                pdf.AppendFormat("#{0:X2}", (int)name[idx]);
            }
            WriteRaw(pdf.ToString());
            _lastCat = CharCat.Character;
        }

        public void Write(PdfLiteral value)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(value.Value);
            _lastCat = CharCat.Character;
        }

        public void Write(PdfRectangle rect)
        {
            const string format = Config.SignificantFigures3;
            WriteSeparator(CharCat.Delimiter, '/');
            WriteRaw(PdfEncoders.Format("[{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "}]", rect.X1, rect.Y1, rect.X2, rect.Y2));
            _lastCat = CharCat.Delimiter;
        }

        public void Write(PdfReference iref)
        {
            WriteSeparator(CharCat.Character);
            WriteRaw(iref.ToString());
            _lastCat = CharCat.Character;
        }

        public void WriteDocString(string text, bool unicode)
        {
            WriteSeparator(CharCat.Delimiter);
            byte[] bytes;
            if (!unicode)
                bytes = PdfEncoders.DocEncoding.GetBytes(text);
            else
                bytes = PdfEncoders.UnicodeEncoding.GetBytes(text);
            bytes = PdfEncoders.FormatStringLiteral(bytes, unicode, true, false, _securityHandler);
            Write(bytes);
            _lastCat = CharCat.Delimiter;
        }

        public void WriteDocString(string text)
        {
            WriteSeparator(CharCat.Delimiter);
            byte[] bytes = PdfEncoders.DocEncoding.GetBytes(text);
            bytes = PdfEncoders.FormatStringLiteral(bytes, false, false, false, _securityHandler);
            Write(bytes);
            _lastCat = CharCat.Delimiter;
        }

        public void WriteDocStringHex(string text)
        {
            WriteSeparator(CharCat.Delimiter);
            byte[] bytes = PdfEncoders.DocEncoding.GetBytes(text);
            bytes = PdfEncoders.FormatStringLiteral(bytes, false, false, true, _securityHandler);
            _stream.Write(bytes, 0, bytes.Length);
            _lastCat = CharCat.Delimiter;
        }

        public void WriteBeginObject(PdfObject obj)
        {
            bool indirect = obj.IsIndirect;
            if (indirect)
            {
                WriteObjectAddress(obj);
                if (_securityHandler != null)
                    _securityHandler.SetHashKey(obj.ObjectID);
            }
            _stack.Add(new StackItem(obj));
            if (indirect)
            {
                if (obj is PdfArray)
                    WriteRaw("[\n");
                else if (obj is PdfDictionary)
                    WriteRaw("<<\n");
                _lastCat = CharCat.NewLine;
            }
            else
            {
                if (obj is PdfArray)
                {
                    WriteSeparator(CharCat.Delimiter);
                    WriteRaw('[');
                    _lastCat = CharCat.Delimiter;
                }
                else if (obj is PdfDictionary)
                {
                    NewLine();
                    WriteSeparator(CharCat.Delimiter);
                    WriteRaw("<<\n");
                    _lastCat = CharCat.NewLine;
                }
            }
            if (_layout == PdfWriterLayout.Verbose)
                IncreaseIndent();
        }

        public void WriteEndObject()
        {
            int count = _stack.Count;
            Debug.Assert(count > 0, "PdfWriter stack underflow.");

            StackItem stackItem = _stack[count - 1];
            _stack.RemoveAt(count - 1);

            PdfObject value = stackItem.Object;
            bool indirect = value.IsIndirect;
            if (_layout == PdfWriterLayout.Verbose)
                DecreaseIndent();
            if (value is PdfArray)
            {
                if (indirect)
                {
                    WriteRaw("\n]\n");
                    _lastCat = CharCat.NewLine;
                }
                else
                {
                    WriteRaw("]");
                    _lastCat = CharCat.Delimiter;
                }
            }
            else if (value is PdfDictionary)
            {
                if (indirect)
                {
                    if (!stackItem.HasStream)
                        WriteRaw(_lastCat == CharCat.NewLine ? ">>\n" : " >>\n");
                }
                else
                {
                    Debug.Assert(!stackItem.HasStream, "Direct object with stream??");
                    WriteSeparator(CharCat.NewLine);
                    WriteRaw(">>\n");
                    _lastCat = CharCat.NewLine;
                }
            }
            if (indirect)
            {
                NewLine();
                WriteRaw("endobj\n");
                if (_layout == PdfWriterLayout.Verbose)
                    WriteRaw("%--------------------------------------------------------------------------------------------------\n");
            }
        }

        public void WriteStream(PdfDictionary value, bool omitStream)
        {
            StackItem stackItem = _stack[_stack.Count - 1];
            Debug.Assert(stackItem.Object is PdfDictionary);
            Debug.Assert(stackItem.Object.IsIndirect);
            stackItem.HasStream = true;

            WriteRaw(_lastCat == CharCat.NewLine ? ">>\nstream\n" : " >>\nstream\n");

            if (omitStream)
            {
                WriteRaw("  «...stream content omitted...»\n");      
            }
            else
            {
                byte[] bytes = value.Stream.Value;
                if (bytes.Length != 0)
                {
                    if (_securityHandler != null)
                    {
                        bytes = (byte[])bytes.Clone();
                        bytes = _securityHandler.EncryptBytes(bytes);
                    }
                    Write(bytes);
                    if (_lastCat != CharCat.NewLine)
                        WriteRaw('\n');
                }
            }
            WriteRaw("endstream\n");
        }

        public void WriteRaw(string rawString)
        {
            if (String.IsNullOrEmpty(rawString))
                return;

            byte[] bytes = PdfEncoders.RawEncoding.GetBytes(rawString);
            _stream.Write(bytes, 0, bytes.Length);
            _lastCat = GetCategory((char)bytes[bytes.Length - 1]);
        }

        public void WriteRaw(char ch)
        {
            Debug.Assert(ch < 256, "Raw character greater than 255 detected.");

            _stream.WriteByte((byte)ch);
            _lastCat = GetCategory(ch);
        }

        public void Write(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return;

            _stream.Write(bytes, 0, bytes.Length);
            _lastCat = GetCategory((char)bytes[bytes.Length - 1]);
        }

        void WriteObjectAddress(PdfObject value)
        {
            if (_layout == PdfWriterLayout.Verbose)
                WriteRaw(String.Format("{0} {1} obj   % {2}\n",
                    value.ObjectID.ObjectNumber, value.ObjectID.GenerationNumber,
                    value.GetType().FullName));
            else
                WriteRaw(String.Format("{0} {1} obj\n", value.ObjectID.ObjectNumber, value.ObjectID.GenerationNumber));
        }

        public void WriteFileHeader(PdfDocument document)
        {
            StringBuilder header = new StringBuilder("%PDF-");
            int version = document._version;
            header.Append((version / 10).ToString(CultureInfo.InvariantCulture) + "." +
              (version % 10).ToString(CultureInfo.InvariantCulture) + "\n%\xD3\xF4\xCC\xE1\n");
            WriteRaw(header.ToString());

            if (_layout == PdfWriterLayout.Verbose)
            {
                WriteRaw(String.Format("% PDFsharp Version {0} (verbose mode)\n", VersionInfo.Version));
                _commentPosition = (int)_stream.Position + 2;
                WriteRaw("%                                                \n");
                WriteRaw("%                                                \n");
                WriteRaw("%                                                \n");
                WriteRaw("%                                                \n");
                WriteRaw("%                                                \n");
                WriteRaw("%--------------------------------------------------------------------------------------------------\n");
            }
        }

        public void WriteEof(PdfDocument document, int startxref)
        {
            WriteRaw("startxref\n");
            WriteRaw(startxref.ToString(CultureInfo.InvariantCulture));
            WriteRaw("\n%%EOF\n");
            int fileSize = (int)_stream.Position;
            if (_layout == PdfWriterLayout.Verbose)
            {
                TimeSpan duration = DateTime.Now - document._creation;

                _stream.Position = _commentPosition;
                WriteRaw("Creation date: " + document._creation.ToString("G", CultureInfo.InvariantCulture));
                _stream.Position = _commentPosition + 50;
                WriteRaw("Creation time: " + duration.TotalSeconds.ToString("0.000", CultureInfo.InvariantCulture) + " seconds");
                _stream.Position = _commentPosition + 100;
                WriteRaw("File size: " + fileSize.ToString(CultureInfo.InvariantCulture) + " bytes");
                _stream.Position = _commentPosition + 150;
                WriteRaw("Pages: " + document.Pages.Count.ToString(CultureInfo.InvariantCulture));
                _stream.Position = _commentPosition + 200;
                WriteRaw("Objects: " + document._irefTable.ObjectTable.Count.ToString(CultureInfo.InvariantCulture));
            }
        }

        internal int Indent
        {
            get { return _indent; }
            set { _indent = value; }
        }
        int _indent = 2;
        int _writeIndent;

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
                case CharCat.NewLine:
                    if (_layout == PdfWriterLayout.Verbose)
                        WriteIndent();
                    break;

                case CharCat.Delimiter:
                    break;

                case CharCat.Character:
                    if (_layout == PdfWriterLayout.Verbose)
                    {
                        _stream.WriteByte((byte)' ');
                    }
                    else
                    {
                        if (cat == CharCat.Character)
                            _stream.WriteByte((byte)' ');
                    }
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
            if (Lexer.IsDelimiter(ch))
                return CharCat.Delimiter;
            if (ch == Chars.LF)
                return CharCat.NewLine;
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

        internal PdfStandardSecurityHandler SecurityHandler
        {
            get { return _securityHandler; }
            set { _securityHandler = value; }
        }
        PdfStandardSecurityHandler _securityHandler;

        class StackItem
        {
            public StackItem(PdfObject value)
            {
                Object = value;
            }

            public readonly PdfObject Object;
            public bool HasStream;
        }

        readonly List<StackItem> _stack = new List<StackItem>();
        int _commentPosition;
    }
}
