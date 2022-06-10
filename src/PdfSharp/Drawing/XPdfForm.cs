using System;
using System.IO;
using PdfSharp.Internal;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Drawing
{
    public class XPdfForm : XForm
    {
        internal XPdfForm(string path)
        {
            int pageNumber;
            path = ExtractPageNumber(path, out pageNumber);

#if !NETFX_CORE
            path = Path.GetFullPath(path);
            if (!File.Exists(path))
                throw new FileNotFoundException(PSSR.FileNotFound(path));
#endif

            if (PdfReader.TestPdfFile(path) == 0)
                throw new ArgumentException("The specified file has no valid PDF file header.", "path");

            _path = path;
            if (pageNumber != 0)
                PageNumber = pageNumber;
        }

        internal XPdfForm(Stream stream)
        {
            _path = "*" + Guid.NewGuid().ToString("B");

            if (PdfReader.TestPdfFile(stream) == 0)
                throw new ArgumentException("The specified stream has no valid PDF file header.", "stream");

            _externalDocument = PdfReader.Open(stream);
        }

        public static new XPdfForm FromFile(string path)
        {
            return new XPdfForm(path);
        }

        public static new XPdfForm FromStream(Stream stream)
        {
            return new XPdfForm(stream);
        }

        internal override void Finish()
        {
            if (_formState == FormState.NotATemplate || _formState == FormState.Finished)
                return;

            base.Finish();

        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                try
                {
                    if (disposing)
                    {
                    }
                    if (_externalDocument != null)
                        PdfDocument.Tls.DetachDocument(_externalDocument.Handle);
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
        bool _disposed;

        public XImage PlaceHolder
        {
            get { return _placeHolder; }
            set { _placeHolder = value; }
        }
        XImage _placeHolder;

        public PdfPage Page
        {
            get
            {
                if (IsTemplate)
                    return null;
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page;
            }
        }

        public int PageCount
        {
            get
            {
                if (IsTemplate)
                    return 1;
                if (_pageCount == -1)
                    _pageCount = ExternalDocument.Pages.Count;
                return _pageCount;
            }
        }
        int _pageCount = -1;

        [Obsolete("Use either PixelWidth or PointWidth. Temporarily obsolete because of rearrangements for WPF.")]
        public override double Width
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Width;
            }
        }

        [Obsolete("Use either PixelHeight or PointHeight. Temporarily obsolete because of rearrangements for WPF.")]
        public override double Height
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Height;
            }
        }

        public override double PointWidth
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Width;
            }
        }

        public override double PointHeight
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Height;
            }
        }

        public override int PixelWidth
        {
            get
            {
                return DoubleUtil.DoubleToInt(PointWidth);
            }
        }

        public override int PixelHeight
        {
            get
            {
                return DoubleUtil.DoubleToInt(PointHeight);
            }
        }

        public override XSize Size
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return new XSize(page.Width, page.Height);
            }
        }

        public override XMatrix Transform
        {
            get { return _transform; }
            set
            {
                if (_transform != value)
                {
                    _pdfForm = null;
                    _transform = value;
                }
            }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if (IsTemplate)
                    throw new InvalidOperationException("The page number of an XPdfForm template cannot be modified.");

                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    _pdfForm = null;
                }
            }
        }
        int _pageNumber = 1;

        public int PageIndex
        {
            get { return PageNumber - 1; }
            set { PageNumber = value + 1; }
        }

        internal PdfDocument ExternalDocument
        {
            get
            {
                if (IsTemplate)
                    throw new InvalidOperationException("This XPdfForm is a template and not an imported PDF page; therefore it has no external document.");

                if (_externalDocument == null)
                    _externalDocument = PdfDocument.Tls.GetDocument(_path);
                return _externalDocument;
            }
        }
        internal PdfDocument _externalDocument;

        public static string ExtractPageNumber(string path, out int pageNumber)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            pageNumber = 0;
            int length = path.Length;
            if (length != 0)
            {
                length--;
                if (char.IsDigit(path, length))
                {
                    while (char.IsDigit(path, length) && length >= 0)
                        length--;
                    if (length > 0 && path[length] == '#')
                    {
                        if (path.IndexOf('.') != -1)
                        {
                            pageNumber = int.Parse(path.Substring(length + 1));
                            path = path.Substring(0, length);
                        }
                    }
                }
            }
            return path;
        }
    }
}
