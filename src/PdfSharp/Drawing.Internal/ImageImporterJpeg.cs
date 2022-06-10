using System;
using PdfSharp.Pdf;

namespace PdfSharp.Drawing.Internal
{
    internal class ImageImporterJpeg : ImageImporterRoot, IImageImporter
    {
        public ImportedImage ImportImage(StreamReaderHelper stream, PdfDocument document)
        {
            try
            {

                stream.CurrentOffset = 0;
                if (TestFileHeader(stream))
                {
                    stream.CurrentOffset += 2;

                    ImagePrivateDataDct ipd = new ImagePrivateDataDct(stream.Data, stream.Length);
                    ImportedImage ii = new ImportedImageJpeg(this, ipd, document);
                    if (TestJfifHeader(stream, ii))
                    {
                        bool colorHeader = false, infoHeader = false;

                        while (MoveToNextHeader(stream))
                        {
                            if (TestColorFormatHeader(stream, ii))
                            {
                                colorHeader = true;
                            }
                            else if (TestInfoHeader(stream, ii))
                            {
                                infoHeader = true;
                            }
                        }
                        if (colorHeader && infoHeader)
                            return ii;
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        private bool TestFileHeader(StreamReaderHelper stream)
        {
            return stream.GetWord(0, true) == 0xffd8;
        }

        private bool TestJfifHeader(StreamReaderHelper stream, ImportedImage ii)
        {
            if (stream.GetWord(0, true) == 0xffe0)
            {
                if (stream.GetDWord(4, true) == 0x4a464946)
                {
                    int blockLength = stream.GetWord(2, true);
                    if (blockLength >= 16)
                    {
                        int version = stream.GetWord(9, true);
                        int units = stream.GetByte(11);
                        int densityX = stream.GetWord(12, true);
                        int densityY = stream.GetWord(14, true);

                        switch (units)
                        {
                            case 0:    
                                ii.Information.HorizontalAspectRatio = densityX;
                                ii.Information.VerticalAspectRatio = densityY;
                                break;
                            case 1:  
                                ii.Information.HorizontalDPI = densityX;
                                ii.Information.VerticalDPI = densityY;
                                break;
                            case 2:  
                                ii.Information.HorizontalDPM = densityX * 100;
                                ii.Information.VerticalDPM = densityY * 100;
                                break;
                        }

                        return true;
                    }
                }
            }
            return false;
        }

        private bool TestColorFormatHeader(StreamReaderHelper stream, ImportedImage ii)
        {
            if (stream.GetWord(0, true) == 0xffda)
            {
                int components = stream.GetByte(4);
                if (components < 1 || components > 4 || components == 2)
                    return false;
                int blockLength = stream.GetWord(2, true);
                if (blockLength != 6 + 2 * components)
                    return false;

                ii.Information.ImageFormat = components == 3 ? ImageInformation.ImageFormats.JPEG :
                    (components == 1 ? ImageInformation.ImageFormats.JPEGGRAY : ImageInformation.ImageFormats.JPEGRGBW);

                return true;
            }
            return false;
        }

        private bool TestInfoHeader(StreamReaderHelper stream, ImportedImage ii)
        {
            int header = stream.GetWord(0, true);
            if (header >= 0xffc0 && header <= 0xffc3 ||
                header >= 0xffc9 && header <= 0xffcb)
            {
                int sizeY = stream.GetWord(5, true);
                int sizeX = stream.GetWord(7, true);

                ii.Information.Width = (uint)sizeX;
                ii.Information.Height = (uint)sizeY;

                return true;
            }
            return false;
        }

        private bool MoveToNextHeader(StreamReaderHelper stream)
        {
            int blockLength = stream.GetWord(2, true);

            int headerMagic = stream.GetByte(0);
            int headerType = stream.GetByte(1);

            if (headerMagic == 0xff)
            {
                if (headerType == 0xd9)
                    return false;

                if (headerType == 0x01 || headerType >= 0xd0 && headerType <= 0xd7)
                {
                    stream.CurrentOffset += 2;
                    return true;
                }

                stream.CurrentOffset += 2 + blockLength;
                return true;
            }
            return false;
        }

        public ImageData PrepareImage(ImagePrivateData data)
        {
            throw new NotImplementedException();
        }


    }

    internal class ImportedImageJpeg : ImportedImage
    {
        public ImportedImageJpeg(IImageImporter importer, ImagePrivateDataDct data, PdfDocument document)
            : base(importer, data, document)
        { }

        internal override ImageData PrepareImageData()
        {
            ImagePrivateDataDct data = (ImagePrivateDataDct)Data;
            ImageDataDct imageData = new ImageDataDct();
            imageData.Data = data.Data;
            imageData.Length = data.Length;

            return imageData;
        }
    }

    internal class ImageDataDct : ImageData
    {
        public byte[] Data
        {
            get { return _data; }
            internal set { _data = value; }
        }
        private byte[] _data;

        public int Length
        {
            get { return _length; }
            internal set { _length = value; }
        }
        private int _length;
    }

    internal class ImagePrivateDataDct : ImagePrivateData
    {
        public ImagePrivateDataDct(byte[] data, int length)
        {
            _data = data;
            _length = length;
        }

        public byte[] Data
        {
            get { return _data; }
        }
        private readonly byte[] _data;

        public int Length
        {
            get { return _length; }
        }
        private readonly int _length;
    }
}
