using System;
using System.Collections.Generic;
using System.Diagnostics;
using PdfSharp.Pdf.Annotations;

namespace PdfSharp.Pdf.AcroForms
{
    public abstract class PdfButtonField : PdfAcroField
    {
        protected PdfButtonField(PdfDocument document)
            : base(document)
        { }

        protected PdfButtonField(PdfDictionary dict)
            : base(dict)
        { }

        protected string GetNonOffValue()
        {
            PdfDictionary ap = Elements[PdfAnnotation.Keys.AP] as PdfDictionary;
            if (ap != null)
            {
                PdfDictionary n = ap.Elements["/N"] as PdfDictionary;
                if (n != null)
                {
                    foreach (string name in n.Elements.Keys)
                        if (name != "/Off")
                            return name;
                }
            }
            return null;
        }

        internal override void GetDescendantNames(ref List<string> names, string partialName)
        {
            string t = Elements.GetString(PdfAcroField.Keys.T);
            if (t == "")
                t = "???";
            Debug.Assert(t != "");
            if (t.Length > 0)
            {
                if (!String.IsNullOrEmpty(partialName))
                    names.Add(partialName + "." + t);
                else
                    names.Add(t);
            }
        }

        public new class Keys : PdfAcroField.Keys
        {
        }
    }
}
