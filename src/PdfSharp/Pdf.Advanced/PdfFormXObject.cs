using System;
using System.Diagnostics;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfFormXObject : PdfXObject, IContentStream
    {
        internal PdfFormXObject(PdfDocument thisDocument)
            : base(thisDocument)
        {
            Elements.SetName(Keys.Type, "/XObject");
            Elements.SetName(Keys.Subtype, "/Form");
        }

        internal PdfFormXObject(PdfDocument thisDocument, XForm form)
            : base(thisDocument)
        {
            Elements.SetName(Keys.Type, "/XObject");
            Elements.SetName(Keys.Subtype, "/Form");

        }

        internal double DpiX
        {
            get { return _dpiX; }
            set { _dpiX = value; }
        }
        double _dpiX = 72;

        internal double DpiY
        {
            get { return _dpiY; }
            set { _dpiY = value; }
        }
        double _dpiY = 72;

        internal PdfFormXObject(PdfDocument thisDocument, PdfImportedObjectTable importedObjectTable, XPdfForm form)
            : base(thisDocument)
        {
            Debug.Assert(importedObjectTable != null);
            Debug.Assert(ReferenceEquals(thisDocument, importedObjectTable.Owner));
            Elements.SetName(Keys.Type, "/XObject");
            Elements.SetName(Keys.Subtype, "/Form");

            if (form.IsTemplate)
            {
                Debug.Assert(importedObjectTable == null);
                return;
            }

            XPdfForm pdfForm = form;
            PdfPages importPages = importedObjectTable.ExternalDocument.Pages;
            if (pdfForm.PageNumber < 1 || pdfForm.PageNumber > importPages.Count)
                PSSR.ImportPageNumberOutOfRange(pdfForm.PageNumber, importPages.Count, form._path);
            PdfPage importPage = importPages[pdfForm.PageNumber - 1];

            PdfItem res = importPage.Elements["/Resources"];
            if (res != null)    
            {
                PdfObject root;
                if (res is PdfReference)
                    root = ((PdfReference)res).Value;
                else
                    root = (PdfDictionary)res;

                root = ImportClosure(importedObjectTable, thisDocument, root);
                if (root.Reference == null)
                    thisDocument._irefTable.Add(root);

                Debug.Assert(root.Reference != null);
                Elements["/Resources"] = root.Reference;
            }

            PdfRectangle rect = importPage.Elements.GetRectangle(PdfPage.Keys.MediaBox);
            int rotate = (importPage.Elements.GetInteger(PdfPage.Keys.Rotate) % 360 + 360) % 360;
            if (rotate == 0)
            {
                Elements["/BBox"] = rect;
            }
            else
            {
                Elements["/BBox"] = rect;

                XMatrix matrix = new XMatrix();
                double width = rect.Width;
                double height = rect.Height;
                matrix.RotateAtPrepend(-rotate, new XPoint(width / 2, height / 2));

                double offset = (height - width) / 2;
                if (rotate == 90)
                {
                    if (height > width)
                        matrix.TranslatePrepend(offset, offset);  
                    else
                        matrix.TranslatePrepend(offset, offset);    
                }
                else if (rotate == 270)
                {
                    if (height > width)
                        matrix.TranslatePrepend(-offset, -offset);  
                    else
                        matrix.TranslatePrepend(-offset, -offset);  
                }

                Elements.SetMatrix(Keys.Matrix, matrix);
            }

            PdfContent content = importPage.Contents.CreateSingleContent();
            content.Compressed = true;
            PdfItem filter = content.Elements["/Filter"];
            if (filter != null)
                Elements["/Filter"] = filter.Clone();

            Stream = content.Stream;    
            Elements.SetInteger("/Length", content.Stream.Value.Length);
        }

        public PdfResources Resources
        {
            get
            {
                if (_resources == null)
                    _resources = (PdfResources)Elements.GetValue(Keys.Resources, VCF.Create);
                return _resources;
            }
        }
        PdfResources _resources;

        PdfResources IContentStream.Resources
        {
            get { return Resources; }
        }

        internal string GetFontName(XFont font, out PdfFont pdfFont)
        {
            pdfFont = _document.FontTable.GetFont(font);
            Debug.Assert(pdfFont != null);
            string name = Resources.AddFont(pdfFont);
            return name;
        }

        string IContentStream.GetFontName(XFont font, out PdfFont pdfFont)
        {
            return GetFontName(font, out pdfFont);
        }

        internal string GetFontName(string idName, byte[] fontData, out PdfFont pdfFont)
        {
            pdfFont = _document.FontTable.GetFont(idName, fontData);
            Debug.Assert(pdfFont != null);
            string name = Resources.AddFont(pdfFont);
            return name;
        }

        string IContentStream.GetFontName(string idName, byte[] fontData, out PdfFont pdfFont)
        {
            return GetFontName(idName, fontData, out pdfFont);
        }

        string IContentStream.GetImageName(XImage image)
        {
            throw new NotImplementedException();
        }

        string IContentStream.GetFormName(XForm form)
        {
            throw new NotImplementedException();
        }


        public sealed new class Keys : PdfXObject.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string Subtype = "/Subtype";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string FormType = "/FormType";

            [KeyInfo(KeyType.Rectangle | KeyType.Required)]
            public const string BBox = "/BBox";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Matrix = "/Matrix";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResources))]
            public const string Resources = "/Resources";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string Group = "/Group";

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
