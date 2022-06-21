using System.Collections.Generic;
using System.IO;
using PdfSharp.Pdf;

namespace PdfSharp.Drawing.Internal
{
    internal class ImageImporter
    {
        public static ImageImporter GetImageImporter()
        {
            return new ImageImporter();
        }

        private ImageImporter()
        {
            _importers.Add(new ImageImporterJpeg());
            _importers.Add(new ImageImporterBmp());
        }

        public ImportedImage ImportImage(Stream stream, PdfDocument document)
        {
            StreamReaderHelper helper = new StreamReaderHelper(stream);

            foreach (IImageImporter importer in _importers)
            {
                helper.Reset();
                ImportedImage image = importer.ImportImage(helper, document);
                if (image != null)
                    return image;
            }
            return null;
        }

        public ImportedImage ImportImage(string filename, PdfDocument document)
        {
            ImportedImage ii;
            using (Stream fs = File.OpenRead(filename))
            {
                ii = ImportImage(fs, document);
            }
            return ii;
        }


        private readonly List<IImageImporter> _importers = new List<IImageImporter>();
    }
}
