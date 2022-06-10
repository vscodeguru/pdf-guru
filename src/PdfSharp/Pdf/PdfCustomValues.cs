using System;

namespace PdfSharp.Pdf
{
    public class PdfCustomValues : PdfDictionary
    {
        internal PdfCustomValues()
        { }

        internal PdfCustomValues(PdfDocument document)
            : base(document)
        { }

        internal PdfCustomValues(PdfDictionary dict)
            : base(dict)
        { }

        public PdfCustomValueCompressionMode CompressionMode
        {
            set { throw new NotImplementedException(); }
        }

        public bool Contains(string key)
        {
            return Elements.ContainsKey(key);
        }

        public PdfCustomValue this[string key]
        {
            get
            {
                PdfDictionary dict = Elements.GetDictionary(key);
                if (dict == null)
                    return null;
                PdfCustomValue cust = dict as PdfCustomValue;
                if (cust == null)
                    cust = new PdfCustomValue(dict);
                return cust;
            }
            set
            {
                if (value == null)
                {
                    Elements.Remove(key);
                }
                else
                {
                    Owner.Internals.AddObject(value);
                    Elements.SetReference(key, value);
                }
            }
        }

        public static void ClearAllCustomValues(PdfDocument document)
        {
            document.CustomValues = null;
            foreach (PdfPage page in document.Pages)
                page.CustomValues = null;
        }

        internal static PdfCustomValues Get(DictionaryElements elem)
        {
            string key = elem.Owner.Owner.Internals.CustomValueKey;
            PdfCustomValues customValues;
            PdfDictionary dict = elem.GetDictionary(key);
            if (dict == null)
            {
                customValues = new PdfCustomValues();
                elem.Owner.Owner.Internals.AddObject(customValues);
                elem.Add(key, customValues);
            }
            else
            {
                customValues = dict as PdfCustomValues;
                if (customValues == null)
                    customValues = new PdfCustomValues(dict);
            }
            return customValues;
        }

        internal static void Remove(DictionaryElements elem)
        {
            elem.Remove(elem.Owner.Owner.Internals.CustomValueKey);
        }
    }
}
