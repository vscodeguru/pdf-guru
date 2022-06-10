using System;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    public abstract class PdfItem : ICloneable
    {
        object ICloneable.Clone()
        {
            return Copy();
        }

        public PdfItem Clone()
        {
            return (PdfItem)Copy();
        }

        protected virtual object Copy()
        {
            return MemberwiseClone();
        }

        internal abstract void WriteObject(PdfWriter writer);
    }
}
