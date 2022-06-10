using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Drawing.Pdf;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;

namespace PdfSharp.Drawing
{
    public class XForm : XImage, IContentStream
    {
        internal enum FormState
        {
            NotATemplate,

            Created,

            UnderConstruction,

            Finished,
        }

        protected XForm()
        { }


        public XForm(PdfDocument document, XRect viewBox)
        {
            if (viewBox.Width < 1 || viewBox.Height < 1)
                throw new ArgumentNullException("viewBox", "The size of the XPdfForm is to small.");
            if (document == null)
                throw new ArgumentNullException("document", "An XPdfForm template must be associated with a document at creation time.");

            _formState = FormState.Created;
            _document = document;
            _pdfForm = new PdfFormXObject(document, this);
            _viewBox = viewBox;
            PdfRectangle rect = new PdfRectangle(viewBox);
            _pdfForm.Elements.SetRectangle(PdfFormXObject.Keys.BBox, rect);
        }

        public XForm(PdfDocument document, XSize size)
            : this(document, new XRect(0, 0, size.Width, size.Height))
        {
        }

        public XForm(PdfDocument document, XUnit width, XUnit height)
            : this(document, new XRect(0, 0, width, height))
        { }

        public void DrawingFinished()
        {
            if (_formState == FormState.Finished)
                return;

            if (_formState == FormState.NotATemplate)
                throw new InvalidOperationException("This object is an imported PDF page and you cannot finish drawing on it because you must not draw on it at all.");

            Finish();
        }

        internal void AssociateGraphics(XGraphics gfx)
        {
            if (_formState == FormState.NotATemplate)
                throw new NotImplementedException("The current version of PDFsharp cannot draw on an imported page.");

            if (_formState == FormState.UnderConstruction)
                throw new InvalidOperationException("An XGraphics object already exists for this form.");

            if (_formState == FormState.Finished)
                throw new InvalidOperationException("After drawing a form it cannot be modified anymore.");

            Debug.Assert(_formState == FormState.Created);
            _formState = FormState.UnderConstruction;
            Gfx = gfx;
        }
        internal XGraphics Gfx;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        internal virtual void Finish()
        {
        }

        internal PdfDocument Owner
        {
            get { return _document; }
        }
        PdfDocument _document;

        internal PdfColorMode ColorMode
        {
            get
            {
                if (_document == null)
                    return PdfColorMode.Undefined;
                return _document.Options.ColorMode;
            }
        }

        internal bool IsTemplate
        {
            get { return _formState != FormState.NotATemplate; }
        }
        internal FormState _formState;

        [Obsolete("Use either PixelWidth or PointWidth. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelWidth, but will become PointWidth in future releases of PDFsharp.")]
        public override double Width
        {
            get { return _viewBox.Width; }
        }

        [Obsolete("Use either PixelHeight or PointHeight. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelHeight, but will become PointHeight in future releases of PDFsharp.")]
        public override double Height
        {
            get { return _viewBox.Height; }
        }

        public override double PointWidth
        {
            get { return _viewBox.Width; }
        }

        public override double PointHeight
        {
            get { return _viewBox.Height; }
        }

        public override int PixelWidth
        {
            get { return (int)_viewBox.Width; }
        }

        public override int PixelHeight
        {
            get { return (int)_viewBox.Height; }
        }

        public override XSize Size
        {
            get { return _viewBox.Size; }
        }
        public XRect ViewBox
        {
            get { return _viewBox; }
        }
        XRect _viewBox;

        public override double HorizontalResolution
        {
            get { return 72; }
        }

        public override double VerticalResolution
        {
            get { return 72; }
        }

        public XRect BoundingBox
        {
            get { return _boundingBox; }
            set { _boundingBox = value; }      
        }
        XRect _boundingBox;

        public virtual XMatrix Transform
        {
            get { return _transform; }
            set
            {
                if (_formState == FormState.Finished)
                    throw new InvalidOperationException("After a XPdfForm was once drawn it must not be modified.");
                _transform = value;
            }
        }
        internal XMatrix _transform;

        internal PdfResources Resources
        {
            get
            {
                Debug.Assert(IsTemplate, "This function is for form templates only.");
                return PdfForm.Resources;
            }
        }
        PdfResources IContentStream.Resources
        {
            get { return Resources; }
        }

        internal string GetFontName(XFont font, out PdfFont pdfFont)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            pdfFont = _document.FontTable.GetFont(font);
            Debug.Assert(pdfFont != null);
            string name = Resources.AddFont(pdfFont);
            return name;
        }

        string IContentStream.GetFontName(XFont font, out PdfFont pdfFont)
        {
            return GetFontName(font, out pdfFont);
        }

        internal string TryGetFontName(string idName, out PdfFont pdfFont)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            pdfFont = _document.FontTable.TryGetFont(idName);
            string name = null;
            if (pdfFont != null)
                name = Resources.AddFont(pdfFont);
            return name;
        }

        internal string GetFontName(string idName, byte[] fontData, out PdfFont pdfFont)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
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
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            PdfImage pdfImage = _document.ImageTable.GetImage(image);
            Debug.Assert(pdfImage != null);
            string name = Resources.AddImage(pdfImage);
            return name;
        }

        string IContentStream.GetImageName(XImage image)
        {
            return GetImageName(image);
        }

        internal PdfFormXObject PdfForm
        {
            get
            {
                Debug.Assert(IsTemplate, "This function is for form templates only.");
                if (_pdfForm.Reference == null)
                    _document._irefTable.Add(_pdfForm);
                return _pdfForm;
            }
        }

        internal string GetFormName(XForm form)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            PdfFormXObject pdfForm = _document.FormTable.GetForm(form);
            Debug.Assert(pdfForm != null);
            string name = Resources.AddForm(pdfForm);
            return name;
        }

        string IContentStream.GetFormName(XForm form)
        {
            return GetFormName(form);
        }

        internal PdfFormXObject _pdfForm;     

        internal XGraphicsPdfRenderer PdfRenderer;

    }
}