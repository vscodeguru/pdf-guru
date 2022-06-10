
using System.IO;
using PdfSharp.Internal;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Content.Objects;


namespace PdfSharp.Pdf.Content
{
    public sealed class CParser
    {
        public CParser(PdfPage page)
        {
            _page = page;
            PdfContent content = page.Contents.CreateSingleContent();
            byte[] bytes = content.Stream.Value;
            _lexer = new CLexer(bytes);
        }

        public CParser(byte[] content)
        {
            _lexer = new CLexer(content);
        }

        public CParser(MemoryStream content)
        {
            _lexer = new CLexer(content.ToArray());
        }


        public CParser(CLexer lexer)
        {
            _lexer = lexer;
        }

        public CSymbol Symbol
        {
            get { return _lexer.Symbol; }
        }

        public CSequence ReadContent()
        {
            CSequence sequence = new CSequence();
            ParseObject(sequence, CSymbol.Eof);

            return sequence;
        }

        void ParseObject(CSequence sequence, CSymbol stop)
        {
            CSymbol symbol;
            while ((symbol = ScanNextToken()) != CSymbol.Eof)
            {
                if (symbol == stop)
                    return;

                CString s;
                COperator op;
                switch (symbol)
                {
                    case CSymbol.Comment:
                        break;

                    case CSymbol.Integer:
                        CInteger n = new CInteger();
                        n.Value = _lexer.TokenToInteger;
                        _operands.Add(n);
                        break;

                    case CSymbol.Real:
                        CReal r = new CReal();
                        r.Value = _lexer.TokenToReal;
                        _operands.Add(r);
                        break;

                    case CSymbol.String:
                    case CSymbol.HexString:
                    case CSymbol.UnicodeString:
                    case CSymbol.UnicodeHexString:
                        s = new CString();
                        s.Value = _lexer.Token;
                        _operands.Add(s);
                        break;

                    case CSymbol.Dictionary:
                        s = new CString();
                        s.Value = _lexer.Token;
                        s.CStringType = CStringType.Dictionary;
                        _operands.Add(s);
                        op = CreateOperator(OpCodeName.Dictionary);
                        sequence.Add(op);

                        break;

                    case CSymbol.Name:
                        CName name = new CName();
                        name.Name = _lexer.Token;
                        _operands.Add(name);
                        break;

                    case CSymbol.Operator:
                        op = CreateOperator();
                        sequence.Add(op);
                        break;

                    case CSymbol.BeginArray:
                        CArray array = new CArray();
                        if (_operands.Count != 0)
                            ContentReaderDiagnostics.ThrowContentReaderException("Array within array...");

                        ParseObject(array, CSymbol.EndArray);
                        array.Add(_operands);
                        _operands.Clear();
                        _operands.Add((CObject)array);
                        break;

                    case CSymbol.EndArray:
                        ContentReaderDiagnostics.HandleUnexpectedCharacter(']');
                        break;

                }
            }
        }

        COperator CreateOperator()
        {
            string name = _lexer.Token;
            COperator op = OpCodes.OperatorFromName(name);
            return CreateOperator(op);
        }

        COperator CreateOperator(OpCodeName nameop)
        {
            string name = nameop.ToString();
            COperator op = OpCodes.OperatorFromName(name);
            return CreateOperator(op);
        }

        COperator CreateOperator(COperator op)
        {
            if (op.OpCode.OpCodeName == OpCodeName.BI)
            {
                _lexer.ScanInlineImage();
            }
            op.Operands.Add(_operands);
            _operands.Clear();
            return op;
        }

        CSymbol ScanNextToken()
        {
            return _lexer.ScanNextToken();
        }

        CSymbol ScanNextToken(out string token)
        {
            CSymbol symbol = _lexer.ScanNextToken();
            token = _lexer.Token;
            return symbol;
        }

        CSymbol ReadSymbol(CSymbol symbol)
        {
            CSymbol current = _lexer.ScanNextToken();
            if (symbol != current)
                ContentReaderDiagnostics.ThrowContentReaderException(PSSR.UnexpectedToken(_lexer.Token));
            return current;
        }

        readonly CSequence _operands = new CSequence();
        PdfPage _page;
        readonly CLexer _lexer;
    }
}
