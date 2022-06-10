namespace PdfSharp.Pdf
{
    public class PdfCustomValue : PdfDictionary
    {
        public PdfCustomValue()
        {
            CreateStream(new byte[] { });
        }

        public PdfCustomValue(byte[] bytes)
        {
            CreateStream(bytes);
        }

        internal PdfCustomValue(PdfDocument document)
            : base(document)
        {
            CreateStream(new byte[] { });
        }

        internal PdfCustomValue(PdfDictionary dict)
            : base(dict)
        {
        }

        public PdfCustomValueCompressionMode CompressionMode;

        public byte[] Value
        {
            get { return Stream.Value; }
            set { Stream.Value = value; }
        }
    }
}
