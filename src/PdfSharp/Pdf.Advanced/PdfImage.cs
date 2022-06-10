using System;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Internal;
using PdfSharp.Pdf.Filters;

namespace PdfSharp.Pdf.Advanced
{
    public sealed partial class PdfImage : PdfXObject
    {
        public PdfImage(PdfDocument document, XImage image)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/XObject");
            Elements.SetName(Keys.Subtype, "/Image");

            _image = image;

#if !SILVERLIGHT
            switch (_image.Format.Guid.ToString("B").ToUpper())
            {
                case "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}":  
                    InitializeJpeg();
                    break;

                case "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}":  
                case "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}":  
                case "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}":  
                case "{B96B3CB5-0728-11D3-9D7B-0000F81EF32E}":  
                    InitializeNonJpeg();
                    break;

                case "{84570158-DBF0-4C6B-8368-62D6A3CA76E0}":  
                    Debug.Assert(false, "XPdfForm not expected here.");
                    break;

                default:
                    Debug.Assert(false, "Unexpected image type.");
                    break;
            }

#endif
        }


        public XImage Image
        {
            get { return _image; }
        }

        readonly XImage _image;

        public override string ToString()
        {
            return "Image";
        }

        void InitializeJpeg()
        {
            MemoryStream memory = null;
            bool ownMemory = false;

            byte[] imageBits = null;
            int streamLength = 0;

#if CORE || GDI || WPF
            if (_image._importedImage != null)
            {
                ImageDataDct idd = (ImageDataDct)_image._importedImage.ImageData;
                imageBits = idd.Data;
                streamLength = idd.Length;
            }
#endif

#if CORE || GDI
            if (_image._importedImage == null)
            {
                if (!_image._path.StartsWith("*"))
                {
                    using (FileStream sourceFile = File.OpenRead(_image._path))
                    {
                        int count;
                        byte[] buffer = new byte[8192];
                        memory = new MemoryStream((int)sourceFile.Length);
                        ownMemory = true;
                        do
                        {
                            count = sourceFile.Read(buffer, 0, buffer.Length);
                            memory.Write(buffer, 0, count);
                        } while (count > 0);
                    }
                }
                else
                {
                    memory = new MemoryStream();
                    ownMemory = true;
                    if (_image._stream != null && _image._stream.CanSeek)
                    {
                        Stream stream = _image._stream;
                        stream.Seek(0, SeekOrigin.Begin);
                        byte[] buffer = new byte[32 * 1024];   
                        int bytesRead;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            memory.Write(buffer, 0, bytesRead);
                        }
                    }
                    else
                    {
#if CORE_WITH_GDI
                        _image._gdiImage.Save(memory, ImageFormat.Jpeg);
#endif
                    }
                }

                if ((int)memory.Length == 0)
                {
                    Debug.Assert(false, "Internal error? JPEG image, but file not found!");
                }
            }
#endif

            if (imageBits == null)
            {
                streamLength = (int)memory.Length;
                imageBits = new byte[streamLength];
                memory.Seek(0, SeekOrigin.Begin);
                memory.Read(imageBits, 0, streamLength);
                if (ownMemory)
                {
#if UWP || true
                    memory.Dispose();
#endif
                }
            }

            bool tryFlateDecode = _document.Options.UseFlateDecoderForJpegImages == PdfUseFlateDecoderForJpegImages.Automatic;
            bool useFlateDecode = _document.Options.UseFlateDecoderForJpegImages == PdfUseFlateDecoderForJpegImages.Always;

            FlateDecode fd = new FlateDecode();
            byte[] imageDataCompressed = (useFlateDecode || tryFlateDecode) ? fd.Encode(imageBits, _document.Options.FlateEncodeMode) : null;
            if (useFlateDecode || tryFlateDecode && imageDataCompressed.Length < imageBits.Length)
            {
                Stream = new PdfStream(imageDataCompressed, this);
                Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataCompressed.Length);
                PdfArray arrayFilters = new PdfArray(_document);
                arrayFilters.Elements.Add(new PdfName("/FlateDecode"));
                arrayFilters.Elements.Add(new PdfName("/DCTDecode"));
                Elements[PdfStream.Keys.Filter] = arrayFilters;
            }
            else
            {
                Stream = new PdfStream(imageBits, this);
                Elements[PdfStream.Keys.Length] = new PdfInteger(streamLength);
                Elements[PdfStream.Keys.Filter] = new PdfName("/DCTDecode");
            }
            if (_image.Interpolate)
                Elements[Keys.Interpolate] = PdfBoolean.True;
            Elements[Keys.Width] = new PdfInteger(_image.PixelWidth);
            Elements[Keys.Height] = new PdfInteger(_image.PixelHeight);
            Elements[Keys.BitsPerComponent] = new PdfInteger(8);

#if CORE || GDI || WPF
            if (_image._importedImage != null)
            {
                if (_image._importedImage.Information.ImageFormat == ImageInformation.ImageFormats.JPEGCMYK ||
                    _image._importedImage.Information.ImageFormat == ImageInformation.ImageFormats.JPEGRGBW)
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceCMYK");
                    if (_image._importedImage.Information.ImageFormat == ImageInformation.ImageFormats.JPEGRGBW)
                        Elements["/Decode"] = new PdfLiteral("[1 0 1 0 1 0 1 0]");       
                }
                else if (_image._importedImage.Information.ImageFormat == ImageInformation.ImageFormats.JPEGGRAY)
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceGray");
                }
                else
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceRGB");
                }
            }
#endif
#if CORE_WITH_GDI
            if (_image._importedImage == null)
            {
                if ((_image._gdiImage.Flags & ((int)ImageFlags.ColorSpaceCmyk | (int)ImageFlags.ColorSpaceYcck)) != 0)
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceCMYK");
                    if ((_image._gdiImage.Flags & (int)ImageFlags.ColorSpaceYcck) != 0)
                        Elements["/Decode"] = new PdfLiteral("[1 0 1 0 1 0 1 0]");    
                }
                else if ((_image._gdiImage.Flags & (int)ImageFlags.ColorSpaceGray) != 0)
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceGray");
                }
                else
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceRGB");
                }
            }
#endif

        }

        void InitializeNonJpeg()
        {
#if CORE || GDI || WPF
            if (_image._importedImage != null)
            {
                switch (_image._importedImage.Information.ImageFormat)
                {
                    case ImageInformation.ImageFormats.RGB24:
                        CreateTrueColorMemoryBitmap(3, 8, false);
                        break;

                    case ImageInformation.ImageFormats.Palette8:
                        CreateIndexedMemoryBitmap(8);
                        break;

                    case ImageInformation.ImageFormats.Palette4:
                        CreateIndexedMemoryBitmap(4);
                        break;

                    case ImageInformation.ImageFormats.Palette1:
                        CreateIndexedMemoryBitmap(1);
                        break;

                    default:
                        throw new NotImplementedException("Image format not supported.");
                }
                return;
            }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
            switch (_image._gdiImage.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    ReadTrueColorMemoryBitmap(3, 8, false);
                    break;

                case PixelFormat.Format32bppRgb:
                    ReadTrueColorMemoryBitmap(4, 8, false);
                    break;

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    ReadTrueColorMemoryBitmap(3, 8, true);
                    break;

                case PixelFormat.Format8bppIndexed:
                    ReadIndexedMemoryBitmap(8);
                    break;

                case PixelFormat.Format4bppIndexed:
                    ReadIndexedMemoryBitmap(4);
                    break;

                case PixelFormat.Format1bppIndexed:
                    ReadIndexedMemoryBitmap(1);
                    break;

                default:
#if DEBUGxxx
          image.image.Save("$$$.bmp", ImageFormat.Bmp);
#endif
                    throw new NotImplementedException("Image format not supported.");
            }
#endif
        }

#if CORE || GDI || WPF
        private void CreateIndexedMemoryBitmap(int bits)
        {
            ImageDataBitmap idb = (ImageDataBitmap)_image._importedImage.ImageData;
            ImageInformation ii = _image._importedImage.Information;

            int pdfVersion = Owner.Version;
            int firstMaskColor = -1, lastMaskColor = -1;
            bool segmentedColorMask = idb.SegmentedColorMask;

            {

                FlateDecode fd = new FlateDecode();
                if (firstMaskColor != -1 &&
                  lastMaskColor != -1)
                {
                    if (!segmentedColorMask && pdfVersion >= 13 && !idb.IsGray)
                    {
                        PdfArray array = new PdfArray(_document);
                        array.Elements.Add(new PdfInteger(firstMaskColor));
                        array.Elements.Add(new PdfInteger(lastMaskColor));
                        Elements[Keys.Mask] = array;
                    }
                    else
                    {
                        byte[] maskDataCompressed = fd.Encode(idb.BitmapMask, _document.Options.FlateEncodeMode);
                        PdfDictionary pdfMask = new PdfDictionary(_document);
                        pdfMask.Elements.SetName(Keys.Type, "/XObject");
                        pdfMask.Elements.SetName(Keys.Subtype, "/Image");

                        Owner._irefTable.Add(pdfMask);
                        pdfMask.Stream = new PdfStream(maskDataCompressed, pdfMask);
                        pdfMask.Elements[PdfStream.Keys.Length] = new PdfInteger(maskDataCompressed.Length);
                        pdfMask.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                        pdfMask.Elements[Keys.Width] = new PdfInteger((int)ii.Width);
                        pdfMask.Elements[Keys.Height] = new PdfInteger((int)ii.Height);
                        pdfMask.Elements[Keys.BitsPerComponent] = new PdfInteger(1);
                        pdfMask.Elements[Keys.ImageMask] = new PdfBoolean(true);
                        Elements[Keys.Mask] = pdfMask.Reference;
                    }
                }

                byte[] imageDataCompressed = fd.Encode(idb.Data, _document.Options.FlateEncodeMode);
                byte[] imageDataFaxCompressed = idb.DataFax != null ? fd.Encode(idb.DataFax, _document.Options.FlateEncodeMode) : null;

                bool usesCcittEncoding = false;
                if (idb.DataFax != null &&
                  (idb.LengthFax < imageDataCompressed.Length ||
                  imageDataFaxCompressed.Length < imageDataCompressed.Length))
                {
                    usesCcittEncoding = true;

                    if (idb.LengthFax < imageDataCompressed.Length)
                    {
                        Stream = new PdfStream(idb.DataFax, this);
                        Elements[PdfStream.Keys.Length] = new PdfInteger(idb.LengthFax);
                        Elements[PdfStream.Keys.Filter] = new PdfName("/CCITTFaxDecode");
                        PdfDictionary dictionary = new PdfDictionary();
                        if (idb.K != 0)
                            dictionary.Elements.Add("/K", new PdfInteger(idb.K));
                        if (idb.IsBitonal < 0)
                            dictionary.Elements.Add("/BlackIs1", new PdfBoolean(true));
                        dictionary.Elements.Add("/EndOfBlock", new PdfBoolean(false));
                        dictionary.Elements.Add("/Columns", new PdfInteger((int)ii.Width));
                        dictionary.Elements.Add("/Rows", new PdfInteger((int)ii.Height));
                        Elements[PdfStream.Keys.DecodeParms] = dictionary;
                    }
                    else
                    {
                        Stream = new PdfStream(imageDataFaxCompressed, this);
                        Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataFaxCompressed.Length);
                        PdfArray arrayFilters = new PdfArray(_document);
                        arrayFilters.Elements.Add(new PdfName("/FlateDecode"));
                        arrayFilters.Elements.Add(new PdfName("/CCITTFaxDecode"));
                        Elements[PdfStream.Keys.Filter] = arrayFilters;
                        PdfArray arrayDecodeParms = new PdfArray(_document);

                        PdfDictionary dictFlateDecodeParms = new PdfDictionary();

                        PdfDictionary dictCcittFaxDecodeParms = new PdfDictionary();
                        if (idb.K != 0)
                            dictCcittFaxDecodeParms.Elements.Add("/K", new PdfInteger(idb.K));
                        if (idb.IsBitonal < 0)
                            dictCcittFaxDecodeParms.Elements.Add("/BlackIs1", new PdfBoolean(true));
                        dictCcittFaxDecodeParms.Elements.Add("/EndOfBlock", new PdfBoolean(false));
                        dictCcittFaxDecodeParms.Elements.Add("/Columns", new PdfInteger((int)ii.Width));
                        dictCcittFaxDecodeParms.Elements.Add("/Rows", new PdfInteger((int)ii.Height));

                        arrayDecodeParms.Elements.Add(dictFlateDecodeParms);       
                        arrayDecodeParms.Elements.Add(dictCcittFaxDecodeParms);
                        Elements[PdfStream.Keys.DecodeParms] = arrayDecodeParms;
                    }
                }
                else
                {
                    Stream = new PdfStream(imageDataCompressed, this);
                    Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataCompressed.Length);
                    Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                }

                Elements[Keys.Width] = new PdfInteger((int)ii.Width);
                Elements[Keys.Height] = new PdfInteger((int)ii.Height);
                Elements[Keys.BitsPerComponent] = new PdfInteger(bits);
                if ((usesCcittEncoding && idb.IsBitonal == 0) ||
                  (!usesCcittEncoding && idb.IsBitonal <= 0 && !idb.IsGray))
                {
                    PdfDictionary colorPalette = null;
                    colorPalette = new PdfDictionary(_document);
                    byte[] packedPaletteData = idb.PaletteDataLength >= 48 ? fd.Encode(idb.PaletteData, _document.Options.FlateEncodeMode) : null;     
                    if (packedPaletteData != null && packedPaletteData.Length + 20 < idb.PaletteDataLength)        
                    {
                        colorPalette.CreateStream(packedPaletteData);
                        colorPalette.Elements[PdfStream.Keys.Length] = new PdfInteger(packedPaletteData.Length);
                        colorPalette.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                    }
                    else
                    {
                        colorPalette.CreateStream(idb.PaletteData);
                        colorPalette.Elements[PdfStream.Keys.Length] = new PdfInteger(idb.PaletteDataLength);
                    }
                    Owner._irefTable.Add(colorPalette);

                    PdfArray arrayColorSpace = new PdfArray(_document);
                    arrayColorSpace.Elements.Add(new PdfName("/Indexed"));
                    arrayColorSpace.Elements.Add(new PdfName("/DeviceRGB"));
                    arrayColorSpace.Elements.Add(new PdfInteger((int)ii.ColorsUsed - 1));
                    arrayColorSpace.Elements.Add(colorPalette.Reference);
                    Elements[Keys.ColorSpace] = arrayColorSpace;
                }
                else
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceGray");
                }
                if (_image.Interpolate)
                    Elements[Keys.Interpolate] = PdfBoolean.True;
            }
        }

        private void CreateTrueColorMemoryBitmap(int components, int bits, bool hasAlpha)
        {
            int pdfVersion = Owner.Version;
            FlateDecode fd = new FlateDecode();
            ImageDataBitmap idb = (ImageDataBitmap)_image._importedImage.ImageData;
            ImageInformation ii = _image._importedImage.Information;
            bool hasMask = idb.AlphaMaskLength > 0 || idb.BitmapMaskLength > 0;
            bool hasAlphaMask = idb.AlphaMaskLength > 0;

            if (hasMask)
            {
                byte[] maskDataCompressed = fd.Encode(idb.BitmapMask, _document.Options.FlateEncodeMode);
                PdfDictionary pdfMask = new PdfDictionary(_document);
                pdfMask.Elements.SetName(Keys.Type, "/XObject");
                pdfMask.Elements.SetName(Keys.Subtype, "/Image");

                Owner._irefTable.Add(pdfMask);
                pdfMask.Stream = new PdfStream(maskDataCompressed, pdfMask);
                pdfMask.Elements[PdfStream.Keys.Length] = new PdfInteger(maskDataCompressed.Length);
                pdfMask.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                pdfMask.Elements[Keys.Width] = new PdfInteger((int)ii.Width);
                pdfMask.Elements[Keys.Height] = new PdfInteger((int)ii.Height);
                pdfMask.Elements[Keys.BitsPerComponent] = new PdfInteger(1);
                pdfMask.Elements[Keys.ImageMask] = new PdfBoolean(true);
                Elements[Keys.Mask] = pdfMask.Reference;
            }
            if (hasMask && hasAlphaMask && pdfVersion >= 14)
            {
                byte[] alphaMaskCompressed = fd.Encode(idb.AlphaMask, _document.Options.FlateEncodeMode);
                PdfDictionary smask = new PdfDictionary(_document);
                smask.Elements.SetName(Keys.Type, "/XObject");
                smask.Elements.SetName(Keys.Subtype, "/Image");

                Owner._irefTable.Add(smask);
                smask.Stream = new PdfStream(alphaMaskCompressed, smask);
                smask.Elements[PdfStream.Keys.Length] = new PdfInteger(alphaMaskCompressed.Length);
                smask.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                smask.Elements[Keys.Width] = new PdfInteger((int)ii.Width);
                smask.Elements[Keys.Height] = new PdfInteger((int)ii.Height);
                smask.Elements[Keys.BitsPerComponent] = new PdfInteger(8);
                smask.Elements[Keys.ColorSpace] = new PdfName("/DeviceGray");
                Elements[Keys.SMask] = smask.Reference;
            }

            byte[] imageDataCompressed = fd.Encode(idb.Data, _document.Options.FlateEncodeMode);

            Stream = new PdfStream(imageDataCompressed, this);
            Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataCompressed.Length);
            Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
            Elements[Keys.Width] = new PdfInteger((int)ii.Width);
            Elements[Keys.Height] = new PdfInteger((int)ii.Height);
            Elements[Keys.BitsPerComponent] = new PdfInteger(8);
            Elements[Keys.ColorSpace] = new PdfName("/DeviceRGB");
            if (_image.Interpolate)
                Elements[Keys.Interpolate] = PdfBoolean.True;
        }
#endif

        private static int ReadWord(byte[] ab, int offset)
        {
            return ab[offset] + 256 * ab[offset + 1];
        }

        private static int ReadDWord(byte[] ab, int offset)
        {
            return ReadWord(ab, offset) + 0x10000 * ReadWord(ab, offset + 2);
        }

        private void ReadTrueColorMemoryBitmap(int components, int bits, bool hasAlpha)
        {

            int pdfVersion = Owner.Version;
            MemoryStream memory = new MemoryStream();
#if CORE_WITH_GDI
            _image._gdiImage.Save(memory, ImageFormat.Bmp);
#endif

            int streamLength = (int)memory.Length;
            Debug.Assert(streamLength > 0, "Bitmap image encoding failed.");
            if (streamLength > 0)
            {
#if !NETFX_CORE && !UWP
                byte[] imageBits = memory.GetBuffer();

#endif

                int height = _image.PixelHeight;
                int width = _image.PixelWidth;

                if (ReadWord(imageBits, 0) != 0x4d42 ||  
                    ReadDWord(imageBits, 2) != streamLength ||
                    ReadDWord(imageBits, 14) != 40 ||   
                    ReadDWord(imageBits, 18) != width ||
                    ReadDWord(imageBits, 22) != height)
                {
                    throw new NotImplementedException("ReadTrueColorMemoryBitmap: unsupported format");
                }
                if (ReadWord(imageBits, 26) != 1 ||
                  (!hasAlpha && ReadWord(imageBits, 28) != components * bits ||
                   hasAlpha && ReadWord(imageBits, 28) != (components + 1) * bits) ||
                  ReadDWord(imageBits, 30) != 0)
                {
                    throw new NotImplementedException("ReadTrueColorMemoryBitmap: unsupported format #2");
                }

                int nFileOffset = ReadDWord(imageBits, 10);
                int logicalComponents = components;
                if (components == 4)
                    logicalComponents = 3;

                byte[] imageData = new byte[components * width * height];

                bool hasMask = false;
                bool hasAlphaMask = false;
                byte[] alphaMask = hasAlpha ? new byte[width * height] : null;
                MonochromeMask mask = hasAlpha ?
                  new MonochromeMask(width, height) : null;

                int nOffsetRead = 0;
                if (logicalComponents == 3)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        int nOffsetWrite = 3 * (height - 1 - y) * width;
                        int nOffsetWriteAlpha = 0;
                        if (hasAlpha)
                        {
                            mask.StartLine(y);
                            nOffsetWriteAlpha = (height - 1 - y) * width;
                        }

                        for (int x = 0; x < width; ++x)
                        {
                            imageData[nOffsetWrite] = imageBits[nFileOffset + nOffsetRead + 2];
                            imageData[nOffsetWrite + 1] = imageBits[nFileOffset + nOffsetRead + 1];
                            imageData[nOffsetWrite + 2] = imageBits[nFileOffset + nOffsetRead];
                            if (hasAlpha)
                            {
                                mask.AddPel(imageBits[nFileOffset + nOffsetRead + 3]);
                                alphaMask[nOffsetWriteAlpha] = imageBits[nFileOffset + nOffsetRead + 3];
                                if (!hasMask || !hasAlphaMask)
                                {
                                    if (imageBits[nFileOffset + nOffsetRead + 3] != 255)
                                    {
                                        hasMask = true;
                                        if (imageBits[nFileOffset + nOffsetRead + 3] != 0)
                                            hasAlphaMask = true;
                                    }
                                }
                                ++nOffsetWriteAlpha;
                            }
                            nOffsetRead += hasAlpha ? 4 : components;
                            nOffsetWrite += 3;
                        }
                        nOffsetRead = 4 * ((nOffsetRead + 3) / 4);      
                    }
                }
                else if (components == 1)
                {
                    throw new NotImplementedException("Image format not supported (grayscales).");
                }

                FlateDecode fd = new FlateDecode();
                if (hasMask)
                {
                    byte[] maskDataCompressed = fd.Encode(mask.MaskData, _document.Options.FlateEncodeMode);
                    PdfDictionary pdfMask = new PdfDictionary(_document);
                    pdfMask.Elements.SetName(Keys.Type, "/XObject");
                    pdfMask.Elements.SetName(Keys.Subtype, "/Image");

                    Owner._irefTable.Add(pdfMask);
                    pdfMask.Stream = new PdfStream(maskDataCompressed, pdfMask);
                    pdfMask.Elements[PdfStream.Keys.Length] = new PdfInteger(maskDataCompressed.Length);
                    pdfMask.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                    pdfMask.Elements[Keys.Width] = new PdfInteger(width);
                    pdfMask.Elements[Keys.Height] = new PdfInteger(height);
                    pdfMask.Elements[Keys.BitsPerComponent] = new PdfInteger(1);
                    pdfMask.Elements[Keys.ImageMask] = new PdfBoolean(true);
                    Elements[Keys.Mask] = pdfMask.Reference;
                }
                if (hasMask && hasAlphaMask && pdfVersion >= 14)
                {
                    byte[] alphaMaskCompressed = fd.Encode(alphaMask, _document.Options.FlateEncodeMode);
                    PdfDictionary smask = new PdfDictionary(_document);
                    smask.Elements.SetName(Keys.Type, "/XObject");
                    smask.Elements.SetName(Keys.Subtype, "/Image");

                    Owner._irefTable.Add(smask);
                    smask.Stream = new PdfStream(alphaMaskCompressed, smask);
                    smask.Elements[PdfStream.Keys.Length] = new PdfInteger(alphaMaskCompressed.Length);
                    smask.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                    smask.Elements[Keys.Width] = new PdfInteger(width);
                    smask.Elements[Keys.Height] = new PdfInteger(height);
                    smask.Elements[Keys.BitsPerComponent] = new PdfInteger(8);
                    smask.Elements[Keys.ColorSpace] = new PdfName("/DeviceGray");
                    Elements[Keys.SMask] = smask.Reference;
                }

                byte[] imageDataCompressed = fd.Encode(imageData, _document.Options.FlateEncodeMode);

                Stream = new PdfStream(imageDataCompressed, this);
                Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataCompressed.Length);
                Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                Elements[Keys.Width] = new PdfInteger(width);
                Elements[Keys.Height] = new PdfInteger(height);
                Elements[Keys.BitsPerComponent] = new PdfInteger(8);
                Elements[Keys.ColorSpace] = new PdfName("/DeviceRGB");
                if (_image.Interpolate)
                    Elements[Keys.Interpolate] = PdfBoolean.True;
            }
        }

        private void ReadIndexedMemoryBitmap(int bits)
        {
            int pdfVersion = Owner.Version;
            int firstMaskColor = -1, lastMaskColor = -1;
            bool segmentedColorMask = false;

            MemoryStream memory = new MemoryStream();
#if CORE_WITH_GDI
            _image._gdiImage.Save(memory, ImageFormat.Bmp);
#endif

            int streamLength = (int)memory.Length;
            Debug.Assert(streamLength > 0, "Bitmap image encoding failed.");
            if (streamLength > 0)
            {
                byte[] imageBits = new byte[streamLength];
                memory.Seek(0, SeekOrigin.Begin);
                memory.Read(imageBits, 0, streamLength);
#if !UWP
                memory.Close();
#endif

                int height = _image.PixelHeight;
                int width = _image.PixelWidth;

                if (ReadWord(imageBits, 0) != 0x4d42 ||  
                  ReadDWord(imageBits, 2) != streamLength ||
                  ReadDWord(imageBits, 14) != 40 ||   
#if WPF
#else
                  ReadDWord(imageBits, 18) != width ||
                  ReadDWord(imageBits, 22) != height)
#endif
                {
                    throw new NotImplementedException("ReadIndexedMemoryBitmap: unsupported format");
                }
                int fileBits = ReadWord(imageBits, 28);
                if (fileBits != bits)
                {
                    if (fileBits == 1 || fileBits == 4 || fileBits == 8)
                        bits = fileBits;
                }

                if (ReadWord(imageBits, 26) != 1 ||
                    ReadWord(imageBits, 28) != bits ||
                    ReadDWord(imageBits, 30) != 0)
                {
                    throw new NotImplementedException("ReadIndexedMemoryBitmap: unsupported format #2");
                }

                int bytesFileOffset = ReadDWord(imageBits, 10);
                const int bytesColorPaletteOffset = 0x36;           
                int paletteColors = ReadDWord(imageBits, 46);
                if ((bytesFileOffset - bytesColorPaletteOffset) / 4 != paletteColors)
                {
                    throw new NotImplementedException("ReadIndexedMemoryBitmap: unsupported format #3");
                }

                MonochromeMask mask = new MonochromeMask(width, height);

                bool isGray = bits == 8 && (paletteColors == 256 || paletteColors == 0);
                int isBitonal = 0;        
                byte[] paletteData = new byte[3 * paletteColors];
                for (int color = 0; color < paletteColors; ++color)
                {
                    paletteData[3 * color] = imageBits[bytesColorPaletteOffset + 4 * color + 2];
                    paletteData[3 * color + 1] = imageBits[bytesColorPaletteOffset + 4 * color + 1];
                    paletteData[3 * color + 2] = imageBits[bytesColorPaletteOffset + 4 * color + 0];
                    if (isGray)
                        isGray = paletteData[3 * color] == paletteData[3 * color + 1] &&
                          paletteData[3 * color] == paletteData[3 * color + 2];

                    if (imageBits[bytesColorPaletteOffset + 4 * color + 3] < 128)
                    {
                        if (firstMaskColor == -1)
                            firstMaskColor = color;
                        if (lastMaskColor == -1 || lastMaskColor == color - 1)
                            lastMaskColor = color;
                        if (lastMaskColor != color)
                            segmentedColorMask = true;
                    }
                }

                if (bits == 1)
                {
                    if (paletteColors == 0)
                        isBitonal = 1;
                    if (paletteColors == 2)
                    {
                        if (paletteData[0] == 0 &&
                          paletteData[1] == 0 &&
                          paletteData[2] == 0 &&
                          paletteData[3] == 255 &&
                          paletteData[4] == 255 &&
                          paletteData[5] == 255)
                            isBitonal = 1;    
                        if (paletteData[5] == 0 &&
                          paletteData[4] == 0 &&
                          paletteData[3] == 0 &&
                          paletteData[2] == 255 &&
                          paletteData[1] == 255 &&
                          paletteData[0] == 255)
                            isBitonal = -1;    
                    }
                }

                bool hasMask = firstMaskColor != -1 && lastMaskColor != -1;

                bool isFaxEncoding = false;
                byte[] imageData = new byte[((width * bits + 7) / 8) * height];
                byte[] imageDataFax = null;
                int k = 0;

                if (bits == 1 && _document.Options.EnableCcittCompressionForBilevelImages)
                {
                    byte[] tempG4 = new byte[imageData.Length];
                    int ccittSizeG4 = DoFaxEncodingGroup4(ref tempG4, imageBits, (uint)bytesFileOffset, (uint)width, (uint)height);

                    isFaxEncoding =     ccittSizeG4 > 0;
                    if (isFaxEncoding)
                    {
                        if (ccittSizeG4 == 0)
                            ccittSizeG4 = 0x7fffffff;
                        {
                            Array.Resize(ref tempG4, ccittSizeG4);
                            imageDataFax = tempG4;
                            k = -1;
                        }
                    }
                }

                {
                    int bytesOffsetRead = 0;
                    if (bits == 8 || bits == 4 || bits == 1)
                    {
                        int bytesPerLine = (width * bits + 7) / 8;
                        for (int y = 0; y < height; ++y)
                        {
                            mask.StartLine(y);
                            int bytesOffsetWrite = (height - 1 - y) * ((width * bits + 7) / 8);
                            for (int x = 0; x < bytesPerLine; ++x)
                            {
                                if (isGray)
                                {
                                    imageData[bytesOffsetWrite] = paletteData[3 * imageBits[bytesFileOffset + bytesOffsetRead]];
                                }
                                else
                                {
                                    imageData[bytesOffsetWrite] = imageBits[bytesFileOffset + bytesOffsetRead];
                                }
                                if (firstMaskColor != -1)
                                {
                                    int n = imageBits[bytesFileOffset + bytesOffsetRead];
                                    if (bits == 8)
                                    {
                                        mask.AddPel((n >= firstMaskColor) && (n <= lastMaskColor));
                                    }
                                    else if (bits == 4)
                                    {
                                        int n1 = (n & 0xf0) / 16;
                                        int n2 = (n & 0x0f);
                                        mask.AddPel((n1 >= firstMaskColor) && (n1 <= lastMaskColor));
                                        mask.AddPel((n2 >= firstMaskColor) && (n2 <= lastMaskColor));
                                    }
                                    else if (bits == 1)
                                    {
                                        for (int bit = 1; bit <= 8; ++bit)
                                        {
                                            int n1 = (n & 0x80) / 128;
                                            mask.AddPel((n1 >= firstMaskColor) && (n1 <= lastMaskColor));
                                            n *= 2;
                                        }
                                    }
                                }
                                bytesOffsetRead += 1;
                                bytesOffsetWrite += 1;
                            }
                            bytesOffsetRead = 4 * ((bytesOffsetRead + 3) / 4);      
                        }
                    }
                    else
                    {
                        throw new NotImplementedException("ReadIndexedMemoryBitmap: unsupported format #3");
                    }
                }

                FlateDecode fd = new FlateDecode();
                if (hasMask)
                {
                    if (!segmentedColorMask && pdfVersion >= 13 && !isGray)
                    {
                        PdfArray array = new PdfArray(_document);
                        array.Elements.Add(new PdfInteger(firstMaskColor));
                        array.Elements.Add(new PdfInteger(lastMaskColor));
                        Elements[Keys.Mask] = array;
                    }
                    else
                    {
                        byte[] maskDataCompressed = fd.Encode(mask.MaskData, _document.Options.FlateEncodeMode);
                        PdfDictionary pdfMask = new PdfDictionary(_document);
                        pdfMask.Elements.SetName(Keys.Type, "/XObject");
                        pdfMask.Elements.SetName(Keys.Subtype, "/Image");

                        Owner._irefTable.Add(pdfMask);
                        pdfMask.Stream = new PdfStream(maskDataCompressed, pdfMask);
                        pdfMask.Elements[PdfStream.Keys.Length] = new PdfInteger(maskDataCompressed.Length);
                        pdfMask.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                        pdfMask.Elements[Keys.Width] = new PdfInteger(width);
                        pdfMask.Elements[Keys.Height] = new PdfInteger(height);
                        pdfMask.Elements[Keys.BitsPerComponent] = new PdfInteger(1);
                        pdfMask.Elements[Keys.ImageMask] = new PdfBoolean(true);
                        Elements[Keys.Mask] = pdfMask.Reference;
                    }
                }

                byte[] imageDataCompressed = fd.Encode(imageData, _document.Options.FlateEncodeMode);
                byte[] imageDataFaxCompressed = isFaxEncoding ? fd.Encode(imageDataFax, _document.Options.FlateEncodeMode) : null;

                bool usesCcittEncoding = false;
                if (isFaxEncoding &&
                  (imageDataFax.Length < imageDataCompressed.Length ||
                  imageDataFaxCompressed.Length < imageDataCompressed.Length))
                {
                    usesCcittEncoding = true;

                    if (imageDataFax.Length < imageDataCompressed.Length)
                    {
                        Stream = new PdfStream(imageDataFax, this);
                        Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataFax.Length);
                        Elements[PdfStream.Keys.Filter] = new PdfName("/CCITTFaxDecode");
                        PdfDictionary dictionary = new PdfDictionary();
                        if (k != 0)
                            dictionary.Elements.Add("/K", new PdfInteger(k));
                        if (isBitonal < 0)
                            dictionary.Elements.Add("/BlackIs1", new PdfBoolean(true));
                        dictionary.Elements.Add("/EndOfBlock", new PdfBoolean(false));
                        dictionary.Elements.Add("/Columns", new PdfInteger(width));
                        dictionary.Elements.Add("/Rows", new PdfInteger(height));
                        Elements[PdfStream.Keys.DecodeParms] = dictionary;  
                    }
                    else
                    {
                        Stream = new PdfStream(imageDataFaxCompressed, this);
                        Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataFaxCompressed.Length);
                        PdfArray arrayFilters = new PdfArray(_document);
                        arrayFilters.Elements.Add(new PdfName("/FlateDecode"));
                        arrayFilters.Elements.Add(new PdfName("/CCITTFaxDecode"));
                        Elements[PdfStream.Keys.Filter] = arrayFilters;
                        PdfArray arrayDecodeParms = new PdfArray(_document);

                        PdfDictionary dictFlateDecodeParms = new PdfDictionary();

                        PdfDictionary dictCcittFaxDecodeParms = new PdfDictionary();
                        if (k != 0)
                            dictCcittFaxDecodeParms.Elements.Add("/K", new PdfInteger(k));
                        if (isBitonal < 0)
                            dictCcittFaxDecodeParms.Elements.Add("/BlackIs1", new PdfBoolean(true));
                        dictCcittFaxDecodeParms.Elements.Add("/EndOfBlock", new PdfBoolean(false));
                        dictCcittFaxDecodeParms.Elements.Add("/Columns", new PdfInteger(width));
                        dictCcittFaxDecodeParms.Elements.Add("/Rows", new PdfInteger(height));

                        arrayDecodeParms.Elements.Add(dictFlateDecodeParms);       
                        arrayDecodeParms.Elements.Add(dictCcittFaxDecodeParms);
                        Elements[PdfStream.Keys.DecodeParms] = arrayDecodeParms;
                    }
                }
                else
                {
                    Stream = new PdfStream(imageDataCompressed, this);
                    Elements[PdfStream.Keys.Length] = new PdfInteger(imageDataCompressed.Length);
                    Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                }

                Elements[Keys.Width] = new PdfInteger(width);
                Elements[Keys.Height] = new PdfInteger(height);
                Elements[Keys.BitsPerComponent] = new PdfInteger(bits);
                if ((usesCcittEncoding && isBitonal == 0) ||
                  (!usesCcittEncoding && isBitonal <= 0 && !isGray))
                {
                    PdfDictionary colorPalette = null;
                    colorPalette = new PdfDictionary(_document);
                    byte[] packedPaletteData = paletteData.Length >= 48 ? fd.Encode(paletteData, _document.Options.FlateEncodeMode) : null;     
                    if (packedPaletteData != null && packedPaletteData.Length + 20 < paletteData.Length)        
                    {
                        colorPalette.CreateStream(packedPaletteData);
                        colorPalette.Elements[PdfStream.Keys.Length] = new PdfInteger(packedPaletteData.Length);
                        colorPalette.Elements[PdfStream.Keys.Filter] = new PdfName("/FlateDecode");
                    }
                    else
                    {
                        colorPalette.CreateStream(paletteData);
                        colorPalette.Elements[PdfStream.Keys.Length] = new PdfInteger(paletteData.Length);
                    }
                    Owner._irefTable.Add(colorPalette);

                    PdfArray arrayColorSpace = new PdfArray(_document);
                    arrayColorSpace.Elements.Add(new PdfName("/Indexed"));
                    arrayColorSpace.Elements.Add(new PdfName("/DeviceRGB"));
                    arrayColorSpace.Elements.Add(new PdfInteger(paletteColors - 1));
                    arrayColorSpace.Elements.Add(colorPalette.Reference);
                    Elements[Keys.ColorSpace] = arrayColorSpace;
                }
                else
                {
                    Elements[Keys.ColorSpace] = new PdfName("/DeviceGray");
                }
                if (_image.Interpolate)
                    Elements[Keys.Interpolate] = PdfBoolean.True;
            }
        }

        public sealed new class Keys : PdfXObject.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string Subtype = "/Subtype";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string Width = "/Width";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string Height = "/Height";

            [KeyInfo(KeyType.NameOrArray | KeyType.Required)]
            public const string ColorSpace = "/ColorSpace";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string BitsPerComponent = "/BitsPerComponent";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Intent = "/Intent";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string ImageMask = "/ImageMask";

            [KeyInfo(KeyType.StreamOrArray | KeyType.Optional)]
            public const string Mask = "/Mask";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Decode = "/Decode";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string Interpolate = "/Interpolate";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Alternates = "/Alternates";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string SMask = "/SMask";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string SMaskInData = "/SMaskInData";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Name = "/Name";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string StructParent = "/StructParent";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string ID = "/ID";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string OPI = "/OPI";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string Metadata = "/Metadata";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string OC = "/OC";

        }
    }

    class MonochromeMask
    {
        public byte[] MaskData
        {
            get { return _maskData; }
        }
        private readonly byte[] _maskData;

        public MonochromeMask(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            int byteSize = ((sizeX + 7) / 8) * sizeY;
            _maskData = new byte[byteSize];
            StartLine(0);
        }

        public void StartLine(int newCurrentLine)
        {
            _bitsWritten = 0;
            _byteBuffer = 0;
            _writeOffset = ((_sizeX + 7) / 8) * (_sizeY - 1 - newCurrentLine);
        }

        public void AddPel(bool isTransparent)
        {
            if (_bitsWritten < _sizeX)
            {
                if (isTransparent)
                    _byteBuffer = (_byteBuffer << 1) + 1;
                else
                    _byteBuffer = _byteBuffer << 1;
                ++_bitsWritten;
                if ((_bitsWritten & 7) == 0)
                {
                    _maskData[_writeOffset] = (byte)_byteBuffer;
                    ++_writeOffset;
                    _byteBuffer = 0;
                }
                else if (_bitsWritten == _sizeX)
                {
                    int n = 8 - (_bitsWritten & 7);
                    _byteBuffer = _byteBuffer << n;
                    _maskData[_writeOffset] = (byte)_byteBuffer;
                }
            }
        }

        public void AddPel(int shade)
        {
            AddPel(shade < 128);
        }

        private readonly int _sizeX;
        private readonly int _sizeY;
        private int _writeOffset;
        private int _byteBuffer;
        private int _bitsWritten;
    }
}
