namespace PdfSharp.Pdf
{
    public sealed class PdfDocumentSettings
    {
        internal PdfDocumentSettings(PdfDocument document)
        { }

        public TrimMargins TrimMargins
        {
            get
            {
                if (_trimMargins == null)
                    _trimMargins = new TrimMargins();
                return _trimMargins;
            }
            set
            {
                if (_trimMargins == null)
                    _trimMargins = new TrimMargins();
                if (value != null)
                {
                    _trimMargins.Left = value.Left;
                    _trimMargins.Right = value.Right;
                    _trimMargins.Top = value.Top;
                    _trimMargins.Bottom = value.Bottom;
                }
                else
                    _trimMargins.All = 0;
            }
        }
        TrimMargins _trimMargins = new TrimMargins();
    }
}