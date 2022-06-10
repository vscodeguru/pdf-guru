using System;
using System.Diagnostics;
using System.Collections.Generic;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    internal sealed class PdfFormXObjectTable : PdfResourceTable
    {
        public PdfFormXObjectTable(PdfDocument document)
            : base(document)
        { }

        public PdfFormXObject GetForm(XForm form)
        {
            if (form._pdfForm != null)
            {
                Debug.Assert(form.IsTemplate, "An XPdfForm must not have a PdfFormXObject.");
                if (ReferenceEquals(form._pdfForm.Owner, Owner))
                    return form._pdfForm;
                form._pdfForm = null;
            }

            XPdfForm pdfForm = form as XPdfForm;
            if (pdfForm != null)
            {
                Selector selector = new Selector(form);
                PdfImportedObjectTable importedObjectTable;
                if (!_forms.TryGetValue(selector, out importedObjectTable))
                {
                    PdfDocument doc = pdfForm.ExternalDocument;
                    importedObjectTable = new PdfImportedObjectTable(Owner, doc);
                    _forms[selector] = importedObjectTable;
                }

                PdfFormXObject xObject = importedObjectTable.GetXObject(pdfForm.PageNumber);
                if (xObject == null)
                {
                    xObject = new PdfFormXObject(Owner, importedObjectTable, pdfForm);
                    importedObjectTable.SetXObject(pdfForm.PageNumber, xObject);
                }
                return xObject;
            }
            Debug.Assert(form.GetType() == typeof(XForm));
            form._pdfForm = new PdfFormXObject(Owner, form);
            return form._pdfForm;
        }

        public PdfImportedObjectTable GetImportedObjectTable(PdfPage page)
        {
            Selector selector = new Selector(page);
            PdfImportedObjectTable importedObjectTable;
            if (!_forms.TryGetValue(selector, out importedObjectTable))
            {
                importedObjectTable = new PdfImportedObjectTable(Owner, page.Owner);
                _forms[selector] = importedObjectTable;
            }
            return importedObjectTable;
        }

        public PdfImportedObjectTable GetImportedObjectTable(PdfDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            Selector selector = new Selector(document);
            PdfImportedObjectTable importedObjectTable;
            if (!_forms.TryGetValue(selector, out importedObjectTable))
            {
                importedObjectTable = new PdfImportedObjectTable(Owner, document);
                _forms[selector] = importedObjectTable;
            }
            return importedObjectTable;
        }

        public void DetachDocument(PdfDocument.DocumentHandle handle)
        {
            if (handle.IsAlive)
            {
                foreach (Selector selector in _forms.Keys)
                {
                    PdfImportedObjectTable table = _forms[selector];
                    if (table.ExternalDocument != null && table.ExternalDocument.Handle == handle)
                    {
                        _forms.Remove(selector);
                        break;
                    }
                }
            }

            bool itemRemoved = true;
            while (itemRemoved)
            {
                itemRemoved = false;
                foreach (Selector selector in _forms.Keys)
                {
                    PdfImportedObjectTable table = _forms[selector];
                    if (table.ExternalDocument == null)
                    {
                        _forms.Remove(selector);
                        itemRemoved = true;
                        break;
                    }
                }
            }
        }

        readonly Dictionary<Selector, PdfImportedObjectTable> _forms = new Dictionary<Selector, PdfImportedObjectTable>();

        public class Selector
        {
            public Selector(XForm form)
            {
                _path = form._path.ToLowerInvariant();
            }

            public Selector(PdfPage page)
            {
                PdfDocument owner = page.Owner;
                _path = "*" + owner.Guid.ToString("B");
                _path = _path.ToLowerInvariant();
            }

            public Selector(PdfDocument document)
            {
                _path = "*" + document.Guid.ToString("B");
                _path = _path.ToLowerInvariant();
            }

            public string Path
            {
                get { return _path; }
                set { _path = value; }
            }
            string _path;

            public override bool Equals(object obj)
            {
                Selector selector = obj as Selector;
                if (selector == null)
                    return false;
                return _path == selector._path;
            }

            public override int GetHashCode()
            {
                return _path.GetHashCode();
            }
        }
    }
}
