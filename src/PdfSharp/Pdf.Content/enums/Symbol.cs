namespace PdfSharp.Pdf.Content
{
    public enum CSymbol
    {
        None,
        Comment,
        Integer,
        Real,
        String,
        HexString,
        UnicodeString,
        UnicodeHexString,
        Name,
        Operator,
        BeginArray,
        EndArray,
        Dictionary,           
        Eof,
        Error = -1,
    }
}
