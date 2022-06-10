using System;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using System.IO;
using PdfSharp.Internal;
using PdfSharp.Pdf.Internal;


namespace PdfSharp.Pdf.IO
{
    public class Lexer
    {
        public Lexer(Stream pdfInputStream)
        {
            _pdfSteam = pdfInputStream;
            _pdfLength = (int)_pdfSteam.Length;
            _idxChar = 0;
            Position = 0;
        }

        public int Position
        {
            get { return _idxChar; }
            set
            {
                _idxChar = value;
                _pdfSteam.Position = value;
                _currChar = (char)_pdfSteam.ReadByte();
                _nextChar = (char)_pdfSteam.ReadByte();
                _token = new StringBuilder();
            }
        }

        public Symbol ScanNextToken()
        {
            Again:
            _token = new StringBuilder();

            char ch = MoveToNonWhiteSpace();
            switch (ch)
            {
                case '%':
                    ScanComment();
                    goto Again;

                case '/':
                    return _symbol = ScanName();

                case '+':     
                case '-':
                    return _symbol = ScanNumber();

                case '(':
                    return _symbol = ScanLiteralString();

                case '[':
                    ScanNextChar(true);
                    return _symbol = Symbol.BeginArray;

                case ']':
                    ScanNextChar(true);
                    return _symbol = Symbol.EndArray;

                case '<':
                    if (_nextChar == '<')
                    {
                        ScanNextChar(true);
                        ScanNextChar(true);
                        return _symbol = Symbol.BeginDictionary;
                    }
                    return _symbol = ScanHexadecimalString();

                case '>':
                    if (_nextChar == '>')
                    {
                        ScanNextChar(true);
                        ScanNextChar(true);
                        return _symbol = Symbol.EndDictionary;
                    }
                    ParserDiagnostics.HandleUnexpectedCharacter(_nextChar);
                    break;

                case '.':
                    return _symbol = ScanNumber();
            }
            if (char.IsDigit(ch))
#if true_
#else
                if (PeekReference())
                    return _symbol = ScanNumber();
                else
                    return _symbol = ScanNumber();
#endif

            if (char.IsLetter(ch))
                return _symbol = ScanKeyword();

            if (ch == Chars.EOF)
                return _symbol = Symbol.Eof;

            ParserDiagnostics.HandleUnexpectedCharacter(ch);
            return _symbol = Symbol.None;
        }

        public byte[] ReadStream(int length)
        {
            int pos;

            while (_currChar == Chars.SP)
                ScanNextChar(true);

            if (_currChar == Chars.CR)
            {
                if (_nextChar == Chars.LF)
                    pos = _idxChar + 2;
                else
                    pos = _idxChar + 1;
            }
            else
                pos = _idxChar + 1;

            _pdfSteam.Position = pos;
            byte[] bytes = new byte[length];
            int read = _pdfSteam.Read(bytes, 0, length);
            Debug.Assert(read == length);
            if (bytes.Length != read)
            {
                Array.Resize(ref bytes, read);
            }

            Position = pos + read;
            return bytes;
        }

        public String ReadRawString(int position, int length)
        {
            _pdfSteam.Position = position;
            byte[] bytes = new byte[length];
            _pdfSteam.Read(bytes, 0, length);
            return PdfEncoders.RawEncoding.GetString(bytes, 0, bytes.Length);
        }

        public Symbol ScanComment()
        {
            Debug.Assert(_currChar == Chars.Percent);

            _token = new StringBuilder();
            while (true)
            {
                char ch = AppendAndScanNextChar();
                if (ch == Chars.LF || ch == Chars.EOF)
                    break;
            }
            if (_token.ToString().StartsWith("%%EOF"))
                return Symbol.Eof;
            return _symbol = Symbol.Comment;
        }

        public Symbol ScanName()
        {
            Debug.Assert(_currChar == Chars.Slash);

            _token = new StringBuilder();
            while (true)
            {
                char ch = AppendAndScanNextChar();
                if (IsWhiteSpace(ch) || IsDelimiter(ch) || ch == Chars.EOF)
                    return _symbol = Symbol.Name;

                if (ch == '#')
                {
                    ScanNextChar(true);
                    char[] hex = new char[2];
                    hex[0] = _currChar;
                    hex[1] = _nextChar;
                    ScanNextChar(true);
                    ch = (char)(ushort)int.Parse(new string(hex), NumberStyles.AllowHexSpecifier);
                    _currChar = ch;
                }
            }
        }

        public Symbol ScanNumber()
        {
            bool period = false;
            _token = new StringBuilder();
            char ch = _currChar;
            if (ch == '+' || ch == '-')
            {
                _token.Append(ch);
                ch = ScanNextChar(true);
            }
            while (true)
            {
                if (char.IsDigit(ch))
                {
                    _token.Append(ch);
                }
                else if (ch == '.')
                {
                    if (period)
                        ParserDiagnostics.ThrowParserException("More than one period in number.");

                    period = true;
                    _token.Append(ch);
                }
                else
                    break;
                ch = ScanNextChar(true);
            }

            if (period)
                return Symbol.Real;
            long l = Int64.Parse(_token.ToString(), CultureInfo.InvariantCulture);
            if (l >= Int32.MinValue && l <= Int32.MaxValue)
                return Symbol.Integer;
            if (l > 0 && l <= UInt32.MaxValue)
                return Symbol.UInteger;

            return Symbol.Real;
        }

        public Symbol ScanNumberOrReference()
        {
            Symbol result = ScanNumber();
            if (result == Symbol.Integer)
            {
                int pos = Position;
                string objectNumber = Token;
            }
            return result;
        }

        public Symbol ScanKeyword()
        {
            _token = new StringBuilder();
            char ch = _currChar;
            while (true)
            {
                if (char.IsLetter(ch))
                    _token.Append(ch);
                else
                    break;
                ch = ScanNextChar(false);
            }

            switch (_token.ToString())
            {
                case "obj":
                    return _symbol = Symbol.Obj;

                case "endobj":
                    return _symbol = Symbol.EndObj;

                case "null":
                    return _symbol = Symbol.Null;

                case "true":
                case "false":
                    return _symbol = Symbol.Boolean;

                case "R":
                    return _symbol = Symbol.R;

                case "stream":
                    return _symbol = Symbol.BeginStream;

                case "endstream":
                    return _symbol = Symbol.EndStream;

                case "xref":
                    return _symbol = Symbol.XRef;

                case "trailer":
                    return _symbol = Symbol.Trailer;

                case "startxref":
                    return _symbol = Symbol.StartXRef;
            }

            return _symbol = Symbol.Keyword;
        }

        public Symbol ScanLiteralString()
        {
            Debug.Assert(_currChar == Chars.ParenLeft);
            _token = new StringBuilder();
            int parenLevel = 0;
            char ch = ScanNextChar(false);

            while (ch != Chars.EOF)
            {
                switch (ch)
                {
                    case '(':
                        parenLevel++;
                        break;

                    case ')':
                        if (parenLevel == 0)
                        {
                            ScanNextChar(false);
                            goto Phase2;
                        }
                        parenLevel--;
                        break;

                    case '\\':
                        {
                            ch = ScanNextChar(false);
                            switch (ch)
                            {
                                case 'n':
                                    ch = Chars.LF;
                                    break;

                                case 'r':
                                    ch = Chars.CR;
                                    break;

                                case 't':
                                    ch = Chars.HT;
                                    break;

                                case 'b':
                                    ch = Chars.BS;
                                    break;

                                case 'f':
                                    ch = Chars.FF;
                                    break;

                                case '(':
                                    ch = Chars.ParenLeft;
                                    break;

                                case ')':
                                    ch = Chars.ParenRight;
                                    break;

                                case '\\':
                                    ch = Chars.BackSlash;
                                    break;

                                case ' ':
                                    ch = ' ';
                                    break;

                                case Chars.CR:
                                case Chars.LF:
                                    ch = ScanNextChar(false);
                                    continue;

                                default:
                                    if (char.IsDigit(ch) && _nextChar != '8' && _nextChar != '9')     
                                    {
                                        int n = ch - '0';
                                        if (char.IsDigit(_nextChar) && _nextChar != '8' && _nextChar != '9')     
                                        {
                                            ch = ScanNextChar(false);
                                            n = n * 8 + ch - '0';
                                            if (char.IsDigit(_nextChar) && _nextChar != '8' && _nextChar != '9')     
                                            {
                                                ch = ScanNextChar(false);
                                                n = n * 8 + ch - '0';
                                            }
                                        }
                                        ch = (char)n;
                                    }
                                    else
                                    {
                                    }
                                    break;
                            }
                            break;
                        }
                    default:
                        break;
                }

                _token.Append(ch);
                ch = ScanNextChar(false);
            }

            Phase2:
            if (_token.Length >= 2 && _token[0] == '\xFE' && _token[1] == '\xFF')
            {
                StringBuilder temp = _token;
                int length = temp.Length;
                if ((length & 1) == 1)
                {
                    temp.Append(0);
                    ++length;
                    DebugBreak.Break();
                }
                _token = new StringBuilder();
                for (int i = 2; i < length; i += 2)
                {
                    _token.Append((char)(256 * temp[i] + temp[i + 1]));
                }
                return _symbol = Symbol.UnicodeString;
            }
            if (_token.Length >= 2 && _token[0] == '\xFF' && _token[1] == '\xFE')
            {
                StringBuilder temp = _token;
                int length = temp.Length;
                if ((length & 1) == 1)
                {
                    temp.Append(0);
                    ++length;
                    DebugBreak.Break();
                }
                _token = new StringBuilder();
                for (int i = 2; i < length; i += 2)
                {
                    _token.Append((char)(256 * temp[i + 1] + temp[i]));
                }
                return _symbol = Symbol.UnicodeString;
            }
            return _symbol = Symbol.String;
        }

        public Symbol ScanHexadecimalString()
        {
            Debug.Assert(_currChar == Chars.Less);

            _token = new StringBuilder();
            char[] hex = new char[2];
            ScanNextChar(true);
            while (true)
            {
                MoveToNonWhiteSpace();
                if (_currChar == '>')
                {
                    ScanNextChar(true);
                    break;
                }
                if (char.IsLetterOrDigit(_currChar))
                {
                    hex[0] = char.ToUpper(_currChar);
                    if (char.IsLetterOrDigit(_nextChar))
                    {
                        hex[1] = char.ToUpper(_nextChar);
                        ScanNextChar(true);
                    }
                    else
                    {
                        hex[1] = '0';
                    }
                    ScanNextChar(true);

                    int ch = int.Parse(new string(hex), NumberStyles.AllowHexSpecifier);
                    _token.Append(Convert.ToChar(ch));
                }
                else
                    ParserDiagnostics.HandleUnexpectedCharacter(_currChar);
            }
            string chars = _token.ToString();
            int count = chars.Length;
            if (count > 2 && chars[0] == (char)0xFE && chars[1] == (char)0xFF)
            {
                Debug.Assert(count % 2 == 0);
                _token.Length = 0;
                for (int idx = 2; idx < count; idx += 2)
                    _token.Append((char)(chars[idx] * 256 + chars[idx + 1]));
                return _symbol = Symbol.UnicodeHexString;
            }
            return _symbol = Symbol.HexString;
        }

        internal char ScanNextChar(bool handleCRLF)
        {
            if (_pdfLength <= _idxChar)
            {
                _currChar = Chars.EOF;
                _nextChar = Chars.EOF;
            }
            else
            {
                _currChar = _nextChar;
                _nextChar = (char)_pdfSteam.ReadByte();
                _idxChar++;
                if (handleCRLF && _currChar == Chars.CR)
                {
                    if (_nextChar == Chars.LF)
                    {
                        _currChar = _nextChar;
                        _nextChar = (char)_pdfSteam.ReadByte();
                        _idxChar++;
                    }
                    else
                    {
                        _currChar = Chars.LF;
                    }
                }
            }
            return _currChar;
        }

        bool PeekReference()
        {
            int positon = Position;

            while (char.IsDigit(_currChar))
                ScanNextChar(true);

            if (_currChar != Chars.SP)
                goto False;

            while (_currChar == Chars.SP)
                ScanNextChar(true);

            if (!char.IsDigit(_currChar))
                goto False;

            while (char.IsDigit(_currChar))
                ScanNextChar(true);

            if (_currChar != Chars.SP)
                goto False;

            while (_currChar == Chars.SP)
                ScanNextChar(true);

            if (_currChar != 'R')
                goto False;

            Position = positon;
            return true;

            False:
            Position = positon;
            return false;
        }

        internal char AppendAndScanNextChar()
        {
            if (_currChar == Chars.EOF)
                ParserDiagnostics.ThrowParserException("Undetected EOF reached.");

            _token.Append(_currChar);
            return ScanNextChar(true);
        }

        public char MoveToNonWhiteSpace()
        {
            while (_currChar != Chars.EOF)
            {
                switch (_currChar)
                {
                    case Chars.NUL:
                    case Chars.HT:
                    case Chars.LF:
                    case Chars.FF:
                    case Chars.CR:
                    case Chars.SP:
                        ScanNextChar(true);
                        break;

                    case (char)11:
                    case (char)173:
                        ScanNextChar(true);
                        break;


                    default:
                        return _currChar;
                }
            }
            return _currChar;
        }

        public Symbol Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public string Token
        {
            get { return _token.ToString(); }
        }

        public bool TokenToBoolean
        {
            get
            {
                Debug.Assert(_token.ToString() == "true" || _token.ToString() == "false");
                return _token.ToString()[0] == 't';
            }
        }

        public int TokenToInteger
        {
            get
            {
                return int.Parse(_token.ToString(), CultureInfo.InvariantCulture);
            }
        }

        public uint TokenToUInteger
        {
            get
            {
                return uint.Parse(_token.ToString(), CultureInfo.InvariantCulture);
            }
        }

        public double TokenToReal
        {
            get { return double.Parse(_token.ToString(), CultureInfo.InvariantCulture); }
        }

        public PdfObjectID TokenToObjectID
        {
            get
            {
                string[] numbers = Token.Split('|');
                int objectNumber = Int32.Parse(numbers[0]);
                int generationNumber = Int32.Parse(numbers[1]);
                return new PdfObjectID(objectNumber, generationNumber);
            }
        }

        internal static bool IsWhiteSpace(char ch)
        {
            switch (ch)
            {
                case Chars.NUL:    
                case Chars.HT:      
                case Chars.LF:      
                case Chars.FF:      
                case Chars.CR:      
                case Chars.SP:     
                    return true;
            }
            return false;
        }

        internal static bool IsDelimiter(char ch)
        {
            switch (ch)
            {
                case '(':
                case ')':
                case '<':
                case '>':
                case '[':
                case ']':
                case '{':
                case '}':
                case '/':
                case '%':
                    return true;
            }
            return false;
        }

        public int PdfLength
        {
            get { return _pdfLength; }
        }

        readonly int _pdfLength;
        int _idxChar;
        char _currChar;
        char _nextChar;
        StringBuilder _token;
        Symbol _symbol = Symbol.None;

        readonly Stream _pdfSteam;
    }
}
