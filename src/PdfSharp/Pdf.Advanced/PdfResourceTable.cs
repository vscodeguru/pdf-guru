using System;

namespace PdfSharp.Pdf.Advanced
{
    public class PdfResourceTable
    {
        public PdfResourceTable(PdfDocument owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            _owner = owner;
        }

        protected PdfDocument Owner
        {
            get { return _owner; }
        }
        readonly PdfDocument _owner;
    }
}
