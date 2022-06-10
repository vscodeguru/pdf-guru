using System.Collections.Generic;

namespace PdfSharp.Pdf.Advanced
{
    internal class PdfResourceMap : PdfDictionary  
    {
        public PdfResourceMap()
        { }

        public PdfResourceMap(PdfDocument document)
            : base(document)
        { }

        protected PdfResourceMap(PdfDictionary dict)
            : base(dict)
        { }

        internal void CollectResourceNames(Dictionary<string, object> usedResourceNames)
        {
            PdfName[] names = Elements.KeyNames;
            foreach (PdfName name in names)
                usedResourceNames.Add(name.ToString(), null);
        }
    }
}
