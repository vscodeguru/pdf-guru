using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf;
#if CORE
using System.Drawing;
#endif
using PdfSharp.Drawing.Internal;
using PdfSharp.Internal;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;

namespace PdfSharp.Drawing
{
    [Flags]
    internal enum XImageState
    {
        UsedInDrawingContext = 0x00000001,

        StateMask = 0x0000FFFF,
    }

    public class XImage : IDisposable
    {

        protected XImage()
        { }

#if GDI || CORE || WPF
        XImage(ImportedImage image)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            _importedImage = image;
            Initialize();
        }
#endif

        XImage(string path)
        {
#if !NETFX_CORE && !UWP
            path = Path.GetFullPath(path);
            if (!File.Exists(path))
                throw new FileNotFoundException(PSSR.FileNotFound(path));
#endif
            _path = path;

#if CORE_WITH_GDI || GDI
            try
            {
                Lock.EnterGdiPlus();
                _gdiImage = Image.FromFile(path);
            }
            finally { Lock.ExitGdiPlus(); }
#endif

            Initialize();
        }

        XImage(Stream stream)
        {
            _path = "*" + Guid.NewGuid().ToString("B");

#if CORE_WITH_GDI
            try
            {
                Lock.EnterGdiPlus();
                _gdiImage = Image.FromStream(stream);
            }
            finally { Lock.ExitGdiPlus(); }
#endif

            _stream = stream;
            Initialize();
        }


        public static XImage FromFile(string path)
        {
            if (PdfReader.TestPdfFile(path) > 0)
                return new XPdfForm(path);
            return new XImage(path);
        }

        public static XImage FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (PdfReader.TestPdfFile(stream) > 0)
                return new XPdfForm(stream);
            return new XImage(stream);
        }

        public static bool ExistsFile(string path)
        {
            if (PdfReader.TestPdfFile(path) > 0)
                return true;
#if !NETFX_CORE && !UWP
            return File.Exists(path);
#endif
        }

        internal XImageState XImageState
        {
            get { return _xImageState; }
            set { _xImageState = value; }
        }
        XImageState _xImageState;

        internal void Initialize()
        {
#if CORE || GDI || WPF
            if (_importedImage != null)
            {
                ImportedImageJpeg iiJpeg = _importedImage as ImportedImageJpeg;
                if (iiJpeg != null)
                    _format = XImageFormat.Jpeg;
                else
                    _format = XImageFormat.Png;
                return;
            }
#endif

#if CORE_WITH_GDI
            if (_gdiImage != null)
            {
                string guid;
                try
                {
                    Lock.EnterGdiPlus();
                    guid = _gdiImage.RawFormat.Guid.ToString("B").ToUpper();
                }
                finally
                {
                    Lock.ExitGdiPlus();
                }

                switch (guid)
                {
                    case "{B96B3CAA-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Png;
                        break;

                    case "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Jpeg;
                        break;

                    case "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Gif;
                        break;

                    case "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Tiff;
                        break;

                    case "{B96B3CB5-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Icon;
                        break;

                    case "{B96B3CAC-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CAD-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CB2-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CB3-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CB4-0728-11D3-9D7B-0000F81EF32E}":   

                    default:
                        throw new InvalidOperationException("Unsupported image format.");
                }
                return;
            }
#endif

        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;

#if CORE || GDI || WPF
            {
                _importedImage = null;
            }
#endif

#if CORE_WITH_GDI || GDI
            if (_gdiImage != null)
            {
                try
                {
                    Lock.EnterGdiPlus();
                    _gdiImage.Dispose();
                    _gdiImage = null;
                }
                finally { Lock.ExitGdiPlus(); }
            }
#endif
#if WPF
            _wpfImage = null;
#endif
        }
        bool _disposed;

        [Obsolete("Use either PixelWidth or PointWidth. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelWidth, but will become PointWidth in future releases of PDFsharp.")]
        public virtual double Width
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    return _importedImage.Information.Width;
                }
#endif

#if (CORE_WITH_GDI || GDI)  && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        [Obsolete("Use either PixelHeight or PointHeight. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelHeight, but will become PointHeight in future releases of PDFsharp.")]
        public virtual double Height
        {
            get
            {
#if CORE_WITH_GDI || GDI || WPF
                if (_importedImage != null)
                {
                    return _importedImage.Information.Height;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

#if CORE || GDI || WPF
        private const decimal FactorDPM72 = 72000 / 25.4m;

        private const decimal FactorDPM = 1000 / 25.4m;
#endif

        public virtual double PointWidth
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.HorizontalDPM > 0)
                        return (double)(_importedImage.Information.Width * FactorDPM72 / _importedImage.Information.HorizontalDPM);
                    if (_importedImage.Information.HorizontalDPI > 0)
                        return (double)(_importedImage.Information.Width * 72 / _importedImage.Information.HorizontalDPI);
                    return _importedImage.Information.Width;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width * 72 / _gdiImage.HorizontalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif

            }
        }

        public virtual double PointHeight
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.VerticalDPM > 0)
                        return (double)(_importedImage.Information.Height * FactorDPM72 / _importedImage.Information.VerticalDPM);
                    if (_importedImage.Information.VerticalDPI > 0)
                        return (double)(_importedImage.Information.Height * 72 / _importedImage.Information.VerticalDPI);
                    return _importedImage.Information.Width;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height * 72 / _gdiImage.HorizontalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif

            }
        }

        public virtual int PixelWidth
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                    return (int)_importedImage.Information.Width;
#endif

#if CORE_WITH_GDI
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual int PixelHeight
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                    return (int)_importedImage.Information.Height;
#endif

#if CORE_WITH_GDI
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual XSize Size
        {
            get { return new XSize(PointWidth, PointHeight); }
        }

        public virtual double HorizontalResolution
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.HorizontalDPI > 0)
                        return (double)_importedImage.Information.HorizontalDPI;
                    if (_importedImage.Information.HorizontalDPM > 0)
                        return (double)(_importedImage.Information.HorizontalDPM / FactorDPM);
                    return 72;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.HorizontalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual double VerticalResolution
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.VerticalDPI > 0)
                        return (double)_importedImage.Information.VerticalDPI;
                    if (_importedImage.Information.VerticalDPM > 0)
                        return (double)(_importedImage.Information.VerticalDPM / FactorDPM);
                    return 72;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.VerticalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual bool Interpolate
        {
            get { return _interpolate; }
            set { _interpolate = value; }
        }
        bool _interpolate = true;

        public XImageFormat Format
        {
            get { return _format; }
        }
        XImageFormat _format;


        internal void AssociateWithGraphics(XGraphics gfx)
        {
            if (_associatedGraphics != null)
                throw new InvalidOperationException("XImage already associated with XGraphics.");
            _associatedGraphics = null;
        }

        internal void DisassociateWithGraphics()
        {
            if (_associatedGraphics == null)
                throw new InvalidOperationException("XImage not associated with XGraphics.");
            _associatedGraphics.DisassociateImage();

            Debug.Assert(_associatedGraphics == null);
        }

        internal void DisassociateWithGraphics(XGraphics gfx)
        {
            if (_associatedGraphics != gfx)
                throw new InvalidOperationException("XImage not associated with XGraphics.");
            _associatedGraphics = null;
        }

        internal XGraphics AssociatedGraphics
        {
            get { return _associatedGraphics; }
            set { _associatedGraphics = value; }
        }
        XGraphics _associatedGraphics;

#if CORE || GDI || WPF
        internal ImportedImage _importedImage;
#endif

#if CORE_WITH_GDI || GDI
        internal Image _gdiImage;
#endif
#if WPF
        internal BitmapSource _wpfImage;
#if SILVERLIGHT
        //internal byte[] _bytes;
#endif
#endif
#if NETFX_CORE  || UWP
        internal BitmapSource _wrtImage;
#endif

        internal string _path;

        internal Stream _stream;

        internal PdfImageTable.ImageSelector _selector;
    }
}
