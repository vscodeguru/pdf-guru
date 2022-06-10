using System;

namespace PdfSharp.Drawing
{
    public sealed class XImageFormat
    {
        XImageFormat(Guid guid)
        {
            _guid = guid;
        }

        internal Guid Guid
        {
            get { return _guid; }
        }

        public override bool Equals(object obj)
        {
            XImageFormat format = obj as XImageFormat;
            if (format == null)
                return false;
            return _guid == format._guid;
        }

        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        public static XImageFormat Png
        {
            get { return _png; }
        }

        public static XImageFormat Gif
        {
            get { return _gif; }
        }

        public static XImageFormat Jpeg
        {
            get { return _jpeg; }
        }

        public static XImageFormat Tiff
        {
            get { return _tiff; }
        }

        public static XImageFormat Pdf
        {
            get { return _pdf; }
        }

        public static XImageFormat Icon
        {
            get { return _icon; }
        }

        readonly Guid _guid;

        private static readonly XImageFormat _png = new XImageFormat(new Guid("{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}"));
        private static readonly XImageFormat _gif = new XImageFormat(new Guid("{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}"));
        private static readonly XImageFormat _jpeg = new XImageFormat(new Guid("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}"));
        private static readonly XImageFormat _tiff = new XImageFormat(new Guid("{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}"));
        private static readonly XImageFormat _icon = new XImageFormat(new Guid("{B96B3CB5-0728-11D3-9D7B-0000F81EF32E}"));
        private static readonly XImageFormat _pdf = new XImageFormat(new Guid("{84570158-DBF0-4C6B-8368-62D6A3CA76E0}"));
    }
}