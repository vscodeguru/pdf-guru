using System;
using System.Diagnostics;

namespace PdfSharp.Pdf.Filters
{
    public static class Filtering
    {
        public static Filter GetFilter(string filterName)
        {
            if (filterName.StartsWith("/"))
                filterName = filterName.Substring(1);

            switch (filterName)
            {
                case "ASCIIHexDecode":
                case "AHx":
                    return _asciiHexDecode ?? (_asciiHexDecode = new AsciiHexDecode());

                case "ASCII85Decode":
                case "A85":
                    return _ascii85Decode ?? (_ascii85Decode = new Ascii85Decode());

                case "LZWDecode":
                case "LZW":
                    return _lzwDecode ?? (_lzwDecode = new LzwDecode());

                case "FlateDecode":
                case "Fl":
                    return _flateDecode ?? (_flateDecode = new FlateDecode());

                case "RunLengthDecode":
                case "CCITTFaxDecode":
                case "JBIG2Decode":
                case "DCTDecode":
                case "JPXDecode":
                case "Crypt":
                    Debug.WriteLine("Filter not implemented: " + filterName);
                    return null;
            }
            throw new NotImplementedException("Unknown filter: " + filterName);
        }

        public static AsciiHexDecode ASCIIHexDecode
        {
            get { return _asciiHexDecode ?? (_asciiHexDecode = new AsciiHexDecode()); }
        }
        static AsciiHexDecode _asciiHexDecode;

        public static Ascii85Decode ASCII85Decode
        {
            get { return _ascii85Decode ?? (_ascii85Decode = new Ascii85Decode()); }
        }
        static Ascii85Decode _ascii85Decode;

        public static LzwDecode LzwDecode
        {
            get { return _lzwDecode ?? (_lzwDecode = new LzwDecode()); }
        }
        static LzwDecode _lzwDecode;

        public static FlateDecode FlateDecode
        {
            get { return _flateDecode ?? (_flateDecode = new FlateDecode()); }
        }
        static FlateDecode _flateDecode;

        public static byte[] Encode(byte[] data, string filterName)
        {
            Filter filter = GetFilter(filterName);
            if (filter != null)
                return filter.Encode(data);
            return null;
        }

        public static byte[] Encode(string rawString, string filterName)
        {
            Filter filter = GetFilter(filterName);
            if (filter != null)
                return filter.Encode(rawString);
            return null;
        }

        public static byte[] Decode(byte[] data, string filterName, FilterParms parms)
        {
            Filter filter = GetFilter(filterName);
            if (filter != null)
                return filter.Decode(data, parms);
            return null;
        }

        public static byte[] Decode(byte[] data, string filterName)
        {
            Filter filter = GetFilter(filterName);
            if (filter != null)
                return filter.Decode(data, null);
            return null;
        }

        public static byte[] Decode(byte[] data, PdfItem filterItem)
        {
            byte[] result = null;
            if (filterItem is PdfName)
            {
                Filter filter = GetFilter(filterItem.ToString());
                if (filter != null)
                    result = filter.Decode(data);
            }
            else if (filterItem is PdfArray)
            {
                PdfArray array = (PdfArray)filterItem;
                foreach (PdfItem item in array)
                    data = Decode(data, item);
                result = data;
            }
            return result;
        }

        public static string DecodeToString(byte[] data, string filterName, FilterParms parms)
        {
            Filter filter = GetFilter(filterName);
            if (filter != null)
                return filter.DecodeToString(data, parms);
            return null;
        }

        public static string DecodeToString(byte[] data, string filterName)
        {
            Filter filter = GetFilter(filterName);
            if (filter != null)
                return filter.DecodeToString(data, null);
            return null;
        }
    }
}
