using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using PdfSharp.Internal;
using PdfSharp.Pdf.Advanced;

namespace PdfSharp.Pdf.IO
{
    internal sealed class Parser
    {
        public Parser(PdfDocument document, Stream pdf)
        {
            _document = document;
            _lexer = new Lexer(pdf);
            _stack = new ShiftStack();
        }

        public Parser(PdfDocument document)
        {
            _document = document;
            _lexer = document._lexer;
            _stack = new ShiftStack();
        }

        public int MoveToObject(PdfObjectID objectID)
        {
            int position = _document._irefTable[objectID].Position;
            return _lexer.Position = position;
        }

        public Symbol Symbol
        {
            get { return _lexer.Symbol; }
        }

        public PdfObjectID ReadObjectNumber(int position)
        {
            _lexer.Position = position;
            int objectNumber = ReadInteger();
            int generationNumber = ReadInteger();
#if DEBUG && CORE
#endif
            return new PdfObjectID(objectNumber, generationNumber);
        }


        public PdfObject ReadObject(PdfObject pdfObject, PdfObjectID objectID, bool includeReferences, bool fromObjecStream)
        {
#if DEBUG_
#endif
            int objectNumber = objectID.ObjectNumber;
            int generationNumber = objectID.GenerationNumber;
            if (!fromObjecStream)
            {
                MoveToObject(objectID);
                objectNumber = ReadInteger();
                generationNumber = ReadInteger();
            }

            objectNumber = objectID.ObjectNumber;
            generationNumber = objectID.GenerationNumber;

            if (!fromObjecStream)
                ReadSymbol(Symbol.Obj);

            bool checkForStream = false;
            Symbol symbol = ScanNextToken();
            switch (symbol)
            {
                case Symbol.BeginArray:
                    PdfArray array;
                    if (pdfObject == null)
                        array = new PdfArray(_document);
                    else
                        array = (PdfArray)pdfObject;
                    pdfObject = ReadArray(array, includeReferences);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    break;

                case Symbol.BeginDictionary:
                    PdfDictionary dict;
                    if (pdfObject == null)
                        dict = new PdfDictionary(_document);
                    else
                        dict = (PdfDictionary)pdfObject;
                    checkForStream = true;
                    pdfObject = ReadDictionary(dict, includeReferences);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    break;

                case Symbol.Null:
                    pdfObject = new PdfNullObject(_document);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    if (!fromObjecStream)
                        ReadSymbol(Symbol.EndObj);
                    return pdfObject;

                case Symbol.EndObj:
                    pdfObject = new PdfNullObject(_document);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    return pdfObject;

                case Symbol.Boolean:
                    pdfObject = new PdfBooleanObject(_document, String.Compare(_lexer.Token, Boolean.TrueString, StringComparison.OrdinalIgnoreCase) == 0);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    if (!fromObjecStream)
                        ReadSymbol(Symbol.EndObj);
                    return pdfObject;

                case Symbol.Integer:
                    pdfObject = new PdfIntegerObject(_document, _lexer.TokenToInteger);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    if (!fromObjecStream)
                        ReadSymbol(Symbol.EndObj);
                    return pdfObject;

                case Symbol.UInteger:
                    pdfObject = new PdfUIntegerObject(_document, _lexer.TokenToUInteger);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    if (!fromObjecStream)
                        ReadSymbol(Symbol.EndObj);
                    return pdfObject;

                case Symbol.Real:
                    pdfObject = new PdfRealObject(_document, _lexer.TokenToReal);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    if (!fromObjecStream)
                        ReadSymbol(Symbol.EndObj);
                    return pdfObject;

                case Symbol.String:
                case Symbol.UnicodeString:
                case Symbol.HexString:
                case Symbol.UnicodeHexString:
                    pdfObject = new PdfStringObject(_document, _lexer.Token);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    if (!fromObjecStream)
                        ReadSymbol(Symbol.EndObj);
                    return pdfObject;

                case Symbol.Name:
                    pdfObject = new PdfNameObject(_document, _lexer.Token);
                    pdfObject.SetObjectID(objectNumber, generationNumber);
                    if (!fromObjecStream)
                        ReadSymbol(Symbol.EndObj);
                    return pdfObject;

                case Symbol.Keyword:
                    ParserDiagnostics.HandleUnexpectedToken(_lexer.Token);
                    break;

                default:
                    ParserDiagnostics.HandleUnexpectedToken(_lexer.Token);
                    break;
            }
            symbol = ScanNextToken();
            if (symbol == Symbol.BeginStream)
            {
                PdfDictionary dict = (PdfDictionary)pdfObject;
                Debug.Assert(checkForStream, "Unexpected stream...");
#if true_
#else
                int length = GetStreamLength(dict);
                byte[] bytes = _lexer.ReadStream(length);

                PdfDictionary.PdfStream stream = new PdfDictionary.PdfStream(bytes, dict);
                dict.Stream = stream;
                ReadSymbol(Symbol.EndStream);
                symbol = ScanNextToken();
#endif
            }
            if (!fromObjecStream && symbol != Symbol.EndObj)
                ParserDiagnostics.ThrowParserException(PSSR.UnexpectedToken(_lexer.Token));
            return pdfObject;
        }

        private void ReadStream(PdfDictionary dict)
        {
            Symbol symbol = _lexer.Symbol;
            Debug.Assert(symbol == Symbol.BeginStream);
            int length = GetStreamLength(dict);
            byte[] bytes = _lexer.ReadStream(length);
            PdfDictionary.PdfStream stream = new PdfDictionary.PdfStream(bytes, dict);
            Debug.Assert(dict.Stream == null, "Dictionary already has a stream.");
            dict.Stream = stream;
            ReadSymbol(Symbol.EndStream);
            ScanNextToken();
        }

        private int GetStreamLength(PdfDictionary dict)
        {
            if (dict.Elements["/F"] != null)
                throw new NotImplementedException("File streams are not yet implemented.");

            PdfItem value = dict.Elements["/Length"];
            if (value is PdfInteger)
                return Convert.ToInt32(value);

            PdfReference reference = value as PdfReference;
            if (reference != null)
            {
                ParserState state = SaveState();
                object length = ReadObject(null, reference.ObjectID, false, false);
                RestoreState(state);
                int len = ((PdfIntegerObject)length).Value;
                dict.Elements["/Length"] = new PdfInteger(len);
                return len;
            }
            throw new InvalidOperationException("Cannot retrieve stream length.");
        }

        public PdfArray ReadArray(PdfArray array, bool includeReferences)
        {
            Debug.Assert(Symbol == Symbol.BeginArray);

            if (array == null)
                array = new PdfArray(_document);

            int sp = _stack.SP;
            ParseObject(Symbol.EndArray);
            int count = _stack.SP - sp;
            PdfItem[] items = _stack.ToArray(sp, count);
            _stack.Reduce(count);
            for (int idx = 0; idx < count; idx++)
            {
                PdfItem val = items[idx];
                if (includeReferences && val is PdfReference)
                    val = ReadReference((PdfReference)val, true);
                array.Elements.Add(val);
            }
            return array;
        }

        internal PdfDictionary ReadDictionary(PdfDictionary dict, bool includeReferences)
        {
            Debug.Assert(Symbol == Symbol.BeginDictionary);


            if (dict == null)
                dict = new PdfDictionary(_document);
            DictionaryMeta meta = dict.Meta;

            int sp = _stack.SP;
            ParseObject(Symbol.EndDictionary);
            int count = _stack.SP - sp;
            Debug.Assert(count % 2 == 0);
            PdfItem[] items = _stack.ToArray(sp, count);
            _stack.Reduce(count);
            for (int idx = 0; idx < count; idx += 2)
            {
                PdfItem val = items[idx];
                if (!(val is PdfName))
                    ParserDiagnostics.ThrowParserException("Name expected.");     

                string key = val.ToString();
                val = items[idx + 1];
                if (includeReferences && val is PdfReference)
                    val = ReadReference((PdfReference)val, true);
                dict.Elements[key] = val;
            }
            return dict;
        }


        private void ParseObject(Symbol stop)
        {
            Symbol symbol;
            while ((symbol = ScanNextToken()) != Symbol.Eof)
            {
                if (symbol == stop)
                    return;

                switch (symbol)
                {
                    case Symbol.Comment:
                        break;

                    case Symbol.Null:
                        _stack.Shift(PdfNull.Value);
                        break;

                    case Symbol.Boolean:
                        _stack.Shift(new PdfBoolean(_lexer.TokenToBoolean));
                        break;

                    case Symbol.Integer:
                        _stack.Shift(new PdfInteger(_lexer.TokenToInteger));
                        break;

                    case Symbol.UInteger:
                        _stack.Shift(new PdfUInteger(_lexer.TokenToUInteger));
                        break;

                    case Symbol.Real:
                        _stack.Shift(new PdfReal(_lexer.TokenToReal));
                        break;

                    case Symbol.String:
                        _stack.Shift(new PdfString(_lexer.Token, PdfStringFlags.RawEncoding));
                        break;

                    case Symbol.UnicodeString:
                        _stack.Shift(new PdfString(_lexer.Token, PdfStringFlags.Unicode));
                        break;

                    case Symbol.HexString:
                        _stack.Shift(new PdfString(_lexer.Token, PdfStringFlags.HexLiteral));
                        break;

                    case Symbol.UnicodeHexString:
                        _stack.Shift(new PdfString(_lexer.Token, PdfStringFlags.Unicode | PdfStringFlags.HexLiteral));
                        break;

                    case Symbol.Name:
                        _stack.Shift(new PdfName(_lexer.Token));
                        break;

                    case Symbol.R:
                        {
                            Debug.Assert(_stack.GetItem(-1) is PdfInteger && _stack.GetItem(-2) is PdfInteger);
                            PdfObjectID objectID = new PdfObjectID(_stack.GetInteger(-2), _stack.GetInteger(-1));

                            PdfReference iref = _document._irefTable[objectID];
                            if (iref == null)
                            {
                                if (_document._irefTable.IsUnderConstruction)
                                {
                                    iref = new PdfReference(objectID, 0);
                                    _stack.Reduce(iref, 2);
                                    break;
                                }
                                _stack.Reduce(PdfNull.Value, 2);
                            }
                            else
                                _stack.Reduce(iref, 2);
                            break;
                        }

                    case Symbol.BeginArray:
                        PdfArray array = new PdfArray(_document);
                        ReadArray(array, false);
                        _stack.Shift(array);
                        break;

                    case Symbol.BeginDictionary:
                        PdfDictionary dict = new PdfDictionary(_document);
                        ReadDictionary(dict, false);
                        _stack.Shift(dict);
                        break;

                    case Symbol.BeginStream:
                        throw new NotImplementedException();

                    default:
                        ParserDiagnostics.HandleUnexpectedToken(_lexer.Token);
                        SkipCharsUntil(stop);
                        return;
                }
            }
            ParserDiagnostics.ThrowParserException("Unexpected end of file.");     
        }

        private Symbol ScanNextToken()
        {
            return _lexer.ScanNextToken();
        }

        private Symbol ScanNextToken(out string token)
        {
            Symbol symbol = _lexer.ScanNextToken();
            token = _lexer.Token;
            return symbol;
        }

        private Symbol SkipCharsUntil(Symbol stop)
        {
            Symbol symbol;
            switch (stop)
            {
                case Symbol.EndDictionary:
                    return SkipCharsUntil(">>", stop);

                default:
                    do
                    {
                        symbol = ScanNextToken();
                    } while (symbol != stop && symbol != Symbol.Eof);
                    return symbol;
            }
        }

        private Symbol SkipCharsUntil(string text, Symbol stop)
        {
            int length = text.Length;
            int idx = 0;
            char ch;
            while ((ch = _lexer.ScanNextChar(true)) != Chars.EOF)
            {
                if (ch == text[idx])
                {
                    if (idx + 1 == length)
                    {
                        _lexer.ScanNextChar(true);
                        return stop;
                    }
                    idx++;
                }
                else
                    idx = 0;
            }
            return Symbol.Eof;
        }

        private void ReadObjectID(PdfObject obj)
        {
            int objectNubmer = ReadInteger();
            int generationNumber = ReadInteger();
            ReadSymbol(Symbol.Obj);
            if (obj != null)
                obj.SetObjectID(objectNubmer, generationNumber);
        }

        private PdfItem ReadReference(PdfReference iref, bool includeReferences)
        {
            throw new NotImplementedException("ReadReference");
        }

        private Symbol ReadSymbol(Symbol symbol)
        {
            if (symbol == Symbol.EndStream)
            {
                Skip:
                char ch = _lexer.MoveToNonWhiteSpace();

                if (ch == Chars.EOF)
                    ParserDiagnostics.HandleUnexpectedCharacter(ch);

                if (ch != 'e')
                {
                    _lexer.ScanNextChar(false);
                    goto Skip;
                }
            }
            Symbol current = _lexer.ScanNextToken();
            if (symbol != current)
                ParserDiagnostics.HandleUnexpectedToken(_lexer.Token);
            return current;
        }

        private Symbol ReadToken(string token)
        {
            Symbol current = _lexer.ScanNextToken();
            if (token != _lexer.Token)
                ParserDiagnostics.HandleUnexpectedToken(_lexer.Token);
            return current;
        }

        private string ReadName()
        {
            string name;
            Symbol symbol = ScanNextToken(out name);
            if (symbol != Symbol.Name)
                ParserDiagnostics.HandleUnexpectedToken(name);
            return name;
        }

        private int ReadInteger(bool canBeIndirect)
        {
            Symbol symbol = _lexer.ScanNextToken();
            if (symbol == Symbol.Integer)
                return _lexer.TokenToInteger;

            if (symbol == Symbol.R)
            {
                int position = _lexer.Position;
                ReadObjectID(null);
                int n = ReadInteger();
                ReadSymbol(Symbol.EndObj);
                _lexer.Position = position;
                return n;
            }
            ParserDiagnostics.HandleUnexpectedToken(_lexer.Token);
            return 0;
        }

        private int ReadInteger()
        {
            return ReadInteger(false);
        }

        public static PdfObject ReadObject(PdfDocument owner, PdfObjectID objectID)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");

            Parser parser = new Parser(owner);
            return parser.ReadObject(null, objectID, false, false);
        }

        internal void ReadIRefsFromCompressedObject(PdfObjectID objectID)
        {
            PdfReference iref;

            Debug.Assert(_document._irefTable.ObjectTable.ContainsKey(objectID));
            if (!_document._irefTable.ObjectTable.TryGetValue(objectID, out iref))
            {
                throw new NotImplementedException("This case is not coded or something else went wrong");
            }

            if (iref.Value == null)
            {
                try
                {
                    Debug.Assert(_document._irefTable.Contains(iref.ObjectID));
                    PdfDictionary pdfObject = (PdfDictionary)ReadObject(null, iref.ObjectID, false, false);
                    PdfObjectStream objectStream = new PdfObjectStream(pdfObject);
                    Debug.Assert(objectStream.Reference == iref);
                    Debug.Assert(objectStream.Reference.Value != null, "Something went wrong.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
            Debug.Assert(iref.Value != null);

            PdfObjectStream objectStreamStream = iref.Value as PdfObjectStream;
            if (objectStreamStream == null)
            {
                Debug.Assert(((PdfDictionary)iref.Value).Elements.GetName("/Type") == "/ObjStm");

                objectStreamStream = new PdfObjectStream((PdfDictionary)iref.Value);
                Debug.Assert(objectStreamStream.Reference == iref);
                Debug.Assert(objectStreamStream.Reference.Value != null, "Something went wrong.");
            }
            Debug.Assert(objectStreamStream != null);


            if (objectStreamStream == null)
                throw new Exception("Something went wrong here.");
            objectStreamStream.ReadReferences(_document._irefTable);
        }

        internal PdfReference ReadCompressedObject(PdfObjectID objectID, int index)
        {
            PdfReference iref;
#if true
            Debug.Assert(_document._irefTable.ObjectTable.ContainsKey(objectID));
            if (!_document._irefTable.ObjectTable.TryGetValue(objectID, out iref))
            {
                throw new NotImplementedException("This case is not coded or something else went wrong");
            }

#endif

            if (iref.Value == null)
            {
                try
                {
                    Debug.Assert(_document._irefTable.Contains(iref.ObjectID));
                    PdfDictionary pdfObject = (PdfDictionary)ReadObject(null, iref.ObjectID, false, false);
                    PdfObjectStream objectStream = new PdfObjectStream(pdfObject);
                    Debug.Assert(objectStream.Reference == iref);
                    Debug.Assert(objectStream.Reference.Value != null, "Something went wrong.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
            Debug.Assert(iref.Value != null);

            PdfObjectStream objectStreamStream = iref.Value as PdfObjectStream;
            if (objectStreamStream == null)
            {
                Debug.Assert(((PdfDictionary)iref.Value).Elements.GetName("/Type") == "/ObjStm");

                objectStreamStream = new PdfObjectStream((PdfDictionary)iref.Value);
                Debug.Assert(objectStreamStream.Reference == iref);
                Debug.Assert(objectStreamStream.Reference.Value != null, "Something went wrong.");
            }
            Debug.Assert(objectStreamStream != null);


            if (objectStreamStream == null)
                throw new Exception("Something went wrong here.");
            return objectStreamStream.ReadCompressedObject(index);
        }

        internal PdfReference ReadCompressedObject(int objectNumber, int offset)
        {

            PdfObjectID objectID = new PdfObjectID(objectNumber);
            _lexer.Position = offset;
            PdfObject obj = ReadObject(null, objectID, false, true);
            return obj.Reference;
        }

        internal int[][] ReadObjectStreamHeader(int n, int first)
        {
            int[][] header = new int[n][];
            for (int idx = 0; idx < n; idx++)
            {
                int number = ReadInteger();

                int offset = ReadInteger() + first;     
                header[idx] = new int[] { number, offset };
            }
            return header;
        }

        internal PdfTrailer ReadTrailer()
        {
            int length = _lexer.PdfLength;

            int idx;
            if (length < 1030)
            {
                string trail = _lexer.ReadRawString(length - 31, 30);   
                idx = trail.LastIndexOf("startxref", StringComparison.Ordinal);
                _lexer.Position = length - 31 + idx;
            }
            else
            {
                string trail = _lexer.ReadRawString(length - 1031, 1030);
                idx = trail.LastIndexOf("startxref", StringComparison.Ordinal);
                _lexer.Position = length - 1031 + idx;
            }

            if (idx == -1)
            {
                string trail = _lexer.ReadRawString(0, length);
                idx = trail.LastIndexOf("startxref", StringComparison.Ordinal);
                _lexer.Position = idx;
            }
            if (idx == -1)
                throw new Exception("The StartXRef table could not be found, the file cannot be opened.");

            ReadSymbol(Symbol.StartXRef);
            _lexer.Position = ReadInteger();

            while (true)
            {
                PdfTrailer trailer = ReadXRefTableAndTrailer(_document._irefTable);
                if (_document._trailer == null)
                    _document._trailer = trailer;
                int prev = trailer != null ? trailer.Elements.GetInteger(PdfTrailer.Keys.Prev) : 0;
                if (prev == 0)
                    break;
                _lexer.Position = prev;
            }

            return _document._trailer;
        }

        private PdfTrailer ReadXRefTableAndTrailer(PdfCrossReferenceTable xrefTable)
        {
            Debug.Assert(xrefTable != null);

            Symbol symbol = ScanNextToken();

            if (symbol == Symbol.XRef)       
            {
                while (true)
                {
                    symbol = ScanNextToken();
                    if (symbol == Symbol.Integer)
                    {
                        int start = _lexer.TokenToInteger;
                        int length = ReadInteger();
                        for (int id = start; id < start + length; id++)
                        {
                            int position = ReadInteger();
                            int generation = ReadInteger();
                            ReadSymbol(Symbol.Keyword);
                            string token = _lexer.Token;
                            if (id == 0)
                                continue;
                            if (token != "n")
                                continue;
#if true
                            int idToUse = id;
                            int idChecked, generationChecked;
                            if (!CheckXRefTableEntry(position, id, generation, out idChecked, out generationChecked))
                            {
                                if (generation == generationChecked && id == idChecked + 1)
                                    idToUse = idChecked;
                                else
                                    ParserDiagnostics.ThrowParserException("Invalid entry in XRef table, ID=" + id + ", Generation=" + generation + ", Position=" + position + ", ID of referenced object=" + idChecked + ", Generation of referenced object=" + generationChecked);      
                            }
#endif
                            PdfObjectID objectID = new PdfObjectID(idToUse, generation);
                            if (xrefTable.Contains(objectID))
                                continue;
                            xrefTable.Add(new PdfReference(objectID, position));
                        }
                    }
                    else if (symbol == Symbol.Trailer)
                    {
                        ReadSymbol(Symbol.BeginDictionary);
                        PdfTrailer trailer = new PdfTrailer(_document);
                        ReadDictionary(trailer, false);
                        return trailer;
                    }
                    else
                        ParserDiagnostics.HandleUnexpectedToken(_lexer.Token);
                }
            }
            else if (symbol == Symbol.Integer)      
            {
                return ReadXRefStream(xrefTable);
            }
            return null;
        }

        private bool CheckXRefTableEntry(int position, int id, int generation, out int idChecked, out int generationChecked)
        {
            int origin = _lexer.Position;
            idChecked = -1;
            generationChecked = -1;
            try
            {
                _lexer.Position = position;
                idChecked = ReadInteger();
                generationChecked = ReadInteger();
                Symbol symbol = _lexer.ScanNextToken();
                if (symbol != Symbol.Obj)
                    ParserDiagnostics.ThrowParserException("Invalid entry in XRef table, ID=" + id + ", Generation=" + generation + ", Position=" + position);     

                if (id != idChecked || generation != generationChecked)
                    return false;
            }
            catch (PdfReaderException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ParserDiagnostics.ThrowParserException("Invalid entry in XRef table, ID=" + id + ", Generation=" + generation + ", Position=" + position, ex);     
            }
            finally
            {
                _lexer.Position = origin;
            }
            return true;
        }

        private PdfTrailer ReadXRefStream(PdfCrossReferenceTable xrefTable)
        {
            int number = _lexer.TokenToInteger;
            int generation = ReadInteger();
            Debug.Assert(generation == 0);

            ReadSymbol(Symbol.Obj);
            ReadSymbol(Symbol.BeginDictionary);
            PdfObjectID objectID = new PdfObjectID(number, generation);

            PdfCrossReferenceStream xrefStream = new PdfCrossReferenceStream(_document);

            ReadDictionary(xrefStream, false);
            ReadSymbol(Symbol.BeginStream);
            ReadStream(xrefStream);

            PdfReference iref = new PdfReference(xrefStream);
            iref.ObjectID = objectID;
            iref.Value = xrefStream;
            xrefTable.Add(iref);

            Debug.Assert(xrefStream.Stream != null);
            byte[] bytesRaw = xrefStream.Stream.UnfilteredValue;
            byte[] bytes = bytesRaw;

            if (xrefStream.Stream.HasDecodeParams)
            {
                int predictor = xrefStream.Stream.DecodePredictor;
                int columns = xrefStream.Stream.DecodeColumns;
                bytes = DecodeCrossReferenceStream(bytesRaw, columns, predictor);
            }


            int size = xrefStream.Elements.GetInteger(PdfCrossReferenceStream.Keys.Size);
            PdfArray index = xrefStream.Elements.GetValue(PdfCrossReferenceStream.Keys.Index) as PdfArray;
            int prev = xrefStream.Elements.GetInteger(PdfCrossReferenceStream.Keys.Prev);
            PdfArray w = (PdfArray)xrefStream.Elements.GetValue(PdfCrossReferenceStream.Keys.W);

            int subsectionCount;
            int[][] subsections = null;
            int subsectionEntryCount = 0;
            if (index == null)
            {
                subsectionCount = 1;
                subsections = new int[subsectionCount][];
                subsections[0] = new int[] { 0, size };         
                subsectionEntryCount = size;
            }
            else
            {
                Debug.Assert(index.Elements.Count % 2 == 0);
                subsectionCount = index.Elements.Count / 2;
                subsections = new int[subsectionCount][];
                for (int idx = 0; idx < subsectionCount; idx++)
                {
                    subsections[idx] = new int[] { index.Elements.GetInteger(2 * idx), index.Elements.GetInteger(2 * idx + 1) };
                    subsectionEntryCount += subsections[idx][1];
                }
            }

            Debug.Assert(w.Elements.Count == 3);
            int[] wsize = { w.Elements.GetInteger(0), w.Elements.GetInteger(1), w.Elements.GetInteger(2) };
            int wsum = StreamHelper.WSize(wsize);
            if (wsum * subsectionEntryCount != bytes.Length)
                GetType();
            Debug.Assert(wsum * subsectionEntryCount == bytes.Length, "Check implementation here.");
            int testcount = subsections[0][1];
            int[] currentSubsection = subsections[0];

            int index2 = -1;
            for (int ssc = 0; ssc < subsectionCount; ssc++)
            {
                int abc = subsections[ssc][1];
                for (int idx = 0; idx < abc; idx++)
                {
                    index2++;

                    PdfCrossReferenceStream.CrossReferenceStreamEntry item =
                        new PdfCrossReferenceStream.CrossReferenceStreamEntry();

                    item.Type = StreamHelper.ReadBytes(bytes, index2 * wsum, wsize[0]);
                    item.Field2 = StreamHelper.ReadBytes(bytes, index2 * wsum + wsize[0], wsize[1]);
                    item.Field3 = StreamHelper.ReadBytes(bytes, index2 * wsum + wsize[0] + wsize[1], wsize[2]);

                    xrefStream.Entries.Add(item);

                    switch (item.Type)
                    {
                        case 0:
                            break;

                        case 1:     
                            int position = (int)item.Field2;
                            objectID = ReadObjectNumber(position);

                            Debug.Assert(objectID.GenerationNumber == item.Field3);

                            if (!xrefTable.Contains(objectID))
                            {

                                xrefTable.Add(new PdfReference(objectID, position));

                            }
                            break;

                        case 2:
                            break;
                    }
                }
            }
            return xrefStream;
        }

        internal static DateTime ParseDateTime(string date, DateTime errorValue)    
        {
            DateTime datetime = errorValue;
            try
            {
                if (date.StartsWith("D:"))
                {
                    int length = date.Length;
                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, hh = 0, mm = 0;
                    char o = 'Z';
                    if (length >= 10)
                    {
                        year = Int32.Parse(date.Substring(2, 4));
                        month = Int32.Parse(date.Substring(6, 2));
                        day = Int32.Parse(date.Substring(8, 2));
                        if (length >= 16)
                        {
                            hour = Int32.Parse(date.Substring(10, 2));
                            minute = Int32.Parse(date.Substring(12, 2));
                            second = Int32.Parse(date.Substring(14, 2));
                            if (length >= 23)
                            {
                                if ((o = date[16]) != 'Z')
                                {
                                    hh = Int32.Parse(date.Substring(17, 2));
                                    mm = Int32.Parse(date.Substring(20, 2));
                                }
                            }
                        }
                    }
                    month = Math.Min(Math.Max(month, 1), 12);
                    datetime = new DateTime(year, month, day, hour, minute, second);
                    if (o != 'Z')
                    {
                        TimeSpan ts = new TimeSpan(hh, mm, 0);
                        if (o == '-')
                            datetime = datetime.Add(ts);
                        else
                            datetime = datetime.Subtract(ts);
                    }
                    DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
                }
                else
                {
                    datetime = DateTime.Parse(date, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
            return datetime;
        }

        private ParserState SaveState()
        {
            ParserState state = new ParserState();
            state.Position = _lexer.Position;
            state.Symbol = _lexer.Symbol;
            return state;
        }

        private void RestoreState(ParserState state)
        {
            _lexer.Position = state.Position;
            _lexer.Symbol = state.Symbol;
        }

        private class ParserState
        {
            public int Position;
            public Symbol Symbol;
        }

        byte[] DecodeCrossReferenceStream(byte[] bytes, int columns, int predictor)
        {
            int size = bytes.Length;
            if (predictor < 10 || predictor > 15)
                throw new ArgumentException("Invalid predictor.", "predictor");

            int rowSizeRaw = columns + 1;

            if (size % rowSizeRaw != 0)
                throw new ArgumentException("Columns and size of array do not match.");

            int rows = size / rowSizeRaw;

            byte[] result = new byte[rows * columns];

            for (int row = 0; row < rows; ++row)
            {
                if (bytes[row * rowSizeRaw] != 2)
                    throw new ArgumentException("Invalid predictor in array.");

                for (int col = 0; col < columns; ++col)
                {
                    if (row == 0)
                        result[row * columns + col] = bytes[row * rowSizeRaw + col + 1];
                    else
                    {
                        result[row * columns + col] = (byte)(result[row * columns - columns + col] + bytes[row * rowSizeRaw + col + 1]);
                    }
                }
            }
            return result;
        }

        private readonly PdfDocument _document;
        private readonly Lexer _lexer;
        private readonly ShiftStack _stack;
    }

    static class StreamHelper
    {
        public static int WSize(int[] w)
        {
            Debug.Assert(w.Length == 3);
            return w[0] + w[1] + w[2];
        }

        public static uint ReadBytes(byte[] bytes, int index, int byteCount)
        {
            uint value = 0;
            for (int idx = 0; idx < byteCount; idx++)
            {
                value *= 256;
                value += bytes[index + idx];
            }
            return value;
        }
    }
}