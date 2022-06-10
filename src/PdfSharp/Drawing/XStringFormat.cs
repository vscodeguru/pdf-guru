using System;

namespace PdfSharp.Drawing
{

    public class XStringFormat
    {
        public XStringFormat()
        {

        }

        public XStringAlignment Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
            }
        }
        XStringAlignment _alignment;

        public XLineAlignment LineAlignment
        {
            get { return _lineAlignment; }
            set
            {
                _lineAlignment = value;
            }
        }
        XLineAlignment _lineAlignment;

        [Obsolete("Use XStringFormats.Default. (Note plural in class name.)")]
        public static XStringFormat Default
        {
            get { return XStringFormats.Default; }
        }

        [Obsolete("Use XStringFormats.Default. (Note plural in class name.)")]
        public static XStringFormat TopLeft
        {
            get { return XStringFormats.TopLeft; }
        }

        [Obsolete("Use XStringFormats.Center. (Note plural in class name.)")]
        public static XStringFormat Center
        {
            get { return XStringFormats.Center; }
        }

        [Obsolete("Use XStringFormats.TopCenter. (Note plural in class name.)")]
        public static XStringFormat TopCenter
        {
            get { return XStringFormats.TopCenter; }
        }

        [Obsolete("Use XStringFormats.BottomCenter. (Note plural in class name.)")]
        public static XStringFormat BottomCenter
        {
            get { return XStringFormats.BottomCenter; }
        }

    }
}
