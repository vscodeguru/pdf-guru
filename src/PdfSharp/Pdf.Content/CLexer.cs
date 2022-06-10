
using System;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using System.IO;
using PdfSharp.Internal;


namespace PdfSharp.Pdf.Content
{
    public class CLexer
    {
        public CLexer(byte[] content)
        {
            _content = content;
            _charIndex = 0;
        }

        public CLexer(MemoryStream content)
        {
            _content = content.ToArray();
            _charIndex = 0;
        }

        public CSymbol ScanNextToken()
        {
            Again:
            ClearToken();
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

                case '[':
                    ScanNextChar();
                    return _symbol = CSymbol.BeginArray;

                case ']':
                    ScanNextChar();
                    return _symbol = CSymbol.EndArray;

                case '(':
                    return _symbol = ScanLiteralString();

                case '<':
                    if (_nextChar == '<')
                        return _symbol = ScanDictionary();
                    return _symbol = ScanHexadecimalString();

                case '.':
                    return _symbol = ScanNumber();

                case '"':
                case '\'':
                    return _symbol = ScanOperator();
            }
            if (char.IsDigit(ch))
                return _symbol = ScanNumber();

            if (char.IsLetter(ch))
                return _symbol = ScanOperator();

            if (ch == Chars.EOF)
                return _symbol = CSymbol.Eof;

            ContentReaderDiagnostics.HandleUnexpectedCharacter(ch);
            return _symbol = CSymbol.None;
        }

        public CSymbol ScanComment()
        {
            Debug.Assert(_currChar == Chars.Percent);

            ClearToken();
            char ch;
            while ((ch = AppendAndScanNextChar()) != Chars.LF && ch != Chars.EOF) { }
            return _symbol = CSymbol.Comment;
        }

        public CSymbol ScanInlineImage()
        {
            bool ascii85 = false;
            do
            {
                ScanNextToken();
                if (!ascii85 && _symbol == CSymbol.Name && (Token == "/ASCII85Decode" || Token == "/A85"))
                    ascii85 = true;
            } while (_symbol != CSymbol.Operator || Token != "ID");

            if (ascii85)
            {
                while (_currChar != Chars.EOF && (_currChar != '~' || _nextChar != '>'))
                    ScanNextChar();
                if (_currChar == Chars.EOF)
                    ContentReaderDiagnostics.HandleUnexpectedCharacter(_currChar);
            }

            while (_currChar != Chars.EOF)
            {
                if (IsWhiteSpace(_currChar))
                {
                    if (ScanNextChar() == 'E')
                        if (ScanNextChar() == 'I')
                            if (IsWhiteSpace(ScanNextChar()))
                                break;
                }
                else
                    ScanNextChar();
            }
            if (_currChar == Chars.EOF)
                ContentReaderDiagnostics.HandleUnexpectedCharacter(_currChar);

            return CSymbol.None;
        }

        public CSymbol ScanName()
        {
            Debug.Assert(_currChar == Chars.Slash);

            ClearToken();
            while (true)
            {
                char ch = AppendAndScanNextChar();
                if (IsWhiteSpace(ch) || IsDelimiter(ch))
                    return _symbol = CSymbol.Name;

                if (ch == '#')
                {
                    ScanNextChar();
                    char[] hex = new char[2];
                    hex[0] = _currChar;
                    hex[1] = _nextChar;
                    ScanNextChar();
                    ch = (char)(ushort)int.Parse(new string(hex), NumberStyles.AllowHexSpecifier);
                    _currChar = ch;
                }
            }
        }

        protected CSymbol ScanDictionary()
        {
            ClearToken();
            _token.Append(_currChar);       
            _token.Append(ScanNextChar());  

            bool inString = false, inHexString = false;
            int nestedDict = 0, nestedStringParen = 0;
            char ch;
            while (true)
            {
                _token.Append(ch = ScanNextChar());
                if (ch == '<')
                {
                    if (_nextChar == '<')
                    {
                        _token.Append(ScanNextChar());
                        ++nestedDict;
                    }
                    else
                        inHexString = true;
                }
                else if (!inHexString && ch == '(')
                {
                    if (inString)
                        ++nestedStringParen;
                    else
                    {
                        inString = true;
                        nestedStringParen = 0;
                    }
                }
                else if (inString && ch == ')')
                {
                    if (nestedStringParen > 0)
                        --nestedStringParen;
                    else
                        inString = false;
                }
                else if (inString && ch == '\\')
                    _token.Append(ScanNextChar());
                else if (ch == '>')
                {
                    if (inHexString)
                        inHexString = false;
                    else if (_nextChar == '>')
                    {
                        _token.Append(ScanNextChar());
                        if (nestedDict > 0)
                            --nestedDict;
                        else
                        {
                            ScanNextChar();

#if true
                            return CSymbol.Dictionary;

#endif
                        }
                    }
                }
                else if (ch == Chars.EOF)
                    ContentReaderDiagnostics.HandleUnexpectedCharacter(ch);
            }
        }

        public CSymbol ScanNumber()
        {
            long value = 0;
            int decimalDigits = 0;
            bool period = false;
            bool negative = false;

            ClearToken();
            char ch = _currChar;
            if (ch == '+' || ch == '-')
            {
                if (ch == '-')
                    negative = true;
                _token.Append(ch);
                ch = ScanNextChar();
            }
            while (true)
            {
                if (char.IsDigit(ch))
                {
                    _token.Append(ch);
                    if (decimalDigits < 10)
                    {
                        value = 10 * value + ch - '0';
                        if (period)
                            decimalDigits++;
                    }
                }
                else if (ch == '.')
                {
                    if (period)
                        ContentReaderDiagnostics.ThrowContentReaderException("More than one period in number.");

                    period = true;
                    _token.Append(ch);
                }
                else
                    break;
                ch = ScanNextChar();
            }

            if (negative)
                value = -value;
            if (period)
            {
                if (decimalDigits > 0)
                {
                    _tokenAsReal = value / PowersOf10[decimalDigits];
                }
                else
                {
                    _tokenAsReal = value;
                    _tokenAsLong = value;
                }
                return CSymbol.Real;
            }
            _tokenAsLong = value;
            _tokenAsReal = Convert.ToDouble(value);

            Debug.Assert(Int64.Parse(_token.ToString(), CultureInfo.InvariantCulture) == value);

            if (value >= Int32.MinValue && value < Int32.MaxValue)
                return CSymbol.Integer;

            ContentReaderDiagnostics.ThrowNumberOutOfIntegerRange(value);
            return CSymbol.Error;
        }
        static readonly double[] PowersOf10 = { 1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000, 10000000000 };

        public CSymbol ScanOperator()
        {
            ClearToken();
            char ch = _currChar;
            while (IsOperatorChar(ch))
                ch = AppendAndScanNextChar();

            return _symbol = CSymbol.Operator;
        }

        public CSymbol ScanLiteralString()
        {
            Debug.Assert(_currChar == Chars.ParenLeft);

            ClearToken();
            int parenLevel = 0;
            char ch = ScanNextChar();
            if (ch == '\xFE' && _nextChar == '\xFF')
            {
                ScanNextChar();
                char chHi = ScanNextChar();
                if (chHi == ')')
                {
                    ScanNextChar();
                    return _symbol = CSymbol.String;
                }
                char chLo = ScanNextChar();
                ch = (char)(chHi * 256 + chLo);
                while (true)
                {
                    SkipChar:
                    switch (ch)
                    {
                        case '(':
                            parenLevel++;
                            break;

                        case ')':
                            if (parenLevel == 0)
                            {
                                ScanNextChar();
                                return _symbol = CSymbol.String;
                            }
                            parenLevel--;
                            break;

                        case '\\':
                            {
                                ch = ScanNextChar();
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

                                    case Chars.LF:
                                        ch = ScanNextChar();
                                        goto SkipChar;

                                    default:
                                        if (char.IsDigit(ch))
                                        {
                                            int n = ch - '0';
                                            if (char.IsDigit(_nextChar))
                                            {
                                                n = n * 8 + ScanNextChar() - '0';
                                                if (char.IsDigit(_nextChar))
                                                    n = n * 8 + ScanNextChar() - '0';
                                            }
                                            ch = (char)n;
                                        }
                                        break;
                                }
                                break;
                            }

                        default:
                            break;
                    }
                    _token.Append(ch);
                    chHi = ScanNextChar();
                    if (chHi == ')')
                    {
                        ScanNextChar();
                        return _symbol = CSymbol.String;
                    }
                    chLo = ScanNextChar();
                    ch = (char)(chHi * 256 + chLo);
                }
            }
            else
            {
                while (true)
                {
                    SkipChar:
                    switch (ch)
                    {
                        case '(':
                            parenLevel++;
                            break;

                        case ')':
                            if (parenLevel == 0)
                            {
                                ScanNextChar();
                                return _symbol = CSymbol.String;
                            }
                            parenLevel--;
                            break;

                        case '\\':
                            {
                                ch = ScanNextChar();
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

                                    case Chars.LF:
                                        ch = ScanNextChar();
                                        goto SkipChar;

                                    default:
                                        if (char.IsDigit(ch))
                                        {
                                            int n = ch - '0';
                                            if (char.IsDigit(_nextChar))
                                            {
                                                n = n * 8 + ScanNextChar() - '0';
                                                if (char.IsDigit(_nextChar))
                                                    n = n * 8 + ScanNextChar() - '0';
                                            }
                                            ch = (char)n;
                                        }
                                        break;
                                }
                                break;
                            }

                        default:
                            break;
                    }
                    _token.Append(ch);
                    ch = ScanNextChar();
                }
            }
        }

        public CSymbol ScanHexadecimalString()
        {
            Debug.Assert(_currChar == Chars.Less);

            ClearToken();
            char[] hex = new char[2];
            ScanNextChar();
            while (true)
            {
                MoveToNonWhiteSpace();
                if (_currChar == '>')
                {
                    ScanNextChar();
                    break;
                }
                if (char.IsLetterOrDigit(_currChar))
                {
                    hex[0] = char.ToUpper(_currChar);
                    hex[1] = char.ToUpper(_nextChar);
                    int ch = int.Parse(new string(hex), NumberStyles.AllowHexSpecifier);
                    _token.Append(Convert.ToChar(ch));
                    ScanNextChar();
                    ScanNextChar();
                }
            }
            string chars = _token.ToString();
            int count = chars.Length;
            if (count > 2 && chars[0] == (char)0xFE && chars[1] == (char)0xFF)
            {
                Debug.Assert(count % 2 == 0);
                _token.Length = 0;
                for (int idx = 2; idx < count; idx += 2)
                    _token.Append((char)(chars[idx] * 256 + chars[idx + 1]));
            }
            return _symbol = CSymbol.HexString;
        }

        internal char ScanNextChar()
        {
            if (ContLength <= _charIndex)
            {
                _currChar = Chars.EOF;
                if (IsOperatorChar(_nextChar))
                    _token.Append(_nextChar);
                _nextChar = Chars.EOF;
            }
            else
            {
                _currChar = _nextChar;
                _nextChar = (char)_content[_charIndex++];
                if (_currChar == Chars.CR)
                {
                    if (_nextChar == Chars.LF)
                    {
                        _currChar = _nextChar;
                        if (ContLength <= _charIndex)
                            _nextChar = Chars.EOF;
                        else
                            _nextChar = (char)_content[_charIndex++];
                    }
                    else
                    {
                        _currChar = Chars.LF;
                    }
                }
            }
            return _currChar;
        }

        void ClearToken()
        {
            _token.Length = 0;
            _tokenAsLong = 0;
            _tokenAsReal = 0;
        }

        internal char AppendAndScanNextChar()
        {
            _token.Append(_currChar);
            return ScanNextChar();
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
                        ScanNextChar();
                        break;

                    default:
                        return _currChar;
                }
            }
            return _currChar;
        }

        public CSymbol Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        public string Token
        {
            get { return _token.ToString(); }
        }

        internal int TokenToInteger
        {
            get
            {
                Debug.Assert(_tokenAsLong == int.Parse(_token.ToString(), CultureInfo.InvariantCulture));
                return (int)_tokenAsLong;
            }
        }

        internal double TokenToReal
        {
            get
            {
                Debug.Assert(_tokenAsReal == double.Parse(_token.ToString(), CultureInfo.InvariantCulture));
                return _tokenAsReal;
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

        internal static bool IsOperatorChar(char ch)
        {
            if (char.IsLetter(ch))
                return true;
            switch (ch)
            {
                case Chars.Asterisk:     
                case Chars.QuoteSingle:  
                case Chars.QuoteDbl:     
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
                case '/':
                case '%':
                    return true;
            }
            return false;
        }

        public int ContLength
        {
            get { return _content.Length; }
        }

        public int Position
        {
            get { return _charIndex; }
            set
            {
                _charIndex = value;
                _currChar = (char)_content[_charIndex - 1];
                _nextChar = (char)_content[_charIndex - 1];
            }
        }

        readonly byte[] _content;
        int _charIndex;
        char _currChar;
        char _nextChar;

        readonly StringBuilder _token = new StringBuilder();
        long _tokenAsLong;
        double _tokenAsReal;
        CSymbol _symbol = CSymbol.None;
    }
}
