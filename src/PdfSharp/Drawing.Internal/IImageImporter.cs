using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf;

namespace PdfSharp.Drawing
{
    internal interface IImageImporter
    {
        ImportedImage ImportImage(StreamReaderHelper stream, PdfDocument document);

        ImageData PrepareImage(ImagePrivateData data);
    }

    internal class StreamReaderHelper
    {
        internal StreamReaderHelper(Stream stream)
        {
            _stream = stream;
            _stream.Position = 0;
            if (_stream.Length > int.MaxValue)
                throw new ArgumentException("Stream is too large.", "stream");
            _length = (int)_stream.Length;
            _data = new byte[_length];
            _stream.Read(_data, 0, _length);
        }

        internal byte GetByte(int offset)
        {
            if (_currentOffset + offset >= _length)
            {
                Debug.Assert(false);
                return 0;
            }
            return _data[_currentOffset + offset];
        }

        internal ushort GetWord(int offset, bool bigEndian)
        {
            return (ushort)(bigEndian ?
                GetByte(offset) * 256 + GetByte(offset + 1) :
                GetByte(offset)       + GetByte(offset + 1) * 256);
        }

        internal uint GetDWord(int offset, bool bigEndian)
        {
            return (uint)(bigEndian ?
                GetWord(offset, true) * 65536 + GetWord(offset + 2, true) :
                GetWord(offset, false)        + GetWord(offset + 2, false) * 65536);
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[65536];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public void Reset()
        {
            _currentOffset = 0;
        }

        public Stream OriginalStream
        {
            get { return _stream; }
        }
        private readonly Stream _stream;

        internal int CurrentOffset
        {
            get { return _currentOffset; }
            set { _currentOffset = value; }
        }
        private int _currentOffset;

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

    internal abstract class ImportedImage
    {
        protected ImportedImage(IImageImporter importer, ImagePrivateData data, PdfDocument document)
        {
            Data = data;
            _document = document;
            data.Image = this;
            _importer = importer;
        }


        public ImageInformation Information
        {
            get { return _information; }
            private set { _information = value; }
        }
        private ImageInformation _information = new ImageInformation();

        public bool HasImageData
        {
            get { return _imageData != null; }
        }

        public ImageData ImageData
        {
            get { if(!HasImageData) _imageData = PrepareImageData();  return _imageData; }
            private set { _imageData = value; }
        }
        private ImageData _imageData;

        internal virtual ImageData PrepareImageData()
        {
            throw new NotImplementedException();
        }

        private IImageImporter _importer;
        internal ImagePrivateData Data;
        internal readonly PdfDocument _document;
    }

    internal class ImageInformation
    {
        internal enum ImageFormats
        {
            JPEG,
            JPEGGRAY,
            JPEGRGBW,
            JPEGCMYK,
            Palette1,
            Palette4,
            Palette8,
            RGB24,
            ARGB32
        }

        internal ImageFormats ImageFormat;

        internal uint Width;
        internal uint Height;

        internal decimal HorizontalDPI;
        internal decimal VerticalDPI;

        internal decimal HorizontalDPM;
        internal decimal VerticalDPM;

        internal decimal HorizontalAspectRatio;
        internal decimal VerticalAspectRatio;

        internal uint ColorsUsed;
    }

    internal abstract class ImagePrivateData
    {
        internal ImagePrivateData()
        {
        }

        public ImportedImage Image
        {
            get { return _image; }
            internal set { _image = value; }
        }
        private ImportedImage _image;
    }

    internal abstract class ImageData
    {
    }
}
