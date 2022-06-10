using System;

namespace PdfSharp.Pdf.AcroForms
{
    public abstract class PdfChoiceField : PdfAcroField
    {
        protected PdfChoiceField(PdfDocument document)
            : base(document)
        { }

        protected PdfChoiceField(PdfDictionary dict)
            : base(dict)
        { }

        protected int IndexInOptArray(string value)
        {
            PdfArray opt = Elements.GetArray(Keys.Opt);

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
                    else if (item is PdfArray)
                    {
                        PdfArray array = (PdfArray)item;
                        if (array.Elements.Count != 0)
                        {
                            if (array.Elements[0].ToString() == value)
                                return idx;
                        }
                    }
                }
            }
            return -1;
        }

        protected string ValueInOptArray(int index)
        {
            PdfArray opt = Elements.GetArray(Keys.Opt);
            if (opt != null)
            {
                int count = opt.Elements.Count;
                if (index < 0 || index >= count)
                    throw new ArgumentOutOfRangeException("index");

                PdfItem item = opt.Elements[index];
                if (item is PdfString)
                    return item.ToString();

                if (item is PdfArray)
                {
                    PdfArray array = (PdfArray)item;
                    if (array.Elements.Count != 0)
                        return array.Elements[0].ToString();
                }
            }
            return "";
        }

        public new class Keys : PdfAcroField.Keys
        {
            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Opt = "/Opt";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string TI = "/TI";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string I = "/I";

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
