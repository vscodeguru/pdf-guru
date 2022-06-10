using System;
using System.Collections.Generic;
using System.Diagnostics;
using PdfSharp.Pdf.Advanced;

namespace PdfSharp.Pdf.AcroForms
{
    public abstract class PdfAcroField : PdfDictionary
    {
        internal PdfAcroField(PdfDocument document)
            : base(document)
        { }

        protected PdfAcroField(PdfDictionary dict)
            : base(dict)
        { }

        public string Name
        {
            get
            {
                string name = Elements.GetString(Keys.T);
                return name;
            }
        }

        public PdfAcroFieldFlags Flags
        {
            get { return (PdfAcroFieldFlags)Elements.GetInteger(Keys.Ff); }
        }

        internal PdfAcroFieldFlags SetFlags
        {
            get { return (PdfAcroFieldFlags)Elements.GetInteger(Keys.Ff); }
            set { Elements.SetInteger(Keys.Ff, (int)value); }
        }

        public virtual PdfItem Value
        {
            get { return Elements[Keys.V]; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("The field is read only.");
                if (value is PdfString || value is PdfName)
                    Elements[Keys.V] = value;
                else
                    throw new NotImplementedException("Values other than string cannot be set.");
            }
        }

        public bool ReadOnly
        {
            get { return (Flags & PdfAcroFieldFlags.ReadOnly) != 0; }
            set
            {
                if (value)
                    SetFlags |= PdfAcroFieldFlags.ReadOnly;
                else
                    SetFlags &= ~PdfAcroFieldFlags.ReadOnly;
            }
        }

        public PdfAcroField this[string name]
        {
            get { return GetValue(name); }
        }

        protected virtual PdfAcroField GetValue(string name)
        {
            if (String.IsNullOrEmpty(name))
                return this;
            if (HasKids)
                return Fields.GetValue(name);
            return null;
        }

        public bool HasKids
        {
            get
            {
                PdfItem item = Elements[Keys.Kids];
                if (item == null)
                    return false;
                if (item is PdfArray)
                    return ((PdfArray)item).Elements.Count > 0;
                return false;
            }
        }

        [Obsolete("Use GetDescendantNames")]
        public string[] DescendantNames       
        {
            get { return GetDescendantNames(); }
        }

        public string[] GetDescendantNames()
        {
            List<string> names = new List<string>();
            if (HasKids)
            {
                PdfAcroFieldCollection fields = Fields;
                fields.GetDescendantNames(ref names, null);
            }
            List<string> temp = new List<string>();
            foreach (string name in names)
                temp.Add(name);
            return temp.ToArray();
        }

        public string[] GetAppearanceNames()
        {
            Dictionary<string, object> names = new Dictionary<string, object>();
            PdfDictionary dict = Elements["/AP"] as PdfDictionary;
            if (dict != null)
            {
                AppDict(dict, names);

                if (HasKids)
                {
                    PdfItem[] kids = Fields.Elements.Items;
                    foreach (PdfItem pdfItem in kids)
                    {
                        if (pdfItem is PdfReference)
                        {
                            PdfDictionary xxx = ((PdfReference)pdfItem).Value as PdfDictionary;
                            if (xxx != null)
                                AppDict(xxx, names);
                        }
                    }
                }
            }
            string[] array = new string[names.Count];
            names.Keys.CopyTo(array, 0);
            return array;
        }

        static void AppDict(PdfDictionary dict, Dictionary<string, object> names)
        {
            PdfDictionary sub;
            if ((sub = dict.Elements["/D"] as PdfDictionary) != null)
                AppDict2(sub, names);
            if ((sub = dict.Elements["/N"] as PdfDictionary) != null)
                AppDict2(sub, names);
        }

        static void AppDict2(PdfDictionary dict, Dictionary<string, object> names)
        {
            foreach (string key in dict.Elements.Keys)
            {
                if (!names.ContainsKey(key))
                    names.Add(key, null);
            }
        }

        internal virtual void GetDescendantNames(ref List<string> names, string partialName)
        {
            if (HasKids)
            {
                PdfAcroFieldCollection fields = Fields;
                string t = Elements.GetString(Keys.T);
                Debug.Assert(t != "");
                if (t.Length > 0)
                {
                    if (!String.IsNullOrEmpty(partialName))
                        partialName += "." + t;
                    else
                        partialName = t;
                    fields.GetDescendantNames(ref names, partialName);
                }
            }
            else
            {
                string t = Elements.GetString(Keys.T);
                Debug.Assert(t != "");
                if (t.Length > 0)
                {
                    if (!String.IsNullOrEmpty(partialName))
                        names.Add(partialName + "." + t);
                    else
                        names.Add(t);
                }
            }
        }

        public PdfAcroFieldCollection Fields
        {
            get
            {
                if (_fields == null)
                {
                    object o = Elements.GetValue(Keys.Kids, VCF.CreateIndirect);
                    _fields = (PdfAcroFieldCollection)o;
                }
                return _fields;
            }
        }
        PdfAcroFieldCollection _fields;

        public sealed class PdfAcroFieldCollection : PdfArray
        {
            PdfAcroFieldCollection(PdfArray array)
                : base(array)
            { }

            public int Count
            {
                get
                {
                    return Elements.Count;
                }
            }

            public string[] Names
            {
                get
                {
                    int count = Elements.Count;
                    string[] names = new string[count];
                    for (int idx = 0; idx < count; idx++)
                        names[idx] = ((PdfDictionary)((PdfReference)Elements[idx]).Value).Elements.GetString(Keys.T);
                    return names;
                }
            }

            public string[] DescendantNames
            {
                get
                {
                    List<string> names = new List<string>();
                    GetDescendantNames(ref names, null);
                    return names.ToArray();
                }
            }

            internal void GetDescendantNames(ref List<string> names, string partialName)
            {
                int count = Elements.Count;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfAcroField field = this[idx];
                    if (field != null)
                        field.GetDescendantNames(ref names, partialName);
                }
            }

            public PdfAcroField this[int index]
            {
                get
                {
                    PdfItem item = Elements[index];
                    Debug.Assert(item is PdfReference);
                    PdfDictionary dict = ((PdfReference)item).Value as PdfDictionary;
                    Debug.Assert(dict != null);
                    PdfAcroField field = dict as PdfAcroField;
                    if (field == null && dict != null)
                    {
                        field = CreateAcroField(dict);
                    }
                    return field;
                }
            }

            public PdfAcroField this[string name]
            {
                get { return GetValue(name); }
            }

            internal PdfAcroField GetValue(string name)
            {
                if (String.IsNullOrEmpty(name))
                    return null;

                int dot = name.IndexOf('.');
                string prefix = dot == -1 ? name : name.Substring(0, dot);
                string suffix = dot == -1 ? "" : name.Substring(dot + 1);

                int count = Elements.Count;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfAcroField field = this[idx];
                    if (field.Name == prefix)
                        return field.GetValue(suffix);
                }
                return null;
            }

            PdfAcroField CreateAcroField(PdfDictionary dict)
            {
                string ft = dict.Elements.GetName(Keys.FT);
                PdfAcroFieldFlags flags = (PdfAcroFieldFlags)dict.Elements.GetInteger(Keys.Ff);
                switch (ft)
                {
                    case "/Btn":
                        if ((flags & PdfAcroFieldFlags.Pushbutton) != 0)
                            return new PdfPushButtonField(dict);

                        if ((flags & PdfAcroFieldFlags.Radio) != 0)
                            return new PdfRadioButtonField(dict);

                        return new PdfCheckBoxField(dict);

                    case "/Tx":
                        return new PdfTextField(dict);

                    case "/Ch":
                        if ((flags & PdfAcroFieldFlags.Combo) != 0)
                            return new PdfComboBoxField(dict);
                        else
                            return new PdfListBoxField(dict);

                    case "/Sig":
                        return new PdfSignatureField(dict);

                    default:
                        return new PdfGenericField(dict);
                }
            }
        }

        public class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string FT = "/FT";

            [KeyInfo(KeyType.Dictionary)]
            public const string Parent = "/Parent";

            [KeyInfo(KeyType.Array | KeyType.Optional, typeof(PdfAcroFieldCollection))]
            public const string Kids = "/Kids";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string T = "/T";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string TU = "/TU";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string TM = "/TM";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string Ff = "/Ff";

            [KeyInfo(KeyType.Various | KeyType.Optional)]
            public const string V = "/V";

            [KeyInfo(KeyType.Various | KeyType.Optional)]
            public const string DV = "/DV";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string AA = "/AA";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string DR = "/DR";

            [KeyInfo(KeyType.String | KeyType.Required)]
            public const string DA = "/DA";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string Q = "/Q";

        }
    }
}
