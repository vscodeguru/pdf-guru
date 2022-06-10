using System;

namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfRadioButtonField : PdfButtonField
    {
        internal PdfRadioButtonField(PdfDocument document)
            : base(document)
        {
            _document = document;
        }

        internal PdfRadioButtonField(PdfDictionary dict)
            : base(dict)
        { }

        public int SelectedIndex
        {
            get
            {
                string value = Elements.GetString(Keys.V);
                return IndexInOptStrings(value);
            }
            set
            {
                PdfArray opt = Elements[Keys.Opt] as PdfArray;

                if (opt == null)
                    opt = Elements[Keys.Kids] as PdfArray;

                if (opt != null)
                {
                    int count = opt.Elements.Count;
                    if (value < 0 || value >= count)
                        throw new ArgumentOutOfRangeException("value");
                    Elements.SetName(Keys.V, opt.Elements[value].ToString());
                }
            }
        }

        int IndexInOptStrings(string value)
        {
            PdfArray opt = Elements[Keys.Opt] as PdfArray;
            if (opt != null)
            {
                int count = opt.Elements.Count;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfItem item = opt.Elements[idx];
                    if (item is PdfString)
                    {
                        if (item.ToString() == value)
                            return idx;
                    }
                }
            }
            return -1;
        }

        public new class Keys : PdfButtonField.Keys
        {
            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Opt = "/Opt";

            internal static DictionaryMeta Meta
            {
                get { return _meta ?? (_meta = CreateMeta(typeof(Keys))); }
            }
            static DictionaryMeta _meta;
        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
