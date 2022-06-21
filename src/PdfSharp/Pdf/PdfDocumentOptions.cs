namespace PdfSharp.Pdf
{
    public sealed class PdfDocumentOptions
    {
        internal PdfDocumentOptions(PdfDocument document)
        {
        }

        public PdfColorMode ColorMode
        {
            get { return _colorMode; }
            set { _colorMode = value; }
        }
        PdfColorMode _colorMode = PdfColorMode.Rgb;

        public bool CompressContentStreams
        {
            get { return _compressContentStreams; }
            set { _compressContentStreams = value; }
        }

        bool _compressContentStreams = true;

        public bool NoCompression
        {
            get { return _noCompression; }
            set { _noCompression = value; }
        }
        bool _noCompression;

        public PdfFlateEncodeMode FlateEncodeMode
        {
            get { return _flateEncodeMode; }
            set { _flateEncodeMode = value; }
        }
        PdfFlateEncodeMode _flateEncodeMode = PdfFlateEncodeMode.Default;

        public bool EnableCcittCompressionForBilevelImages
        {
            get { return _enableCcittCompressionForBilevelImages; }
            set { _enableCcittCompressionForBilevelImages = value; }
        }
        bool _enableCcittCompressionForBilevelImages = false;

        public PdfUseFlateDecoderForJpegImages UseFlateDecoderForJpegImages
        {
            get { return _useFlateDecoderForJpegImages; }
            set { _useFlateDecoderForJpegImages = value; }
        }
        PdfUseFlateDecoderForJpegImages _useFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Never;
    }
}