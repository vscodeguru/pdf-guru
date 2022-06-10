using System;

namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfComboBoxField : PdfChoiceField
    {
        internal PdfComboBoxField(PdfDocument document)
            : base(document)
        { }

        internal PdfComboBoxField(PdfDictionary dict)
            : base(dict)
        { }

        public int SelectedIndex
        {
            get
            {
                string value = Elements.GetString(Keys.V);
                return IndexInOptArray(value);
            }
            set
            {
                if (value != -1) 
                {
                    string key = ValueInOptArray(value);
                    Elements.SetString(Keys.V, key);
                    Elements.SetInteger("/I", value);         
                }
            }
        }

        public override PdfItem Value 
        {
            get { return Elements[Keys.V]; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("The field is read only.");
                if (value is PdfString || value is PdfName)
                {
                    Elements[Keys.V] = value;
                    SelectedIndex = SelectedIndex;  
                    if (SelectedIndex == -1)
                    {
                        try
                        {
                            ((PdfArray)(((PdfItem[])(Elements.Values))[2])).Elements.Add(Value);
                            SelectedIndex = SelectedIndex;
                        }
                        catch { }
                    }
                }
                else
                    throw new NotImplementedException("Values other than string cannot be set.");
            }
        }

        public new class Keys : PdfAcroField.Keys
        {
            internal static DictionaryMeta Meta
            {
                get
                {
                    if (Keys._meta == null)
                        Keys._meta = CreateMeta(typeof(Keys));
                    return Keys._meta;
                }
            }
            static DictionaryMeta _meta;
        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
