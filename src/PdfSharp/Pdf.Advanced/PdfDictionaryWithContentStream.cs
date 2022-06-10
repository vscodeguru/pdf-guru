using System;
using System.Diagnostics;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    public abstract class PdfDictionaryWithContentStream : PdfDictionary, IContentStream
    {
        public PdfDictionaryWithContentStream()
        { }

        public PdfDictionaryWithContentStream(PdfDocument document)
            : base(document)
        { }

        protected PdfDictionaryWithContentStream(PdfDictionary dict)
            : base(dict)
        { }

        internal PdfResources Resources
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

        internal string GetImageName(XImage image)
        {
            PdfImage pdfImage = _document.ImageTable.GetImage(image);
            Debug.Assert(pdfImage != null);
            string name = Resources.AddImage(pdfImage);
            return name;
        }

        string IContentStream.GetImageName(XImage image)
        {
            throw new NotImplementedException();
        }

        internal string GetFormName(XForm form)
        {
            PdfFormXObject pdfForm = _document.FormTable.GetForm(form);
            Debug.Assert(pdfForm != null);
            string name = Resources.AddForm(pdfForm);
            return name;
        }

        string IContentStream.GetFormName(XForm form)
        {
            throw new NotImplementedException();
        }

        public class Keys : PdfDictionary.PdfStream.Keys
        {
            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResources))]
            public const string Resources = "/Resources";
        }
    }
}
