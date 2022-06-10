using System;

namespace PdfSharp.Pdf.Content
{
    public class ContentReaderException : PdfSharpException
    {
        public ContentReaderException()
        { }

        public ContentReaderException(string message)
            : base(message)
        { }

        public ContentReaderException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
