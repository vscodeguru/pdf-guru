using System;
using System.Diagnostics;
using System.Collections.Generic;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    internal sealed class PdfImageTable : PdfResourceTable
    {
        public PdfImageTable(PdfDocument document)
            : base(document)
        { }

        public PdfImage GetImage(XImage image)
        {
            ImageSelector selector = image._selector;
            if (selector == null)
            {
                selector = new ImageSelector(image);
                image._selector = selector;
            }
            PdfImage pdfImage;
            if (!_images.TryGetValue(selector, out pdfImage))
            {
                pdfImage = new PdfImage(Owner, image);
                Debug.Assert(pdfImage.Owner == Owner);
                _images[selector] = pdfImage;
 }
            return pdfImage;
        }

        readonly Dictionary<ImageSelector, PdfImage> _images = new Dictionary<ImageSelector, PdfImage>();

        public class ImageSelector
        {
            public ImageSelector(XImage image)
            {
                if (image._path == null)
                    image._path = "*" + Guid.NewGuid().ToString("B");

                _path = image._path.ToLowerInvariant();
            }

            public string Path
            {
                get { return _path; }
                set { _path = value; }
            }
            string _path;

            public override bool Equals(object obj)
            {
                ImageSelector selector = obj as ImageSelector;
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
