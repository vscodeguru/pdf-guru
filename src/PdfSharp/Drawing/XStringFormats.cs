
namespace PdfSharp.Drawing
{
    public static class XStringFormats
    {
        public static XStringFormat Default
        {
            get { return BaseLineLeft; }
        }

        public static XStringFormat BaseLineLeft
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Near;
                format.LineAlignment = XLineAlignment.BaseLine;
                return format;
            }
        }

        public static XStringFormat TopLeft
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Near;
                format.LineAlignment = XLineAlignment.Near;
                return format;
            }
        }

        public static XStringFormat CenterLeft
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Near;
                format.LineAlignment = XLineAlignment.Center;
                return format;
            }
        }

        public static XStringFormat BottomLeft
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Near;
                format.LineAlignment = XLineAlignment.Far;
                return format;
            }
        }

        public static XStringFormat BaseLineCenter
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Center;
                format.LineAlignment = XLineAlignment.BaseLine;
                return format;
            }
        }

        public static XStringFormat TopCenter
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Center;
                format.LineAlignment = XLineAlignment.Near;
                return format;
            }
        }

        public static XStringFormat Center
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Center;
                format.LineAlignment = XLineAlignment.Center;
                return format;
            }
        }

        public static XStringFormat BottomCenter
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Center;
                format.LineAlignment = XLineAlignment.Far;
                return format;
            }
        }

        public static XStringFormat BaseLineRight
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Far;
                format.LineAlignment = XLineAlignment.BaseLine;
                return format;
            }
        }

        public static XStringFormat TopRight
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Far;
                format.LineAlignment = XLineAlignment.Near;
                return format;
            }
        }

        public static XStringFormat CenterRight
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Far;
                format.LineAlignment = XLineAlignment.Center;
                return format;
            }
        }

        public static XStringFormat BottomRight
        {
            get
            {
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Far;
                format.LineAlignment = XLineAlignment.Far;
                return format;
            }
        }
    }
}