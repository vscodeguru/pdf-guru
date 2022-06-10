using System;
using System.Globalization;
namespace PdfSharp.Internal
{
    class TokenizerHelper
    {
        public TokenizerHelper(string str, IFormatProvider formatProvider)
        {
            char numericListSeparator = GetNumericListSeparator(formatProvider);
            Initialize(str, '\'', numericListSeparator);
        }

        public TokenizerHelper(string str, char quoteChar, char separator)
        {
            Initialize(str, quoteChar, separator);
        }

        void Initialize(string str, char quoteChar, char separator)
        {
            _str = str;
            _strLen = str == null ? 0 : str.Length;
            _currentTokenIndex = -1;
            _quoteChar = quoteChar;
            _argSeparator = separator;

            while (_charIndex < _strLen)
            {
                if (!char.IsWhiteSpace(_str, _charIndex))
                    return;
                _charIndex++;
            }
        }

        public string NextTokenRequired()
        {
            if (!NextToken(false))
                throw new InvalidOperationException("PrematureStringTermination");   
            return GetCurrentToken();
        }

        public string NextTokenRequired(bool allowQuotedToken)
        {
            if (!NextToken(allowQuotedToken))
                throw new InvalidOperationException("PrematureStringTermination");    
            return GetCurrentToken();
        }

        public string GetCurrentToken()
        {
            if (_currentTokenIndex < 0)
                return null;
            return _str.Substring(_currentTokenIndex, _currentTokenLength);
        }

        public void LastTokenRequired()
        {
            if (_charIndex != _strLen)
                throw new InvalidOperationException("Extra data encountered");   
        }

        public bool NextToken()
        {
            return NextToken(false);
        }

        public bool NextToken(bool allowQuotedToken)
        {
            return NextToken(allowQuotedToken, _argSeparator);
        }

        public bool NextToken(bool allowQuotedToken, char separator)
        {
            _currentTokenIndex = -1;
            _foundSeparator = false;

            if (_charIndex >= _strLen)
                return false;

            char currentChar = _str[_charIndex];

            int quoteCount = 0;

            if (allowQuotedToken &&
                currentChar == _quoteChar)
            {
                quoteCount++;
                _charIndex++;
            }

            int newTokenIndex = _charIndex;
            int newTokenLength = 0;

            while (_charIndex < _strLen)
            {
                currentChar = _str[_charIndex];

                if (quoteCount > 0)
                {
                    if (currentChar == _quoteChar)
                    {
                        quoteCount--;

                        if (quoteCount == 0)
                        {
                            ++_charIndex;
                            break;
                        }
                    }
                }
                else if ((char.IsWhiteSpace(currentChar)) || (currentChar == separator))
                {
                    if (currentChar == separator)
                        _foundSeparator = true;
                    break;
                }

                _charIndex++;
                newTokenLength++;
            }

            if (quoteCount > 0)
                throw new InvalidOperationException("Missing end quote");   

            ScanToNextToken(separator); 

            _currentTokenIndex = newTokenIndex;
            _currentTokenLength = newTokenLength;

            if (_currentTokenLength < 1)
                throw new InvalidOperationException("Empty token");    

            return true;
        }

        private void ScanToNextToken(char separator)
        {
            if (_charIndex < _strLen)
            {
                char currentChar = _str[_charIndex];

                if (currentChar != separator && !char.IsWhiteSpace(currentChar))
                    throw new InvalidOperationException("ExtraDataEncountered");   

                int argSepCount = 0;
                while (_charIndex < _strLen)
                {
                    currentChar = _str[_charIndex];
                    if (currentChar == separator)
                    {
                        _foundSeparator = true;
                        argSepCount++;
                        _charIndex++;

                        if (argSepCount > 1)
                            throw new InvalidOperationException("EmptyToken");   
                    }
                    else if (char.IsWhiteSpace(currentChar))
                    {
                        ++_charIndex;
                    }
                    else
                        break;
                }

                if (argSepCount > 0 && _charIndex >= _strLen)
                    throw new InvalidOperationException("EmptyToken");    
            }
        }

        public static char GetNumericListSeparator(IFormatProvider provider)
        {
            char numericSeparator = ',';
            NumberFormatInfo numberFormat = NumberFormatInfo.GetInstance(provider);
            if (numberFormat.NumberDecimalSeparator.Length > 0 && numericSeparator == numberFormat.NumberDecimalSeparator[0])
                numericSeparator = ';';
            return numericSeparator;
        }

        public bool FoundSeparator
        {
            get { return _foundSeparator; }
        }
        bool _foundSeparator;

        char _argSeparator;
        int _charIndex;
        int _currentTokenIndex;
        int _currentTokenLength;
        char _quoteChar;
        string _str;
        int _strLen;
    }
}