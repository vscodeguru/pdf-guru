using System;
using System;
using System.Collections.Generic;
using System.Text;
using System;
using System;
using System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using GdiFontFamily = System.Drawing.FontFamily;
using System;
using System.Diagnostics;
using System.Drawing;
using GdiFont = System.Drawing.Font;
using GdiFontStyle = System.Drawing.FontStyle;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System;
using System;
using System.Diagnostics;
using System.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;
using System;
using System.Globalization;
using System.ComponentModel;
using System.Threading;
using System;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using GdiFontFamily = System.Drawing.FontFamily;
using GdiFont = System.Drawing.Font;
using GdiFontStyle = System.Drawing.FontStyle;
using System;
using GdiFont = System.Drawing.Font;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using GdiFont = System.Drawing.Font;
using System;
using System.Diagnostics;
using System;
using System.Diagnostics;
using System.Globalization;
using GdiFont = System.Drawing.Font;
using GdiFontStyle = System.Drawing.FontStyle;
using System;
using System.Diagnostics;
using System;
using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System;
using System;
using System.ComponentModel;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices
using System;
using System.IO;
using System;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System;
using System;
using System;
using System.Diagnostics;
using System.Globalization;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;


namespace pdf_guru
{
    internal enum PathStart
    {
        MoveTo1st,

        LineTo1st,

        Ignore1st,
    }
    public enum XColorSpace
    {
        Rgb,

        Cmyk,

        GrayScale,
    }
    public enum XCombineMode
    {
        Replace = 0,

        Intersect = 1,

        Union = 2,

        Xor = 3,

        Exclude = 4,

        Complement = 5,
    }
    public enum XDashStyle
    {
        Solid = 0,

        Dash = 1,

        Dot = 2,

        DashDot = 3,

        DashDotDot = 4,

        Custom = 5,
    }
    public enum XFillMode
    {
        Alternate = 0,

        Winding = 1,
    }
    public enum XFontStyle
    {
        Regular = XGdiFontStyle.Regular,

        Bold = XGdiFontStyle.Bold,

        Italic = XGdiFontStyle.Italic,

        BoldItalic = XGdiFontStyle.BoldItalic,

        Underline = XGdiFontStyle.Underline,

        Strikeout = XGdiFontStyle.Strikeout,

    }
    internal enum XGdiFontStyle
    {
        Regular = 0,

        Bold = 1,

        Italic = 2,

        BoldItalic = 3,

        Underline = 4,

        Strikeout = 8,
    }
    enum XGraphicTargetContext
    {
        NONE = 0,

        CORE = 1,

        GDI = 2,

        WPF = 3,

        UWP = 10,
    }
    internal enum XGraphicsPathItemType
    {
        Lines,
        Beziers,
        Curve,
        Arc,
        Rectangle,
        RoundedRectangle,
        Ellipse,
        Polygon,
        CloseFigure,
        StartFigure,
    }
    public enum XGraphicsPdfPageOptions
    {
        Append,

        Prepend,

        Replace,
    }
    public enum XGraphicsUnit
    {
        Point = 0,

        Inch = 1,

        Millimeter = 2,

        Centimeter = 3,

        Presentation = 4,
    }
    public enum XKnownColor
    {
        AliceBlue = 0,

        AntiqueWhite = 1,

        Aqua = 2,

        Aquamarine = 3,

        Azure = 4,

        Beige = 5,

        Bisque = 6,

        Black = 7,

        BlanchedAlmond = 8,

        Blue = 9,

        BlueViolet = 10,

        Brown = 11,

        BurlyWood = 12,

        CadetBlue = 13,

        Chartreuse = 14,

        Chocolate = 15,

        Coral = 16,

        CornflowerBlue = 17,

        Cornsilk = 18,

        Crimson = 19,

        Cyan = 20,

        DarkBlue = 21,

        DarkCyan = 22,

        DarkGoldenrod = 23,

        DarkGray = 24,

        DarkGreen = 25,

        DarkKhaki = 26,

        DarkMagenta = 27,

        DarkOliveGreen = 28,

        DarkOrange = 29,

        DarkOrchid = 30,

        DarkRed = 31,

        DarkSalmon = 32,

        DarkSeaGreen = 33,

        DarkSlateBlue = 34,

        DarkSlateGray = 35,

        DarkTurquoise = 36,

        DarkViolet = 37,

        DeepPink = 38,

        DeepSkyBlue = 39,

        DimGray = 40,

        DodgerBlue = 41,

        Firebrick = 42,

        FloralWhite = 43,

        ForestGreen = 44,

        Fuchsia = 45,

        Gainsboro = 46,

        GhostWhite = 47,

        Gold = 48,

        Goldenrod = 49,

        Gray = 50,

        Green = 51,

        GreenYellow = 52,

        Honeydew = 53,

        HotPink = 54,

        IndianRed = 55,

        Indigo = 56,

        Ivory = 57,

        Khaki = 58,

        Lavender = 59,

        LavenderBlush = 60,

        LawnGreen = 61,

        LemonChiffon = 62,

        LightBlue = 63,

        LightCoral = 64,

        LightCyan = 65,

        LightGoldenrodYellow = 66,

        LightGray = 67,

        LightGreen = 68,

        LightPink = 69,

        LightSalmon = 70,

        LightSeaGreen = 71,

        LightSkyBlue = 72,

        LightSlateGray = 73,

        LightSteelBlue = 74,

        LightYellow = 75,

        Lime = 76,

        LimeGreen = 77,

        Linen = 78,

        Magenta = 79,

        Maroon = 80,

        MediumAquamarine = 81,

        MediumBlue = 82,

        MediumOrchid = 83,

        MediumPurple = 84,

        MediumSeaGreen = 85,

        MediumSlateBlue = 86,

        MediumSpringGreen = 87,

        MediumTurquoise = 88,

        MediumVioletRed = 89,

        MidnightBlue = 90,

        MintCream = 91,

        MistyRose = 92,

        Moccasin = 93,

        NavajoWhite = 94,

        Navy = 95,

        OldLace = 96,

        Olive = 97,

        OliveDrab = 98,

        Orange = 99,

        OrangeRed = 100,

        Orchid = 101,

        PaleGoldenrod = 102,

        PaleGreen = 103,

        PaleTurquoise = 104,

        PaleVioletRed = 105,

        PapayaWhip = 106,

        PeachPuff = 107,

        Peru = 108,

        Pink = 109,

        Plum = 110,

        PowderBlue = 111,

        Purple = 112,

        Red = 113,

        RosyBrown = 114,

        RoyalBlue = 115,

        SaddleBrown = 116,

        Salmon = 117,

        SandyBrown = 118,

        SeaGreen = 119,

        SeaShell = 120,

        Sienna = 121,

        Silver = 122,

        SkyBlue = 123,

        SlateBlue = 124,

        SlateGray = 125,

        Snow = 126,

        SpringGreen = 127,

        SteelBlue = 128,

        Tan = 129,

        Teal = 130,

        Thistle = 131,

        Tomato = 132,

        Transparent = 133,

        Turquoise = 134,

        Violet = 135,

        Wheat = 136,

        White = 137,

        WhiteSmoke = 138,

        Yellow = 139,

        YellowGreen = 140,
    }
    public enum XLineAlignment
    {
        Near = 0,

        Center = 1,

        Far = 2,

        BaseLine = 3,
    }
    public enum XLinearGradientMode
    {
        Horizontal = 0,

        Vertical = 1,

        ForwardDiagonal = 2,

        BackwardDiagonal = 3,
    }
    public enum XLineCap
    {
        Flat = 0,

        Round = 1,

        Square = 2
    }
    public enum XLineJoin
    {
        Miter = 0,

        Round = 1,

        Bevel = 2,
    }
    public enum XMatrixOrder
    {
        Prepend = 0,

        Append = 1,
    }
    public enum XPageDirection
    {
        Downwards = 0,

        [Obsolete("Not implemeted - yagni")]
        Upwards = 1,
    }
    public enum XSmoothingMode
    {
        Invalid = -1,

        Default = 0,

        HighSpeed = 1,

        HighQuality = 2,

        None = 3,

        AntiAlias = 4,
    }
    public enum XStringAlignment
    {
        Near = 0,

        Center = 1,

        Far = 2,
    }
    public enum XStyleSimulations
    {
        None = 0,

        BoldSimulation = 1,

        ItalicSimulation = 2,

        BoldItalicSimulation = ItalicSimulation | BoldSimulation,
    }
    public enum XSweepDirection
    {
        Counterclockwise = 0,

        Clockwise = 1,
    }
    internal class CoreGraphicsPath
    {
        const byte PathPointTypeStart = 0;
        const byte PathPointTypeLine = 1;
        const byte PathPointTypeBezier = 3;
        const byte PathPointTypePathTypeMask = 0x07;
        const byte PathPointTypeCloseSubpath = 0x80;

        public CoreGraphicsPath()
        { }

        public CoreGraphicsPath(CoreGraphicsPath path)
        {
            _points = new List<XPoint>(path._points);
            _types = new List<byte>(path._types);
        }

        public void MoveOrLineTo(double x, double y)
        {
            if (_types.Count == 0 || (_types[_types.Count - 1] & PathPointTypeCloseSubpath) == PathPointTypeCloseSubpath)
                MoveTo(x, y);
            else
                LineTo(x, y, false);
        }

        public void MoveTo(double x, double y)
        {
            _points.Add(new XPoint(x, y));
            _types.Add(PathPointTypeStart);
        }

        public void LineTo(double x, double y, bool closeSubpath)
        {
            if (_points.Count > 0 && _points[_points.Count - 1].Equals(new XPoint(x, y)))
                return;

            _points.Add(new XPoint(x, y));
            _types.Add((byte)(PathPointTypeLine | (closeSubpath ? PathPointTypeCloseSubpath : 0)));
        }

        public void BezierTo(double x1, double y1, double x2, double y2, double x3, double y3, bool closeSubpath)
        {
            _points.Add(new XPoint(x1, y1));
            _types.Add(PathPointTypeBezier);
            _points.Add(new XPoint(x2, y2));
            _types.Add(PathPointTypeBezier);
            _points.Add(new XPoint(x3, y3));
            _types.Add((byte)(PathPointTypeBezier | (closeSubpath ? PathPointTypeCloseSubpath : 0)));
        }

        public void QuadrantArcTo(double x, double y, double width, double height, int quadrant, bool clockwise)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException("width");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height");

            double w = Const.κ * width;
            double h = Const.κ * height;
            double x1, y1, x2, y2, x3, y3;
            switch (quadrant)
            {
                case 1:
                    if (clockwise)
                    {
                        x1 = x + w;
                        y1 = y - height;
                        x2 = x + width;
                        y2 = y - h;
                        x3 = x + width;
                        y3 = y;
                    }
                    else
                    {
                        x1 = x + width;
                        y1 = y - h;
                        x2 = x + w;
                        y2 = y - height;
                        x3 = x;
                        y3 = y - height;
                    }
                    break;

                case 2:
                    if (clockwise)
                    {
                        x1 = x - width;
                        y1 = y - h;
                        x2 = x - w;
                        y2 = y - height;
                        x3 = x;
                        y3 = y - height;
                    }
                    else
                    {
                        x1 = x - w;
                        y1 = y - height;
                        x2 = x - width;
                        y2 = y - h;
                        x3 = x - width;
                        y3 = y;
                    }
                    break;

                case 3:
                    if (clockwise)
                    {
                        x1 = x - w;
                        y1 = y + height;
                        x2 = x - width;
                        y2 = y + h;
                        x3 = x - width;
                        y3 = y;
                    }
                    else
                    {
                        x1 = x - width;
                        y1 = y + h;
                        x2 = x - w;
                        y2 = y + height;
                        x3 = x;
                        y3 = y + height;
                    }
                    break;

                case 4:
                    if (clockwise)
                    {
                        x1 = x + width;
                        y1 = y + h;
                        x2 = x + w;
                        y2 = y + height;
                        x3 = x;
                        y3 = y + height;
                    }
                    else
                    {
                        x1 = x + w;
                        y1 = y + height;
                        x2 = x + width;
                        y2 = y + h;
                        x3 = x + width;
                        y3 = y;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("quadrant");
            }
            BezierTo(x1, y1, x2, y2, x3, y3, false);
        }

        public void CloseSubpath()
        {
            int count = _types.Count;
            if (count > 0)
                _types[count - 1] |= PathPointTypeCloseSubpath;
        }

        XFillMode FillMode
        {
            get { return _fillMode; }
            set { _fillMode = value; }
        }
        XFillMode _fillMode;

        public void AddArc(double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            XMatrix matrix = XMatrix.Identity;
            List<XPoint> points = GeometryHelper.BezierCurveFromArc(x, y, width, height, startAngle, sweepAngle, PathStart.MoveTo1st, ref matrix);
            int count = points.Count;
            Debug.Assert((count + 2) % 3 == 0);

            MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
                BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y, points[idx + 2].X, points[idx + 2].Y, false);
        }

        public void AddArc(XPoint point1, XPoint point2, XSize size, double rotationAngle, bool isLargeArg, XSweepDirection sweepDirection)
        {
            List<XPoint> points = GeometryHelper.BezierCurveFromArc(point1, point2, size, rotationAngle, isLargeArg,
                sweepDirection == XSweepDirection.Clockwise, PathStart.MoveTo1st);
            int count = points.Count;
            Debug.Assert((count + 2) % 3 == 0);

            MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
                BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y, points[idx + 2].X, points[idx + 2].Y, false);
        }

        public void AddCurve(XPoint[] points, double tension)
        {
            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("AddCurve requires two or more points.", "points");

            tension /= 3;
            MoveOrLineTo(points[0].X, points[0].Y);
            if (count == 2)
            {
                ToCurveSegment(points[0], points[0], points[1], points[1], tension);
            }
            else
            {
                ToCurveSegment(points[0], points[0], points[1], points[2], tension);
                for (int idx = 1; idx < count - 2; idx++)
                {
                    ToCurveSegment(points[idx - 1], points[idx], points[idx + 1], points[idx + 2], tension);
                }
                ToCurveSegment(points[count - 3], points[count - 2], points[count - 1], points[count - 1], tension);
            }
        }

        void ToCurveSegment(XPoint pt0, XPoint pt1, XPoint pt2, XPoint pt3, double tension3)
        {
            BezierTo(
                pt1.X + tension3 * (pt2.X - pt0.X), pt1.Y + tension3 * (pt2.Y - pt0.Y),
                pt2.X - tension3 * (pt3.X - pt1.X), pt2.Y - tension3 * (pt3.Y - pt1.Y),
                pt2.X, pt2.Y,
                false);
        }

        public XPoint[] PathPoints { get { return _points.ToArray(); } }

        public byte[] PathTypes { get { return _types.ToArray(); } }

        readonly List<XPoint> _points = new List<XPoint>();
        readonly List<byte> _types = new List<byte>();
    }
    internal sealed class FontFamilyCache
    {
        FontFamilyCache()
        {
            _familiesByName = new Dictionary<string, FontFamilyInternal>(StringComparer.OrdinalIgnoreCase);
        }

        public static FontFamilyInternal GetFamilyByName(string familyName)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal family;
                Singleton._familiesByName.TryGetValue(familyName, out family);
                return family;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static FontFamilyInternal CacheOrGetFontFamily(FontFamilyInternal fontFamily)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal existingFontFamily;
                if (Singleton._familiesByName.TryGetValue(fontFamily.Name, out existingFontFamily))
                {

                    return existingFontFamily;
                }
                Singleton._familiesByName.Add(fontFamily.Name, fontFamily);
                return fontFamily;
            }
            finally { Lock.ExitFontFactory(); }
        }

        static FontFamilyCache Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    try
                    {
                        Lock.EnterFontFactory();
                        if (_singleton == null)
                            _singleton = new FontFamilyCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }
        static volatile FontFamilyCache _singleton;

        internal static string GetCacheState()
        {
            StringBuilder state = new StringBuilder();
            state.Append("====================\n");
            state.Append("Font families by name\n");
            Dictionary<string, FontFamilyInternal>.KeyCollection familyKeys = Singleton._familiesByName.Keys;
            int count = familyKeys.Count;
            string[] keys = new string[count];
            familyKeys.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, Singleton._familiesByName[key].DebuggerDisplay);
            state.Append("\n");
            return state.ToString();
        }

        readonly Dictionary<string, FontFamilyInternal> _familiesByName;
    }
    internal class FontFamilyInternal
    {
        FontFamilyInternal(string familyName, bool createPlatformObjects)
        {
            _sourceName = _name = familyName;

            if (createPlatformObjects)
            {
                _gdiFontFamily = new GdiFontFamily(familyName);
                _name = _gdiFontFamily.Name;
            }

        }

#if CORE || GDI
        FontFamilyInternal(GdiFontFamily gdiFontFamily)
        {
            _sourceName = _name = gdiFontFamily.Name;
            _gdiFontFamily = gdiFontFamily;
        }
#endif


        internal static FontFamilyInternal GetOrCreateFromName(string familyName, bool createPlatformObject)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal family = FontFamilyCache.GetFamilyByName(familyName);
                if (family == null)
                {
                    family = new FontFamilyInternal(familyName, createPlatformObject);
                    family = FontFamilyCache.CacheOrGetFontFamily(family);
                }
                return family;
            }
            finally { Lock.ExitFontFactory(); }
        }

#if CORE || GDI
        internal static FontFamilyInternal GetOrCreateFromGdi(GdiFontFamily gdiFontFamily)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal fontFamily = new FontFamilyInternal(gdiFontFamily);
                fontFamily = FontFamilyCache.CacheOrGetFontFamily(fontFamily);
                return fontFamily;
            }
            finally { Lock.ExitFontFactory(); }
        }
#endif

        public string SourceName
        {
            get { return _sourceName; }
        }
        readonly string _sourceName;

        public string Name
        {
            get { return _name; }
        }
        readonly string _name;

#if CORE || GDI
        public GdiFontFamily GdiFamily
        {
            get { return _gdiFontFamily; }
        }
        readonly GdiFontFamily _gdiFontFamily;
#endif


        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "FontFamily: '{0}'", Name); }
        }
    }
    static class FontHelper
    {
        public static XSize MeasureString(string text, XFont font, XStringFormat stringFormat_notyetused)
        {
            XSize size = new XSize();

            OpenTypeDescriptor descriptor = FontDescriptorCache.GetOrCreateDescriptorFor(font) as OpenTypeDescriptor;
            if (descriptor != null)
            {
                size.Height = (descriptor.Ascender + descriptor.Descender) * font.Size / font.UnitsPerEm;
                Debug.Assert(descriptor.Ascender > 0);

                bool symbol = descriptor.FontFace.cmap.symbol;
                int length = text.Length;
                int width = 0;
                for (int idx = 0; idx < length; idx++)
                {
                    char ch = text[idx];
                    if (ch < 32)
                        continue;

                    if (symbol)
                    {
                        ch = (char)(ch | (descriptor.FontFace.os2.usFirstCharIndex & 0xFF00));
                    }
                    int glyphIndex = descriptor.CharCodeToGlyphIndex(ch);
                    width += descriptor.GlyphIndexToWidth(glyphIndex);
                }
                size.Width = width * font.Size / descriptor.UnitsPerEm;

                if ((font.GlyphTypeface.StyleSimulations & XStyleSimulations.BoldSimulation) == XStyleSimulations.BoldSimulation)
                {
                    size.Width += length * font.Size * Const.BoldEmphasis;
                }
            }
            Debug.Assert(descriptor != null, "No OpenTypeDescriptor.");
            return size;
        }

#if CORE || GDI
        public static GdiFont CreateFont(string familyName, double emSize, GdiFontStyle style, out XFontSource fontSource)
        {
            fontSource = null;
            GdiFont font;

            font = new GdiFont(familyName, (float)emSize, style, GraphicsUnit.World);
            return font;
        }
#endif


        public static ulong CalcChecksum(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            const uint prime = 65521;
            uint s1 = 0;
            uint s2 = 0;
            int length = buffer.Length;
            int offset = 0;
            while (length > 0)
            {
                int n = 3800;
                if (n > length)
                    n = length;
                length -= n;
                while (--n >= 0)
                {
                    s1 += buffer[offset++];
                    s2 = s2 + s1;
                }
                s1 %= prime;
                s2 %= prime;
            }
            ulong ul1 = (ulong)s2 << 16;
            ul1 = ul1 | s1;
            ulong ul2 = (ulong)buffer.Length;
            return (ul1 << 32) | ul2;
        }

        public static XFontStyle CreateStyle(bool isBold, bool isItalic)
        {
            return (isBold ? XFontStyle.Bold : 0) | (isItalic ? XFontStyle.Italic : 0);
        }
    }
    static class GeometryHelper
    {


        public static List<XPoint> BezierCurveFromArc(double x, double y, double width, double height, double startAngle, double sweepAngle,
            PathStart pathStart, ref XMatrix matrix)
        {
            List<XPoint> points = new List<XPoint>();

            double α = startAngle;
            if (α < 0)
                α = α + (1 + Math.Floor((Math.Abs(α) / 360))) * 360;
            else if (α > 360)
                α = α - Math.Floor(α / 360) * 360;
            Debug.Assert(α >= 0 && α <= 360);

            double β = sweepAngle;
            if (β < -360)
                β = -360;
            else if (β > 360)
                β = 360;

            if (α == 0 && β < 0)
                α = 360;
            else if (α == 360 && β > 0)
                α = 0;

            bool smallAngle = Math.Abs(β) <= 90;

            β = α + β;
            if (β < 0)
                β = β + (1 + Math.Floor((Math.Abs(β) / 360))) * 360;

            bool clockwise = sweepAngle > 0;
            int startQuadrant = Quadrant(α, true, clockwise);
            int endQuadrant = Quadrant(β, false, clockwise);

            if (startQuadrant == endQuadrant && smallAngle)
                AppendPartialArcQuadrant(points, x, y, width, height, α, β, pathStart, matrix);
            else
            {
                int currentQuadrant = startQuadrant;
                bool firstLoop = true;
                do
                {
                    if (currentQuadrant == startQuadrant && firstLoop)
                    {
                        double ξ = currentQuadrant * 90 + (clockwise ? 90 : 0);
                        AppendPartialArcQuadrant(points, x, y, width, height, α, ξ, pathStart, matrix);
                    }
                    else if (currentQuadrant == endQuadrant)
                    {
                        double ξ = currentQuadrant * 90 + (clockwise ? 0 : 90);
                        AppendPartialArcQuadrant(points, x, y, width, height, ξ, β, PathStart.Ignore1st, matrix);
                    }
                    else
                    {
                        double ξ1 = currentQuadrant * 90 + (clockwise ? 0 : 90);
                        double ξ2 = currentQuadrant * 90 + (clockwise ? 90 : 0);
                        AppendPartialArcQuadrant(points, x, y, width, height, ξ1, ξ2, PathStart.Ignore1st, matrix);
                    }

                    if (currentQuadrant == endQuadrant && smallAngle)
                        break;
                    smallAngle = true;

                    if (clockwise)
                        currentQuadrant = currentQuadrant == 3 ? 0 : currentQuadrant + 1;
                    else
                        currentQuadrant = currentQuadrant == 0 ? 3 : currentQuadrant - 1;

                    firstLoop = false;
                } while (true);
            }
            return points;
        }

        static int Quadrant(double φ, bool start, bool clockwise)
        {
            Debug.Assert(φ >= 0);
            if (φ > 360)
                φ = φ - Math.Floor(φ / 360) * 360;

            int quadrant = (int)(φ / 90);
            if (quadrant * 90 == φ)
            {
                if ((start && !clockwise) || (!start && clockwise))
                    quadrant = quadrant == 0 ? 3 : quadrant - 1;
            }
            else
                quadrant = clockwise ? ((int)Math.Floor(φ / 90)) % 4 : (int)Math.Floor(φ / 90);
            return quadrant;
        }

        static void AppendPartialArcQuadrant(List<XPoint> points, double x, double y, double width, double height, double α, double β, PathStart pathStart, XMatrix matrix)
        {
            Debug.Assert(α >= 0 && α <= 360);
            Debug.Assert(β >= 0);
            if (β > 360)
                β = β - Math.Floor(β / 360) * 360;
            Debug.Assert(Math.Abs(α - β) <= 90);

            double δx = width / 2;
            double δy = height / 2;

            double x0 = x + δx;
            double y0 = y + δy;

            bool reflect = false;
            if (α >= 180 && β >= 180)
            {
                α -= 180;
                β -= 180;
                reflect = true;
            }

            double cosα, cosβ, sinα, sinβ;
            if (width == height)
            {
                α = α * Calc.Deg2Rad;
                β = β * Calc.Deg2Rad;
            }
            else
            {
                α = α * Calc.Deg2Rad;
                sinα = Math.Sin(α);
                if (Math.Abs(sinα) > 1E-10)
                    α = Math.PI / 2 - Math.Atan(δy * Math.Cos(α) / (δx * sinα));
                β = β * Calc.Deg2Rad;
                sinβ = Math.Sin(β);
                if (Math.Abs(sinβ) > 1E-10)
                    β = Math.PI / 2 - Math.Atan(δy * Math.Cos(β) / (δx * sinβ));
            }

            double κ = 4 * (1 - Math.Cos((α - β) / 2)) / (3 * Math.Sin((β - α) / 2));
            sinα = Math.Sin(α);
            cosα = Math.Cos(α);
            sinβ = Math.Sin(β);
            cosβ = Math.Cos(β);

            if (!reflect)
            {
                switch (pathStart)
                {
                    case PathStart.MoveTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 + δx * cosα, y0 + δy * sinα)));
                        break;

                    case PathStart.LineTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 + δx * cosα, y0 + δy * sinα)));
                        break;

                    case PathStart.Ignore1st:
                        break;
                }
                points.Add(matrix.Transform(new XPoint(x0 + δx * (cosα - κ * sinα), y0 + δy * (sinα + κ * cosα))));
                points.Add(matrix.Transform(new XPoint(x0 + δx * (cosβ + κ * sinβ), y0 + δy * (sinβ - κ * cosβ))));
                points.Add(matrix.Transform(new XPoint(x0 + δx * cosβ, y0 + δy * sinβ)));
            }
            else
            {
                switch (pathStart)
                {
                    case PathStart.MoveTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 - δx * cosα, y0 - δy * sinα)));
                        break;

                    case PathStart.LineTo1st:
                        points.Add(matrix.Transform(new XPoint(x0 - δx * cosα, y0 - δy * sinα)));
                        break;

                    case PathStart.Ignore1st:
                        break;
                }
                points.Add(matrix.Transform(new XPoint(x0 - δx * (cosα - κ * sinα), y0 - δy * (sinα + κ * cosα))));
                points.Add(matrix.Transform(new XPoint(x0 - δx * (cosβ + κ * sinβ), y0 - δy * (sinβ - κ * cosβ))));
                points.Add(matrix.Transform(new XPoint(x0 - δx * cosβ, y0 - δy * sinβ)));
            }
        }

        public static List<XPoint> BezierCurveFromArc(XPoint point1, XPoint point2, XSize size,
            double rotationAngle, bool isLargeArc, bool clockwise, PathStart pathStart)
        {
            double δx = size.Width;
            double δy = size.Height;
            Debug.Assert(δx * δy > 0);
            double factor = δy / δx;
            bool isCounterclockwise = !clockwise;

            XMatrix matrix = new XMatrix();
            matrix.RotateAppend(-rotationAngle);
            matrix.ScaleAppend(δy / δx, 1);
            XPoint pt1 = matrix.Transform(point1);
            XPoint pt2 = matrix.Transform(point2);

            XPoint midPoint = new XPoint((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
            XVector vect = pt2 - pt1;
            double halfChord = vect.Length / 2;

            XVector vectRotated;

            if (isLargeArc == isCounterclockwise)
                vectRotated = new XVector(-vect.Y, vect.X);
            else
                vectRotated = new XVector(vect.Y, -vect.X);

            vectRotated.Normalize();

            double centerDistance = Math.Sqrt(δy * δy - halfChord * halfChord);
            if (double.IsNaN(centerDistance))
                centerDistance = 0;

            XPoint center = midPoint + centerDistance * vectRotated;

            double α = Math.Atan2(pt1.Y - center.Y, pt1.X - center.X);
            double β = Math.Atan2(pt2.Y - center.Y, pt2.X - center.X);

            if (isLargeArc == (Math.Abs(β - α) < Math.PI))
            {
                if (α < β)
                    α += 2 * Math.PI;
                else
                    β += 2 * Math.PI;
            }

            matrix.Invert();
            double sweepAngle = β - α;

            return BezierCurveFromArc(center.X - δx * factor, center.Y - δy, 2 * δx * factor, 2 * δy,
              α / Calc.Deg2Rad, sweepAngle / Calc.Deg2Rad, pathStart, ref matrix);
        }
    }
    internal class GraphicsStateStack
    {
        public GraphicsStateStack(XGraphics gfx)
        {
            _current = new InternalGraphicsState(gfx);
        }

        public int Count
        {
            get { return _stack.Count; }
        }

        public void Push(InternalGraphicsState state)
        {
            _stack.Push(state);
            state.Pushed();
        }

        public int Restore(InternalGraphicsState state)
        {
            if (!_stack.Contains(state))
                throw new ArgumentException("State not on stack.", "state");
            if (state.Invalid)
                throw new ArgumentException("State already restored.", "state");

            int count = 1;
            InternalGraphicsState top = _stack.Pop();
            top.Popped();
            while (top != state)
            {
                count++;
                state.Invalid = true;
                top = _stack.Pop();
                top.Popped();
            }
            state.Invalid = true;
            return count;
        }

        public InternalGraphicsState Current
        {
            get
            {
                if (_stack.Count == 0)
                    return _current;
                return _stack.Peek();
            }
        }

        readonly InternalGraphicsState _current;

        readonly Stack<InternalGraphicsState> _stack = new Stack<InternalGraphicsState>();
    }
    internal class InternalGraphicsState
    {
        public InternalGraphicsState(XGraphics gfx)
        {
            _gfx = gfx;
        }

        public InternalGraphicsState(XGraphics gfx, XGraphicsState state)
        {
            _gfx = gfx;
            State = state;
            State.InternalState = this;
        }

        public InternalGraphicsState(XGraphics gfx, XGraphicsContainer container)
        {
            _gfx = gfx;
            container.InternalState = this;
        }

        public XMatrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        XMatrix _transform;

        public void Pushed()
        {


        }

        public void Popped()
        {
            Invalid = true;



        }

        public bool Invalid;

        readonly XGraphics _gfx;

        internal XGraphicsState State;
    }
    internal interface IXGraphicsRenderer
    {
        void Close();

        void DrawLine(XPen pen, double x1, double y1, double x2, double y2);

        void DrawLines(XPen pen, XPoint[] points);

        void DrawBezier(XPen pen, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4);

        void DrawBeziers(XPen pen, XPoint[] points);

        void DrawCurve(XPen pen, XPoint[] points, double tension);

        void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle);

        void DrawRectangle(XPen pen, XBrush brush, double x, double y, double width, double height);

        void DrawRectangles(XPen pen, XBrush brush, XRect[] rects);

        void DrawRoundedRectangle(XPen pen, XBrush brush, double x, double y, double width, double height, double ellipseWidth, double ellipseHeight);

        void DrawEllipse(XPen pen, XBrush brush, double x, double y, double width, double height);

        void DrawPolygon(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode);

        void DrawPie(XPen pen, XBrush brush, double x, double y, double width, double height, double startAngle, double sweepAngle);

        void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points, double tension, XFillMode fillmode);

        void DrawPath(XPen pen, XBrush brush, XGraphicsPath path);

        void DrawString(string s, XFont font, XBrush brush, XRect layoutRectangle, XStringFormat format);

        void DrawImage(XImage image, double x, double y, double width, double height);
        void DrawImage(XImage image, XRect destRect, XRect srcRect, XGraphicsUnit srcUnit);

        void Save(XGraphicsState state);

        void Restore(XGraphicsState state);

        void BeginContainer(XGraphicsContainer container, XRect dstrect, XRect srcrect, XGraphicsUnit unit);

        void EndContainer(XGraphicsContainer container);

        void AddTransform(XMatrix transform, XMatrixOrder matrixOrder);

        void SetClip(XGraphicsPath path, XCombineMode combineMode);

        void ResetClip();

        void WriteComment(string comment);

    }
    public class XPdfFontOptions
    {
        internal XPdfFontOptions() { }

        [Obsolete("Must not specify an embedding option anymore.")]
        public XPdfFontOptions(PdfFontEncoding encoding, PdfFontEmbedding embedding)
        {
            _fontEncoding = encoding;
        }

        public XPdfFontOptions(PdfFontEncoding encoding)
        {
            _fontEncoding = encoding;
        }

        [Obsolete("Must not specify an embedding option anymore.")]
        public XPdfFontOptions(PdfFontEmbedding embedding)
        {
            _fontEncoding = PdfFontEncoding.WinAnsi;
        }

        public PdfFontEmbedding FontEmbedding
        {
            get { return PdfFontEmbedding.Always; }
        }

        public PdfFontEncoding FontEncoding
        {
            get { return _fontEncoding; }
        }
        readonly PdfFontEncoding _fontEncoding;

        public static XPdfFontOptions WinAnsiDefault
        {
            get { return new XPdfFontOptions(PdfFontEncoding.WinAnsi); }
        }

        public static XPdfFontOptions UnicodeDefault
        {
            get { return new XPdfFontOptions(PdfFontEncoding.Unicode); }
        }
    }
    public class XBitmapDecoder
    {
        internal XBitmapDecoder()
        { }

        public static XBitmapDecoder GetPngDecoder()
        {
            return new XPngBitmapDecoder();
        }
    }
    internal sealed class XPngBitmapDecoder : XBitmapDecoder
    {
        internal XPngBitmapDecoder()
        { }
    }
    public abstract class XBitmapEncoder
    {
        internal XBitmapEncoder()
        {
        }

        public static XBitmapEncoder GetPngEncoder()
        {
            return new XPngBitmapEncoder();
        }

        public XBitmapSource Source
        {
            get { return _source; }
            set { _source = value; }
        }
        XBitmapSource _source;

        public abstract void Save(Stream stream);
    }
    internal sealed class XPngBitmapEncoder : XBitmapEncoder
    {
        internal XPngBitmapEncoder()
        { }

        public override void Save(Stream stream)
        {
            if (Source == null)
                throw new InvalidOperationException("No image source.");
#if CORE_WITH_GDI || GDI
            if (Source.AssociatedGraphics != null)
            {
                Source.DisassociateWithGraphics();
                Debug.Assert(Source.AssociatedGraphics == null);
            }
            try
            {
                Lock.EnterGdiPlus();
                Source._gdiImage.Save(stream, ImageFormat.Png);
            }
            finally { Lock.ExitGdiPlus(); }
#endif
        }
    }
    public sealed class XBitmapImage : XBitmapSource
    {
        internal XBitmapImage(int width, int height)
        {
#if GDI || CORE_WITH_GDI
            try
            {
                Lock.EnterGdiPlus();
                _gdiImage = new Bitmap(width, height);
            }
            finally { Lock.ExitGdiPlus(); }
#endif

#if CORE || GDI && !WPF     
            Initialize();
#endif
        }

        public static XBitmapSource CreateBitmap(int width, int height)
        {
            return new XBitmapImage(width, height);
        }
    }
    public abstract class XBitmapSource : XImage
    {
        public override int PixelWidth
        {
            get
            {
#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public override int PixelHeight
        {
            get
            {
#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }
    }
    public static class XBrushes
    {
        public static XSolidBrush AliceBlue
        {
            get { return new XSolidBrush(XColors.AliceBlue, true); }
        }

        public static XSolidBrush AntiqueWhite
        {
            get { return new XSolidBrush(XColors.AntiqueWhite, true); }
        }

        public static XSolidBrush Aqua
        {
            get { return new XSolidBrush(XColors.Aqua, true); }
        }

        public static XSolidBrush Aquamarine
        {
            get { return new XSolidBrush(XColors.Aquamarine, true); }
        }

        public static XSolidBrush Azure
        {
            get { return new XSolidBrush(XColors.Azure, true); }
        }

        public static XSolidBrush Beige
        {
            get { return new XSolidBrush(XColors.Beige, true); }
        }

        public static XSolidBrush Bisque
        {
            get { return new XSolidBrush(XColors.Bisque, true); }
        }

        public static XSolidBrush Black
        {
            get { return new XSolidBrush(XColors.Black, true); }
        }

        public static XSolidBrush BlanchedAlmond
        {
            get { return new XSolidBrush(XColors.BlanchedAlmond, true); }
        }

        public static XSolidBrush Blue
        {
            get { return new XSolidBrush(XColors.Blue, true); }
        }

        public static XSolidBrush BlueViolet
        {
            get { return new XSolidBrush(XColors.BlueViolet, true); }
        }

        public static XSolidBrush Brown
        {
            get { return new XSolidBrush(XColors.Brown, true); }
        }

        public static XSolidBrush BurlyWood
        {
            get { return new XSolidBrush(XColors.BurlyWood, true); }
        }

        public static XSolidBrush CadetBlue
        {
            get { return new XSolidBrush(XColors.CadetBlue, true); }
        }

        public static XSolidBrush Chartreuse
        {
            get { return new XSolidBrush(XColors.Chartreuse, true); }
        }

        public static XSolidBrush Chocolate
        {
            get { return new XSolidBrush(XColors.Chocolate, true); }
        }

        public static XSolidBrush Coral
        {
            get { return new XSolidBrush(XColors.Coral, true); }
        }

        public static XSolidBrush CornflowerBlue
        {
            get { return new XSolidBrush(XColors.CornflowerBlue, true); }
        }

        public static XSolidBrush Cornsilk
        {
            get { return new XSolidBrush(XColors.Cornsilk, true); }
        }

        public static XSolidBrush Crimson
        {
            get { return new XSolidBrush(XColors.Crimson, true); }
        }

        public static XSolidBrush Cyan
        {
            get { return new XSolidBrush(XColors.Cyan, true); }
        }

        public static XSolidBrush DarkBlue
        {
            get { return new XSolidBrush(XColors.DarkBlue, true); }
        }

        public static XSolidBrush DarkCyan
        {
            get { return new XSolidBrush(XColors.DarkCyan, true); }
        }

        public static XSolidBrush DarkGoldenrod
        {
            get { return new XSolidBrush(XColors.DarkGoldenrod, true); }
        }

        public static XSolidBrush DarkGray
        {
            get { return new XSolidBrush(XColors.DarkGray, true); }
        }

        public static XSolidBrush DarkGreen
        {
            get { return new XSolidBrush(XColors.DarkGreen, true); }
        }

        public static XSolidBrush DarkKhaki
        {
            get { return new XSolidBrush(XColors.DarkKhaki, true); }
        }

        public static XSolidBrush DarkMagenta
        {
            get { return new XSolidBrush(XColors.DarkMagenta, true); }
        }

        public static XSolidBrush DarkOliveGreen
        {
            get { return new XSolidBrush(XColors.DarkOliveGreen, true); }
        }

        public static XSolidBrush DarkOrange
        {
            get { return new XSolidBrush(XColors.DarkOrange, true); }
        }

        public static XSolidBrush DarkOrchid
        {
            get { return new XSolidBrush(XColors.DarkOrchid, true); }
        }

        public static XSolidBrush DarkRed
        {
            get { return new XSolidBrush(XColors.DarkRed, true); }
        }

        public static XSolidBrush DarkSalmon
        {
            get { return new XSolidBrush(XColors.DarkSalmon, true); }
        }

        public static XSolidBrush DarkSeaGreen
        {
            get { return new XSolidBrush(XColors.DarkSeaGreen, true); }
        }

        public static XSolidBrush DarkSlateBlue
        {
            get { return new XSolidBrush(XColors.DarkSlateBlue, true); }
        }

        public static XSolidBrush DarkSlateGray
        {
            get { return new XSolidBrush(XColors.DarkSlateGray, true); }
        }

        public static XSolidBrush DarkTurquoise
        {
            get { return new XSolidBrush(XColors.DarkTurquoise, true); }
        }

        public static XSolidBrush DarkViolet
        {
            get { return new XSolidBrush(XColors.DarkViolet, true); }
        }

        public static XSolidBrush DeepPink
        {
            get { return new XSolidBrush(XColors.DeepPink, true); }
        }

        public static XSolidBrush DeepSkyBlue
        {
            get { return new XSolidBrush(XColors.DeepSkyBlue, true); }
        }

        public static XSolidBrush DimGray
        {
            get { return new XSolidBrush(XColors.DimGray, true); }
        }

        public static XSolidBrush DodgerBlue
        {
            get { return new XSolidBrush(XColors.DodgerBlue, true); }
        }

        public static XSolidBrush Firebrick
        {
            get { return new XSolidBrush(XColors.Firebrick, true); }
        }

        public static XSolidBrush FloralWhite
        {
            get { return new XSolidBrush(XColors.FloralWhite, true); }
        }

        public static XSolidBrush ForestGreen
        {
            get { return new XSolidBrush(XColors.ForestGreen, true); }
        }

        public static XSolidBrush Fuchsia
        {
            get { return new XSolidBrush(XColors.Fuchsia, true); }
        }

        public static XSolidBrush Gainsboro
        {
            get { return new XSolidBrush(XColors.Gainsboro, true); }
        }

        public static XSolidBrush GhostWhite
        {
            get { return new XSolidBrush(XColors.GhostWhite, true); }
        }

        public static XSolidBrush Gold
        {
            get { return new XSolidBrush(XColors.Gold, true); }
        }

        public static XSolidBrush Goldenrod
        {
            get { return new XSolidBrush(XColors.Goldenrod, true); }
        }

        public static XSolidBrush Gray
        {
            get { return new XSolidBrush(XColors.Gray, true); }
        }

        public static XSolidBrush Green
        {
            get { return new XSolidBrush(XColors.Green, true); }
        }

        public static XSolidBrush GreenYellow
        {
            get { return new XSolidBrush(XColors.GreenYellow, true); }
        }

        public static XSolidBrush Honeydew
        {
            get { return new XSolidBrush(XColors.Honeydew, true); }
        }

        public static XSolidBrush HotPink
        {
            get { return new XSolidBrush(XColors.HotPink, true); }
        }

        public static XSolidBrush IndianRed
        {
            get { return new XSolidBrush(XColors.IndianRed, true); }
        }

        public static XSolidBrush Indigo
        {
            get { return new XSolidBrush(XColors.Indigo, true); }
        }

        public static XSolidBrush Ivory
        {
            get { return new XSolidBrush(XColors.Ivory, true); }
        }

        public static XSolidBrush Khaki
        {
            get { return new XSolidBrush(XColors.Khaki, true); }
        }

        public static XSolidBrush Lavender
        {
            get { return new XSolidBrush(XColors.Lavender, true); }
        }

        public static XSolidBrush LavenderBlush
        {
            get { return new XSolidBrush(XColors.LavenderBlush, true); }
        }

        public static XSolidBrush LawnGreen
        {
            get { return new XSolidBrush(XColors.LawnGreen, true); }
        }

        public static XSolidBrush LemonChiffon
        {
            get { return new XSolidBrush(XColors.LemonChiffon, true); }
        }

        public static XSolidBrush LightBlue
        {
            get { return new XSolidBrush(XColors.LightBlue, true); }
        }

        public static XSolidBrush LightCoral
        {
            get { return new XSolidBrush(XColors.LightCoral, true); }
        }

        public static XSolidBrush LightCyan
        {
            get { return new XSolidBrush(XColors.LightCyan, true); }
        }

        public static XSolidBrush LightGoldenrodYellow
        {
            get { return new XSolidBrush(XColors.LightGoldenrodYellow, true); }
        }

        public static XSolidBrush LightGray
        {
            get { return new XSolidBrush(XColors.LightGray, true); }
        }

        public static XSolidBrush LightGreen
        {
            get { return new XSolidBrush(XColors.LightGreen, true); }
        }

        public static XSolidBrush LightPink
        {
            get { return new XSolidBrush(XColors.LightPink, true); }
        }

        public static XSolidBrush LightSalmon
        {
            get { return new XSolidBrush(XColors.LightSalmon, true); }
        }

        public static XSolidBrush LightSeaGreen
        {
            get { return new XSolidBrush(XColors.LightSeaGreen, true); }
        }

        public static XSolidBrush LightSkyBlue
        {
            get { return new XSolidBrush(XColors.LightSkyBlue, true); }
        }

        public static XSolidBrush LightSlateGray
        {
            get { return new XSolidBrush(XColors.LightSlateGray, true); }
        }

        public static XSolidBrush LightSteelBlue
        {
            get { return new XSolidBrush(XColors.LightSteelBlue, true); }
        }

        public static XSolidBrush LightYellow
        {
            get { return new XSolidBrush(XColors.LightYellow, true); }
        }

        public static XSolidBrush Lime
        {
            get { return new XSolidBrush(XColors.Lime, true); }
        }

        public static XSolidBrush LimeGreen
        {
            get { return new XSolidBrush(XColors.LimeGreen, true); }
        }

        public static XSolidBrush Linen
        {
            get { return new XSolidBrush(XColors.Linen, true); }
        }

        public static XSolidBrush Magenta
        {
            get { return new XSolidBrush(XColors.Magenta, true); }
        }

        public static XSolidBrush Maroon
        {
            get { return new XSolidBrush(XColors.Maroon, true); }
        }

        public static XSolidBrush MediumAquamarine
        {
            get { return new XSolidBrush(XColors.MediumAquamarine, true); }
        }

        public static XSolidBrush MediumBlue
        {
            get { return new XSolidBrush(XColors.MediumBlue, true); }
        }

        public static XSolidBrush MediumOrchid
        {
            get { return new XSolidBrush(XColors.MediumOrchid, true); }
        }

        public static XSolidBrush MediumPurple
        {
            get { return new XSolidBrush(XColors.MediumPurple, true); }
        }

        public static XSolidBrush MediumSeaGreen
        {
            get { return new XSolidBrush(XColors.MediumSeaGreen, true); }
        }

        public static XSolidBrush MediumSlateBlue
        {
            get { return new XSolidBrush(XColors.MediumSlateBlue, true); }
        }

        public static XSolidBrush MediumSpringGreen
        {
            get { return new XSolidBrush(XColors.MediumSpringGreen, true); }
        }

        public static XSolidBrush MediumTurquoise
        {
            get { return new XSolidBrush(XColors.MediumTurquoise, true); }
        }

        public static XSolidBrush MediumVioletRed
        {
            get { return new XSolidBrush(XColors.MediumVioletRed, true); }
        }

        public static XSolidBrush MidnightBlue
        {
            get { return new XSolidBrush(XColors.MidnightBlue, true); }
        }

        public static XSolidBrush MintCream
        {
            get { return new XSolidBrush(XColors.MintCream, true); }
        }

        public static XSolidBrush MistyRose
        {
            get { return new XSolidBrush(XColors.MistyRose, true); }
        }

        public static XSolidBrush Moccasin
        {
            get { return new XSolidBrush(XColors.Moccasin, true); }
        }

        public static XSolidBrush NavajoWhite
        {
            get { return new XSolidBrush(XColors.NavajoWhite, true); }
        }

        public static XSolidBrush Navy
        {
            get { return new XSolidBrush(XColors.Navy, true); }
        }

        public static XSolidBrush OldLace
        {
            get { return new XSolidBrush(XColors.OldLace, true); }
        }

        public static XSolidBrush Olive
        {
            get { return new XSolidBrush(XColors.Olive, true); }
        }

        public static XSolidBrush OliveDrab
        {
            get { return new XSolidBrush(XColors.OliveDrab, true); }
        }

        public static XSolidBrush Orange
        {
            get { return new XSolidBrush(XColors.Orange, true); }
        }

        public static XSolidBrush OrangeRed
        {
            get { return new XSolidBrush(XColors.OrangeRed, true); }
        }

        public static XSolidBrush Orchid
        {
            get { return new XSolidBrush(XColors.Orchid, true); }
        }

        public static XSolidBrush PaleGoldenrod
        {
            get { return new XSolidBrush(XColors.PaleGoldenrod, true); }
        }

        public static XSolidBrush PaleGreen
        {
            get { return new XSolidBrush(XColors.PaleGreen, true); }
        }

        public static XSolidBrush PaleTurquoise
        {
            get { return new XSolidBrush(XColors.PaleTurquoise, true); }
        }

        public static XSolidBrush PaleVioletRed
        {
            get { return new XSolidBrush(XColors.PaleVioletRed, true); }
        }

        public static XSolidBrush PapayaWhip
        {
            get { return new XSolidBrush(XColors.PapayaWhip, true); }
        }

        public static XSolidBrush PeachPuff
        {
            get { return new XSolidBrush(XColors.PeachPuff, true); }
        }

        public static XSolidBrush Peru
        {
            get { return new XSolidBrush(XColors.Peru, true); }
        }

        public static XSolidBrush Pink
        {
            get { return new XSolidBrush(XColors.Pink, true); }
        }

        public static XSolidBrush Plum
        {
            get { return new XSolidBrush(XColors.Plum, true); }
        }

        public static XSolidBrush PowderBlue
        {
            get { return new XSolidBrush(XColors.PowderBlue, true); }
        }

        public static XSolidBrush Purple
        {
            get { return new XSolidBrush(XColors.Purple, true); }
        }

        public static XSolidBrush Red
        {
            get { return new XSolidBrush(XColors.Red, true); }
        }

        public static XSolidBrush RosyBrown
        {
            get { return new XSolidBrush(XColors.RosyBrown, true); }
        }

        public static XSolidBrush RoyalBlue
        {
            get { return new XSolidBrush(XColors.RoyalBlue, true); }
        }

        public static XSolidBrush SaddleBrown
        {
            get { return new XSolidBrush(XColors.SaddleBrown, true); }
        }

        public static XSolidBrush Salmon
        {
            get { return new XSolidBrush(XColors.Salmon, true); }
        }

        public static XSolidBrush SandyBrown
        {
            get { return new XSolidBrush(XColors.SandyBrown, true); }
        }

        public static XSolidBrush SeaGreen
        {
            get { return new XSolidBrush(XColors.SeaGreen, true); }
        }

        public static XSolidBrush SeaShell
        {
            get { return new XSolidBrush(XColors.SeaShell, true); }
        }

        public static XSolidBrush Sienna
        {
            get { return new XSolidBrush(XColors.Sienna, true); }
        }

        public static XSolidBrush Silver
        {
            get { return new XSolidBrush(XColors.Silver, true); }
        }

        public static XSolidBrush SkyBlue
        {
            get { return new XSolidBrush(XColors.SkyBlue, true); }
        }

        public static XSolidBrush SlateBlue
        {
            get { return new XSolidBrush(XColors.SlateBlue, true); }
        }

        public static XSolidBrush SlateGray
        {
            get { return new XSolidBrush(XColors.SlateGray, true); }
        }

        public static XSolidBrush Snow
        {
            get { return new XSolidBrush(XColors.Snow, true); }
        }

        public static XSolidBrush SpringGreen
        {
            get { return new XSolidBrush(XColors.SpringGreen, true); }
        }

        public static XSolidBrush SteelBlue
        {
            get { return new XSolidBrush(XColors.SteelBlue, true); }
        }

        public static XSolidBrush Tan
        {
            get { return new XSolidBrush(XColors.Tan, true); }
        }

        public static XSolidBrush Teal
        {
            get { return new XSolidBrush(XColors.Teal, true); }
        }

        public static XSolidBrush Thistle
        {
            get { return new XSolidBrush(XColors.Thistle, true); }
        }

        public static XSolidBrush Tomato
        {
            get { return new XSolidBrush(XColors.Tomato, true); }
        }

        public static XSolidBrush Transparent
        {
            get { return new XSolidBrush(XColors.Transparent, true); }
        }

        public static XSolidBrush Turquoise
        {
            get { return new XSolidBrush(XColors.Turquoise, true); }
        }

        public static XSolidBrush Violet
        {
            get { return new XSolidBrush(XColors.Violet, true); }
        }

        public static XSolidBrush Wheat
        {
            get { return new XSolidBrush(XColors.Wheat, true); }
        }

        public static XSolidBrush White
        {
            get { return new XSolidBrush(XColors.White, true); }
        }

        public static XSolidBrush WhiteSmoke
        {
            get { return new XSolidBrush(XColors.WhiteSmoke, true); }
        }

        public static XSolidBrush Yellow
        {
            get { return new XSolidBrush(XColors.Yellow, true); }
        }

        public static XSolidBrush YellowGreen
        {
            get { return new XSolidBrush(XColors.YellowGreen, true); }
        }
    }
    public struct XColor
    {
        XColor(uint argb)
        {
            _cs = XColorSpace.Rgb;
            _a = (byte)((argb >> 24) & 0xff) / 255f;
            _r = (byte)((argb >> 16) & 0xff);
            _g = (byte)((argb >> 8) & 0xff);
            _b = (byte)(argb & 0xff);
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            _gs = 0;
            RgbChanged();
        }

        XColor(byte alpha, byte red, byte green, byte blue)
        {
            _cs = XColorSpace.Rgb;
            _a = alpha / 255f;
            _r = red;
            _g = green;
            _b = blue;
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            _gs = 0;
            RgbChanged();
        }

        XColor(double alpha, double cyan, double magenta, double yellow, double black)
        {
            _cs = XColorSpace.Cmyk;
            _a = (float)(alpha > 1 ? 1 : (alpha < 0 ? 0 : alpha));
            _c = (float)(cyan > 1 ? 1 : (cyan < 0 ? 0 : cyan));
            _m = (float)(magenta > 1 ? 1 : (magenta < 0 ? 0 : magenta));
            _y = (float)(yellow > 1 ? 1 : (yellow < 0 ? 0 : yellow));
            _k = (float)(black > 1 ? 1 : (black < 0 ? 0 : black));
            _r = 0;
            _g = 0;
            _b = 0;
            _gs = 0f;
            CmykChanged();
        }

        XColor(double cyan, double magenta, double yellow, double black)
            : this(1.0, cyan, magenta, yellow, black)
        { }

        XColor(double gray)
        {
            _cs = XColorSpace.GrayScale;
            if (gray < 0)
                _gs = 0;
            else if (gray > 1)
                _gs = 1;
            else
                _gs = (float)gray;

            _a = 1;
            _r = 0;
            _g = 0;
            _b = 0;
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 0;
            GrayChanged();
        }

        internal XColor(XKnownColor knownColor)
            : this(XKnownColorTable.KnownColorToArgb(knownColor))
        { }

        public static XColor FromArgb(int argb)
        {
            return new XColor((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)(argb));
        }

        public static XColor FromArgb(uint argb)
        {
            return new XColor((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)(argb));
        }

        public static XColor FromArgb(int red, int green, int blue)
        {
            CheckByte(red, "red");
            CheckByte(green, "green");
            CheckByte(blue, "blue");
            return new XColor(255, (byte)red, (byte)green, (byte)blue);
        }

        public static XColor FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, "alpha");
            CheckByte(red, "red");
            CheckByte(green, "green");
            CheckByte(blue, "blue");
            return new XColor((byte)alpha, (byte)red, (byte)green, (byte)blue);
        }

        public static XColor FromArgb(int alpha, XColor color)
        {
            color.A = ((byte)alpha) / 255.0;
            return color;
        }


        public static XColor FromCmyk(double cyan, double magenta, double yellow, double black)
        {
            return new XColor(cyan, magenta, yellow, black);
        }

        public static XColor FromCmyk(double alpha, double cyan, double magenta, double yellow, double black)
        {
            return new XColor(alpha, cyan, magenta, yellow, black);
        }

        public static XColor FromGrayScale(double grayScale)
        {
            return new XColor(grayScale);
        }

        public static XColor FromKnownColor(XKnownColor color)
        {
            return new XColor(color);
        }

        public static XColor FromName(string name)
        {
            return Empty;
        }

        public XColorSpace ColorSpace
        {
            get { return _cs; }
            set
            {
                if (!Enum.IsDefined(typeof(XColorSpace), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(XColorSpace));
                _cs = value;
            }
        }

        public bool IsEmpty
        {
            get { return this == Empty; }
        }

        public override bool Equals(object obj)
        {
            if (obj is XColor)
            {
                XColor color = (XColor)obj;
                if (_r == color._r && _g == color._g && _b == color._b &&
                  _c == color._c && _m == color._m && _y == color._y && _k == color._k &&
                  _gs == color._gs)
                {
                    return _a == color._a;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ((byte)(_a * 255)) ^ _r ^ _g ^ _b;
        }

        public static bool operator ==(XColor left, XColor right)
        {
            if (left._r == right._r && left._g == right._g && left._b == right._b &&
                left._c == right._c && left._m == right._m && left._y == right._y && left._k == right._k &&
                left._gs == right._gs)
            {
                return left._a == right._a;
            }
            return false;
        }

        public static bool operator !=(XColor left, XColor right)
        {
            return !(left == right);
        }

        public bool IsKnownColor
        {
            get { return XKnownColorTable.IsKnownColor(Argb); }
        }

        public double GetHue()
        {
            if ((_r == _g) && (_g == _b))
                return 0;

            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value7 = 0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            double value6 = value4 - value5;
            if (value1 == value4)
                value7 = (value2 - value3) / value6;
            else if (value2 == value4)
                value7 = 2f + ((value3 - value1) / value6);
            else if (value3 == value4)
                value7 = 4f + ((value1 - value2) / value6);

            value7 *= 60;
            if (value7 < 0)
                value7 += 360;
            return value7;
        }

        public double GetSaturation()
        {
            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value7 = 0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            if (value4 == value5)
                return value7;

            double value6 = (value4 + value5) / 2;
            if (value6 <= 0.5)
                return (value4 - value5) / (value4 + value5);
            return (value4 - value5) / ((2f - value4) - value5);
        }

        public double GetBrightness()
        {
            double value1 = _r / 255.0;
            double value2 = _g / 255.0;
            double value3 = _b / 255.0;
            double value4 = value1;
            double value5 = value1;
            if (value2 > value4)
                value4 = value2;

            if (value3 > value4)
                value4 = value3;

            if (value2 < value5)
                value5 = value2;

            if (value3 < value5)
                value5 = value3;

            return (value4 + value5) / 2;
        }

        void RgbChanged()
        {
            _cs = XColorSpace.Rgb;
            int c = 255 - _r;
            int m = 255 - _g;
            int y = 255 - _b;
            int k = Math.Min(c, Math.Min(m, y));
            if (k == 255)
                _c = _m = _y = 0;
            else
            {
                float black = 255f - k;
                _c = (c - k) / black;
                _m = (m - k) / black;
                _y = (y - k) / black;
            }
            _k = _gs = k / 255f;
        }

        void CmykChanged()
        {
            _cs = XColorSpace.Cmyk;
            float black = _k * 255;
            float factor = 255f - black;
            _r = (byte)(255 - Math.Min(255f, _c * factor + black));
            _g = (byte)(255 - Math.Min(255f, _m * factor + black));
            _b = (byte)(255 - Math.Min(255f, _y * factor + black));
            _gs = (float)(1 - Math.Min(1.0, 0.3f * _c + 0.59f * _m + 0.11 * _y + _k));
        }

        void GrayChanged()
        {
            _cs = XColorSpace.GrayScale;
            _r = (byte)(_gs * 255);
            _g = (byte)(_gs * 255);
            _b = (byte)(_gs * 255);
            _c = 0;
            _m = 0;
            _y = 0;
            _k = 1 - _gs;
        }

        public double A
        {
            get { return _a; }
            set
            {
                if (value < 0)
                    _a = 0;
                else if (value > 1)
                    _a = 1;
                else
                    _a = (float)value;
            }
        }

        public byte R
        {
            get { return _r; }
            set { _r = value; RgbChanged(); }
        }

        public byte G
        {
            get { return _g; }
            set { _g = value; RgbChanged(); }
        }

        public byte B
        {
            get { return _b; }
            set { _b = value; RgbChanged(); }
        }

        internal uint Rgb
        {
            get { return ((uint)_r << 16) | ((uint)_g << 8) | _b; }
        }

        internal uint Argb
        {
            get { return ((uint)(_a * 255) << 24) | ((uint)_r << 16) | ((uint)_g << 8) | _b; }
        }

        public double C
        {
            get { return _c; }
            set
            {
                if (value < 0)
                    _c = 0;
                else if (value > 1)
                    _c = 1;
                else
                    _c = (float)value;
                CmykChanged();
            }
        }

        public double M
        {
            get { return _m; }
            set
            {
                if (value < 0)
                    _m = 0;
                else if (value > 1)
                    _m = 1;
                else
                    _m = (float)value;
                CmykChanged();
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                if (value < 0)
                    _y = 0;
                else if (value > 1)
                    _y = 1;
                else
                    _y = (float)value;
                CmykChanged();
            }
        }

        public double K
        {
            get { return _k; }
            set
            {
                if (value < 0)
                    _k = 0;
                else if (value > 1)
                    _k = 1;
                else
                    _k = (float)value;
                CmykChanged();
            }
        }

        public double GS
        {
            get { return _gs; }
            set
            {
                if (value < 0)
                    _gs = 0;
                else if (value > 1)
                    _gs = 1;
                else
                    _gs = (float)value;
                GrayChanged();
            }
        }

        public static XColor Empty;

        public string RgbCmykG
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture,
                  "{0};{1};{2};{3};{4};{5};{6};{7};{8}", _r, _g, _b, _c, _m, _y, _k, _gs, _a);
            }
            set
            {
                string[] values = value.Split(';');
                _r = byte.Parse(values[0], CultureInfo.InvariantCulture);
                _g = byte.Parse(values[1], CultureInfo.InvariantCulture);
                _b = byte.Parse(values[2], CultureInfo.InvariantCulture);
                _c = float.Parse(values[3], CultureInfo.InvariantCulture);
                _m = float.Parse(values[4], CultureInfo.InvariantCulture);
                _y = float.Parse(values[5], CultureInfo.InvariantCulture);
                _k = float.Parse(values[6], CultureInfo.InvariantCulture);
                _gs = float.Parse(values[7], CultureInfo.InvariantCulture);
                _a = float.Parse(values[8], CultureInfo.InvariantCulture);
            }
        }

        static void CheckByte(int val, string name)
        {
            if (val < 0 || val > 0xFF)
                throw new ArgumentException(PSSR.InvalidValue(val, name, 0, 255));
        }

        XColorSpace _cs;

        float _a;

        byte _r;
        byte _g;
        byte _b;

        float _c;
        float _m;
        float _y;
        float _k;

        float _gs;
    }
    public class XColorResourceManager
    {
        public XColorResourceManager()
#if !NETFX_CORE && !UWP
            : this(Thread.CurrentThread.CurrentUICulture)
#endif
        { }

        public XColorResourceManager(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
        }

        readonly CultureInfo _cultureInfo;


        public static XKnownColor GetKnownColor(uint argb)
        {
            XKnownColor knownColor = XKnownColorTable.GetKnownColor(argb);
            if ((int)knownColor == -1)
                throw new ArgumentException("The argument is not a known color", "argb");
            return knownColor;
        }

        public static XKnownColor[] GetKnownColors(bool includeTransparent)
        {
            int count = colorInfos.Length;
            XKnownColor[] knownColor = new XKnownColor[count - (includeTransparent ? 0 : 1)];
            for (int idxIn = includeTransparent ? 0 : 1, idxOut = 0; idxIn < count; idxIn++, idxOut++)
                knownColor[idxOut] = colorInfos[idxIn].KnownColor;
            return knownColor;
        }

        public string ToColorName(XKnownColor knownColor)
        {
            ColorResourceInfo colorInfo = GetColorInfo(knownColor);

            if (_cultureInfo.TwoLetterISOLanguageName == "de")
                return colorInfo.NameDE;

            return colorInfo.Name;
        }

        public string ToColorName(XColor color)
        {
            string name;
            if (color.IsKnownColor)
                name = ToColorName(XKnownColorTable.GetKnownColor(color.Argb));
            else
                name = String.Format("{0}, {1}, {2}, {3}", (int)(255 * color.A), color.R, color.G, color.B);
            return name;
        }

        static ColorResourceInfo GetColorInfo(XKnownColor knownColor)
        {
            for (int idx = 0; idx < colorInfos.Length; idx++)
            {
                ColorResourceInfo colorInfo = colorInfos[idx];
                if (colorInfo.KnownColor == knownColor)
                    return colorInfo;
            }
            throw new InvalidEnumArgumentException("Enum is not an XKnownColor.");
        }

        internal static ColorResourceInfo[] colorInfos = new ColorResourceInfo[]
            {
              new ColorResourceInfo(XKnownColor.Transparent, XColors.Transparent, 0x00FFFFFF, "Transparent", "Transparent"),
              new ColorResourceInfo(XKnownColor.Black, XColors.Black, 0xFF000000, "Black", "Schwarz"),
              new ColorResourceInfo(XKnownColor.DarkSlateGray, XColors.DarkSlateGray, 0xFF8FBC8F, "Darkslategray", "Dunkles Schiefergrau"),
              new ColorResourceInfo(XKnownColor.SlateGray, XColors.SlateGray, 0xFF708090, "Slategray", "Schiefergrau"),
              new ColorResourceInfo(XKnownColor.LightSlateGray, XColors.LightSlateGray, 0xFF778899, "Lightslategray", "Helles Schiefergrau"),
              new ColorResourceInfo(XKnownColor.LightSteelBlue, XColors.LightSteelBlue, 0xFFB0C4DE, "Lightsteelblue", "Helles Stahlblau"),
              new ColorResourceInfo(XKnownColor.DimGray, XColors.DimGray, 0xFF696969, "Dimgray", "Gedecktes Grau"),
              new ColorResourceInfo(XKnownColor.Gray, XColors.Gray, 0xFF808080, "Gray", "Grau"),
              new ColorResourceInfo(XKnownColor.DarkGray, XColors.DarkGray, 0xFFA9A9A9, "Darkgray", "Dunkelgrau"),
              new ColorResourceInfo(XKnownColor.Silver, XColors.Silver, 0xFFC0C0C0, "Silver", "Silber"),
              new ColorResourceInfo(XKnownColor.Gainsboro, XColors.Gainsboro, 0xFFDCDCDC, "Gainsboro", "Helles Blaugrau"),
              new ColorResourceInfo(XKnownColor.WhiteSmoke, XColors.WhiteSmoke, 0xFFF5F5F5, "Whitesmoke", "Rauchweiß"),
              new ColorResourceInfo(XKnownColor.GhostWhite, XColors.GhostWhite, 0xFFF8F8FF, "Ghostwhite", "Schattenweiß"),
              new ColorResourceInfo(XKnownColor.White, XColors.White, 0xFFFFFFFF, "White", "Weiß"),
              new ColorResourceInfo(XKnownColor.Snow, XColors.Snow, 0xFFFFFAFA, "Snow", "Schneeweiß"),
              new ColorResourceInfo(XKnownColor.Ivory, XColors.Ivory, 0xFFFFFFF0, "Ivory", "Elfenbein"),
              new ColorResourceInfo(XKnownColor.FloralWhite, XColors.FloralWhite, 0xFFFFFAF0, "Floralwhite", "Blütenweiß"),
              new ColorResourceInfo(XKnownColor.SeaShell, XColors.SeaShell, 0xFFFFF5EE, "Seashell", "Muschel"),
              new ColorResourceInfo(XKnownColor.OldLace, XColors.OldLace, 0xFFFDF5E6, "Oldlace", "Altweiß"),
              new ColorResourceInfo(XKnownColor.Linen, XColors.Linen, 0xFFFAF0E6, "Linen", "Leinen"),
              new ColorResourceInfo(XKnownColor.AntiqueWhite, XColors.AntiqueWhite, 0xFFFAEBD7, "Antiquewhite", "Antikes Weiß"),
              new ColorResourceInfo(XKnownColor.BlanchedAlmond, XColors.BlanchedAlmond, 0xFFFFEBCD, "Blanchedalmond", "Mandelweiß"),
              new ColorResourceInfo(XKnownColor.PapayaWhip, XColors.PapayaWhip, 0xFFFFEFD5, "Papayawhip", "Papayacreme"),
              new ColorResourceInfo(XKnownColor.Beige, XColors.Beige, 0xFFF5F5DC, "Beige", "Beige"),
              new ColorResourceInfo(XKnownColor.Cornsilk, XColors.Cornsilk, 0xFFFFF8DC, "Cornsilk", "Mais"),
              new ColorResourceInfo(XKnownColor.LightGoldenrodYellow, XColors.LightGoldenrodYellow, 0xFFFAFAD2, "Lightgoldenrodyellow", "Helles Goldgelb"),
              new ColorResourceInfo(XKnownColor.LightYellow, XColors.LightYellow, 0xFFFFFFE0, "Lightyellow", "Hellgelb"),
              new ColorResourceInfo(XKnownColor.LemonChiffon, XColors.LemonChiffon, 0xFFFFFACD, "Lemonchiffon", "Pastellgelb"),
              new ColorResourceInfo(XKnownColor.PaleGoldenrod, XColors.PaleGoldenrod, 0xFFEEE8AA, "Palegoldenrod", "Blasses Goldgelb"),
              new ColorResourceInfo(XKnownColor.Khaki, XColors.Khaki, 0xFFF0E68C, "Khaki", "Khaki"),
              new ColorResourceInfo(XKnownColor.Yellow, XColors.Yellow, 0xFFFFFF00, "Yellow", "Gelb"),
              new ColorResourceInfo(XKnownColor.Gold, XColors.Gold, 0xFFFFD700, "Gold", "Gold"),
              new ColorResourceInfo(XKnownColor.Orange, XColors.Orange, 0xFFFFA500, "Orange", "Orange"),
              new ColorResourceInfo(XKnownColor.DarkOrange, XColors.DarkOrange, 0xFFFF8C00, "Darkorange", "Dunkles Orange"),
              new ColorResourceInfo(XKnownColor.Goldenrod, XColors.Goldenrod, 0xFFDAA520, "Goldenrod", "Goldgelb"),
              new ColorResourceInfo(XKnownColor.DarkGoldenrod, XColors.DarkGoldenrod, 0xFFB8860B, "Darkgoldenrod", "Dunkles Goldgelb"),
              new ColorResourceInfo(XKnownColor.Peru, XColors.Peru, 0xFFCD853F, "Peru", "Peru"),
              new ColorResourceInfo(XKnownColor.Chocolate, XColors.Chocolate, 0xFFD2691E, "Chocolate", "Schokolade"),
              new ColorResourceInfo(XKnownColor.SaddleBrown, XColors.SaddleBrown, 0xFF8B4513, "Saddlebrown", "Sattelbraun"),
              new ColorResourceInfo(XKnownColor.Sienna, XColors.Sienna, 0xFFA0522D, "Sienna", "Ocker"),
              new ColorResourceInfo(XKnownColor.Brown, XColors.Brown, 0xFFA52A2A, "Brown", "Braun"),
              new ColorResourceInfo(XKnownColor.DarkRed, XColors.DarkRed, 0xFF8B0000, "Darkred", "Dunkelrot"),
              new ColorResourceInfo(XKnownColor.Maroon, XColors.Maroon, 0xFF800000, "Maroon", "Kastanienbraun"),
              new ColorResourceInfo(XKnownColor.PaleTurquoise, XColors.PaleTurquoise, 0xFFAFEEEE, "Paleturquoise", "Blasses Türkis"),
              new ColorResourceInfo(XKnownColor.Firebrick, XColors.Firebrick, 0xFFB22222, "Firebrick", "Ziegel"),
              new ColorResourceInfo(XKnownColor.IndianRed, XColors.IndianRed, 0xFFCD5C5C, "Indianred", "Indischrot"),
              new ColorResourceInfo(XKnownColor.Crimson, XColors.Crimson, 0xFFDC143C, "Crimson", "Karmesinrot"),
              new ColorResourceInfo(XKnownColor.Red, XColors.Red, 0xFFFF0000, "Red", "Rot"),
              new ColorResourceInfo(XKnownColor.OrangeRed, XColors.OrangeRed, 0xFFFF4500, "Orangered", "Orangerot"),
              new ColorResourceInfo(XKnownColor.Tomato, XColors.Tomato, 0xFFFF6347, "Tomato", "Tomate"),
              new ColorResourceInfo(XKnownColor.Coral, XColors.Coral, 0xFFFF7F50, "Coral", "Koralle"),
              new ColorResourceInfo(XKnownColor.Salmon, XColors.Salmon, 0xFFFA8072, "Salmon", "Lachs"),
              new ColorResourceInfo(XKnownColor.LightCoral, XColors.LightCoral, 0xFFF08080, "Lightcoral", "Helles Korallenrot"),
              new ColorResourceInfo(XKnownColor.DarkSalmon, XColors.DarkSalmon, 0xFFE9967A, "Darksalmon", "Dunkles Lachs"),
              new ColorResourceInfo(XKnownColor.LightSalmon, XColors.LightSalmon, 0xFFFFA07A, "Lightsalmon", "Helles Lachs"),
              new ColorResourceInfo(XKnownColor.SandyBrown, XColors.SandyBrown, 0xFFF4A460, "Sandybrown", "Sandbraun"),
              new ColorResourceInfo(XKnownColor.RosyBrown, XColors.RosyBrown, 0xFFBC8F8F, "Rosybrown", "Rotbraun"),
              new ColorResourceInfo(XKnownColor.Tan, XColors.Tan, 0xFFD2B48C, "Tan", "Gelbbraun"),
              new ColorResourceInfo(XKnownColor.BurlyWood, XColors.BurlyWood, 0xFFDEB887, "Burlywood", "Kräftiges Sandbraun"),
              new ColorResourceInfo(XKnownColor.Wheat, XColors.Wheat, 0xFFF5DEB3, "Wheat", "Weizen"),
              new ColorResourceInfo(XKnownColor.PeachPuff, XColors.PeachPuff, 0xFFFFDAB9, "Peachpuff", "Pfirsich"),
              new ColorResourceInfo(XKnownColor.NavajoWhite, XColors.NavajoWhite, 0xFFFFDEAD, "Navajowhite", "Orangeweiß"),
              new ColorResourceInfo(XKnownColor.Bisque, XColors.Bisque, 0xFFFFE4C4, "Bisque", "Blasses Rotbraun"),
              new ColorResourceInfo(XKnownColor.Moccasin, XColors.Moccasin, 0xFFFFE4B5, "Moccasin", "Mokassin"),
              new ColorResourceInfo(XKnownColor.LavenderBlush, XColors.LavenderBlush, 0xFFFFF0F5, "Lavenderblush", "Roter Lavendel"),
              new ColorResourceInfo(XKnownColor.MistyRose, XColors.MistyRose, 0xFFFFE4E1, "Mistyrose", "Altrosa"),
              new ColorResourceInfo(XKnownColor.Pink, XColors.Pink, 0xFFFFC0CB, "Pink", "Rosa"),
              new ColorResourceInfo(XKnownColor.LightPink, XColors.LightPink, 0xFFFFB6C1, "Lightpink", "Hellrosa"),
              new ColorResourceInfo(XKnownColor.HotPink, XColors.HotPink, 0xFFFF69B4, "Hotpink", "Leuchtendes Rosa"),
              new ColorResourceInfo(XKnownColor.Magenta, XColors.Magenta, 0xFFFF00FF, "Magenta", "Magentarot"),
              new ColorResourceInfo(XKnownColor.DeepPink, XColors.DeepPink, 0xFFFF1493, "Deeppink", "Tiefrosa"),
              new ColorResourceInfo(XKnownColor.MediumVioletRed, XColors.MediumVioletRed, 0xFFC71585, "Mediumvioletred", "Mittleres Violettrot"),
              new ColorResourceInfo(XKnownColor.PaleVioletRed, XColors.PaleVioletRed, 0xFFDB7093, "Palevioletred", "Blasses Violettrot"),
              new ColorResourceInfo(XKnownColor.Plum, XColors.Plum, 0xFFDDA0DD, "Plum", "Pflaume"),
              new ColorResourceInfo(XKnownColor.Thistle, XColors.Thistle, 0xFFD8BFD8, "Thistle", "Distel"),
              new ColorResourceInfo(XKnownColor.Lavender, XColors.Lavender, 0xFFE6E6FA, "Lavender", "Lavendel"),
              new ColorResourceInfo(XKnownColor.Violet, XColors.Violet, 0xFFEE82EE, "Violet", "Violett"),
              new ColorResourceInfo(XKnownColor.Orchid, XColors.Orchid, 0xFFDA70D6, "Orchid", "Orchidee"),
              new ColorResourceInfo(XKnownColor.DarkMagenta, XColors.DarkMagenta, 0xFF8B008B, "Darkmagenta", "Dunkles Magentarot"),
              new ColorResourceInfo(XKnownColor.Purple, XColors.Purple, 0xFF800080, "Purple", "Violett"),
              new ColorResourceInfo(XKnownColor.Indigo, XColors.Indigo, 0xFF4B0082, "Indigo", "Indigo"),
              new ColorResourceInfo(XKnownColor.BlueViolet, XColors.BlueViolet, 0xFF8A2BE2, "Blueviolet", "Blauviolett"),
              new ColorResourceInfo(XKnownColor.DarkViolet, XColors.DarkViolet, 0xFF9400D3, "Darkviolet", "Dunkles Violett"),
              new ColorResourceInfo(XKnownColor.DarkOrchid, XColors.DarkOrchid, 0xFF9932CC, "Darkorchid", "Dunkle Orchidee"),
              new ColorResourceInfo(XKnownColor.MediumPurple, XColors.MediumPurple, 0xFF9370DB, "Mediumpurple", "Mittleres Violett"),
              new ColorResourceInfo(XKnownColor.MediumOrchid, XColors.MediumOrchid, 0xFFBA55D3, "Mediumorchid", "Mittlere Orchidee"),
              new ColorResourceInfo(XKnownColor.MediumSlateBlue, XColors.MediumSlateBlue, 0xFF7B68EE, "Mediumslateblue", "Mittleres Schieferblau"),
              new ColorResourceInfo(XKnownColor.SlateBlue, XColors.SlateBlue, 0xFF6A5ACD, "Slateblue", "Schieferblau"),
              new ColorResourceInfo(XKnownColor.DarkSlateBlue, XColors.DarkSlateBlue, 0xFF483D8B, "Darkslateblue", "Dunkles Schiefergrau"),
              new ColorResourceInfo(XKnownColor.MidnightBlue, XColors.MidnightBlue, 0xFF191970, "Midnightblue", "Mitternachtsblau"),
              new ColorResourceInfo(XKnownColor.Navy, XColors.Navy, 0xFF000080, "Navy", "Marineblau"),
              new ColorResourceInfo(XKnownColor.DarkBlue, XColors.DarkBlue, 0xFF00008B, "Darkblue", "Dunkelblau"),
              new ColorResourceInfo(XKnownColor.LightGray, XColors.LightGray, 0xFFD3D3D3, "Lightgray", "Hellgrau"),
              new ColorResourceInfo(XKnownColor.MediumBlue, XColors.MediumBlue, 0xFF0000CD, "Mediumblue", "Mittelblau"),
              new ColorResourceInfo(XKnownColor.Blue, XColors.Blue, 0xFF0000FF, "Blue", "Blau"),
              new ColorResourceInfo(XKnownColor.RoyalBlue, XColors.RoyalBlue, 0xFF4169E1, "Royalblue", "Königsblau"),
              new ColorResourceInfo(XKnownColor.SteelBlue, XColors.SteelBlue, 0xFF4682B4, "Steelblue", "Stahlblau"),
              new ColorResourceInfo(XKnownColor.CornflowerBlue, XColors.CornflowerBlue, 0xFF6495ED, "Cornflowerblue", "Kornblumenblau"),
              new ColorResourceInfo(XKnownColor.DodgerBlue, XColors.DodgerBlue, 0xFF1E90FF, "Dodgerblue", "Dodger-Blau"),
              new ColorResourceInfo(XKnownColor.DeepSkyBlue, XColors.DeepSkyBlue, 0xFF00BFFF, "Deepskyblue", "Tiefes Himmelblau"),
              new ColorResourceInfo(XKnownColor.LightSkyBlue, XColors.LightSkyBlue, 0xFF87CEFA, "Lightskyblue", "Helles Himmelblau"),
              new ColorResourceInfo(XKnownColor.SkyBlue, XColors.SkyBlue, 0xFF87CEEB, "Skyblue", "Himmelblau"),
              new ColorResourceInfo(XKnownColor.LightBlue, XColors.LightBlue, 0xFFADD8E6, "Lightblue", "Hellblau"),
              new ColorResourceInfo(XKnownColor.Cyan, XColors.Cyan, 0xFF00FFFF, "Cyan", "Zyan"),
              new ColorResourceInfo(XKnownColor.PowderBlue, XColors.PowderBlue, 0xFFB0E0E6, "Powderblue", "Taubenblau"),
              new ColorResourceInfo(XKnownColor.LightCyan, XColors.LightCyan, 0xFFE0FFFF, "Lightcyan", "Helles Cyanblau"),
              new ColorResourceInfo(XKnownColor.AliceBlue, XColors.AliceBlue, 0xFFA0CE00, "Aliceblue", "Aliceblau"),
              new ColorResourceInfo(XKnownColor.Azure, XColors.Azure, 0xFFF0FFFF, "Azure", "Himmelblau"),
              new ColorResourceInfo(XKnownColor.MintCream, XColors.MintCream, 0xFFF5FFFA, "Mintcream", "Helles Pfefferminzgrün"),
              new ColorResourceInfo(XKnownColor.Honeydew, XColors.Honeydew, 0xFFF0FFF0, "Honeydew", "Honigmelone"),
              new ColorResourceInfo(XKnownColor.Aquamarine, XColors.Aquamarine, 0xFF7FFFD4, "Aquamarine", "Aquamarinblau"),
              new ColorResourceInfo(XKnownColor.Turquoise, XColors.Turquoise, 0xFF40E0D0, "Turquoise", "Türkis"),
              new ColorResourceInfo(XKnownColor.MediumTurquoise, XColors.MediumTurquoise, 0xFF48D1CC, "Mediumturqoise", "Mittleres Türkis"),
              new ColorResourceInfo(XKnownColor.DarkTurquoise, XColors.DarkTurquoise, 0xFF00CED1, "Darkturquoise", "Dunkles Türkis"),
              new ColorResourceInfo(XKnownColor.MediumAquamarine, XColors.MediumAquamarine, 0xFF66CDAA, "Mediumaquamarine", "Mittleres Aquamarinblau"),
              new ColorResourceInfo(XKnownColor.LightSeaGreen, XColors.LightSeaGreen, 0xFF20B2AA, "Lightseagreen", "Helles Seegrün"),
              new ColorResourceInfo(XKnownColor.DarkCyan, XColors.DarkCyan, 0xFF008B8B, "Darkcyan", "Dunkles Zyanblau"),
              new ColorResourceInfo(XKnownColor.Teal, XColors.Teal, 0xFF008080, "Teal", "Entenblau"),
              new ColorResourceInfo(XKnownColor.CadetBlue, XColors.CadetBlue, 0xFF5F9EA0, "Cadetblue", "Kadettblau"),
              new ColorResourceInfo(XKnownColor.MediumSeaGreen, XColors.MediumSeaGreen, 0xFF3CB371, "Mediumseagreen", "Mittleres Seegrün"),
              new ColorResourceInfo(XKnownColor.DarkSeaGreen, XColors.DarkSeaGreen, 0xFF8FBC8F, "Darkseagreen", "Dunkles Seegrün"),
              new ColorResourceInfo(XKnownColor.LightGreen, XColors.LightGreen, 0xFF90EE90, "Lightgreen", "Hellgrün"),
              new ColorResourceInfo(XKnownColor.PaleGreen, XColors.PaleGreen, 0xFF98FB98, "Palegreen", "Blassgrün"),
              new ColorResourceInfo(XKnownColor.MediumSpringGreen, XColors.MediumSpringGreen, 0xFF00FA9A, "Mediumspringgreen", "Mittleres Frühlingsgrün"),
              new ColorResourceInfo(XKnownColor.SpringGreen, XColors.SpringGreen, 0xFF00FF7F, "Springgreen", "Frühlingsgrün"),
              new ColorResourceInfo(XKnownColor.Lime, XColors.Lime, 0xFF00FF00, "Lime", "Zitronengrün"),
              new ColorResourceInfo(XKnownColor.LimeGreen, XColors.LimeGreen, 0xFF32CD32, "Limegreen", "Gelbgrün"),
              new ColorResourceInfo(XKnownColor.SeaGreen, XColors.SeaGreen, 0xFF2E8B57, "Seagreen", "Seegrün"),
              new ColorResourceInfo(XKnownColor.ForestGreen, XColors.ForestGreen, 0xFF228B22, "Forestgreen", "Waldgrün"),
              new ColorResourceInfo(XKnownColor.Green, XColors.Green, 0xFF008000, "Green", "Grün"),
              new ColorResourceInfo(XKnownColor.LawnGreen, XColors.LawnGreen, 0xFF008000, "LawnGreen", "Grasgrün"),
              new ColorResourceInfo(XKnownColor.DarkGreen, XColors.DarkGreen, 0xFF006400, "Darkgreen", "Dunkelgrün"),
              new ColorResourceInfo(XKnownColor.OliveDrab, XColors.OliveDrab, 0xFF6B8E23, "Olivedrab", "Reife Olive"),
              new ColorResourceInfo(XKnownColor.DarkOliveGreen, XColors.DarkOliveGreen, 0xFF556B2F, "Darkolivegreen", "Dunkles Olivgrün"),
              new ColorResourceInfo(XKnownColor.Olive, XColors.Olive, 0xFF808000, "Olive", "Olivgrün"),
              new ColorResourceInfo(XKnownColor.DarkKhaki, XColors.DarkKhaki, 0xFFBDB76B, "Darkkhaki", "Dunkles Khaki"),
              new ColorResourceInfo(XKnownColor.YellowGreen, XColors.YellowGreen, 0xFF9ACD32, "Yellowgreen", "Gelbgrün"),
              new ColorResourceInfo(XKnownColor.Chartreuse, XColors.Chartreuse, 0xFF7FFF00, "Chartreuse", "Hellgrün"),
              new ColorResourceInfo(XKnownColor.GreenYellow, XColors.GreenYellow, 0xFFADFF2F, "Greenyellow", "Grüngelb"),
            };

        internal struct ColorResourceInfo
        {
            public ColorResourceInfo(XKnownColor knownColor, XColor color, uint argb, string name, string nameDE)
            {
                KnownColor = knownColor;
                Color = color;
                Argb = argb;
                Name = name;
                NameDE = nameDE;
            }
            public XKnownColor KnownColor;
            public XColor Color;
            public uint Argb;
            public string Name;
            public string NameDE;
        }
    }
    public static class XColors
    {
        public static XColor AliceBlue { get { return new XColor(XKnownColor.AliceBlue); } }

        public static XColor AntiqueWhite { get { return new XColor(XKnownColor.AntiqueWhite); } }

        public static XColor Aqua { get { return new XColor(XKnownColor.Aqua); } }

        public static XColor Aquamarine { get { return new XColor(XKnownColor.Aquamarine); } }

        public static XColor Azure { get { return new XColor(XKnownColor.Azure); } }

        public static XColor Beige { get { return new XColor(XKnownColor.Beige); } }

        public static XColor Bisque { get { return new XColor(XKnownColor.Bisque); } }

        public static XColor Black { get { return new XColor(XKnownColor.Black); } }

        public static XColor BlanchedAlmond { get { return new XColor(XKnownColor.BlanchedAlmond); } }

        public static XColor Blue { get { return new XColor(XKnownColor.Blue); } }

        public static XColor BlueViolet { get { return new XColor(XKnownColor.BlueViolet); } }

        public static XColor Brown { get { return new XColor(XKnownColor.Brown); } }

        public static XColor BurlyWood { get { return new XColor(XKnownColor.BurlyWood); } }

        public static XColor CadetBlue { get { return new XColor(XKnownColor.CadetBlue); } }

        public static XColor Chartreuse { get { return new XColor(XKnownColor.Chartreuse); } }

        public static XColor Chocolate { get { return new XColor(XKnownColor.Chocolate); } }

        public static XColor Coral { get { return new XColor(XKnownColor.Coral); } }

        public static XColor CornflowerBlue { get { return new XColor(XKnownColor.CornflowerBlue); } }

        public static XColor Cornsilk { get { return new XColor(XKnownColor.Cornsilk); } }

        public static XColor Crimson { get { return new XColor(XKnownColor.Crimson); } }

        public static XColor Cyan { get { return new XColor(XKnownColor.Cyan); } }

        public static XColor DarkBlue { get { return new XColor(XKnownColor.DarkBlue); } }

        public static XColor DarkCyan { get { return new XColor(XKnownColor.DarkCyan); } }

        public static XColor DarkGoldenrod { get { return new XColor(XKnownColor.DarkGoldenrod); } }

        public static XColor DarkGray { get { return new XColor(XKnownColor.DarkGray); } }

        public static XColor DarkGreen { get { return new XColor(XKnownColor.DarkGreen); } }

        public static XColor DarkKhaki { get { return new XColor(XKnownColor.DarkKhaki); } }

        public static XColor DarkMagenta { get { return new XColor(XKnownColor.DarkMagenta); } }

        public static XColor DarkOliveGreen { get { return new XColor(XKnownColor.DarkOliveGreen); } }

        public static XColor DarkOrange { get { return new XColor(XKnownColor.DarkOrange); } }

        public static XColor DarkOrchid { get { return new XColor(XKnownColor.DarkOrchid); } }

        public static XColor DarkRed { get { return new XColor(XKnownColor.DarkRed); } }

        public static XColor DarkSalmon { get { return new XColor(XKnownColor.DarkSalmon); } }

        public static XColor DarkSeaGreen { get { return new XColor(XKnownColor.DarkSeaGreen); } }

        public static XColor DarkSlateBlue { get { return new XColor(XKnownColor.DarkSlateBlue); } }

        public static XColor DarkSlateGray { get { return new XColor(XKnownColor.DarkSlateGray); } }

        public static XColor DarkTurquoise { get { return new XColor(XKnownColor.DarkTurquoise); } }

        public static XColor DarkViolet { get { return new XColor(XKnownColor.DarkViolet); } }

        public static XColor DeepPink { get { return new XColor(XKnownColor.DeepPink); } }

        public static XColor DeepSkyBlue { get { return new XColor(XKnownColor.DeepSkyBlue); } }

        public static XColor DimGray { get { return new XColor(XKnownColor.DimGray); } }

        public static XColor DodgerBlue { get { return new XColor(XKnownColor.DodgerBlue); } }

        public static XColor Firebrick { get { return new XColor(XKnownColor.Firebrick); } }

        public static XColor FloralWhite { get { return new XColor(XKnownColor.FloralWhite); } }

        public static XColor ForestGreen { get { return new XColor(XKnownColor.ForestGreen); } }

        public static XColor Fuchsia { get { return new XColor(XKnownColor.Fuchsia); } }

        public static XColor Gainsboro { get { return new XColor(XKnownColor.Gainsboro); } }

        public static XColor GhostWhite { get { return new XColor(XKnownColor.GhostWhite); } }

        public static XColor Gold { get { return new XColor(XKnownColor.Gold); } }

        public static XColor Goldenrod { get { return new XColor(XKnownColor.Goldenrod); } }

        public static XColor Gray { get { return new XColor(XKnownColor.Gray); } }

        public static XColor Green { get { return new XColor(XKnownColor.Green); } }

        public static XColor GreenYellow { get { return new XColor(XKnownColor.GreenYellow); } }

        public static XColor Honeydew { get { return new XColor(XKnownColor.Honeydew); } }

        public static XColor HotPink { get { return new XColor(XKnownColor.HotPink); } }

        public static XColor IndianRed { get { return new XColor(XKnownColor.IndianRed); } }

        public static XColor Indigo { get { return new XColor(XKnownColor.Indigo); } }

        public static XColor Ivory { get { return new XColor(XKnownColor.Ivory); } }

        public static XColor Khaki { get { return new XColor(XKnownColor.Khaki); } }

        public static XColor Lavender { get { return new XColor(XKnownColor.Lavender); } }

        public static XColor LavenderBlush { get { return new XColor(XKnownColor.LavenderBlush); } }

        public static XColor LawnGreen { get { return new XColor(XKnownColor.LawnGreen); } }

        public static XColor LemonChiffon { get { return new XColor(XKnownColor.LemonChiffon); } }

        public static XColor LightBlue { get { return new XColor(XKnownColor.LightBlue); } }

        public static XColor LightCoral { get { return new XColor(XKnownColor.LightCoral); } }

        public static XColor LightCyan { get { return new XColor(XKnownColor.LightCyan); } }

        public static XColor LightGoldenrodYellow { get { return new XColor(XKnownColor.LightGoldenrodYellow); } }

        public static XColor LightGray { get { return new XColor(XKnownColor.LightGray); } }

        public static XColor LightGreen { get { return new XColor(XKnownColor.LightGreen); } }

        public static XColor LightPink { get { return new XColor(XKnownColor.LightPink); } }

        public static XColor LightSalmon { get { return new XColor(XKnownColor.LightSalmon); } }

        public static XColor LightSeaGreen { get { return new XColor(XKnownColor.LightSeaGreen); } }

        public static XColor LightSkyBlue { get { return new XColor(XKnownColor.LightSkyBlue); } }

        public static XColor LightSlateGray { get { return new XColor(XKnownColor.LightSlateGray); } }

        public static XColor LightSteelBlue { get { return new XColor(XKnownColor.LightSteelBlue); } }

        public static XColor LightYellow { get { return new XColor(XKnownColor.LightYellow); } }

        public static XColor Lime { get { return new XColor(XKnownColor.Lime); } }

        public static XColor LimeGreen { get { return new XColor(XKnownColor.LimeGreen); } }

        public static XColor Linen { get { return new XColor(XKnownColor.Linen); } }

        public static XColor Magenta { get { return new XColor(XKnownColor.Magenta); } }

        public static XColor Maroon { get { return new XColor(XKnownColor.Maroon); } }

        public static XColor MediumAquamarine { get { return new XColor(XKnownColor.MediumAquamarine); } }

        public static XColor MediumBlue { get { return new XColor(XKnownColor.MediumBlue); } }

        public static XColor MediumOrchid { get { return new XColor(XKnownColor.MediumOrchid); } }

        public static XColor MediumPurple { get { return new XColor(XKnownColor.MediumPurple); } }

        public static XColor MediumSeaGreen { get { return new XColor(XKnownColor.MediumSeaGreen); } }

        public static XColor MediumSlateBlue { get { return new XColor(XKnownColor.MediumSlateBlue); } }

        public static XColor MediumSpringGreen { get { return new XColor(XKnownColor.MediumSpringGreen); } }

        public static XColor MediumTurquoise { get { return new XColor(XKnownColor.MediumTurquoise); } }

        public static XColor MediumVioletRed { get { return new XColor(XKnownColor.MediumVioletRed); } }

        public static XColor MidnightBlue { get { return new XColor(XKnownColor.MidnightBlue); } }

        public static XColor MintCream { get { return new XColor(XKnownColor.MintCream); } }

        public static XColor MistyRose { get { return new XColor(XKnownColor.MistyRose); } }

        public static XColor Moccasin { get { return new XColor(XKnownColor.Moccasin); } }

        public static XColor NavajoWhite { get { return new XColor(XKnownColor.NavajoWhite); } }

        public static XColor Navy { get { return new XColor(XKnownColor.Navy); } }

        public static XColor OldLace { get { return new XColor(XKnownColor.OldLace); } }

        public static XColor Olive { get { return new XColor(XKnownColor.Olive); } }

        public static XColor OliveDrab { get { return new XColor(XKnownColor.OliveDrab); } }

        public static XColor Orange { get { return new XColor(XKnownColor.Orange); } }

        public static XColor OrangeRed { get { return new XColor(XKnownColor.OrangeRed); } }

        public static XColor Orchid { get { return new XColor(XKnownColor.Orchid); } }

        public static XColor PaleGoldenrod { get { return new XColor(XKnownColor.PaleGoldenrod); } }

        public static XColor PaleGreen { get { return new XColor(XKnownColor.PaleGreen); } }

        public static XColor PaleTurquoise { get { return new XColor(XKnownColor.PaleTurquoise); } }

        public static XColor PaleVioletRed { get { return new XColor(XKnownColor.PaleVioletRed); } }

        public static XColor PapayaWhip { get { return new XColor(XKnownColor.PapayaWhip); } }

        public static XColor PeachPuff { get { return new XColor(XKnownColor.PeachPuff); } }

        public static XColor Peru { get { return new XColor(XKnownColor.Peru); } }

        public static XColor Pink { get { return new XColor(XKnownColor.Pink); } }

        public static XColor Plum { get { return new XColor(XKnownColor.Plum); } }

        public static XColor PowderBlue { get { return new XColor(XKnownColor.PowderBlue); } }

        public static XColor Purple { get { return new XColor(XKnownColor.Purple); } }

        public static XColor Red { get { return new XColor(XKnownColor.Red); } }

        public static XColor RosyBrown { get { return new XColor(XKnownColor.RosyBrown); } }

        public static XColor RoyalBlue { get { return new XColor(XKnownColor.RoyalBlue); } }

        public static XColor SaddleBrown { get { return new XColor(XKnownColor.SaddleBrown); } }

        public static XColor Salmon { get { return new XColor(XKnownColor.Salmon); } }

        public static XColor SandyBrown { get { return new XColor(XKnownColor.SandyBrown); } }

        public static XColor SeaGreen { get { return new XColor(XKnownColor.SeaGreen); } }

        public static XColor SeaShell { get { return new XColor(XKnownColor.SeaShell); } }

        public static XColor Sienna { get { return new XColor(XKnownColor.Sienna); } }

        public static XColor Silver { get { return new XColor(XKnownColor.Silver); } }

        public static XColor SkyBlue { get { return new XColor(XKnownColor.SkyBlue); } }

        public static XColor SlateBlue { get { return new XColor(XKnownColor.SlateBlue); } }

        public static XColor SlateGray { get { return new XColor(XKnownColor.SlateGray); } }

        public static XColor Snow { get { return new XColor(XKnownColor.Snow); } }

        public static XColor SpringGreen { get { return new XColor(XKnownColor.SpringGreen); } }

        public static XColor SteelBlue { get { return new XColor(XKnownColor.SteelBlue); } }

        public static XColor Tan { get { return new XColor(XKnownColor.Tan); } }

        public static XColor Teal { get { return new XColor(XKnownColor.Teal); } }

        public static XColor Thistle { get { return new XColor(XKnownColor.Thistle); } }

        public static XColor Tomato { get { return new XColor(XKnownColor.Tomato); } }

        public static XColor Transparent { get { return new XColor(XKnownColor.Transparent); } }

        public static XColor Turquoise { get { return new XColor(XKnownColor.Turquoise); } }

        public static XColor Violet { get { return new XColor(XKnownColor.Violet); } }

        public static XColor Wheat { get { return new XColor(XKnownColor.Wheat); } }

        public static XColor White { get { return new XColor(XKnownColor.White); } }

        public static XColor WhiteSmoke { get { return new XColor(XKnownColor.WhiteSmoke); } }

        public static XColor Yellow { get { return new XColor(XKnownColor.Yellow); } }

        public static XColor YellowGreen { get { return new XColor(XKnownColor.YellowGreen); } }
    }
    public sealed class XFont
    {
        public XFont(string familyName, double emSize)
            : this(familyName, emSize, XFontStyle.Regular, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(string familyName, double emSize, XFontStyle style)
            : this(familyName, emSize, style, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(string familyName, double emSize, XFontStyle style, XPdfFontOptions pdfOptions)
        {
            _familyName = familyName;
            _emSize = emSize;
            _style = style;
            _pdfOptions = pdfOptions;
            Initialize();
        }

        internal XFont(string familyName, double emSize, XFontStyle style, XPdfFontOptions pdfOptions, XStyleSimulations styleSimulations)
        {
            _familyName = familyName;
            _emSize = emSize;
            _style = style;
            _pdfOptions = pdfOptions;
            OverrideStyleSimulations = true;
            StyleSimulations = styleSimulations;
            Initialize();
        }

#if CORE || GDI
        public XFont(GdiFontFamily fontFamily, double emSize, XFontStyle style)
            : this(fontFamily, emSize, style, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(GdiFontFamily fontFamily, double emSize, XFontStyle style, XPdfFontOptions pdfOptions)
        {
            _familyName = fontFamily.Name;
            _gdiFontFamily = fontFamily;
            _emSize = emSize;
            _style = style;
            _pdfOptions = pdfOptions;
            InitializeFromGdi();
        }

        public XFont(GdiFont font)
            : this(font, new XPdfFontOptions(GlobalFontSettings.DefaultFontEncoding))
        { }

        public XFont(GdiFont font, XPdfFontOptions pdfOptions)
        {
            if (font.Unit != GraphicsUnit.World)
                throw new ArgumentException("Font must use GraphicsUnit.World.");
            _gdiFont = font;
            Debug.Assert(font.Name == font.FontFamily.Name);
            _familyName = font.Name;
            _emSize = font.Size;
            _style = FontStyleFrom(font);
            _pdfOptions = pdfOptions;
            InitializeFromGdi();
        }
#endif

        void Initialize()
        {
            FontResolvingOptions fontResolvingOptions = OverrideStyleSimulations
                ? new FontResolvingOptions(_style, StyleSimulations)
                : new FontResolvingOptions(_style);

            if (StringComparer.OrdinalIgnoreCase.Compare(_familyName, GlobalFontSettings.DefaultFontName) == 0)
            {
#if CORE || GDI || WPF
                _familyName = "Calibri";
#endif
            }

            _glyphTypeface = XGlyphTypeface.GetOrCreateFrom(_familyName, fontResolvingOptions);

            CreateDescriptorAndInitializeFontMetrics();
        }

#if CORE || GDI
        void InitializeFromGdi()
        {
            try
            {
                Lock.EnterFontFactory();
                if (_gdiFontFamily != null)
                {
                    _gdiFont = new Font(_gdiFontFamily, (float)_emSize, (GdiFontStyle)_style, GraphicsUnit.World);
                }

                if (_gdiFont != null)
                {

                    _familyName = _gdiFont.FontFamily.Name;
                }
                else
                {
                    Debug.Assert(false);
                }

                if (_glyphTypeface == null)
                    _glyphTypeface = XGlyphTypeface.GetOrCreateFromGdi(_gdiFont);

                CreateDescriptorAndInitializeFontMetrics();
            }
            finally { Lock.ExitFontFactory(); }
        }
#endif

        void CreateDescriptorAndInitializeFontMetrics()
        {
            Debug.Assert(_fontMetrics == null, "InitializeFontMetrics() was already called.");
            _descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptorFor(this);
            _fontMetrics = new XFontMetrics(_descriptor.FontName, _descriptor.UnitsPerEm, _descriptor.Ascender, _descriptor.Descender,
                _descriptor.Leading, _descriptor.LineSpacing, _descriptor.CapHeight, _descriptor.XHeight, _descriptor.StemV, 0, 0, 0,
                _descriptor.UnderlinePosition, _descriptor.UnderlineThickness, _descriptor.StrikeoutPosition, _descriptor.StrikeoutSize);

            XFontMetrics fm = Metrics;

            UnitsPerEm = _descriptor.UnitsPerEm;
            CellAscent = _descriptor.Ascender;
            CellDescent = _descriptor.Descender;
            CellSpace = _descriptor.LineSpacing;

            Debug.Assert(fm.UnitsPerEm == _descriptor.UnitsPerEm);
        }




        [Browsable(false)]
        public XFontFamily FontFamily
        {
            get { return _glyphTypeface.FontFamily; }
        }

        public string Name
        {
            get { return _glyphTypeface.FontFamily.Name; }
        }

        internal string FaceName
        {
            get { return _glyphTypeface.FaceName; }
        }

        public double Size
        {
            get { return _emSize; }
        }
        readonly double _emSize;

        [Browsable(false)]
        public XFontStyle Style
        {
            get { return _style; }
        }
        readonly XFontStyle _style;

        public bool Bold
        {
            get { return (_style & XFontStyle.Bold) == XFontStyle.Bold; }
        }

        public bool Italic
        {
            get { return (_style & XFontStyle.Italic) == XFontStyle.Italic; }
        }

        public bool Strikeout
        {
            get { return (_style & XFontStyle.Strikeout) == XFontStyle.Strikeout; }
        }

        public bool Underline
        {
            get { return (_style & XFontStyle.Underline) == XFontStyle.Underline; }
        }

        internal bool IsVertical
        {
            get { return _isVertical; }
            set { _isVertical = value; }
        }
        bool _isVertical;


        public XPdfFontOptions PdfOptions
        {
            get { return _pdfOptions ?? (_pdfOptions = new XPdfFontOptions()); }
        }
        XPdfFontOptions _pdfOptions;

        internal bool Unicode
        {
            get { return _pdfOptions != null && _pdfOptions.FontEncoding == PdfFontEncoding.Unicode; }
        }

        public int CellSpace
        {
            get { return _cellSpace; }
            internal set { _cellSpace = value; }
        }
        int _cellSpace;

        public int CellAscent
        {
            get { return _cellAscent; }
            internal set { _cellAscent = value; }
        }
        int _cellAscent;

        public int CellDescent
        {
            get { return _cellDescent; }
            internal set { _cellDescent = value; }
        }
        int _cellDescent;

        public XFontMetrics Metrics
        {
            get
            {
                Debug.Assert(_fontMetrics != null, "InitializeFontMetrics() not yet called.");
                return _fontMetrics;
            }
        }
        XFontMetrics _fontMetrics;

        public double GetHeight()
        {
            double value = CellSpace * _emSize / UnitsPerEm;
#if CORE || NETFX_CORE || UWP
            return value;
#endif
        }

        [Obsolete("Use GetHeight() without parameter.")]
        public double GetHeight(XGraphics graphics)
        {
#if true
            throw new InvalidOperationException("Honestly: Use GetHeight() without parameter!");
#else

#endif
        }

        [Browsable(false)]
        public int Height
        {
            get { return (int)Math.Ceiling(GetHeight()); }
        }

        internal XGlyphTypeface GlyphTypeface
        {
            get { return _glyphTypeface; }
        }
        XGlyphTypeface _glyphTypeface;


        internal OpenTypeDescriptor Descriptor
        {
            get { return _descriptor; }
            private set { _descriptor = value; }
        }
        OpenTypeDescriptor _descriptor;


        internal string FamilyName
        {
            get { return _familyName; }
        }
        string _familyName;


        internal int UnitsPerEm
        {
            get { return _unitsPerEm; }
            private set { _unitsPerEm = value; }
        }
        internal int _unitsPerEm;

        internal bool OverrideStyleSimulations;

        internal XStyleSimulations StyleSimulations;


#if CORE || GDI
        public GdiFontFamily GdiFontFamily
        {
            get { return _gdiFontFamily; }
        }
        readonly GdiFontFamily _gdiFontFamily;

        internal GdiFont GdiFont
        {
            get { return _gdiFont; }
        }
        Font _gdiFont;

        internal static XFontStyle FontStyleFrom(GdiFont font)
        {
            return
              (font.Bold ? XFontStyle.Bold : 0) |
              (font.Italic ? XFontStyle.Italic : 0) |
              (font.Strikeout ? XFontStyle.Strikeout : 0) |
              (font.Underline ? XFontStyle.Underline : 0);
        }

#if true || UseGdiObjects
        public static implicit operator XFont(GdiFont font)
        {
            return new XFont(font);
        }
#endif
#endif
        internal string Selector
        {
            get { return _selector; }
            set { _selector = value; }
        }
        string _selector;

        string DebuggerDisplay
        {
            get { return String.Format(CultureInfo.InvariantCulture, "font=('{0}' {1:0.##})", Name, Size); }
        }
    }
    public sealed class XFontFamily
    {
        public XFontFamily(string familyName)
        {
            FamilyInternal = FontFamilyInternal.GetOrCreateFromName(familyName, true);
        }

        internal XFontFamily(string familyName, bool createPlatformObjects)
        {
            FamilyInternal = FontFamilyInternal.GetOrCreateFromName(familyName, createPlatformObjects);
        }

        XFontFamily(FontFamilyInternal fontFamilyInternal)
        {
            FamilyInternal = fontFamilyInternal;
        }


        internal static XFontFamily CreateFromName_not_used(string name, bool createPlatformFamily)
        {
            XFontFamily fontFamily = new XFontFamily(name);
            if (createPlatformFamily)
            {

            }
            return fontFamily;
        }

        internal static XFontFamily GetOrCreateFontFamily(string name)
        {
            FontFamilyInternal fontFamilyInternal = FontFamilyCache.GetFamilyByName(name);
            if (fontFamilyInternal == null)
            {
                fontFamilyInternal = FontFamilyInternal.GetOrCreateFromName(name, false);
                fontFamilyInternal = FontFamilyCache.CacheOrGetFontFamily(fontFamilyInternal);
            }

            return new XFontFamily(fontFamilyInternal);
        }

#if CORE || GDI
        internal static XFontFamily GetOrCreateFromGdi(GdiFont font)
        {
            FontFamilyInternal fontFamilyInternal = FontFamilyInternal.GetOrCreateFromGdi(font.FontFamily);
            return new XFontFamily(fontFamilyInternal);
        }
#endif


        public string Name
        {
            get { return FamilyInternal.Name; }
        }

        public int GetCellAscent(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.Ascender;
            return result;
        }

        public int GetCellDescent(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.Descender;
            return result;
        }

        public int GetEmHeight(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.UnitsPerEm;
            return result;
        }

        public int GetLineSpacing(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.LineSpacing;
            return result;
        }

        public bool IsStyleAvailable(XFontStyle style)
        {
            XGdiFontStyle xStyle = ((XGdiFontStyle)style) & XGdiFontStyle.BoldItalic;
#if CORE
            throw new InvalidOperationException("In CORE build it is the responsibility of the developer to provide all required font faces.");
#endif
        }

        [Obsolete("Use platform API directly.")]
        public static XFontFamily[] Families
        {
            get
            {
                throw new InvalidOperationException("Obsolete and not implemted any more.");
            }
        }

        [Obsolete("Use platform API directly.")]
        public static XFontFamily[] GetFamilies(XGraphics graphics)
        {
            throw new InvalidOperationException("Obsolete and not implemted any more.");
        }
        internal FontFamilyInternal FamilyInternal;
    }
    public sealed class XFontMetrics
    {
        internal XFontMetrics(string name, int unitsPerEm, int ascent, int descent, int leading, int lineSpacing,
            int capHeight, int xHeight, int stemV, int stemH, int averageWidth, int maxWidth,
            int underlinePosition, int underlineThickness, int strikethroughPosition, int strikethroughThickness)
        {
            _name = name;
            _unitsPerEm = unitsPerEm;
            _ascent = ascent;
            _descent = descent;
            _leading = leading;
            _lineSpacing = lineSpacing;
            _capHeight = capHeight;
            _xHeight = xHeight;
            _stemV = stemV;
            _stemH = stemH;
            _averageWidth = averageWidth;
            _maxWidth = maxWidth;
            _underlinePosition = underlinePosition;
            _underlineThickness = underlineThickness;
            _strikethroughPosition = strikethroughPosition;
            _strikethroughThickness = strikethroughThickness;
        }

        public string Name
        {
            get { return _name; }
        }
        readonly string _name;

        public int UnitsPerEm
        {
            get { return _unitsPerEm; }
        }
        readonly int _unitsPerEm;

        public int Ascent
        {
            get { return _ascent; }
        }
        readonly int _ascent;

        public int Descent
        {
            get { return _descent; }
        }
        readonly int _descent;

        public int AverageWidth
        {
            get { return _averageWidth; }
        }
        readonly int _averageWidth;

        public int CapHeight
        {
            get { return _capHeight; }
        }
        readonly int _capHeight;

        public int Leading
        {
            get { return _leading; }
        }
        readonly int _leading;

        public int LineSpacing
        {
            get { return _lineSpacing; }
        }
        readonly int _lineSpacing;

        public int MaxWidth
        {
            get { return _maxWidth; }
        }
        readonly int _maxWidth;

        public int StemH
        {
            get { return _stemH; }
        }
        readonly int _stemH;

        public int StemV
        {
            get { return _stemV; }
        }
        readonly int _stemV;

        public int XHeight
        {
            get { return _xHeight; }
        }
        readonly int _xHeight;

        public int UnderlinePosition
        {
            get { return _underlinePosition; }
        }
        readonly int _underlinePosition;

        public int UnderlineThickness
        {
            get { return _underlineThickness; }
        }
        readonly int _underlineThickness;

        public int StrikethroughPosition
        {
            get { return _strikethroughPosition; }
        }
        readonly int _strikethroughPosition;

        public int StrikethroughThickness
        {
            get { return _strikethroughThickness; }
        }
        readonly int _strikethroughThickness;
    }
    internal class XFontSource
    {
        const uint ttcf = 0x66637474;

        XFontSource(byte[] bytes, ulong key)
        {
            _fontName = null;
            _bytes = bytes;
            _key = key;
        }

        public static XFontSource GetOrCreateFrom(byte[] bytes)
        {
            ulong key = FontHelper.CalcChecksum(bytes);
            XFontSource fontSource;
            if (!FontFactory.TryGetFontSourceByKey(key, out fontSource))
            {
                fontSource = new XFontSource(bytes, key);
                fontSource = FontFactory.CacheFontSource(fontSource);
            }
            return fontSource;
        }

#if CORE || GDI
        internal static XFontSource GetOrCreateFromGdi(string typefaceKey, GdiFont gdiFont)
        {
            byte[] bytes = ReadFontBytesFromGdi(gdiFont);
            XFontSource fontSource = GetOrCreateFrom(typefaceKey, bytes);
            return fontSource;
        }

        static byte[] ReadFontBytesFromGdi(GdiFont gdiFont)
        {
            int error = Marshal.GetLastWin32Error();
            error = Marshal.GetLastWin32Error();
            IntPtr hfont = gdiFont.ToHfont();
#if true
            IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);
#endif
            error = Marshal.GetLastWin32Error();
            IntPtr oldFont = NativeMethods.SelectObject(hdc, hfont);
            error = Marshal.GetLastWin32Error();
            bool isTtcf = false;
            int size = NativeMethods.GetFontData(hdc, 0, 0, null, 0);

            if ((uint)size == 0xc0000022)
                throw new InvalidOperationException("Microsoft Azure returns STATUS_ACCESS_DENIED ((NTSTATUS)0xC0000022L) from GetFontData. This is a bug in Azure. You must implement a FontResolver to circumvent this issue.");

            if (size == NativeMethods.GDI_ERROR)
            {
                size = NativeMethods.GetFontData(hdc, ttcf, 0, null, 0);
                isTtcf = true;
            }
            error = Marshal.GetLastWin32Error();
            if (size == 0)
                throw new InvalidOperationException("Cannot retrieve font data.");

            byte[] bytes = new byte[size];
            int effectiveSize = NativeMethods.GetFontData(hdc, isTtcf ? ttcf : 0, 0, bytes, size);
            Debug.Assert(size == effectiveSize);
            NativeMethods.SelectObject(hdc, oldFont);
            NativeMethods.ReleaseDC(IntPtr.Zero, hdc);

            return bytes;
        }
#endif
        static XFontSource GetOrCreateFrom(string typefaceKey, byte[] fontBytes)
        {
            XFontSource fontSource;
            ulong key = FontHelper.CalcChecksum(fontBytes);
            if (FontFactory.TryGetFontSourceByKey(key, out fontSource))
            {
                FontFactory.CacheExistingFontSourceWithNewTypefaceKey(typefaceKey, fontSource);
            }
            else
            {
                fontSource = new XFontSource(fontBytes, key);
                FontFactory.CacheNewFontSource(typefaceKey, fontSource);
            }
            return fontSource;
        }

        public static XFontSource CreateCompiledFont(byte[] bytes)
        {
            XFontSource fontSource = new XFontSource(bytes, 0);
            return fontSource;
        }

        internal OpenTypeFontface Fontface
        {
            get { return _fontface; }
            set
            {
                _fontface = value;
                _fontName = value.name.FullFontName;
            }
        }
        OpenTypeFontface _fontface;

        internal ulong Key
        {
            get
            {
                if (_key == 0)
                    _key = FontHelper.CalcChecksum(Bytes);
                return _key;
            }
        }
        ulong _key;

        public void IncrementKey()
        {
            _key += 1ul << 32;
        }

        public string FontName
        {
            get { return _fontName; }
        }
        string _fontName;

        public byte[] Bytes
        {
            get { return _bytes; }
        }
        readonly byte[] _bytes;

        public override int GetHashCode()
        {
            return (int)((Key >> 32) ^ Key);
        }

        public override bool Equals(object obj)
        {
            XFontSource fontSource = obj as XFontSource;
            if (fontSource == null)
                return false;
            return Key == fontSource.Key;
        }

        internal string DebuggerDisplay
        {
            get { return String.Format(CultureInfo.InvariantCulture, "XFontSource: '{0}', keyhash={1}", FontName, Key % 99991); }
        }
    }
    enum FontWeightValues
    {
        Thin = 100,
        ExtraLight = 200,
        Light = 300,
        Normal = 400,
        Medium = 500,
        SemiBold = 600,
        Bold = 700,
        ExtraBold = 800,
        Black = 900,
        ExtraBlack = 950,
    }
    public class XForm : XImage, IContentStream
    {
        internal enum FormState
        {
            NotATemplate,

            Created,

            UnderConstruction,

            Finished,
        }

        protected XForm()
        { }


        public XForm(PdfDocument document, XRect viewBox)
        {
            if (viewBox.Width < 1 || viewBox.Height < 1)
                throw new ArgumentNullException("viewBox", "The size of the XPdfForm is to small.");
            if (document == null)
                throw new ArgumentNullException("document", "An XPdfForm template must be associated with a document at creation time.");

            _formState = FormState.Created;
            _document = document;
            _pdfForm = new PdfFormXObject(document, this);
            _viewBox = viewBox;
            PdfRectangle rect = new PdfRectangle(viewBox);
            _pdfForm.Elements.SetRectangle(PdfFormXObject.Keys.BBox, rect);
        }

        public XForm(PdfDocument document, XSize size)
            : this(document, new XRect(0, 0, size.Width, size.Height))
        {
        }

        public XForm(PdfDocument document, XUnit width, XUnit height)
            : this(document, new XRect(0, 0, width, height))
        { }

        public void DrawingFinished()
        {
            if (_formState == FormState.Finished)
                return;

            if (_formState == FormState.NotATemplate)
                throw new InvalidOperationException("This object is an imported PDF page and you cannot finish drawing on it because you must not draw on it at all.");

            Finish();
        }

        internal void AssociateGraphics(XGraphics gfx)
        {
            if (_formState == FormState.NotATemplate)
                throw new NotImplementedException("The current version of PDFsharp cannot draw on an imported page.");

            if (_formState == FormState.UnderConstruction)
                throw new InvalidOperationException("An XGraphics object already exists for this form.");

            if (_formState == FormState.Finished)
                throw new InvalidOperationException("After drawing a form it cannot be modified anymore.");

            Debug.Assert(_formState == FormState.Created);
            _formState = FormState.UnderConstruction;
            Gfx = gfx;
        }
        internal XGraphics Gfx;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        internal virtual void Finish()
        {
        }

        internal PdfDocument Owner
        {
            get { return _document; }
        }
        PdfDocument _document;

        internal PdfColorMode ColorMode
        {
            get
            {
                if (_document == null)
                    return PdfColorMode.Undefined;
                return _document.Options.ColorMode;
            }
        }

        internal bool IsTemplate
        {
            get { return _formState != FormState.NotATemplate; }
        }
        internal FormState _formState;

        [Obsolete("Use either PixelWidth or PointWidth. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelWidth, but will become PointWidth in future releases of PDFsharp.")]
        public override double Width
        {
            get { return _viewBox.Width; }
        }

        [Obsolete("Use either PixelHeight or PointHeight. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelHeight, but will become PointHeight in future releases of PDFsharp.")]
        public override double Height
        {
            get { return _viewBox.Height; }
        }

        public override double PointWidth
        {
            get { return _viewBox.Width; }
        }

        public override double PointHeight
        {
            get { return _viewBox.Height; }
        }

        public override int PixelWidth
        {
            get { return (int)_viewBox.Width; }
        }

        public override int PixelHeight
        {
            get { return (int)_viewBox.Height; }
        }

        public override XSize Size
        {
            get { return _viewBox.Size; }
        }
        public XRect ViewBox
        {
            get { return _viewBox; }
        }
        XRect _viewBox;

        public override double HorizontalResolution
        {
            get { return 72; }
        }

        public override double VerticalResolution
        {
            get { return 72; }
        }

        public XRect BoundingBox
        {
            get { return _boundingBox; }
            set { _boundingBox = value; }
        }
        XRect _boundingBox;

        public virtual XMatrix Transform
        {
            get { return _transform; }
            set
            {
                if (_formState == FormState.Finished)
                    throw new InvalidOperationException("After a XPdfForm was once drawn it must not be modified.");
                _transform = value;
            }
        }
        internal XMatrix _transform;

        internal PdfResources Resources
        {
            get
            {
                Debug.Assert(IsTemplate, "This function is for form templates only.");
                return PdfForm.Resources;
            }
        }
        PdfResources IContentStream.Resources
        {
            get { return Resources; }
        }

        internal string GetFontName(XFont font, out PdfFont pdfFont)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            pdfFont = _document.FontTable.GetFont(font);
            Debug.Assert(pdfFont != null);
            string name = Resources.AddFont(pdfFont);
            return name;
        }

        string IContentStream.GetFontName(XFont font, out PdfFont pdfFont)
        {
            return GetFontName(font, out pdfFont);
        }

        internal string TryGetFontName(string idName, out PdfFont pdfFont)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            pdfFont = _document.FontTable.TryGetFont(idName);
            string name = null;
            if (pdfFont != null)
                name = Resources.AddFont(pdfFont);
            return name;
        }

        internal string GetFontName(string idName, byte[] fontData, out PdfFont pdfFont)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            pdfFont = _document.FontTable.GetFont(idName, fontData);
            Debug.Assert(pdfFont != null);
            string name = Resources.AddFont(pdfFont);
            return name;
        }

        string IContentStream.GetFontName(string idName, byte[] fontData, out PdfFont pdfFont)
        {
            return GetFontName(idName, fontData, out pdfFont);
        }

        internal string GetImageName(XImage image)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            PdfImage pdfImage = _document.ImageTable.GetImage(image);
            Debug.Assert(pdfImage != null);
            string name = Resources.AddImage(pdfImage);
            return name;
        }

        string IContentStream.GetImageName(XImage image)
        {
            return GetImageName(image);
        }

        internal PdfFormXObject PdfForm
        {
            get
            {
                Debug.Assert(IsTemplate, "This function is for form templates only.");
                if (_pdfForm.Reference == null)
                    _document._irefTable.Add(_pdfForm);
                return _pdfForm;
            }
        }

        internal string GetFormName(XForm form)
        {
            Debug.Assert(IsTemplate, "This function is for form templates only.");
            PdfFormXObject pdfForm = _document.FormTable.GetForm(form);
            Debug.Assert(pdfForm != null);
            string name = Resources.AddForm(pdfForm);
            return name;
        }

        string IContentStream.GetFormName(XForm form)
        {
            return GetFormName(form);
        }

        internal PdfFormXObject _pdfForm;

        internal XGraphicsPdfRenderer PdfRenderer;

    }
    internal sealed class XGlyphTypeface
    {
        const string KeyPrefix = "tk:";

#if CORE || GDI
        XGlyphTypeface(string key, XFontFamily fontFamily, XFontSource fontSource, XStyleSimulations styleSimulations, GdiFont gdiFont)
        {
            _key = key;
            _fontFamily = fontFamily;
            _fontSource = fontSource;

            _fontface = OpenTypeFontface.CetOrCreateFrom(fontSource);
            Debug.Assert(ReferenceEquals(_fontSource.Fontface, _fontface));

            _gdiFont = gdiFont;

            _styleSimulations = styleSimulations;
            Initialize();
        }
#endif
        public static XGlyphTypeface GetOrCreateFrom(string familyName, FontResolvingOptions fontResolvingOptions)
        {
            string typefaceKey = ComputeKey(familyName, fontResolvingOptions);
            XGlyphTypeface glyphTypeface;
            try
            {
                Lock.EnterFontFactory();
                if (GlyphTypefaceCache.TryGetGlyphTypeface(typefaceKey, out glyphTypeface))
                {
                    return glyphTypeface;
                }

                FontResolverInfo fontResolverInfo = FontFactory.ResolveTypeface(familyName, fontResolvingOptions, typefaceKey);
                if (fontResolverInfo == null)
                {
                    throw new InvalidOperationException("No appropriate font found.");
                }

#if CORE || GDI
                GdiFont gdiFont = null;
#endif
                XFontFamily fontFamily;
                PlatformFontResolverInfo platformFontResolverInfo = fontResolverInfo as PlatformFontResolverInfo;
                if (platformFontResolverInfo != null)
                {
#if CORE || GDI
                    gdiFont = platformFontResolverInfo.GdiFont;
                    fontFamily = XFontFamily.GetOrCreateFromGdi(gdiFont);
#endif
                }
                else
                {
                    fontFamily = XFontFamily.GetOrCreateFontFamily(familyName);
                }

                XFontSource fontSource = FontFactory.GetFontSourceByFontName(fontResolverInfo.FaceName);
                Debug.Assert(fontSource != null);

#if CORE || GDI
                glyphTypeface = new XGlyphTypeface(typefaceKey, fontFamily, fontSource, fontResolverInfo.StyleSimulations, gdiFont);
#endif
                GlyphTypefaceCache.AddGlyphTypeface(glyphTypeface);
            }
            finally { Lock.ExitFontFactory(); }
            return glyphTypeface;
        }

#if CORE || GDI
        public static XGlyphTypeface GetOrCreateFromGdi(GdiFont gdiFont)
        {
            string typefaceKey = ComputeKey(gdiFont);
            XGlyphTypeface glyphTypeface;
            if (GlyphTypefaceCache.TryGetGlyphTypeface(typefaceKey, out glyphTypeface))
            {
                return glyphTypeface;
            }

            XFontFamily fontFamily = XFontFamily.GetOrCreateFromGdi(gdiFont);
            XFontSource fontSource = XFontSource.GetOrCreateFromGdi(typefaceKey, gdiFont);

            XStyleSimulations styleSimulations = XStyleSimulations.None;
            if (gdiFont.Bold && !fontSource.Fontface.os2.IsBold)
                styleSimulations |= XStyleSimulations.BoldSimulation;
            if (gdiFont.Italic && !fontSource.Fontface.os2.IsItalic)
                styleSimulations |= XStyleSimulations.ItalicSimulation;

            glyphTypeface = new XGlyphTypeface(typefaceKey, fontFamily, fontSource, styleSimulations, gdiFont);
            GlyphTypefaceCache.AddGlyphTypeface(glyphTypeface);

            return glyphTypeface;
        }
#endif

        public XFontFamily FontFamily
        {
            get { return _fontFamily; }
        }
        readonly XFontFamily _fontFamily;

        internal OpenTypeFontface Fontface
        {
            get { return _fontface; }
        }
        readonly OpenTypeFontface _fontface;

        public XFontSource FontSource
        {
            get { return _fontSource; }
        }
        readonly XFontSource _fontSource;

        void Initialize()
        {
            _familyName = _fontface.name.Name;
            if (string.IsNullOrEmpty(_faceName) || _faceName.StartsWith("?"))
                _faceName = _familyName;
            _styleName = _fontface.name.Style;
            _displayName = _fontface.name.FullFontName;
            if (string.IsNullOrEmpty(_displayName))
            {
                _displayName = _familyName;
                if (string.IsNullOrEmpty(_styleName))
                    _displayName += " (" + _styleName + ")";
            }

            _isBold = _fontface.os2.IsBold;
            _isItalic = _fontface.os2.IsItalic;
        }

        internal string FaceName
        {
            get { return _faceName; }
        }
        string _faceName;

        public string FamilyName
        {
            get { return _familyName; }
        }
        string _familyName;

        public string StyleName
        {
            get { return _styleName; }
        }
        string _styleName;

        public string DisplayName
        {
            get { return _displayName; }
        }
        string _displayName;

        public bool IsBold
        {
            get { return _isBold; }
        }
        bool _isBold;

        public bool IsItalic
        {
            get { return _isItalic; }
        }
        bool _isItalic;

        public XStyleSimulations StyleSimulations
        {
            get { return _styleSimulations; }
        }
        XStyleSimulations _styleSimulations;

        string GetFaceNameSuffix()
        {
            if (IsBold)
                return IsItalic ? ",BoldItalic" : ",Bold";
            return IsItalic ? ",Italic" : "";
        }

        internal string GetBaseName()
        {
            string name = DisplayName;
            int ich = name.IndexOf("bold", StringComparison.OrdinalIgnoreCase);
            if (ich > 0)
                name = name.Substring(0, ich) + name.Substring(ich + 4, name.Length - ich - 4);
            ich = name.IndexOf("italic", StringComparison.OrdinalIgnoreCase);
            if (ich > 0)
                name = name.Substring(0, ich) + name.Substring(ich + 6, name.Length - ich - 6);
            name = name.Trim();
            name += GetFaceNameSuffix();
            return name;
        }

        internal static string ComputeKey(string familyName, FontResolvingOptions fontResolvingOptions)
        {
            string simulationSuffix = "";
            if (fontResolvingOptions.OverrideStyleSimulations)
            {
                switch (fontResolvingOptions.StyleSimulations)
                {
                    case XStyleSimulations.BoldSimulation: simulationSuffix = "|b+/i-"; break;
                    case XStyleSimulations.ItalicSimulation: simulationSuffix = "|b-/i+"; break;
                    case XStyleSimulations.BoldItalicSimulation: simulationSuffix = "|b+/i+"; break;
                    case XStyleSimulations.None: break;
                    default: throw new ArgumentOutOfRangeException("fontResolvingOptions");
                }
            }
            string key = KeyPrefix + familyName.ToLowerInvariant()
                + (fontResolvingOptions.IsItalic ? "/i" : "/n")
                + (fontResolvingOptions.IsBold ? "/700" : "/400") + "/5"
                + simulationSuffix;
            return key;
        }

        internal static string ComputeKey(string familyName, bool isBold, bool isItalic)
        {
            return ComputeKey(familyName, new FontResolvingOptions(FontHelper.CreateStyle(isBold, isItalic)));
        }

#if CORE || GDI
        internal static string ComputeKey(GdiFont gdiFont)
        {
            string name1 = gdiFont.Name;
            string name2 = gdiFont.OriginalFontName;
            string name3 = gdiFont.SystemFontName;

            string name = name1;
            GdiFontStyle style = gdiFont.Style;

            string key = KeyPrefix + name.ToLowerInvariant() + ((style & GdiFontStyle.Italic) == GdiFontStyle.Italic ? "/i" : "/n") + ((style & GdiFontStyle.Bold) == GdiFontStyle.Bold ? "/700" : "/400") + "/5";  
            return key;
        }
#endif


        public string Key
        {
            get { return _key; }
        }
        readonly string _key;

#if CORE || GDI
        internal GdiFont GdiFont
        {
            get { return _gdiFont; }
        }

        private readonly GdiFont _gdiFont;
#endif
        internal string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0} - {1} ({2})", FamilyName, StyleName, FaceName); }
        }
    }
    enum InternalGraphicsMode
    {
        DrawingGdiGraphics,
        DrawingPdfContent,
        DrawingBitmap,
    }
    public sealed class XGraphics : IDisposable
    {
        XGraphics(PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit pageUnit, XPageDirection pageDirection)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (page.Owner == null)
                throw new ArgumentException("You cannot draw on a page that is not owned by a PdfDocument object.", "page");

            if (page.RenderContent != null)
                throw new InvalidOperationException("An XGraphics object already exists for this page and must be disposed before a new one can be created.");

            if (page.Owner.IsReadOnly)
                throw new InvalidOperationException("Cannot create XGraphics for a page of a document that cannot be modified. Use PdfDocumentOpenMode.Modify.");

            _gsStack = new GraphicsStateStack(this);
            PdfContent content = null;
            switch (options)
            {
                case XGraphicsPdfPageOptions.Replace:
                    page.Contents.Elements.Clear();
                    goto case XGraphicsPdfPageOptions.Append;

                case XGraphicsPdfPageOptions.Prepend:
                    content = page.Contents.PrependContent();
                    break;

                case XGraphicsPdfPageOptions.Append:
                    content = page.Contents.AppendContent();
                    break;
            }
            page.RenderContent = content;

#if CORE
            TargetContext = XGraphicTargetContext.CORE;
#endif
            _renderer = new PdfSharp.Drawing.Pdf.XGraphicsPdfRenderer(page, this, options);
            _pageSizePoints = new XSize(page.Width, page.Height);
            switch (pageUnit)
            {
                case XGraphicsUnit.Point:
                    _pageSize = new XSize(page.Width, page.Height);
                    break;

                case XGraphicsUnit.Inch:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Inch, XUnit.FromPoint(page.Height).Inch);
                    break;

                case XGraphicsUnit.Millimeter:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Millimeter, XUnit.FromPoint(page.Height).Millimeter);
                    break;

                case XGraphicsUnit.Centimeter:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Centimeter, XUnit.FromPoint(page.Height).Centimeter);
                    break;

                case XGraphicsUnit.Presentation:
                    _pageSize = new XSize(XUnit.FromPoint(page.Width).Presentation, XUnit.FromPoint(page.Height).Presentation);
                    break;

                default:
                    throw new NotImplementedException("unit");
            }
            _pageUnit = pageUnit;
            _pageDirection = pageDirection;

            Initialize();
        }

        XGraphics(XForm form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            _form = form;
            form.AssociateGraphics(this);

            _gsStack = new GraphicsStateStack(this);
#if CORE
            TargetContext = XGraphicTargetContext.CORE;
            _drawGraphics = false;
            if (form.Owner != null)
                _renderer = new XGraphicsPdfRenderer(form, this);
            _pageSize = form.Size;
            Initialize();
#endif

        }

        public static XGraphics CreateMeasureContext(XSize size, XGraphicsUnit pageUnit, XPageDirection pageDirection)
        {
#if CORE
            PdfDocument dummy = new PdfDocument();
            PdfPage page = dummy.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append, pageUnit, pageDirection);
            return gfx;
#endif

        }



        public static XGraphics FromPdfPage(PdfPage page)
        {
            return new XGraphics(page, XGraphicsPdfPageOptions.Append, XGraphicsUnit.Point, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsUnit unit)
        {
            return new XGraphics(page, XGraphicsPdfPageOptions.Append, unit, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XPageDirection pageDirection)
        {
            return new XGraphics(page, XGraphicsPdfPageOptions.Append, XGraphicsUnit.Point, pageDirection);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options)
        {
            return new XGraphics(page, options, XGraphicsUnit.Point, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options, XPageDirection pageDirection)
        {
            return new XGraphics(page, options, XGraphicsUnit.Point, pageDirection);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit unit)
        {
            return new XGraphics(page, options, unit, XPageDirection.Downwards);
        }

        public static XGraphics FromPdfPage(PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit unit, XPageDirection pageDirection)
        {
            return new XGraphics(page, options, unit, pageDirection);
        }

        public static XGraphics FromPdfForm(XPdfForm form)
        {
            if (form.Gfx != null)
                return form.Gfx;

            return new XGraphics(form);
        }

        public static XGraphics FromForm(XForm form)
        {
            if (form.Gfx != null)
                return form.Gfx;

            return new XGraphics(form);
        }

        public static XGraphics FromImage(XImage image)
        {
            return FromImage(image, XGraphicsUnit.Point);
        }

        public static XGraphics FromImage(XImage image, XGraphicsUnit unit)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            XBitmapImage bmImage = image as XBitmapImage;
            if (bmImage != null)
            {
#if CORE
                return null;
#endif
            }
            return null;
        }

        void Initialize()
        {
            _pageOrigin = new XPoint();

            double pageHeight = _pageSize.Height;
            PdfPage targetPage = PdfPage;
            XPoint trimOffset = new XPoint();
            if (targetPage != null && targetPage.TrimMargins.AreSet)
            {
                pageHeight += targetPage.TrimMargins.Top.Point + targetPage.TrimMargins.Bottom.Point;
                trimOffset = new XPoint(targetPage.TrimMargins.Left.Point, targetPage.TrimMargins.Top.Point);
            }

            XMatrix matrix = new XMatrix();
#if CORE
            Debug.Assert(TargetContext == XGraphicTargetContext.CORE);
#endif
            if (_pageDirection != XPageDirection.Downwards)
                matrix.Prepend(new XMatrix(1, 0, 0, -1, 0, pageHeight));

            if (trimOffset != new XPoint())
                matrix.TranslatePrepend(trimOffset.X, -trimOffset.Y);

            DefaultViewMatrix = matrix;
            _transform = new XMatrix();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    if (_associatedImage != null)
                    {
                        _associatedImage.DisassociateWithGraphics(this);
                        _associatedImage = null;
                    }
                }

                if (_form != null)
                    _form.Finish();
                _drawGraphics = false;

                if (_renderer != null)
                {
                    _renderer.Close();
                    _renderer = null;
                }
            }
        }
        bool _disposed;

        public PdfFontEncoding MUH
        {
            get { return _muh; }
            set { _muh = value; }
        }
        PdfFontEncoding _muh;

        internal XGraphicTargetContext TargetContext;

        public XGraphicsUnit PageUnit
        {
            get { return _pageUnit; }
        }
        readonly XGraphicsUnit _pageUnit;

        public XPageDirection PageDirection
        {
            get { return _pageDirection; }
            set
            {
                if (value != XPageDirection.Downwards)
                    throw new NotImplementedException("PageDirection must be XPageDirection.Downwards in current implementation.");
            }
        }
        readonly XPageDirection _pageDirection;

        public XPoint PageOrigin
        {
            get { return _pageOrigin; }
            set
            {
                if (value != new XPoint())
                    throw new NotImplementedException("PageOrigin cannot be modified in current implementation.");
            }
        }
        XPoint _pageOrigin;

        public XSize PageSize
        {
            get { return _pageSize; }
        }
        XSize _pageSize;
        XSize _pageSizePoints;


        public void DrawLine(XPen pen, XPoint pt1, XPoint pt2)
        {
            DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void DrawLine(XPen pen, double x1, double y1, double x2, double y2)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawLines(pen, new[] { new XPoint(x1, y1), new XPoint(x2, y2) });
        }

        public void DrawLines(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawLines(pen, points);
        }

        public void DrawLines(XPen pen, double x, double y, params double[] value)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (value == null)
                throw new ArgumentNullException("value");

            int length = value.Length;
            XPoint[] points = new XPoint[length / 2 + 1];
            points[0].X = x;
            points[0].Y = y;
            for (int idx = 0; idx < length / 2; idx++)
            {
                points[idx + 1].X = value[2 * idx];
                points[idx + 1].Y = value[2 * idx + 1];
            }
            DrawLines(pen, points);
        }



        public void DrawBezier(XPen pen, XPoint pt1, XPoint pt2, XPoint pt3, XPoint pt4)
        {
            DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void DrawBezier(XPen pen, double x1, double y1, double x2, double y2,
          double x3, double y3, double x4, double y4)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawBeziers(pen,
                  new XPoint[] { new XPoint(x1, y1), new XPoint(x2, y2), new XPoint(x3, y3), new XPoint(x4, y4) });
        }


        public void DrawBeziers(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            int count = points.Length;
            if (count == 0)
                return;

            if ((count - 1) % 3 != 0)
                throw new ArgumentException("Invalid number of points for bezier curves. Number must fulfill 4+3n.", "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawBeziers(pen, points);
        }



        public void DrawCurve(XPen pen, XPoint[] points)
        {
            DrawCurve(pen, points, 0.5);
        }


        public void DrawCurve(XPen pen, XPoint[] points, int offset, int numberOfSegments, double tension)
        {
            XPoint[] points2 = new XPoint[numberOfSegments];
            Array.Copy(points, offset, points2, 0, numberOfSegments);
            DrawCurve(pen, points2, tension);
        }

        public void DrawCurve(XPen pen, XPoint[] points, double tension)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("DrawCurve requires two or more points.", "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawCurve(pen, points, tension);
        }



        public void DrawArc(XPen pen, XRect rect, double startAngle, double sweepAngle)
        {
            DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawArc(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (Math.Abs(sweepAngle) >= 360)
            {
                DrawEllipse(pen, x, y, width, height);
            }
            else
            {
                if (_drawGraphics)
                {

                }

                if (_renderer != null)
                    _renderer.DrawArc(pen, x, y, width, height, startAngle, sweepAngle);
            }
        }


        public void DrawRectangle(XPen pen, XRect rect)
        {
            DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(XPen pen, double x, double y, double width, double height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRectangle(pen, null, x, y, width, height);
        }


        public void DrawRectangle(XBrush brush, XRect rect)
        {
            DrawRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(XBrush brush, double x, double y, double width, double height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRectangle(null, brush, x, y, width, height);
        }


        public void DrawRectangle(XPen pen, XBrush brush, XRect rect)
        {
            DrawRectangle(pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawRectangle(XPen pen, XBrush brush, double x, double y, double width, double height)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRectangle(pen, brush, x, y, width, height);
        }

        public void DrawRectangles(XPen pen, XRect[] rectangles)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (rectangles == null)
                throw new ArgumentNullException("rectangles");

            DrawRectangles(pen, null, rectangles);
        }


        public void DrawRectangles(XBrush brush, XRect[] rectangles)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (rectangles == null)
                throw new ArgumentNullException("rectangles");

            DrawRectangles(null, brush, rectangles);
        }

        public void DrawRectangles(XPen pen, XBrush brush, XRect[] rectangles)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            if (rectangles == null)
                throw new ArgumentNullException("rectangles");

            int count = rectangles.Length;
            if (_drawGraphics)
            {

            }

            if (_renderer != null)
            {
                for (int idx = 0; idx < count; idx++)
                {
                    XRect rect = rectangles[idx];
                    _renderer.DrawRectangle(pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }



        public void DrawRoundedRectangle(XPen pen, XRect rect, XSize ellipseSize)
        {
            DrawRoundedRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
        }

        public void DrawRoundedRectangle(XPen pen, double x, double y, double width, double height, double ellipseWidth, double ellipseHeight)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            DrawRoundedRectangle(pen, null, x, y, width, height, ellipseWidth, ellipseHeight);
        }



        public void DrawRoundedRectangle(XBrush brush, XRect rect, XSize ellipseSize)
        {
            DrawRoundedRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
        }

        public void DrawRoundedRectangle(XBrush brush, double x, double y, double width, double height, double ellipseWidth, double ellipseHeight)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            DrawRoundedRectangle(null, brush, x, y, width, height, ellipseWidth, ellipseHeight);
        }

        public void DrawRoundedRectangle(XPen pen, XBrush brush, XRect rect, XSize ellipseSize)
        {
            DrawRoundedRectangle(pen, brush, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
        }

        public void DrawRoundedRectangle(XPen pen, XBrush brush, double x, double y, double width, double height,
            double ellipseWidth, double ellipseHeight)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawRoundedRectangle(pen, brush, x, y, width, height, ellipseWidth, ellipseHeight);
        }


        public void DrawEllipse(XPen pen, XRect rect)
        {
            DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(XPen pen, double x, double y, double width, double height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawEllipse(pen, null, x, y, width, height);
        }


        public void DrawEllipse(XBrush brush, XRect rect)
        {
            DrawEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(XBrush brush, double x, double y, double width, double height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawEllipse(null, brush, x, y, width, height);
        }


        public void DrawEllipse(XPen pen, XBrush brush, XRect rect)
        {
            DrawEllipse(pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(XPen pen, XBrush brush, double x, double y, double width, double height)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawEllipse(pen, brush, x, y, width, height);
        }


        public void DrawPolygon(XPen pen, XPoint[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPolygon(pen, null, points, XFillMode.Alternate);
        }


        public void DrawPolygon(XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPolygon(null, brush, points, fillmode);
        }



        public void DrawPolygon(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length < 2)
                throw new ArgumentException(PSSR.PointArrayAtLeast(2), "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPolygon(pen, brush, points, fillmode);
        }



        public void DrawPie(XPen pen, XRect rect, double startAngle, double sweepAngle)
        {
            DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(XPen pen, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPie(pen, null, x, y, width, height, startAngle, sweepAngle);
        }



        public void DrawPie(XBrush brush, XRect rect, double startAngle, double sweepAngle)
        {
            DrawPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(XBrush brush, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (brush == null)
                throw new ArgumentNullException("brush", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {


            }

            if (_renderer != null)
                _renderer.DrawPie(null, brush, x, y, width, height, startAngle, sweepAngle);
        }


        public void DrawPie(XPen pen, XBrush brush, XRect rect, double startAngle, double sweepAngle)
        {
            DrawPie(pen, brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(XPen pen, XBrush brush, double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            if (pen == null && brush == null)
                throw new ArgumentNullException("pen", PSSR.NeedPenOrBrush);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPie(pen, brush, x, y, width, height, startAngle, sweepAngle);
        }



        public void DrawClosedCurve(XPen pen, XPoint[] points)
        {
            DrawClosedCurve(pen, null, points, XFillMode.Alternate, 0.5);
        }



        public void DrawClosedCurve(XPen pen, XPoint[] points, double tension)
        {
            DrawClosedCurve(pen, null, points, XFillMode.Alternate, tension);
        }


        public void DrawClosedCurve(XBrush brush, XPoint[] points)
        {
            DrawClosedCurve(null, brush, points, XFillMode.Alternate, 0.5);
        }


        public void DrawClosedCurve(XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            DrawClosedCurve(null, brush, points, fillmode, 0.5);
        }



        public void DrawClosedCurve(XBrush brush, XPoint[] points, XFillMode fillmode, double tension)
        {
            DrawClosedCurve(null, brush, points, fillmode, tension);
        }


        public void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points)
        {
            DrawClosedCurve(pen, brush, points, XFillMode.Alternate, 0.5);
        }


        public void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode)
        {
            DrawClosedCurve(pen, brush, points, fillmode, 0.5);
        }


        public void DrawClosedCurve(XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode, double tension)
        {
            if (pen == null && brush == null)
            {
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            }

            int count = points.Length;
            if (count == 0)
                return;
            if (count < 2)
                throw new ArgumentException("Not enough points.", "points");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawClosedCurve(pen, brush, points, tension, fillmode);
        }

        public void DrawPath(XPen pen, XGraphicsPath path)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPath(pen, null, path);
        }

        public void DrawPath(XBrush brush, XGraphicsPath path)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPath(null, brush, path);
        }

        public void DrawPath(XPen pen, XBrush brush, XGraphicsPath path)
        {
            if (pen == null && brush == null)
            {
                throw new ArgumentNullException("pen and brush", PSSR.NeedPenOrBrush);
            }
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawPath(pen, brush, path);
        }

        public void DrawString(string s, XFont font, XBrush brush, XPoint point)
        {
            DrawString(s, font, brush, new XRect(point.X, point.Y, 0, 0), XStringFormats.Default);
        }

        public void DrawString(string s, XFont font, XBrush brush, XPoint point, XStringFormat format)
        {
            DrawString(s, font, brush, new XRect(point.X, point.Y, 0, 0), format);
        }

        public void DrawString(string s, XFont font, XBrush brush, double x, double y)
        {
            DrawString(s, font, brush, new XRect(x, y, 0, 0), XStringFormats.Default);
        }

        public void DrawString(string s, XFont font, XBrush brush, double x, double y, XStringFormat format)
        {
            DrawString(s, font, brush, new XRect(x, y, 0, 0), format);
        }


        public void DrawString(string s, XFont font, XBrush brush, XRect layoutRectangle)
        {
            DrawString(s, font, brush, layoutRectangle, XStringFormats.Default);
        }

        public void DrawString(string text, XFont font, XBrush brush, XRect layoutRectangle, XStringFormat format)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (font == null)
                throw new ArgumentNullException("font");
            if (brush == null)
                throw new ArgumentNullException("brush");

            if (format != null && format.LineAlignment == XLineAlignment.BaseLine && layoutRectangle.Height != 0)
                throw new InvalidOperationException("DrawString: With XLineAlignment.BaseLine the height of the layout rectangle must be 0.");

            if (text.Length == 0)
                return;

            if (format == null)
                format = XStringFormats.Default;
            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawString(text, font, brush, layoutRectangle, format);
        }

        public XSize MeasureString(string text, XFont font, XStringFormat stringFormat)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (font == null)
                throw new ArgumentNullException("font");
            if (stringFormat == null)
                throw new ArgumentNullException("stringFormat");
#if true
            return FontHelper.MeasureString(text, font, stringFormat);
#else

#endif
#if CORE || NETFX_CORE || UWP
            XSize size = FontHelper.MeasureString(text, font, XStringFormats.Default);
            return size;
#endif

        }

        public XSize MeasureString(string text, XFont font)
        {
            return MeasureString(text, font, XStringFormats.Default);
        }


        public void DrawImage(XImage image, XPoint point)
        {
            DrawImage(image, point.X, point.Y);
        }

        public void DrawImage(XImage image, double x, double y)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            CheckXPdfFormConsistence(image);

            double width = image.PointWidth;
            double height = image.PointHeight;

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawImage(image, x, y, image.PointWidth, image.PointHeight);
        }


        public void DrawImage(XImage image, XRect rect)
        {
            DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawImage(XImage image, double x, double y, double width, double height)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            CheckXPdfFormConsistence(image);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawImage(image, x, y, width, height);
        }


        public void DrawImage(XImage image, XRect destRect, XRect srcRect, XGraphicsUnit srcUnit)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            CheckXPdfFormConsistence(image);

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.DrawImage(image, destRect, srcRect, srcUnit);
        }

        void DrawMissingImageRect(XRect rect)
        {


        }

        void CheckXPdfFormConsistence(XImage image)
        {
            XForm xForm = image as XForm;
            if (xForm != null)
            {
                xForm.Finish();

                if (_renderer != null && (_renderer as XGraphicsPdfRenderer) != null)
                {
                    if (xForm.Owner != null && xForm.Owner != ((XGraphicsPdfRenderer)_renderer).Owner)
                        throw new InvalidOperationException(
                            "A XPdfForm object is always bound to the document it was created for and cannot be drawn in the context of another document.");

                    if (xForm == ((XGraphicsPdfRenderer)_renderer)._form)
                        throw new InvalidOperationException(
                            "A XPdfForm cannot be drawn on itself.");
                }
            }
        }

        public void DrawBarCode(BarCodes.BarCode barcode, XPoint position)
        {
            barcode.Render(this, XBrushes.Black, null, position);
        }

        public void DrawBarCode(BarCodes.BarCode barcode, XBrush brush, XPoint position)
        {
            barcode.Render(this, brush, null, position);
        }

        public void DrawBarCode(BarCodes.BarCode barcode, XBrush brush, XFont font, XPoint position)
        {
            barcode.Render(this, brush, font, position);
        }

        public void DrawMatrixCode(BarCodes.MatrixCode matrixcode, XPoint position)
        {
            matrixcode.Render(this, XBrushes.Black, position);
        }

        public void DrawMatrixCode(BarCodes.MatrixCode matrixcode, XBrush brush, XPoint position)
        {
            matrixcode.Render(this, brush, position);
        }

        public XGraphicsState Save()
        {
            XGraphicsState xState = null;
#if CORE || NETFX_CORE
            if (TargetContext == XGraphicTargetContext.CORE || TargetContext == XGraphicTargetContext.NONE)
            {
                xState = new XGraphicsState();
                InternalGraphicsState iState = new InternalGraphicsState(this, xState);
                iState.Transform = _transform;
                _gsStack.Push(iState);
            }
            else
            {
                Debug.Assert(false, "XGraphicTargetContext must be XGraphicTargetContext.CORE.");
            }
#endif

            if (_renderer != null)
                _renderer.Save(xState);

            return xState;
        }

        public void Restore(XGraphicsState state)
        {
            if (state == null)
                throw new ArgumentNullException("state");

#if CORE
            if (TargetContext == XGraphicTargetContext.CORE)
            {
                _gsStack.Restore(state.InternalState);
                _transform = state.InternalState.Transform;
            }
#endif


            if (_renderer != null)
                _renderer.Restore(state);
        }

        public void Restore()
        {
            if (_gsStack.Count == 0)
                throw new InvalidOperationException("Cannot restore without preceding save operation.");
            Restore(_gsStack.Current.State);
        }

        public XGraphicsContainer BeginContainer()
        {
            return BeginContainer(new XRect(0, 0, 1, 1), new XRect(0, 0, 1, 1), XGraphicsUnit.Point);
        }


        public XGraphicsContainer BeginContainer(XRect dstrect, XRect srcrect, XGraphicsUnit unit)
        {
            if (unit != XGraphicsUnit.Point)
                throw new ArgumentException("The current implementation supports XGraphicsUnit.Point only.", "unit");

            XGraphicsContainer xContainer = null;
#if CORE
            if (TargetContext == XGraphicTargetContext.CORE)
                xContainer = new XGraphicsContainer();
#endif

            InternalGraphicsState iState = new InternalGraphicsState(this, xContainer);
            iState.Transform = _transform;

            _gsStack.Push(iState);

            if (_renderer != null)
                _renderer.BeginContainer(xContainer, dstrect, srcrect, unit);

            XMatrix matrix = new XMatrix();
            double scaleX = dstrect.Width / srcrect.Width;
            double scaleY = dstrect.Height / srcrect.Height;
            matrix.TranslatePrepend(-srcrect.X, -srcrect.Y);
            matrix.ScalePrepend(scaleX, scaleY);
            matrix.TranslatePrepend(dstrect.X / scaleX, dstrect.Y / scaleY);
            AddTransform(matrix, XMatrixOrder.Prepend);

            return xContainer;
        }

        public void EndContainer(XGraphicsContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _gsStack.Restore(container.InternalState);

            _transform = container.InternalState.Transform;

            if (_renderer != null)
                _renderer.EndContainer(container);
        }

        public int GraphicsStateLevel
        {
            get { return _gsStack.Count; }
        }

        public XSmoothingMode SmoothingMode
        {
            get
            {
                return _smoothingMode;
            }
            set
            {
                _smoothingMode = value;

            }
        }
        XSmoothingMode _smoothingMode;

        public void TranslateTransform(double dx, double dy)
        {
            AddTransform(XMatrix.CreateTranslation(dx, dy), XMatrixOrder.Prepend);
        }

        public void TranslateTransform(double dx, double dy, XMatrixOrder order)
        {
            XMatrix matrix = new XMatrix();
            matrix.TranslatePrepend(dx, dy);
            AddTransform(matrix, order);
        }

        public void ScaleTransform(double scaleX, double scaleY)
        {
            AddTransform(XMatrix.CreateScaling(scaleX, scaleY), XMatrixOrder.Prepend);
        }

        public void ScaleTransform(double scaleX, double scaleY, XMatrixOrder order)
        {
            XMatrix matrix = new XMatrix();
            matrix.ScalePrepend(scaleX, scaleY);
            AddTransform(matrix, order);
        }

        public void ScaleTransform(double scaleXY)
        {
            ScaleTransform(scaleXY, scaleXY);
        }

        public void ScaleTransform(double scaleXY, XMatrixOrder order)
        {
            ScaleTransform(scaleXY, scaleXY, order);
        }

        public void ScaleAtTransform(double scaleX, double scaleY, double centerX, double centerY)
        {
            AddTransform(XMatrix.CreateScaling(scaleX, scaleY, centerX, centerY), XMatrixOrder.Prepend);
        }

        public void ScaleAtTransform(double scaleX, double scaleY, XPoint center)
        {
            AddTransform(XMatrix.CreateScaling(scaleX, scaleY, center.X, center.Y), XMatrixOrder.Prepend);
        }

        public void RotateTransform(double angle)
        {
            AddTransform(XMatrix.CreateRotationRadians(angle * Const.Deg2Rad), XMatrixOrder.Prepend);
        }

        public void RotateTransform(double angle, XMatrixOrder order)
        {
            XMatrix matrix = new XMatrix();
            matrix.RotatePrepend(angle);
            AddTransform(matrix, order);
        }

        public void RotateAtTransform(double angle, XPoint point)
        {
            AddTransform(XMatrix.CreateRotationRadians(angle * Const.Deg2Rad, point.X, point.Y), XMatrixOrder.Prepend);
        }

        public void RotateAtTransform(double angle, XPoint point, XMatrixOrder order)
        {
            AddTransform(XMatrix.CreateRotationRadians(angle * Const.Deg2Rad, point.X, point.Y), order);
        }

        public void ShearTransform(double shearX, double shearY)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad), XMatrixOrder.Prepend);
        }

        public void ShearTransform(double shearX, double shearY, XMatrixOrder order)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad), order);
        }

        public void SkewAtTransform(double shearX, double shearY, double centerX, double centerY)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad, centerX, centerY), XMatrixOrder.Prepend);
        }

        public void SkewAtTransform(double shearX, double shearY, XPoint center)
        {
            AddTransform(XMatrix.CreateSkewRadians(shearX * Const.Deg2Rad, shearY * Const.Deg2Rad, center.X, center.Y), XMatrixOrder.Prepend);
        }

        public void MultiplyTransform(XMatrix matrix)
        {
            AddTransform(matrix, XMatrixOrder.Prepend);
        }

        public void MultiplyTransform(XMatrix matrix, XMatrixOrder order)
        {
            AddTransform(matrix, order);
        }

        public XMatrix Transform
        {
            get { return _transform; }
        }

        void AddTransform(XMatrix transform, XMatrixOrder order)
        {
            XMatrix matrix = _transform;
            matrix.Multiply(transform, order);
            _transform = matrix;
            matrix = DefaultViewMatrix;
            matrix.Multiply(_transform, XMatrixOrder.Prepend);
#if CORE
            if (TargetContext == XGraphicTargetContext.CORE)
            {
                GetType();
            }
#endif

            if (_renderer != null)
                _renderer.AddTransform(transform, XMatrixOrder.Prepend);
        }


        public void IntersectClip(XRect rect)
        {
            XGraphicsPath path = new XGraphicsPath();
            path.AddRectangle(rect);
            IntersectClip(path);
        }

        public void IntersectClip(XGraphicsPath path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (_drawGraphics)
            {

            }

            if (_renderer != null)
                _renderer.SetClip(path, XCombineMode.Intersect);
        }

        public void WriteComment(string comment)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");

            if (_drawGraphics)
            {
            }

            if (_renderer != null)
                _renderer.WriteComment(comment);
        }

        public XGraphicsInternals Internals
        {
            get { return _internals ?? (_internals = new XGraphicsInternals(this)); }
        }
        XGraphicsInternals _internals;

        public SpaceTransformer Transformer
        {
            get { return _transformer ?? (_transformer = new SpaceTransformer(this)); }
        }
        SpaceTransformer _transformer;

        internal void DisassociateImage()
        {
            if (_associatedImage == null)
                throw new InvalidOperationException("No image associated.");

            Dispose();
        }

        internal InternalGraphicsMode InternalGraphicsMode
        {
            get { return _internalGraphicsMode; }
            set { _internalGraphicsMode = value; }
        }
        InternalGraphicsMode _internalGraphicsMode;

        internal XImage AssociatedImage
        {
            get { return _associatedImage; }
            set { _associatedImage = value; }
        }
        XImage _associatedImage;

        internal XMatrix DefaultViewMatrix;

        bool _drawGraphics;

        readonly XForm _form;


        IXGraphicsRenderer _renderer;

        XMatrix _transform;

        readonly GraphicsStateStack _gsStack;

        public PdfPage PdfPage
        {
            get
            {
                XGraphicsPdfRenderer renderer = _renderer as PdfSharp.Drawing.Pdf.XGraphicsPdfRenderer;
                return renderer != null ? renderer._page : null;
            }
        }


        public class XGraphicsInternals
        {
            internal XGraphicsInternals(XGraphics gfx)
            {
                _gfx = gfx;
            }
            readonly XGraphics _gfx;

        }

        public class SpaceTransformer
        {
            internal SpaceTransformer(XGraphics gfx)
            {
                _gfx = gfx;
            }
            readonly XGraphics _gfx;

            public XRect WorldToDefaultPage(XRect rect)
            {
                XPoint[] points = new XPoint[4];
                points[0] = new XPoint(rect.X, rect.Y);
                points[1] = new XPoint(rect.X + rect.Width, rect.Y);
                points[2] = new XPoint(rect.X, rect.Y + rect.Height);
                points[3] = new XPoint(rect.X + rect.Width, rect.Y + rect.Height);

                XMatrix matrix = _gfx.Transform;
                matrix.TransformPoints(points);

                double height = _gfx.PageSize.Height;
                points[0].Y = height - points[0].Y;
                points[1].Y = height - points[1].Y;
                points[2].Y = height - points[2].Y;
                points[3].Y = height - points[3].Y;

                double xmin = Math.Min(Math.Min(points[0].X, points[1].X), Math.Min(points[2].X, points[3].X));
                double xmax = Math.Max(Math.Max(points[0].X, points[1].X), Math.Max(points[2].X, points[3].X));
                double ymin = Math.Min(Math.Min(points[0].Y, points[1].Y), Math.Min(points[2].Y, points[3].Y));
                double ymax = Math.Max(Math.Max(points[0].Y, points[1].Y), Math.Max(points[2].Y, points[3].Y));

                return new XRect(xmin, ymin, xmax - xmin, ymax - ymin);
            }
        }
    }
    public sealed class XGraphicsContainer
    {

        internal InternalGraphicsState InternalState;
    }
    public sealed class XGraphicsPath
    {
        public XGraphicsPath()
        {

        }


        public XGraphicsPath Clone()
        {
            XGraphicsPath path = (XGraphicsPath)MemberwiseClone();
            return path;
        }

        public void AddLine(XPoint pt1, XPoint pt2)
        {
            AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void AddLine(double x1, double y1, double x2, double y2)
        {
#if CORE
            _corePath.MoveOrLineTo(x1, y1);
            _corePath.LineTo(x2, y2, false);
#endif
        }

        public void AddLines(XPoint[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count == 0)
                return;
#if CORE
            _corePath.MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx++)
                _corePath.LineTo(points[idx].X, points[idx].Y, false);
#endif
        }


        public void AddBezier(XPoint pt1, XPoint pt2, XPoint pt3, XPoint pt4)
        {
            AddBezier(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void AddBezier(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
#if CORE
            _corePath.MoveOrLineTo(x1, y1);
            _corePath.BezierTo(x2, y2, x3, y3, x4, y4, false);
#endif
        }

        public void AddBeziers(XPoint[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count < 4)
                throw new ArgumentException("At least four points required for bezier curve.", "points");

            if ((count - 1) % 3 != 0)
                throw new ArgumentException("Invalid number of points for bezier curve. Number must fulfil 4+3n.",
                    "points");

#if CORE
            _corePath.MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
            {
                _corePath.BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y,
                    points[idx + 2].X, points[idx + 2].Y, false);
            }
#endif
        }


        public void AddCurve(XPoint[] points)
        {
            AddCurve(points, 0.5);
        }

        public void AddCurve(XPoint[] points, double tension)
        {
            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("AddCurve requires two or more points.", "points");
#if CORE
            _corePath.AddCurve(points, tension);
#endif

        }


        public void AddCurve(XPoint[] points, int offset, int numberOfSegments, double tension)
        {
#if CORE
            throw new NotImplementedException("AddCurve not yet implemented.");
#endif
        }

        public void AddArc(XRect rect, double startAngle, double sweepAngle)
        {
            AddArc(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddArc(double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
#if CORE
            _corePath.AddArc(x, y, width, height, startAngle, sweepAngle);
#endif
        }

        public void AddArc(XPoint point1, XPoint point2, XSize size, double rotationAngle, bool isLargeArg, XSweepDirection sweepDirection)
        {
        }
        public void AddRectangle(XRect rect)
        {
#if CORE
            _corePath.MoveTo(rect.X, rect.Y);
            _corePath.LineTo(rect.X + rect.Width, rect.Y, false);
            _corePath.LineTo(rect.X + rect.Width, rect.Y + rect.Height, false);
            _corePath.LineTo(rect.X, rect.Y + rect.Height, true);
            _corePath.CloseSubpath();
#endif
        }

        public void AddRectangle(double x, double y, double width, double height)
        {
            AddRectangle(new XRect(x, y, width, height));
        }

        public void AddRectangles(XRect[] rects)
        {
            int count = rects.Length;
            for (int idx = 0; idx < count; idx++)
            {
#if CORE
                AddRectangle(rects[idx]);
#endif
            }
        }


        public void AddRoundedRectangle(double x, double y, double width, double height, double ellipseWidth, double ellipseHeight)
        {
#if CORE
#if true
            double arcWidth = ellipseWidth / 2;
            double arcHeight = ellipseHeight / 2;
#if true   
            _corePath.MoveTo(x + width - arcWidth, y);
            _corePath.QuadrantArcTo(x + width - arcWidth, y + arcHeight, arcWidth, arcHeight, 1, true);

            _corePath.LineTo(x + width, y + height - arcHeight, false);
            _corePath.QuadrantArcTo(x + width - arcWidth, y + height - arcHeight, arcWidth, arcHeight, 4, true);

            _corePath.LineTo(x + arcWidth, y + height, false);
            _corePath.QuadrantArcTo(x + arcWidth, y + height - arcHeight, arcWidth, arcHeight, 3, true);

            _corePath.LineTo(x, y + arcHeight, false);
            _corePath.QuadrantArcTo(x + arcWidth, y + arcHeight, arcWidth, arcHeight, 2, true);

            _corePath.CloseSubpath();
 
#endif
#endif
#endif

        }

        public void AddEllipse(XRect rect)
        {
            AddEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void AddEllipse(double x, double y, double width, double height)
        {
#if CORE
            double w = width / 2;
            double h = height / 2;
            double xc = x + w;
            double yc = y + h;
            _corePath.MoveTo(x + w, y);
            _corePath.QuadrantArcTo(xc, yc, w, h, 1, true);
            _corePath.QuadrantArcTo(xc, yc, w, h, 4, true);
            _corePath.QuadrantArcTo(xc, yc, w, h, 3, true);
            _corePath.QuadrantArcTo(xc, yc, w, h, 2, true);
            _corePath.CloseSubpath();
#endif
        }

        public void AddPolygon(XPoint[] points)
        {
#if CORE
            int count = points.Length;
            if (count == 0)
                return;

            _corePath.MoveTo(points[0].X, points[0].Y);
            for (int idx = 0; idx < count - 1; idx++)
                _corePath.LineTo(points[idx].X, points[idx].Y, false);
            _corePath.LineTo(points[count - 1].X, points[count - 1].Y, true);
            _corePath.CloseSubpath();
#endif

        }

        public void AddPie(XRect rect, double startAngle, double sweepAngle)
        {
            AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddPie(double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddPie");
#endif

        }

        public void AddClosedCurve(XPoint[] points)
        {
            AddClosedCurve(points, 0.5);
        }

        public void AddClosedCurve(XPoint[] points, double tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            int count = points.Length;
            if (count == 0)
                return;
            if (count < 2)
                throw new ArgumentException("Not enough points.", "points");

#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddClosedCurve");
#endif

        }

        public void AddPath(XGraphicsPath path, bool connect)
        {
#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddPath");
#endif

        }


        public void AddString(string s, XFontFamily family, XFontStyle style, double emSize, XPoint origin,
            XStringFormat format)
        {
            try
            {
#if CORE
                DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddString");
#endif

            }
            catch
            {
                throw;
            }
        }


        public void AddString(string s, XFontFamily family, XFontStyle style, double emSize, XRect layoutRect,
            XStringFormat format)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            if (family == null)
                throw new ArgumentNullException("family");

            if (format == null)
                format = XStringFormats.Default;

            if (format.LineAlignment == XLineAlignment.BaseLine && layoutRect.Height != 0)
                throw new InvalidOperationException(
                    "DrawString: With XLineAlignment.BaseLine the height of the layout rectangle must be 0.");

            if (s.Length == 0)
                return;

            XFont font = new XFont(family.Name, emSize, style);
#if CORE
            DiagnosticsHelper.HandleNotImplemented("XGraphicsPath.AddString");
#endif

        }

        public void CloseFigure()
        {
#if CORE
            _corePath.CloseSubpath();
#endif

        }

        public void StartFigure()
        {

        }

        public XFillMode FillMode
        {
            get { return _fillMode; }
            set
            {
                _fillMode = value;

            }
        }

        private XFillMode _fillMode;

        public void Flatten()
        {

        }

        public void Flatten(XMatrix matrix)
        {

        }

        public void Flatten(XMatrix matrix, double flatness)
        {

        }

        public void Widen(XPen pen)
        {

        }

        public void Widen(XPen pen, XMatrix matrix)
        {

        }

        public void Widen(XPen pen, XMatrix matrix, double flatness)
        {


        }

        public XGraphicsPathInternals Internals
        {
            get { return new XGraphicsPathInternals(this); }
        }

#if CORE
        internal CoreGraphicsPath _corePath;
#endif
    }
    public sealed class XGraphicsPathInternals
    {
        internal XGraphicsPathInternals(XGraphicsPath path)
        {
            _path = path;
        }
        XGraphicsPath _path;
    }
    public sealed class XGraphicsState
    {
#if CORE
        internal XGraphicsState()
        { }
#endif
        internal InternalGraphicsState InternalState;
    }
    internal enum XImageState
    {
        UsedInDrawingContext = 0x00000001,

        StateMask = 0x0000FFFF,
    }

    public class XImage : IDisposable
    {

        protected XImage()
        { }

#if GDI || CORE || WPF
        XImage(ImportedImage image)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            _importedImage = image;
            Initialize();
        }
#endif

        XImage(string path)
        {
#if !NETFX_CORE && !UWP
            path = Path.GetFullPath(path);
            if (!File.Exists(path))
                throw new FileNotFoundException(PSSR.FileNotFound(path));
#endif
            _path = path;

#if CORE_WITH_GDI || GDI
            try
            {
                Lock.EnterGdiPlus();
                _gdiImage = Image.FromFile(path);
            }
            finally { Lock.ExitGdiPlus(); }
#endif

            Initialize();
        }

        XImage(Stream stream)
        {
            _path = "*" + Guid.NewGuid().ToString("B");

#if CORE_WITH_GDI
            try
            {
                Lock.EnterGdiPlus();
                _gdiImage = Image.FromStream(stream);
            }
            finally { Lock.ExitGdiPlus(); }
#endif

            _stream = stream;
            Initialize();
        }


        public static XImage FromFile(string path)
        {
            if (PdfReader.TestPdfFile(path) > 0)
                return new XPdfForm(path);
            return new XImage(path);
        }

        public static XImage FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (PdfReader.TestPdfFile(stream) > 0)
                return new XPdfForm(stream);
            return new XImage(stream);
        }

        public static bool ExistsFile(string path)
        {
            if (PdfReader.TestPdfFile(path) > 0)
                return true;
#if !NETFX_CORE && !UWP
            return File.Exists(path);
#endif
        }

        internal XImageState XImageState
        {
            get { return _xImageState; }
            set { _xImageState = value; }
        }
        XImageState _xImageState;

        internal void Initialize()
        {
#if CORE || GDI || WPF
            if (_importedImage != null)
            {
                ImportedImageJpeg iiJpeg = _importedImage as ImportedImageJpeg;
                if (iiJpeg != null)
                    _format = XImageFormat.Jpeg;
                else
                    _format = XImageFormat.Png;
                return;
            }
#endif

#if CORE_WITH_GDI
            if (_gdiImage != null)
            {
                string guid;
                try
                {
                    Lock.EnterGdiPlus();
                    guid = _gdiImage.RawFormat.Guid.ToString("B").ToUpper();
                }
                finally
                {
                    Lock.ExitGdiPlus();
                }

                switch (guid)
                {
                    case "{B96B3CAA-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CAF-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Png;
                        break;

                    case "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Jpeg;
                        break;

                    case "{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Gif;
                        break;

                    case "{B96B3CB1-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Tiff;
                        break;

                    case "{B96B3CB5-0728-11D3-9D7B-0000F81EF32E}":   
                        _format = XImageFormat.Icon;
                        break;

                    case "{B96B3CAC-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CAD-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CB2-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CB3-0728-11D3-9D7B-0000F81EF32E}":   
                    case "{B96B3CB4-0728-11D3-9D7B-0000F81EF32E}":   

                    default:
                        throw new InvalidOperationException("Unsupported image format.");
                }
                return;
            }
#endif

        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;

#if CORE || GDI || WPF
            {
                _importedImage = null;
            }
#endif

#if CORE_WITH_GDI || GDI
            if (_gdiImage != null)
            {
                try
                {
                    Lock.EnterGdiPlus();
                    _gdiImage.Dispose();
                    _gdiImage = null;
                }
                finally { Lock.ExitGdiPlus(); }
            }
#endif
#if WPF
            _wpfImage = null;
#endif
        }
        bool _disposed;

        [Obsolete("Use either PixelWidth or PointWidth. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelWidth, but will become PointWidth in future releases of PDFsharp.")]
        public virtual double Width
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    return _importedImage.Information.Width;
                }
#endif

#if (CORE_WITH_GDI || GDI)  && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        [Obsolete("Use either PixelHeight or PointHeight. Temporarily obsolete because of rearrangements for WPF. Currently same as PixelHeight, but will become PointHeight in future releases of PDFsharp.")]
        public virtual double Height
        {
            get
            {
#if CORE_WITH_GDI || GDI || WPF
                if (_importedImage != null)
                {
                    return _importedImage.Information.Height;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

#if CORE || GDI || WPF
        private const decimal FactorDPM72 = 72000 / 25.4m;

        private const decimal FactorDPM = 1000 / 25.4m;
#endif

        public virtual double PointWidth
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.HorizontalDPM > 0)
                        return (double)(_importedImage.Information.Width * FactorDPM72 / _importedImage.Information.HorizontalDPM);
                    if (_importedImage.Information.HorizontalDPI > 0)
                        return (double)(_importedImage.Information.Width * 72 / _importedImage.Information.HorizontalDPI);
                    return _importedImage.Information.Width;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width * 72 / _gdiImage.HorizontalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif

            }
        }

        public virtual double PointHeight
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.VerticalDPM > 0)
                        return (double)(_importedImage.Information.Height * FactorDPM72 / _importedImage.Information.VerticalDPM);
                    if (_importedImage.Information.VerticalDPI > 0)
                        return (double)(_importedImage.Information.Height * 72 / _importedImage.Information.VerticalDPI);
                    return _importedImage.Information.Width;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height * 72 / _gdiImage.HorizontalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif

            }
        }

        public virtual int PixelWidth
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                    return (int)_importedImage.Information.Width;
#endif

#if CORE_WITH_GDI
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Width;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual int PixelHeight
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                    return (int)_importedImage.Information.Height;
#endif

#if CORE_WITH_GDI
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.Height;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual XSize Size
        {
            get { return new XSize(PointWidth, PointHeight); }
        }

        public virtual double HorizontalResolution
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.HorizontalDPI > 0)
                        return (double)_importedImage.Information.HorizontalDPI;
                    if (_importedImage.Information.HorizontalDPM > 0)
                        return (double)(_importedImage.Information.HorizontalDPM / FactorDPM);
                    return 72;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.HorizontalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual double VerticalResolution
        {
            get
            {
#if CORE || GDI || WPF
                if (_importedImage != null)
                {
                    if (_importedImage.Information.VerticalDPI > 0)
                        return (double)_importedImage.Information.VerticalDPI;
                    if (_importedImage.Information.VerticalDPM > 0)
                        return (double)(_importedImage.Information.VerticalDPM / FactorDPM);
                    return 72;
                }
#endif

#if (CORE_WITH_GDI || GDI) && !WPF
                try
                {
                    Lock.EnterGdiPlus();
                    return _gdiImage.VerticalResolution;
                }
                finally { Lock.ExitGdiPlus(); }
#endif
            }
        }

        public virtual bool Interpolate
        {
            get { return _interpolate; }
            set { _interpolate = value; }
        }
        bool _interpolate = true;

        public XImageFormat Format
        {
            get { return _format; }
        }
        XImageFormat _format;


        internal void AssociateWithGraphics(XGraphics gfx)
        {
            if (_associatedGraphics != null)
                throw new InvalidOperationException("XImage already associated with XGraphics.");
            _associatedGraphics = null;
        }

        internal void DisassociateWithGraphics()
        {
            if (_associatedGraphics == null)
                throw new InvalidOperationException("XImage not associated with XGraphics.");
            _associatedGraphics.DisassociateImage();

            Debug.Assert(_associatedGraphics == null);
        }

        internal void DisassociateWithGraphics(XGraphics gfx)
        {
            if (_associatedGraphics != gfx)
                throw new InvalidOperationException("XImage not associated with XGraphics.");
            _associatedGraphics = null;
        }

        internal XGraphics AssociatedGraphics
        {
            get { return _associatedGraphics; }
            set { _associatedGraphics = value; }
        }
        XGraphics _associatedGraphics;

#if CORE || GDI || WPF
        internal ImportedImage _importedImage;
#endif

#if CORE_WITH_GDI || GDI
        internal Image _gdiImage;
#endif
#if WPF
        internal BitmapSource _wpfImage;
#if SILVERLIGHT
        //internal byte[] _bytes;
#endif
#endif
#if NETFX_CORE  || UWP
        internal BitmapSource _wrtImage;
#endif

        internal string _path;

        internal Stream _stream;

        internal PdfImageTable.ImageSelector _selector;
    }
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
    internal class XKnownColorTable
    {
        internal static uint[] ColorTable;

        public static uint KnownColorToArgb(XKnownColor color)
        {
            if (ColorTable == null)
                InitColorTable();
            if (color <= XKnownColor.YellowGreen)
                return ColorTable[(int)color];
            return 0;
        }

        public static bool IsKnownColor(uint argb)
        {
            for (int idx = 0; idx < ColorTable.Length; idx++)
            {
                if (ColorTable[idx] == argb)
                    return true;
            }
            return false;
        }

        public static XKnownColor GetKnownColor(uint argb)
        {
            for (int idx = 0; idx < ColorTable.Length; idx++)
            {
                if (ColorTable[idx] == argb)
                    return (XKnownColor)idx;
            }
            return (XKnownColor)(-1);
        }

        private static void InitColorTable()
        {
            uint[] colors = new uint[141];
            colors[0] = 0xFFF0F8FF;
            colors[1] = 0xFFFAEBD7;
            colors[2] = 0xFF00FFFF;
            colors[3] = 0xFF7FFFD4;
            colors[4] = 0xFFF0FFFF;
            colors[5] = 0xFFF5F5DC;
            colors[6] = 0xFFFFE4C4;
            colors[7] = 0xFF000000;
            colors[8] = 0xFFFFEBCD;
            colors[9] = 0xFF0000FF;
            colors[10] = 0xFF8A2BE2;
            colors[11] = 0xFFA52A2A;
            colors[12] = 0xFFDEB887;
            colors[13] = 0xFF5F9EA0;
            colors[14] = 0xFF7FFF00;
            colors[15] = 0xFFD2691E;
            colors[16] = 0xFFFF7F50;
            colors[17] = 0xFF6495ED;
            colors[18] = 0xFFFFF8DC;
            colors[19] = 0xFFDC143C;
            colors[20] = 0xFF00FFFF;
            colors[21] = 0xFF00008B;
            colors[22] = 0xFF008B8B;
            colors[23] = 0xFFB8860B;
            colors[24] = 0xFFA9A9A9;
            colors[25] = 0xFF006400;
            colors[26] = 0xFFBDB76B;
            colors[27] = 0xFF8B008B;
            colors[28] = 0xFF556B2F;
            colors[29] = 0xFFFF8C00;
            colors[30] = 0xFF9932CC;
            colors[31] = 0xFF8B0000;
            colors[32] = 0xFFE9967A;
            colors[33] = 0xFF8FBC8B;
            colors[34] = 0xFF483D8B;
            colors[35] = 0xFF2F4F4F;
            colors[36] = 0xFF00CED1;
            colors[37] = 0xFF9400D3;
            colors[38] = 0xFFFF1493;
            colors[39] = 0xFF00BFFF;
            colors[40] = 0xFF696969;
            colors[41] = 0xFF1E90FF;
            colors[42] = 0xFFB22222;
            colors[43] = 0xFFFFFAF0;
            colors[44] = 0xFF228B22;
            colors[45] = 0xFFFF00FF;
            colors[46] = 0xFFDCDCDC;
            colors[47] = 0xFFF8F8FF;
            colors[48] = 0xFFFFD700;
            colors[49] = 0xFFDAA520;
            colors[50] = 0xFF808080;
            colors[51] = 0xFF008000;
            colors[52] = 0xFFADFF2F;
            colors[53] = 0xFFF0FFF0;
            colors[54] = 0xFFFF69B4;
            colors[55] = 0xFFCD5C5C;
            colors[56] = 0xFF4B0082;
            colors[57] = 0xFFFFFFF0;
            colors[58] = 0xFFF0E68C;
            colors[59] = 0xFFE6E6FA;
            colors[60] = 0xFFFFF0F5;
            colors[61] = 0xFF7CFC00;
            colors[62] = 0xFFFFFACD;
            colors[63] = 0xFFADD8E6;
            colors[64] = 0xFFF08080;
            colors[65] = 0xFFE0FFFF;
            colors[66] = 0xFFFAFAD2;
            colors[67] = 0xFFD3D3D3;
            colors[68] = 0xFF90EE90;
            colors[69] = 0xFFFFB6C1;
            colors[70] = 0xFFFFA07A;
            colors[71] = 0xFF20B2AA;
            colors[72] = 0xFF87CEFA;
            colors[73] = 0xFF778899;
            colors[74] = 0xFFB0C4DE;
            colors[75] = 0xFFFFFFE0;
            colors[76] = 0xFF00FF00;
            colors[77] = 0xFF32CD32;
            colors[78] = 0xFFFAF0E6;
            colors[79] = 0xFFFF00FF;
            colors[80] = 0xFF800000;
            colors[81] = 0xFF66CDAA;
            colors[82] = 0xFF0000CD;
            colors[83] = 0xFFBA55D3;
            colors[84] = 0xFF9370DB;
            colors[85] = 0xFF3CB371;
            colors[86] = 0xFF7B68EE;
            colors[87] = 0xFF00FA9A;
            colors[88] = 0xFF48D1CC;
            colors[89] = 0xFFC71585;
            colors[90] = 0xFF191970;
            colors[91] = 0xFFF5FFFA;
            colors[92] = 0xFFFFE4E1;
            colors[93] = 0xFFFFE4B5;
            colors[94] = 0xFFFFDEAD;
            colors[95] = 0xFF000080;
            colors[96] = 0xFFFDF5E6;
            colors[97] = 0xFF808000;
            colors[98] = 0xFF6B8E23;
            colors[99] = 0xFFFFA500;
            colors[100] = 0xFFFF4500;
            colors[101] = 0xFFDA70D6;
            colors[102] = 0xFFEEE8AA;
            colors[103] = 0xFF98FB98;
            colors[104] = 0xFFAFEEEE;
            colors[105] = 0xFFDB7093;
            colors[106] = 0xFFFFEFD5;
            colors[107] = 0xFFFFDAB9;
            colors[108] = 0xFFCD853F;
            colors[109] = 0xFFFFC0CB;
            colors[110] = 0xFFDDA0DD;
            colors[111] = 0xFFB0E0E6;
            colors[112] = 0xFF800080;
            colors[113] = 0xFFFF0000;
            colors[114] = 0xFFBC8F8F;
            colors[115] = 0xFF4169E1;
            colors[116] = 0xFF8B4513;
            colors[117] = 0xFFFA8072;
            colors[118] = 0xFFF4A460;
            colors[119] = 0xFF2E8B57;
            colors[120] = 0xFFFFF5EE;
            colors[121] = 0xFFA0522D;
            colors[122] = 0xFFC0C0C0;
            colors[123] = 0xFF87CEEB;
            colors[124] = 0xFF6A5ACD;
            colors[125] = 0xFF708090;
            colors[126] = 0xFFFFFAFA;
            colors[127] = 0xFF00FF7F;
            colors[128] = 0xFF4682B4;
            colors[129] = 0xFFD2B48C;
            colors[130] = 0xFF008080;
            colors[131] = 0xFFD8BFD8;
            colors[132] = 0xFFFF6347;
            colors[133] = 0x00FFFFFF;
            colors[134] = 0xFF40E0D0;
            colors[135] = 0xFFEE82EE;
            colors[136] = 0xFFF5DEB3;
            colors[137] = 0xFFFFFFFF;
            colors[138] = 0xFFF5F5F5;
            colors[139] = 0xFFFFFF00;
            colors[140] = 0xFF9ACD32;

            ColorTable = colors;
        }
    }
    public sealed class XLinearGradientBrush : XBrush
    {


        public XLinearGradientBrush(XPoint point1, XPoint point2, XColor color1, XColor color2)
        {
            _point1 = point1;
            _point2 = point2;
            _color1 = color1;
            _color2 = color2;
        }


        public XLinearGradientBrush(XRect rect, XColor color1, XColor color2, XLinearGradientMode linearGradientMode)
        {
            if (!Enum.IsDefined(typeof(XLinearGradientMode), linearGradientMode))
                throw new InvalidEnumArgumentException("linearGradientMode", (int)linearGradientMode, typeof(XLinearGradientMode));

            if (rect.Width == 0 || rect.Height == 0)
                throw new ArgumentException("Invalid rectangle.", "rect");

            _useRect = true;
            _color1 = color1;
            _color2 = color2;
            _rect = rect;
            _linearGradientMode = linearGradientMode;
        }

        public XMatrix Transform
        {
            get { return _matrix; }
            set { _matrix = value; }
        }

        public void TranslateTransform(double dx, double dy)
        {
            _matrix.TranslatePrepend(dx, dy);
        }

        public void TranslateTransform(double dx, double dy, XMatrixOrder order)
        {
            _matrix.Translate(dx, dy, order);
        }

        public void ScaleTransform(double sx, double sy)
        {
            _matrix.ScalePrepend(sx, sy);
        }

        public void ScaleTransform(double sx, double sy, XMatrixOrder order)
        {
            _matrix.Scale(sx, sy, order);
        }

        public void RotateTransform(double angle)
        {
            _matrix.RotatePrepend(angle);
        }

        public void RotateTransform(double angle, XMatrixOrder order)
        {
            _matrix.Rotate(angle, order);
        }

        public void MultiplyTransform(XMatrix matrix)
        {
            _matrix.Prepend(matrix);
        }

        public void MultiplyTransform(XMatrix matrix, XMatrixOrder order)
        {
            _matrix.Multiply(matrix, order);
        }

        public void ResetTransform()
        {
            _matrix = new XMatrix();
        }

        internal bool _useRect;
        internal XPoint _point1, _point2;
        internal XColor _color1, _color2;
        internal XRect _rect;
        internal XLinearGradientMode _linearGradientMode;

        internal XMatrix _matrix;
    }
    public struct XMatrix : IFormattable
    {
        [Flags]
        internal enum XMatrixTypes
        {
            Identity = 0,
            Translation = 1,
            Scaling = 2,
            Unknown = 4
        }

        public XMatrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
        {
            _m11 = m11;
            _m12 = m12;
            _m21 = m21;
            _m22 = m22;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _type = XMatrixTypes.Unknown;
            DeriveMatrixType();
        }

        public static XMatrix Identity
        {
            get { return s_identity; }
        }

        public void SetIdentity()
        {
            _type = XMatrixTypes.Identity;
        }

        public bool IsIdentity
        {
            get
            {
                if (_type == XMatrixTypes.Identity)
                    return true;
                if (_m11 == 1.0 && _m12 == 0 && _m21 == 0 && _m22 == 1.0 && _offsetX == 0 && _offsetY == 0)
                {
                    _type = XMatrixTypes.Identity;
                    return true;
                }
                return false;
            }
        }

        public double[] GetElements()
        {
            if (_type == XMatrixTypes.Identity)
                return new double[] { 1, 0, 0, 1, 0, 0 };
            return new double[] { _m11, _m12, _m21, _m22, _offsetX, _offsetY };
        }

        public static XMatrix operator *(XMatrix trans1, XMatrix trans2)
        {
            MatrixHelper.MultiplyMatrix(ref trans1, ref trans2);
            return trans1;
        }

        public static XMatrix Multiply(XMatrix trans1, XMatrix trans2)
        {
            MatrixHelper.MultiplyMatrix(ref trans1, ref trans2);
            return trans1;
        }

        public void Append(XMatrix matrix)
        {
            this *= matrix;
        }

        public void Prepend(XMatrix matrix)
        {
            this = matrix * this;
        }

        [Obsolete("Use Append.")]
        public void Multiply(XMatrix matrix)
        {
            Append(matrix);
        }

        [Obsolete("Use Prepend.")]
        public void MultiplyPrepend(XMatrix matrix)
        {
            Prepend(matrix);
        }

        public void Multiply(XMatrix matrix, XMatrixOrder order)
        {
            if (_type == XMatrixTypes.Identity)
                this = CreateIdentity();

            double t11 = M11;
            double t12 = M12;
            double t21 = M21;
            double t22 = M22;
            double tdx = OffsetX;
            double tdy = OffsetY;

            if (order == XMatrixOrder.Append)
            {
                _m11 = t11 * matrix.M11 + t12 * matrix.M21;
                _m12 = t11 * matrix.M12 + t12 * matrix.M22;
                _m21 = t21 * matrix.M11 + t22 * matrix.M21;
                _m22 = t21 * matrix.M12 + t22 * matrix.M22;
                _offsetX = tdx * matrix.M11 + tdy * matrix.M21 + matrix.OffsetX;
                _offsetY = tdx * matrix.M12 + tdy * matrix.M22 + matrix.OffsetY;
            }
            else
            {
                _m11 = t11 * matrix.M11 + t21 * matrix.M12;
                _m12 = t12 * matrix.M11 + t22 * matrix.M12;
                _m21 = t11 * matrix.M21 + t21 * matrix.M22;
                _m22 = t12 * matrix.M21 + t22 * matrix.M22;
                _offsetX = t11 * matrix.OffsetX + t21 * matrix.OffsetY + tdx;
                _offsetY = t12 * matrix.OffsetX + t22 * matrix.OffsetY + tdy;
            }
            DeriveMatrixType();
        }

        [Obsolete("Use TranslateAppend or TranslatePrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void Translate(double offsetX, double offsetY)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void TranslateAppend(double offsetX, double offsetY)
        {
            if (_type == XMatrixTypes.Identity)
            {
                SetMatrix(1, 0, 0, 1, offsetX, offsetY, XMatrixTypes.Translation);
            }
            else if (_type == XMatrixTypes.Unknown)
            {
                _offsetX += offsetX;
                _offsetY += offsetY;
            }
            else
            {
                _offsetX += offsetX;
                _offsetY += offsetY;
                _type |= XMatrixTypes.Translation;
            }
        }

        public void TranslatePrepend(double offsetX, double offsetY)
        {
            this = CreateTranslation(offsetX, offsetY) * this;
        }

        public void Translate(double offsetX, double offsetY, XMatrixOrder order)
        {
            if (_type == XMatrixTypes.Identity)
                this = CreateIdentity();

            if (order == XMatrixOrder.Append)
            {
                _offsetX += offsetX;
                _offsetY += offsetY;
            }
            else
            {
                _offsetX += offsetX * _m11 + offsetY * _m21;
                _offsetY += offsetX * _m12 + offsetY * _m22;
            }
            DeriveMatrixType();
        }

        [Obsolete("Use ScaleAppend or ScalePrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void Scale(double scaleX, double scaleY)
        {
            this = CreateScaling(scaleX, scaleY) * this;
        }

        public void ScaleAppend(double scaleX, double scaleY)
        {
            this *= CreateScaling(scaleX, scaleY);
        }

        public void ScalePrepend(double scaleX, double scaleY)
        {
            this = CreateScaling(scaleX, scaleY) * this;
        }

        public void Scale(double scaleX, double scaleY, XMatrixOrder order)
        {
            if (_type == XMatrixTypes.Identity)
                this = CreateIdentity();

            if (order == XMatrixOrder.Append)
            {
                _m11 *= scaleX;
                _m12 *= scaleY;
                _m21 *= scaleX;
                _m22 *= scaleY;
                _offsetX *= scaleX;
                _offsetY *= scaleY;
            }
            else
            {
                _m11 *= scaleX;
                _m12 *= scaleX;
                _m21 *= scaleY;
                _m22 *= scaleY;
            }
            DeriveMatrixType();
        }

        [Obsolete("Use ScaleAppend or ScalePrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void Scale(double scaleXY)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void ScaleAppend(double scaleXY)
        {
            Scale(scaleXY, scaleXY, XMatrixOrder.Append);
        }

        public void ScalePrepend(double scaleXY)
        {
            Scale(scaleXY, scaleXY, XMatrixOrder.Prepend);
        }

        public void Scale(double scaleXY, XMatrixOrder order)
        {
            Scale(scaleXY, scaleXY, order);
        }

        [Obsolete("Use ScaleAtAppend or ScaleAtPrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void ScaleAt(double scaleX, double scaleY, double centerX, double centerY)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void ScaleAtAppend(double scaleX, double scaleY, double centerX, double centerY)
        {
            this *= CreateScaling(scaleX, scaleY, centerX, centerY);
        }

        public void ScaleAtPrepend(double scaleX, double scaleY, double centerX, double centerY)
        {
            this = CreateScaling(scaleX, scaleY, centerX, centerY) * this;
        }

        [Obsolete("Use RotateAppend or RotatePrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void Rotate(double angle)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void RotateAppend(double angle)
        {
            angle = angle % 360.0;
            this *= CreateRotationRadians(angle * Const.Deg2Rad);
        }

        public void RotatePrepend(double angle)
        {
            angle = angle % 360.0;
            this = CreateRotationRadians(angle * Const.Deg2Rad) * this;
        }

        public void Rotate(double angle, XMatrixOrder order)
        {
            if (_type == XMatrixTypes.Identity)
                this = CreateIdentity();

            angle = angle * Const.Deg2Rad;
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            if (order == XMatrixOrder.Append)
            {
                double t11 = _m11;
                double t12 = _m12;
                double t21 = _m21;
                double t22 = _m22;
                double tdx = _offsetX;
                double tdy = _offsetY;
                _m11 = t11 * cos - t12 * sin;
                _m12 = t11 * sin + t12 * cos;
                _m21 = t21 * cos - t22 * sin;
                _m22 = t21 * sin + t22 * cos;
                _offsetX = tdx * cos - tdy * sin;
                _offsetY = tdx * sin + tdy * cos;
            }
            else
            {
                double t11 = _m11;
                double t12 = _m12;
                double t21 = _m21;
                double t22 = _m22;
                _m11 = t11 * cos + t21 * sin;
                _m12 = t12 * cos + t22 * sin;
                _m21 = -t11 * sin + t21 * cos;
                _m22 = -t12 * sin + t22 * cos;
            }
            DeriveMatrixType();
        }

        [Obsolete("Use RotateAtAppend or RotateAtPrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void RotateAt(double angle, double centerX, double centerY)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void RotateAtAppend(double angle, double centerX, double centerY)
        {
            angle = angle % 360.0;
            this *= CreateRotationRadians(angle * Const.Deg2Rad, centerX, centerY);
        }

        public void RotateAtPrepend(double angle, double centerX, double centerY)
        {
            angle = angle % 360.0;
            this = CreateRotationRadians(angle * Const.Deg2Rad, centerX, centerY) * this;
        }

        [Obsolete("Use RotateAtAppend or RotateAtPrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void RotateAt(double angle, XPoint point)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void RotateAtAppend(double angle, XPoint point)
        {
            RotateAt(angle, point, XMatrixOrder.Append);
        }

        public void RotateAtPrepend(double angle, XPoint point)
        {
            RotateAt(angle, point, XMatrixOrder.Prepend);
        }

        public void RotateAt(double angle, XPoint point, XMatrixOrder order)
        {
            if (order == XMatrixOrder.Append)
            {
                angle = angle % 360.0;
                this *= CreateRotationRadians(angle * Const.Deg2Rad, point.X, point.Y);

            }
            else
            {
                angle = angle % 360.0;
                this = CreateRotationRadians(angle * Const.Deg2Rad, point.X, point.Y) * this;
            }
            DeriveMatrixType();
        }

        [Obsolete("Use ShearAppend or ShearPrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void Shear(double shearX, double shearY)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void ShearAppend(double shearX, double shearY)
        {
            Shear(shearX, shearY, XMatrixOrder.Append);
        }

        public void ShearPrepend(double shearX, double shearY)
        {
            Shear(shearX, shearY, XMatrixOrder.Prepend);
        }

        public void Shear(double shearX, double shearY, XMatrixOrder order)
        {
            if (_type == XMatrixTypes.Identity)
                this = CreateIdentity();

            double t11 = _m11;
            double t12 = _m12;
            double t21 = _m21;
            double t22 = _m22;
            double tdx = _offsetX;
            double tdy = _offsetY;
            if (order == XMatrixOrder.Append)
            {
                _m11 += shearX * t12;
                _m12 += shearY * t11;
                _m21 += shearX * t22;
                _m22 += shearY * t21;
                _offsetX += shearX * tdy;
                _offsetY += shearY * tdx;
            }
            else
            {
                _m11 += shearY * t21;
                _m12 += shearY * t22;
                _m21 += shearX * t11;
                _m22 += shearX * t12;
            }
            DeriveMatrixType();
        }

        [Obsolete("Use SkewAppend or SkewPrepend explicitly, because in GDI+ and WPF the defaults are contrary.", true)]
        public void Skew(double skewX, double skewY)
        {
            throw new InvalidOperationException("Temporarily out of order.");
        }

        public void SkewAppend(double skewX, double skewY)
        {
            skewX = skewX % 360.0;
            skewY = skewY % 360.0;
            this *= CreateSkewRadians(skewX * Const.Deg2Rad, skewY * Const.Deg2Rad);
        }

        public void SkewPrepend(double skewX, double skewY)
        {
            skewX = skewX % 360.0;
            skewY = skewY % 360.0;
            this = CreateSkewRadians(skewX * Const.Deg2Rad, skewY * Const.Deg2Rad) * this;
        }

        public XPoint Transform(XPoint point)
        {
            double x = point.X;
            double y = point.Y;
            MultiplyPoint(ref x, ref y);
            return new XPoint(x, y);
        }

        public void Transform(XPoint[] points)
        {
            if (points != null)
            {
                int count = points.Length;
                for (int idx = 0; idx < count; idx++)
                {
                    double x = points[idx].X;
                    double y = points[idx].Y;
                    MultiplyPoint(ref x, ref y);
                    points[idx].X = x;
                    points[idx].Y = y;
                }
            }
        }

        public void TransformPoints(XPoint[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            if (IsIdentity)
                return;

            int count = points.Length;
            for (int idx = 0; idx < count; idx++)
            {
                double x = points[idx].X;
                double y = points[idx].Y;
                points[idx].X = x * _m11 + y * _m21 + _offsetX;
                points[idx].Y = x * _m12 + y * _m22 + _offsetY;
            }
        }



        public XVector Transform(XVector vector)
        {
            double x = vector.X;
            double y = vector.Y;
            MultiplyVector(ref x, ref y);
            return new XVector(x, y);
        }

        public void Transform(XVector[] vectors)
        {
            if (vectors != null)
            {
                int count = vectors.Length;
                for (int idx = 0; idx < count; idx++)
                {
                    double x = vectors[idx].X;
                    double y = vectors[idx].Y;
                    MultiplyVector(ref x, ref y);
                    vectors[idx].X = x;
                    vectors[idx].Y = y;
                }
            }
        }


        public double Determinant
        {
            get
            {
                switch (_type)
                {
                    case XMatrixTypes.Identity:
                    case XMatrixTypes.Translation:
                        return 1.0;

                    case XMatrixTypes.Scaling:
                    case XMatrixTypes.Scaling | XMatrixTypes.Translation:
                        return _m11 * _m22;
                }
                return (_m11 * _m22) - (_m12 * _m21);
            }
        }

        public bool HasInverse
        {
            get { return !DoubleUtil.IsZero(Determinant); }
        }

        public void Invert()
        {
            double determinant = Determinant;
            if (DoubleUtil.IsZero(determinant))
                throw new InvalidOperationException("NotInvertible");

            switch (_type)
            {
                case XMatrixTypes.Identity:
                    break;

                case XMatrixTypes.Translation:
                    _offsetX = -_offsetX;
                    _offsetY = -_offsetY;
                    return;

                case XMatrixTypes.Scaling:
                    _m11 = 1.0 / _m11;
                    _m22 = 1.0 / _m22;
                    return;

                case XMatrixTypes.Scaling | XMatrixTypes.Translation:
                    _m11 = 1.0 / _m11;
                    _m22 = 1.0 / _m22;
                    _offsetX = -_offsetX * _m11;
                    _offsetY = -_offsetY * _m22;
                    return;

                default:
                    {
                        double detInvers = 1.0 / determinant;
                        SetMatrix(_m22 * detInvers, -_m12 * detInvers, -_m21 * detInvers, _m11 * detInvers, (_m21 * _offsetY - _offsetX * _m22) * detInvers, (_offsetX * _m12 - _m11 * _offsetY) * detInvers, XMatrixTypes.Unknown);
                        break;
                    }
            }
        }

        public double M11
        {
            get
            {
                if (_type == XMatrixTypes.Identity)
                    return 1.0;
                return _m11;
            }
            set
            {
                if (_type == XMatrixTypes.Identity)
                    SetMatrix(value, 0, 0, 1, 0, 0, XMatrixTypes.Scaling);
                else
                {
                    _m11 = value;
                    if (_type != XMatrixTypes.Unknown)
                        _type |= XMatrixTypes.Scaling;
                }
            }
        }

        public double M12
        {
            get
            {
                if (_type == XMatrixTypes.Identity)
                    return 0;
                return _m12;
            }
            set
            {
                if (_type == XMatrixTypes.Identity)
                    SetMatrix(1, value, 0, 1, 0, 0, XMatrixTypes.Unknown);
                else
                {
                    _m12 = value;
                    _type = XMatrixTypes.Unknown;
                }
            }
        }

        public double M21
        {
            get
            {
                if (_type == XMatrixTypes.Identity)
                    return 0;
                return _m21;
            }
            set
            {
                if (_type == XMatrixTypes.Identity)
                    SetMatrix(1, 0, value, 1, 0, 0, XMatrixTypes.Unknown);
                else
                {
                    _m21 = value;
                    _type = XMatrixTypes.Unknown;
                }
            }
        }

        public double M22
        {
            get
            {
                if (_type == XMatrixTypes.Identity)
                    return 1.0;
                return _m22;
            }
            set
            {
                if (_type == XMatrixTypes.Identity)
                    SetMatrix(1, 0, 0, value, 0, 0, XMatrixTypes.Scaling);
                else
                {
                    _m22 = value;
                    if (_type != XMatrixTypes.Unknown)
                        _type |= XMatrixTypes.Scaling;
                }
            }
        }

        public double OffsetX
        {
            get
            {
                if (_type == XMatrixTypes.Identity)
                    return 0;
                return _offsetX;
            }
            set
            {
                if (_type == XMatrixTypes.Identity)
                    SetMatrix(1, 0, 0, 1, value, 0, XMatrixTypes.Translation);
                else
                {
                    _offsetX = value;
                    if (_type != XMatrixTypes.Unknown)
                        _type |= XMatrixTypes.Translation;
                }
            }
        }

        public double OffsetY
        {
            get
            {
                if (_type == XMatrixTypes.Identity)
                    return 0;
                return _offsetY;
            }
            set
            {
                if (_type == XMatrixTypes.Identity)
                    SetMatrix(1, 0, 0, 1, 0, value, XMatrixTypes.Translation);
                else
                {
                    _offsetY = value;
                    if (_type != XMatrixTypes.Unknown)
                        _type |= XMatrixTypes.Translation;
                }
            }
        }


        public static bool operator ==(XMatrix matrix1, XMatrix matrix2)
        {
            if (matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity)
                return (matrix1.IsIdentity == matrix2.IsIdentity);

            return matrix1.M11 == matrix2.M11 && matrix1.M12 == matrix2.M12 && matrix1.M21 == matrix2.M21 && matrix1.M22 == matrix2.M22 &&
              matrix1.OffsetX == matrix2.OffsetX && matrix1.OffsetY == matrix2.OffsetY;
        }

        public static bool operator !=(XMatrix matrix1, XMatrix matrix2)
        {
            return !(matrix1 == matrix2);
        }

        public static bool Equals(XMatrix matrix1, XMatrix matrix2)
        {
            if (matrix1.IsDistinguishedIdentity || matrix2.IsDistinguishedIdentity)
                return matrix1.IsIdentity == matrix2.IsIdentity;

            return matrix1.M11.Equals(matrix2.M11) && matrix1.M12.Equals(matrix2.M12) &&
              matrix1.M21.Equals(matrix2.M21) && matrix1.M22.Equals(matrix2.M22) &&
              matrix1.OffsetX.Equals(matrix2.OffsetX) && matrix1.OffsetY.Equals(matrix2.OffsetY);
        }

        public override bool Equals(object o)
        {
            if (!(o is XMatrix))
                return false;
            return Equals(this, (XMatrix)o);
        }

        public bool Equals(XMatrix value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (IsDistinguishedIdentity)
                return 0;
            return M11.GetHashCode() ^ M12.GetHashCode() ^ M21.GetHashCode() ^ M22.GetHashCode() ^ OffsetX.GetHashCode() ^ OffsetY.GetHashCode();
        }

        public static XMatrix Parse(string source)
        {
            IFormatProvider cultureInfo = CultureInfo.InvariantCulture;
            TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
            string str = helper.NextTokenRequired();
            XMatrix identity = str == "Identity" ? Identity : new XMatrix(
                Convert.ToDouble(str, cultureInfo),
                Convert.ToDouble(helper.NextTokenRequired(), cultureInfo),
                Convert.ToDouble(helper.NextTokenRequired(), cultureInfo),
                Convert.ToDouble(helper.NextTokenRequired(), cultureInfo),
                Convert.ToDouble(helper.NextTokenRequired(), cultureInfo),
                Convert.ToDouble(helper.NextTokenRequired(), cultureInfo));
            helper.LastTokenRequired();
            return identity;
        }

        public override string ToString()
        {
            return ConvertToString(null, null);
        }

        public string ToString(IFormatProvider provider)
        {
            return ConvertToString(null, provider);
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return ConvertToString(format, provider);
        }

        internal string ConvertToString(string format, IFormatProvider provider)
        {
            if (IsIdentity)
                return "Identity";

            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            provider = provider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}{0}{5:" + format + "}{0}{6:" + format + "}",
                new object[] { numericListSeparator, _m11, _m12, _m21, _m22, _offsetX, _offsetY });
        }

        internal void MultiplyVector(ref double x, ref double y)
        {
            switch (_type)
            {
                case XMatrixTypes.Identity:
                case XMatrixTypes.Translation:
                    return;

                case XMatrixTypes.Scaling:
                case XMatrixTypes.Scaling | XMatrixTypes.Translation:
                    x *= _m11;
                    y *= _m22;
                    return;
            }
            double d1 = y * _m21;
            double d2 = x * _m12;
            x *= _m11;
            x += d1;
            y *= _m22;
            y += d2;
        }

        internal void MultiplyPoint(ref double x, ref double y)
        {
            switch (_type)
            {
                case XMatrixTypes.Identity:
                    return;

                case XMatrixTypes.Translation:
                    x += _offsetX;
                    y += _offsetY;
                    return;

                case XMatrixTypes.Scaling:
                    x *= _m11;
                    y *= _m22;
                    return;

                case (XMatrixTypes.Scaling | XMatrixTypes.Translation):
                    x *= _m11;
                    x += _offsetX;
                    y *= _m22;
                    y += _offsetY;
                    return;
            }
            double d1 = (y * _m21) + _offsetX;
            double d2 = (x * _m12) + _offsetY;
            x *= _m11;
            x += d1;
            y *= _m22;
            y += d2;
        }

        internal static XMatrix CreateTranslation(double offsetX, double offsetY)
        {
            XMatrix matrix = new XMatrix();
            matrix.SetMatrix(1, 0, 0, 1, offsetX, offsetY, XMatrixTypes.Translation);
            return matrix;
        }

        internal static XMatrix CreateRotationRadians(double angle)
        {
            return CreateRotationRadians(angle, 0, 0);
        }

        internal static XMatrix CreateRotationRadians(double angle, double centerX, double centerY)
        {
            XMatrix matrix = new XMatrix();
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            double offsetX = (centerX * (1.0 - cos)) + (centerY * sin);
            double offsetY = (centerY * (1.0 - cos)) - (centerX * sin);
            matrix.SetMatrix(cos, sin, -sin, cos, offsetX, offsetY, XMatrixTypes.Unknown);
            return matrix;
        }

        internal static XMatrix CreateScaling(double scaleX, double scaleY)
        {
            XMatrix matrix = new XMatrix();
            matrix.SetMatrix(scaleX, 0, 0, scaleY, 0, 0, XMatrixTypes.Scaling);
            return matrix;
        }

        internal static XMatrix CreateScaling(double scaleX, double scaleY, double centerX, double centerY)
        {
            XMatrix matrix = new XMatrix();
            matrix.SetMatrix(scaleX, 0, 0, scaleY, centerX - scaleX * centerX, centerY - scaleY * centerY, XMatrixTypes.Scaling | XMatrixTypes.Translation);
            return matrix;
        }

        internal static XMatrix CreateSkewRadians(double skewX, double skewY, double centerX, double centerY)
        {
            XMatrix matrix = new XMatrix();
            matrix.Append(CreateTranslation(-centerX, -centerY));
            matrix.Append(new XMatrix(1, Math.Tan(skewY), Math.Tan(skewX), 1, 0, 0));
            matrix.Append(CreateTranslation(centerX, centerY));
            return matrix;
        }

        internal static XMatrix CreateSkewRadians(double skewX, double skewY)
        {
            XMatrix matrix = new XMatrix();
            matrix.SetMatrix(1, Math.Tan(skewY), Math.Tan(skewX), 1, 0, 0, XMatrixTypes.Unknown);
            return matrix;
        }

        static XMatrix CreateIdentity()
        {
            XMatrix matrix = new XMatrix();
            matrix.SetMatrix(1, 0, 0, 1, 0, 0, XMatrixTypes.Identity);
            return matrix;
        }

        void SetMatrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY, XMatrixTypes type)
        {
            _m11 = m11;
            _m12 = m12;
            _m21 = m21;
            _m22 = m22;
            _offsetX = offsetX;
            _offsetY = offsetY;
            _type = type;
        }

        void DeriveMatrixType()
        {
            _type = XMatrixTypes.Identity;
            if (_m12 != 0 || _m21 != 0)
            {
                _type = XMatrixTypes.Unknown;
            }
            else
            {
                if (_m11 != 1 || _m22 != 1)
                    _type = XMatrixTypes.Scaling;

                if (_offsetX != 0 || _offsetY != 0)
                    _type |= XMatrixTypes.Translation;

                if ((_type & (XMatrixTypes.Scaling | XMatrixTypes.Translation)) == XMatrixTypes.Identity)
                    _type = XMatrixTypes.Identity;
            }
        }

        bool IsDistinguishedIdentity
        {
            get { return (_type == XMatrixTypes.Identity); }
        }

        double _m11;
        double _m12;
        double _m21;
        double _m22;
        double _offsetX;
        double _offsetY;
        XMatrixTypes _type;
        static readonly XMatrix s_identity = CreateIdentity();

        internal static class MatrixHelper
        {
            internal static void MultiplyMatrix(ref XMatrix matrix1, ref XMatrix matrix2)
            {
                XMatrixTypes type1 = matrix1._type;
                XMatrixTypes type2 = matrix2._type;
                if (type2 != XMatrixTypes.Identity)
                {
                    if (type1 == XMatrixTypes.Identity)
                        matrix1 = matrix2;
                    else if (type2 == XMatrixTypes.Translation)
                    {
                        matrix1._offsetX += matrix2._offsetX;
                        matrix1._offsetY += matrix2._offsetY;
                        if (type1 != XMatrixTypes.Unknown)
                            matrix1._type |= XMatrixTypes.Translation;
                    }
                    else if (type1 == XMatrixTypes.Translation)
                    {
                        double num = matrix1._offsetX;
                        double num2 = matrix1._offsetY;
                        matrix1 = matrix2;
                        matrix1._offsetX = num * matrix2._m11 + num2 * matrix2._m21 + matrix2._offsetX;
                        matrix1._offsetY = num * matrix2._m12 + num2 * matrix2._m22 + matrix2._offsetY;
                        if (type2 == XMatrixTypes.Unknown)
                            matrix1._type = XMatrixTypes.Unknown;
                        else
                            matrix1._type = XMatrixTypes.Scaling | XMatrixTypes.Translation;
                    }
                    else
                    {
                        switch ((((int)type1) << 4) | (int)type2)
                        {
                            case 0x22:
                                matrix1._m11 *= matrix2._m11;
                                matrix1._m22 *= matrix2._m22;
                                return;

                            case 0x23:
                                matrix1._m11 *= matrix2._m11;
                                matrix1._m22 *= matrix2._m22;
                                matrix1._offsetX = matrix2._offsetX;
                                matrix1._offsetY = matrix2._offsetY;
                                matrix1._type = XMatrixTypes.Scaling | XMatrixTypes.Translation;
                                return;

                            case 0x24:
                            case 0x34:
                            case 0x42:
                            case 0x43:
                            case 0x44:
                                matrix1 = new XMatrix(
                                  matrix1._m11 * matrix2._m11 + matrix1._m12 * matrix2._m21,
                                  matrix1._m11 * matrix2._m12 + matrix1._m12 * matrix2._m22,
                                  matrix1._m21 * matrix2._m11 + matrix1._m22 * matrix2._m21,
                                  matrix1._m21 * matrix2._m12 + matrix1._m22 * matrix2._m22,
                                  matrix1._offsetX * matrix2._m11 + matrix1._offsetY * matrix2._m21 + matrix2._offsetX,
                                  matrix1._offsetX * matrix2._m12 + matrix1._offsetY * matrix2._m22 + matrix2._offsetY);
                                return;

                            case 50:
                                matrix1._m11 *= matrix2._m11;
                                matrix1._m22 *= matrix2._m22;
                                matrix1._offsetX *= matrix2._m11;
                                matrix1._offsetY *= matrix2._m22;
                                return;

                            case 0x33:
                                matrix1._m11 *= matrix2._m11;
                                matrix1._m22 *= matrix2._m22;
                                matrix1._offsetX = matrix2._m11 * matrix1._offsetX + matrix2._offsetX;
                                matrix1._offsetY = matrix2._m22 * matrix1._offsetY + matrix2._offsetY;
                                return;
                        }
                    }
                }
            }

            internal static void PrependOffset(ref XMatrix matrix, double offsetX, double offsetY)
            {
                if (matrix._type == XMatrixTypes.Identity)
                {
                    matrix = new XMatrix(1, 0, 0, 1, offsetX, offsetY);
                    matrix._type = XMatrixTypes.Translation;
                }
                else
                {
                    matrix._offsetX += (matrix._m11 * offsetX) + (matrix._m21 * offsetY);
                    matrix._offsetY += (matrix._m12 * offsetX) + (matrix._m22 * offsetY);
                    if (matrix._type != XMatrixTypes.Unknown)
                        matrix._type |= XMatrixTypes.Translation;
                }
            }

            internal static void TransformRect(ref XRect rect, ref XMatrix matrix)
            {
                if (!rect.IsEmpty)
                {
                    XMatrixTypes type = matrix._type;
                    if (type != XMatrixTypes.Identity)
                    {
                        if ((type & XMatrixTypes.Scaling) != XMatrixTypes.Identity)
                        {
                            rect.X *= matrix._m11;
                            rect.Y *= matrix._m22;
                            rect.Width *= matrix._m11;
                            rect.Height *= matrix._m22;
                            if (rect.Width < 0)
                            {
                                rect.X += rect.Width;
                                rect.Width = -rect.Width;
                            }
                            if (rect.Height < 0)
                            {
                                rect.Y += rect.Height;
                                rect.Height = -rect.Height;
                            }
                        }
                        if ((type & XMatrixTypes.Translation) != XMatrixTypes.Identity)
                        {
                            rect.X += matrix._offsetX;
                            rect.Y += matrix._offsetY;
                        }
                        if (type == XMatrixTypes.Unknown)
                        {
                            XPoint point1 = matrix.Transform(rect.TopLeft);
                            XPoint point2 = matrix.Transform(rect.TopRight);
                            XPoint point3 = matrix.Transform(rect.BottomRight);
                            XPoint point4 = matrix.Transform(rect.BottomLeft);
                            rect.X = Math.Min(Math.Min(point1.X, point2.X), Math.Min(point3.X, point4.X));
                            rect.Y = Math.Min(Math.Min(point1.Y, point2.Y), Math.Min(point3.Y, point4.Y));
                            rect.Width = Math.Max(Math.Max(point1.X, point2.X), Math.Max(point3.X, point4.X)) - rect.X;
                            rect.Height = Math.Max(Math.Max(point1.Y, point2.Y), Math.Max(point3.Y, point4.Y)) - rect.Y;
                        }
                    }
                }
            }
        }

        string DebuggerDisplay
        {
            get
            {
                if (IsIdentity)
                    return "matrix=(Identity)";

                const string format = Config.SignificantFigures7;

                XPoint point = new XMatrix(_m11, _m12, _m21, _m22, 0, 0).Transform(new XPoint(1, 0));
                double φ = Math.Atan2(point.Y, point.X) / Const.Deg2Rad;
                return String.Format(CultureInfo.InvariantCulture,
                    "matrix=({0:" + format + "}, {1:" + format + "}, {2:" + format + "}, {3:" + format + "}, {4:" + format + "}, {5:" + format + "}), φ={6:0.0#########}°",
                    _m11, _m12, _m21, _m22, _offsetX, _offsetY, φ);
            }
        }
    }
    public class XPdfForm : XForm
    {
        internal XPdfForm(string path)
        {
            int pageNumber;
            path = ExtractPageNumber(path, out pageNumber);

#if !NETFX_CORE
            path = Path.GetFullPath(path);
            if (!File.Exists(path))
                throw new FileNotFoundException(PSSR.FileNotFound(path));
#endif

            if (PdfReader.TestPdfFile(path) == 0)
                throw new ArgumentException("The specified file has no valid PDF file header.", "path");

            _path = path;
            if (pageNumber != 0)
                PageNumber = pageNumber;
        }

        internal XPdfForm(Stream stream)
        {
            _path = "*" + Guid.NewGuid().ToString("B");

            if (PdfReader.TestPdfFile(stream) == 0)
                throw new ArgumentException("The specified stream has no valid PDF file header.", "stream");

            _externalDocument = PdfReader.Open(stream);
        }

        public static new XPdfForm FromFile(string path)
        {
            return new XPdfForm(path);
        }

        public static new XPdfForm FromStream(Stream stream)
        {
            return new XPdfForm(stream);
        }

        internal override void Finish()
        {
            if (_formState == FormState.NotATemplate || _formState == FormState.Finished)
                return;

            base.Finish();

        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                try
                {
                    if (disposing)
                    {
                    }
                    if (_externalDocument != null)
                        PdfDocument.Tls.DetachDocument(_externalDocument.Handle);
                }
                finally
                {
                    base.Dispose(disposing);
                }
            }
        }
        bool _disposed;

        public XImage PlaceHolder
        {
            get { return _placeHolder; }
            set { _placeHolder = value; }
        }
        XImage _placeHolder;

        public PdfPage Page
        {
            get
            {
                if (IsTemplate)
                    return null;
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page;
            }
        }

        public int PageCount
        {
            get
            {
                if (IsTemplate)
                    return 1;
                if (_pageCount == -1)
                    _pageCount = ExternalDocument.Pages.Count;
                return _pageCount;
            }
        }
        int _pageCount = -1;

        [Obsolete("Use either PixelWidth or PointWidth. Temporarily obsolete because of rearrangements for WPF.")]
        public override double Width
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Width;
            }
        }

        [Obsolete("Use either PixelHeight or PointHeight. Temporarily obsolete because of rearrangements for WPF.")]
        public override double Height
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Height;
            }
        }

        public override double PointWidth
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Width;
            }
        }

        public override double PointHeight
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return page.Height;
            }
        }

        public override int PixelWidth
        {
            get
            {
                return DoubleUtil.DoubleToInt(PointWidth);
            }
        }

        public override int PixelHeight
        {
            get
            {
                return DoubleUtil.DoubleToInt(PointHeight);
            }
        }

        public override XSize Size
        {
            get
            {
                PdfPage page = ExternalDocument.Pages[_pageNumber - 1];
                return new XSize(page.Width, page.Height);
            }
        }

        public override XMatrix Transform
        {
            get { return _transform; }
            set
            {
                if (_transform != value)
                {
                    _pdfForm = null;
                    _transform = value;
                }
            }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if (IsTemplate)
                    throw new InvalidOperationException("The page number of an XPdfForm template cannot be modified.");

                if (_pageNumber != value)
                {
                    _pageNumber = value;
                    _pdfForm = null;
                }
            }
        }
        int _pageNumber = 1;

        public int PageIndex
        {
            get { return PageNumber - 1; }
            set { PageNumber = value + 1; }
        }

        internal PdfDocument ExternalDocument
        {
            get
            {
                if (IsTemplate)
                    throw new InvalidOperationException("This XPdfForm is a template and not an imported PDF page; therefore it has no external document.");

                if (_externalDocument == null)
                    _externalDocument = PdfDocument.Tls.GetDocument(_path);
                return _externalDocument;
            }
        }
        internal PdfDocument _externalDocument;

        public static string ExtractPageNumber(string path, out int pageNumber)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            pageNumber = 0;
            int length = path.Length;
            if (length != 0)
            {
                length--;
                if (char.IsDigit(path, length))
                {
                    while (char.IsDigit(path, length) && length >= 0)
                        length--;
                    if (length > 0 && path[length] == '#')
                    {
                        if (path.IndexOf('.') != -1)
                        {
                            pageNumber = int.Parse(path.Substring(length + 1));
                            path = path.Substring(0, length);
                        }
                    }
                }
            }
            return path;
        }
    }
    public sealed class XPen
    {
        public XPen(XColor color)
            : this(color, 1, false)
        { }

        public XPen(XColor color, double width)
            : this(color, width, false)
        { }

        internal XPen(XColor color, double width, bool immutable)
        {
            _color = color;
            _width = width;
            _lineJoin = XLineJoin.Miter;
            _lineCap = XLineCap.Flat;
            _dashStyle = XDashStyle.Solid;
            _dashOffset = 0f;
            _immutable = immutable;
        }

        public XPen(XPen pen)
        {
            _color = pen._color;
            _width = pen._width;
            _lineJoin = pen._lineJoin;
            _lineCap = pen._lineCap;
            _dashStyle = pen._dashStyle;
            _dashOffset = pen._dashOffset;
            _dashPattern = pen._dashPattern;
            if (_dashPattern != null)
                _dashPattern = (double[])_dashPattern.Clone();
        }

        public XPen Clone()
        {
            return new XPen(this);
        }

        public XColor Color
        {
            get { return _color; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _color != value;
                _color = value;
            }
        }
        internal XColor _color;

        public double Width
        {
            get { return _width; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _width != value;
                _width = value;
            }
        }
        internal double _width;

        public XLineJoin LineJoin
        {
            get { return _lineJoin; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _lineJoin != value;
                _lineJoin = value;
            }
        }
        internal XLineJoin _lineJoin;

        public XLineCap LineCap
        {
            get { return _lineCap; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _lineCap != value;
                _lineCap = value;
            }
        }
        internal XLineCap _lineCap;

        public double MiterLimit
        {
            get { return _miterLimit; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _miterLimit != value;
                _miterLimit = value;
            }
        }
        internal double _miterLimit;

        public XDashStyle DashStyle
        {
            get { return _dashStyle; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _dashStyle != value;
                _dashStyle = value;
            }
        }
        internal XDashStyle _dashStyle;

        public double DashOffset
        {
            get { return _dashOffset; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _dirty = _dirty || _dashOffset != value;
                _dashOffset = value;
            }
        }
        internal double _dashOffset;

        public double[] DashPattern
        {
            get
            {
                if (_dashPattern == null)
                    _dashPattern = new double[0];
                return _dashPattern;
            }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));

                int length = value.Length;
                for (int idx = 0; idx < length; idx++)
                {
                    if (value[idx] <= 0)
                        throw new ArgumentException("Dash pattern value must greater than zero.");
                }

                _dirty = true;
                _dashStyle = XDashStyle.Custom;
                _dashPattern = (double[])value.Clone();
            }
        }
        internal double[] _dashPattern;

        public bool Overprint
        {
            get { return _overprint; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XPen"));
                _overprint = value;
            }
        }
        internal bool _overprint;

        bool _dirty = true;
        readonly bool _immutable;

    }
    public static class XPens
    {
        public static XPen AliceBlue
        {
            get { return new XPen(XColors.AliceBlue, 1, true); }
        }

        public static XPen AntiqueWhite
        {
            get { return new XPen(XColors.AntiqueWhite, 1, true); }
        }

        public static XPen Aqua
        {
            get { return new XPen(XColors.Aqua, 1, true); }
        }

        public static XPen Aquamarine
        {
            get { return new XPen(XColors.Aquamarine, 1, true); }
        }

        public static XPen Azure
        {
            get { return new XPen(XColors.Azure, 1, true); }
        }

        public static XPen Beige
        {
            get { return new XPen(XColors.Beige, 1, true); }
        }

        public static XPen Bisque
        {
            get { return new XPen(XColors.Bisque, 1, true); }
        }

        public static XPen Black
        {
            get { return new XPen(XColors.Black, 1, true); }
        }

        public static XPen BlanchedAlmond
        {
            get { return new XPen(XColors.BlanchedAlmond, 1, true); }
        }

        public static XPen Blue
        {
            get { return new XPen(XColors.Blue, 1, true); }
        }

        public static XPen BlueViolet
        {
            get { return new XPen(XColors.BlueViolet, 1, true); }
        }

        public static XPen Brown
        {
            get { return new XPen(XColors.Brown, 1, true); }
        }

        public static XPen BurlyWood
        {
            get { return new XPen(XColors.BurlyWood, 1, true); }
        }

        public static XPen CadetBlue
        {
            get { return new XPen(XColors.CadetBlue, 1, true); }
        }

        public static XPen Chartreuse
        {
            get { return new XPen(XColors.Chartreuse, 1, true); }
        }

        public static XPen Chocolate
        {
            get { return new XPen(XColors.Chocolate, 1, true); }
        }

        public static XPen Coral
        {
            get { return new XPen(XColors.Coral, 1, true); }
        }

        public static XPen CornflowerBlue
        {
            get { return new XPen(XColors.CornflowerBlue, 1, true); }
        }

        public static XPen Cornsilk
        {
            get { return new XPen(XColors.Cornsilk, 1, true); }
        }

        public static XPen Crimson
        {
            get { return new XPen(XColors.Crimson, 1, true); }
        }

        public static XPen Cyan
        {
            get { return new XPen(XColors.Cyan, 1, true); }
        }

        public static XPen DarkBlue
        {
            get { return new XPen(XColors.DarkBlue, 1, true); }
        }

        public static XPen DarkCyan
        {
            get { return new XPen(XColors.DarkCyan, 1, true); }
        }

        public static XPen DarkGoldenrod
        {
            get { return new XPen(XColors.DarkGoldenrod, 1, true); }
        }

        public static XPen DarkGray
        {
            get { return new XPen(XColors.DarkGray, 1, true); }
        }

        public static XPen DarkGreen
        {
            get { return new XPen(XColors.DarkGreen, 1, true); }
        }

        public static XPen DarkKhaki
        {
            get { return new XPen(XColors.DarkKhaki, 1, true); }
        }

        public static XPen DarkMagenta
        {
            get { return new XPen(XColors.DarkMagenta, 1, true); }
        }

        public static XPen DarkOliveGreen
        {
            get { return new XPen(XColors.DarkOliveGreen, 1, true); }
        }

        public static XPen DarkOrange
        {
            get { return new XPen(XColors.DarkOrange, 1, true); }
        }

        public static XPen DarkOrchid
        {
            get { return new XPen(XColors.DarkOrchid, 1, true); }
        }

        public static XPen DarkRed
        {
            get { return new XPen(XColors.DarkRed, 1, true); }
        }

        public static XPen DarkSalmon
        {
            get { return new XPen(XColors.DarkSalmon, 1, true); }
        }

        public static XPen DarkSeaGreen
        {
            get { return new XPen(XColors.DarkSeaGreen, 1, true); }
        }

        public static XPen DarkSlateBlue
        {
            get { return new XPen(XColors.DarkSlateBlue, 1, true); }
        }

        public static XPen DarkSlateGray
        {
            get { return new XPen(XColors.DarkSlateGray, 1, true); }
        }

        public static XPen DarkTurquoise
        {
            get { return new XPen(XColors.DarkTurquoise, 1, true); }
        }

        public static XPen DarkViolet
        {
            get { return new XPen(XColors.DarkViolet, 1, true); }
        }

        public static XPen DeepPink
        {
            get { return new XPen(XColors.DeepPink, 1, true); }
        }

        public static XPen DeepSkyBlue
        {
            get { return new XPen(XColors.DeepSkyBlue, 1, true); }
        }

        public static XPen DimGray
        {
            get { return new XPen(XColors.DimGray, 1, true); }
        }

        public static XPen DodgerBlue
        {
            get { return new XPen(XColors.DodgerBlue, 1, true); }
        }

        public static XPen Firebrick
        {
            get { return new XPen(XColors.Firebrick, 1, true); }
        }

        public static XPen FloralWhite
        {
            get { return new XPen(XColors.FloralWhite, 1, true); }
        }

        public static XPen ForestGreen
        {
            get { return new XPen(XColors.ForestGreen, 1, true); }
        }

        public static XPen Fuchsia
        {
            get { return new XPen(XColors.Fuchsia, 1, true); }
        }

        public static XPen Gainsboro
        {
            get { return new XPen(XColors.Gainsboro, 1, true); }
        }

        public static XPen GhostWhite
        {
            get { return new XPen(XColors.GhostWhite, 1, true); }
        }

        public static XPen Gold
        {
            get { return new XPen(XColors.Gold, 1, true); }
        }

        public static XPen Goldenrod
        {
            get { return new XPen(XColors.Goldenrod, 1, true); }
        }

        public static XPen Gray
        {
            get { return new XPen(XColors.Gray, 1, true); }
        }

        public static XPen Green
        {
            get { return new XPen(XColors.Green, 1, true); }
        }

        public static XPen GreenYellow
        {
            get { return new XPen(XColors.GreenYellow, 1, true); }
        }

        public static XPen Honeydew
        {
            get { return new XPen(XColors.Honeydew, 1, true); }
        }

        public static XPen HotPink
        {
            get { return new XPen(XColors.HotPink, 1, true); }
        }

        public static XPen IndianRed
        {
            get { return new XPen(XColors.IndianRed, 1, true); }
        }

        public static XPen Indigo
        {
            get { return new XPen(XColors.Indigo, 1, true); }
        }

        public static XPen Ivory
        {
            get { return new XPen(XColors.Ivory, 1, true); }
        }

        public static XPen Khaki
        {
            get { return new XPen(XColors.Khaki, 1, true); }
        }

        public static XPen Lavender
        {
            get { return new XPen(XColors.Lavender, 1, true); }
        }

        public static XPen LavenderBlush
        {
            get { return new XPen(XColors.LavenderBlush, 1, true); }
        }

        public static XPen LawnGreen
        {
            get { return new XPen(XColors.LawnGreen, 1, true); }
        }

        public static XPen LemonChiffon
        {
            get { return new XPen(XColors.LemonChiffon, 1, true); }
        }

        public static XPen LightBlue
        {
            get { return new XPen(XColors.LightBlue, 1, true); }
        }

        public static XPen LightCoral
        {
            get { return new XPen(XColors.LightCoral, 1, true); }
        }

        public static XPen LightCyan
        {
            get { return new XPen(XColors.LightCyan, 1, true); }
        }

        public static XPen LightGoldenrodYellow
        {
            get { return new XPen(XColors.LightGoldenrodYellow, 1, true); }
        }

        public static XPen LightGray
        {
            get { return new XPen(XColors.LightGray, 1, true); }
        }

        public static XPen LightGreen
        {
            get { return new XPen(XColors.LightGreen, 1, true); }
        }

        public static XPen LightPink
        {
            get { return new XPen(XColors.LightPink, 1, true); }
        }

        public static XPen LightSalmon
        {
            get { return new XPen(XColors.LightSalmon, 1, true); }
        }

        public static XPen LightSeaGreen
        {
            get { return new XPen(XColors.LightSeaGreen, 1, true); }
        }

        public static XPen LightSkyBlue
        {
            get { return new XPen(XColors.LightSkyBlue, 1, true); }
        }

        public static XPen LightSlateGray
        {
            get { return new XPen(XColors.LightSlateGray, 1, true); }
        }

        public static XPen LightSteelBlue
        {
            get { return new XPen(XColors.LightSteelBlue, 1, true); }
        }

        public static XPen LightYellow
        {
            get { return new XPen(XColors.LightYellow, 1, true); }
        }

        public static XPen Lime
        {
            get { return new XPen(XColors.Lime, 1, true); }
        }

        public static XPen LimeGreen
        {
            get { return new XPen(XColors.LimeGreen, 1, true); }
        }

        public static XPen Linen
        {
            get { return new XPen(XColors.Linen, 1, true); }
        }

        public static XPen Magenta
        {
            get { return new XPen(XColors.Magenta, 1, true); }
        }

        public static XPen Maroon
        {
            get { return new XPen(XColors.Maroon, 1, true); }
        }

        public static XPen MediumAquamarine
        {
            get { return new XPen(XColors.MediumAquamarine, 1, true); }
        }

        public static XPen MediumBlue
        {
            get { return new XPen(XColors.MediumBlue, 1, true); }
        }

        public static XPen MediumOrchid
        {
            get { return new XPen(XColors.MediumOrchid, 1, true); }
        }

        public static XPen MediumPurple
        {
            get { return new XPen(XColors.MediumPurple, 1, true); }
        }

        public static XPen MediumSeaGreen
        {
            get { return new XPen(XColors.MediumSeaGreen, 1, true); }
        }

        public static XPen MediumSlateBlue
        {
            get { return new XPen(XColors.MediumSlateBlue, 1, true); }
        }

        public static XPen MediumSpringGreen
        {
            get { return new XPen(XColors.MediumSpringGreen, 1, true); }
        }

        public static XPen MediumTurquoise
        {
            get { return new XPen(XColors.MediumTurquoise, 1, true); }
        }

        public static XPen MediumVioletRed
        {
            get { return new XPen(XColors.MediumVioletRed, 1, true); }
        }

        public static XPen MidnightBlue
        {
            get { return new XPen(XColors.MidnightBlue, 1, true); }
        }

        public static XPen MintCream
        {
            get { return new XPen(XColors.MintCream, 1, true); }
        }

        public static XPen MistyRose
        {
            get { return new XPen(XColors.MistyRose, 1, true); }
        }

        public static XPen Moccasin
        {
            get { return new XPen(XColors.Moccasin, 1, true); }
        }

        public static XPen NavajoWhite
        {
            get { return new XPen(XColors.NavajoWhite, 1, true); }
        }

        public static XPen Navy
        {
            get { return new XPen(XColors.Navy, 1, true); }
        }

        public static XPen OldLace
        {
            get { return new XPen(XColors.OldLace, 1, true); }
        }

        public static XPen Olive
        {
            get { return new XPen(XColors.Olive, 1, true); }
        }

        public static XPen OliveDrab
        {
            get { return new XPen(XColors.OliveDrab, 1, true); }
        }

        public static XPen Orange
        {
            get { return new XPen(XColors.Orange, 1, true); }
        }

        public static XPen OrangeRed
        {
            get { return new XPen(XColors.OrangeRed, 1, true); }
        }

        public static XPen Orchid
        {
            get { return new XPen(XColors.Orchid, 1, true); }
        }

        public static XPen PaleGoldenrod
        {
            get { return new XPen(XColors.PaleGoldenrod, 1, true); }
        }

        public static XPen PaleGreen
        {
            get { return new XPen(XColors.PaleGreen, 1, true); }
        }

        public static XPen PaleTurquoise
        {
            get { return new XPen(XColors.PaleTurquoise, 1, true); }
        }

        public static XPen PaleVioletRed
        {
            get { return new XPen(XColors.PaleVioletRed, 1, true); }
        }

        public static XPen PapayaWhip
        {
            get { return new XPen(XColors.PapayaWhip, 1, true); }
        }

        public static XPen PeachPuff
        {
            get { return new XPen(XColors.PeachPuff, 1, true); }
        }

        public static XPen Peru
        {
            get { return new XPen(XColors.Peru, 1, true); }
        }

        public static XPen Pink
        {
            get { return new XPen(XColors.Pink, 1, true); }
        }

        public static XPen Plum
        {
            get { return new XPen(XColors.Plum, 1, true); }
        }

        public static XPen PowderBlue
        {
            get { return new XPen(XColors.PowderBlue, 1, true); }
        }

        public static XPen Purple
        {
            get { return new XPen(XColors.Purple, 1, true); }
        }

        public static XPen Red
        {
            get { return new XPen(XColors.Red, 1, true); }
        }

        public static XPen RosyBrown
        {
            get { return new XPen(XColors.RosyBrown, 1, true); }
        }

        public static XPen RoyalBlue
        {
            get { return new XPen(XColors.RoyalBlue, 1, true); }
        }

        public static XPen SaddleBrown
        {
            get { return new XPen(XColors.SaddleBrown, 1, true); }
        }

        public static XPen Salmon
        {
            get { return new XPen(XColors.Salmon, 1, true); }
        }

        public static XPen SandyBrown
        {
            get { return new XPen(XColors.SandyBrown, 1, true); }
        }

        public static XPen SeaGreen
        {
            get { return new XPen(XColors.SeaGreen, 1, true); }
        }

        public static XPen SeaShell
        {
            get { return new XPen(XColors.SeaShell, 1, true); }
        }

        public static XPen Sienna
        {
            get { return new XPen(XColors.Sienna, 1, true); }
        }

        public static XPen Silver
        {
            get { return new XPen(XColors.Silver, 1, true); }
        }

        public static XPen SkyBlue
        {
            get { return new XPen(XColors.SkyBlue, 1, true); }
        }

        public static XPen SlateBlue
        {
            get { return new XPen(XColors.SlateBlue, 1, true); }
        }

        public static XPen SlateGray
        {
            get { return new XPen(XColors.SlateGray, 1, true); }
        }

        public static XPen Snow
        {
            get { return new XPen(XColors.Snow, 1, true); }
        }

        public static XPen SpringGreen
        {
            get { return new XPen(XColors.SpringGreen, 1, true); }
        }

        public static XPen SteelBlue
        {
            get { return new XPen(XColors.SteelBlue, 1, true); }
        }

        public static XPen Tan
        {
            get { return new XPen(XColors.Tan, 1, true); }
        }

        public static XPen Teal
        {
            get { return new XPen(XColors.Teal, 1, true); }
        }

        public static XPen Thistle
        {
            get { return new XPen(XColors.Thistle, 1, true); }
        }

        public static XPen Tomato
        {
            get { return new XPen(XColors.Tomato, 1, true); }
        }

        public static XPen Transparent
        {
            get { return new XPen(XColors.Transparent, 1, true); }
        }

        public static XPen Turquoise
        {
            get { return new XPen(XColors.Turquoise, 1, true); }
        }

        public static XPen Violet
        {
            get { return new XPen(XColors.Violet, 1, true); }
        }

        public static XPen Wheat
        {
            get { return new XPen(XColors.Wheat, 1, true); }
        }

        public static XPen White
        {
            get { return new XPen(XColors.White, 1, true); }
        }

        public static XPen WhiteSmoke
        {
            get { return new XPen(XColors.WhiteSmoke, 1, true); }
        }

        public static XPen Yellow
        {
            get { return new XPen(XColors.Yellow, 1, true); }
        }

        public static XPen YellowGreen
        {
            get { return new XPen(XColors.YellowGreen, 1, true); }
        }

    }
    public struct XPoint : IFormattable
    {
        public XPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }


        public static bool operator ==(XPoint point1, XPoint point2)
        {
            return point1._x == point2._x && point1._y == point2._y;
        }

        public static bool operator !=(XPoint point1, XPoint point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(XPoint point1, XPoint point2)
        {
            return point1.X.Equals(point2.X) && point1.Y.Equals(point2.Y);
        }

        public override bool Equals(object o)
        {
            if (!(o is XPoint))
                return false;
            return Equals(this, (XPoint)o);
        }

        public bool Equals(XPoint value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static XPoint Parse(string source)
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
            string str = helper.NextTokenRequired();
            XPoint point = new XPoint(Convert.ToDouble(str, cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo));
            helper.LastTokenRequired();
            return point;
        }

        public static XPoint[] ParsePoints(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            string[] values = value.Split(' ');
            int count = values.Length;
            XPoint[] points = new XPoint[count];
            for (int idx = 0; idx < count; idx++)
                points[idx] = Parse(values[idx]);
            return points;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        double _x;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        double _y;

        public override string ToString()
        {
            return ConvertToString(null, null);
        }

        public string ToString(IFormatProvider provider)
        {
            return ConvertToString(null, provider);
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return ConvertToString(format, provider);
        }

        internal string ConvertToString(string format, IFormatProvider provider)
        {
            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            provider = provider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[] { numericListSeparator, _x, _y });
        }

        public void Offset(double offsetX, double offsetY)
        {
            _x += offsetX;
            _y += offsetY;
        }

        public static XPoint operator +(XPoint point, XVector vector)
        {
            return new XPoint(point._x + vector.X, point._y + vector.Y);
        }

        public static XPoint operator +(XPoint point, XSize size)
        {
            return new XPoint(point._x + size.Width, point._y + size.Height);
        }

        public static XPoint Add(XPoint point, XVector vector)
        {
            return new XPoint(point._x + vector.X, point._y + vector.Y);
        }

        public static XPoint operator -(XPoint point, XVector vector)
        {
            return new XPoint(point._x - vector.X, point._y - vector.Y);
        }

        public static XPoint Subtract(XPoint point, XVector vector)
        {
            return new XPoint(point._x - vector.X, point._y - vector.Y);
        }

        public static XVector operator -(XPoint point1, XPoint point2)
        {
            return new XVector(point1._x - point2._x, point1._y - point2._y);
        }

        [Obsolete("Use XVector instead of XSize as second parameter.")]
        public static XPoint operator -(XPoint point, XSize size)
        {
            return new XPoint(point._x - size.Width, point._y - size.Height);
        }

        public static XVector Subtract(XPoint point1, XPoint point2)
        {
            return new XVector(point1._x - point2._x, point1._y - point2._y);
        }

        public static XPoint operator *(XPoint point, XMatrix matrix)
        {
            return matrix.Transform(point);
        }

        public static XPoint Multiply(XPoint point, XMatrix matrix)
        {
            return matrix.Transform(point);
        }

        public static XPoint operator *(XPoint point, double value)
        {
            return new XPoint(point._x * value, point._y * value);
        }

        public static XPoint operator *(double value, XPoint point)
        {
            return new XPoint(value * point._x, value * point._y);
        }

        public static explicit operator XSize(XPoint point)
        {
            return new XSize(Math.Abs(point._x), Math.Abs(point._y));
        }

        public static explicit operator XVector(XPoint point)
        {
            return new XVector(point._x, point._y);
        }

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture, "point=({0:" + format + "}, {1:" + format + "})", _x, _y);
            }
        }
    }
    public sealed class XPrivateFontCollection
    {
        XPrivateFontCollection()
        {
        }

        internal static XPrivateFontCollection Singleton
        {
            get { return _singleton; }
        }
        internal static XPrivateFontCollection _singleton = new XPrivateFontCollection();

#if GDI
 
#else
        [Obsolete("Use the GDI build of PDFsharp and use Add(Stream stream)")]
#endif
        public static void AddFont(string filename)
        {
            throw new NotImplementedException();
        }


#if GDI
        
#else
        [Obsolete("Use the GDI build of PDFsharp and use Add(Stream stream)")]
#endif
        public static void AddFont(Stream stream, string facename)
        {
            throw new NotImplementedException();
        }


        static string MakeKey(string familyName, XFontStyle style)
        {
            return MakeKey(familyName, (style & XFontStyle.Bold) != 0, (style & XFontStyle.Italic) != 0);
        }

        static string MakeKey(string familyName, bool bold, bool italic)
        {
            return familyName + "#" + (bold ? "b" : "") + (italic ? "i" : "");
        }

        readonly Dictionary<string, XGlyphTypeface> _typefaces = new Dictionary<string, XGlyphTypeface>();


    }
    public struct XRect : IFormattable
    {
        public XRect(double x, double y, double width, double height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("WidthAndHeightCannotBeNegative");
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public XRect(XPoint point1, XPoint point2)
        {
            _x = Math.Min(point1.X, point2.X);
            _y = Math.Min(point1.Y, point2.Y);
            _width = Math.Max(Math.Max(point1.X, point2.X) - _x, 0);
            _height = Math.Max(Math.Max(point1.Y, point2.Y) - _y, 0);
        }

        public XRect(XPoint point, XVector vector)
            : this(point, point + vector)
        { }

        public XRect(XPoint location, XSize size)
        {
            if (size.IsEmpty)
                this = s_empty;
            else
            {
                _x = location.X;
                _y = location.Y;
                _width = size.Width;
                _height = size.Height;
            }
        }

        public XRect(XSize size)
        {
            if (size.IsEmpty)
                this = s_empty;
            else
            {
                _x = _y = 0;
                _width = size.Width;
                _height = size.Height;
            }
        }


        public static XRect FromLTRB(double left, double top, double right, double bottom)
        {
            return new XRect(left, top, right - left, bottom - top);
        }

        public static bool operator ==(XRect rect1, XRect rect2)
        {
            return rect1.X == rect2.X && rect1.Y == rect2.Y && rect1.Width == rect2.Width && rect1.Height == rect2.Height;
        }

        public static bool operator !=(XRect rect1, XRect rect2)
        {
            return !(rect1 == rect2);
        }

        public static bool Equals(XRect rect1, XRect rect2)
        {
            if (rect1.IsEmpty)
                return rect2.IsEmpty;
            return rect1.X.Equals(rect2.X) && rect1.Y.Equals(rect2.Y) && rect1.Width.Equals(rect2.Width) && rect1.Height.Equals(rect2.Height);
        }

        public override bool Equals(object o)
        {
            if (!(o is XRect))
                return false;
            return Equals(this, (XRect)o);
        }

        public bool Equals(XRect value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
                return 0;
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }

        public static XRect Parse(string source)
        {
            XRect empty;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
            string str = helper.NextTokenRequired();
            if (str == "Empty")
                empty = Empty;
            else
                empty = new XRect(Convert.ToDouble(str, cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo));
            helper.LastTokenRequired();
            return empty;
        }

        public override string ToString()
        {
            return ConvertToString(null, null);
        }

        public string ToString(IFormatProvider provider)
        {
            return ConvertToString(null, provider);
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return ConvertToString(format, provider);
        }

        internal string ConvertToString(string format, IFormatProvider provider)
        {
            if (IsEmpty)
                return "Empty";
            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            provider = provider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}{0}{3:" + format + "}{0}{4:" + format + "}", new object[] { numericListSeparator, _x, _y, _width, _height });
        }

        public static XRect Empty
        {
            get { return s_empty; }
        }

        public bool IsEmpty
        {
            get { return _width < 0; }
        }

        public XPoint Location
        {
            get { return new XPoint(_x, _y); }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                _x = value.X;
                _y = value.Y;
            }
        }

        public XSize Size
        {
            get
            {
                if (IsEmpty)
                    return XSize.Empty;
                return new XSize(_width, _height);
            }
            set
            {
                if (value.IsEmpty)
                    this = s_empty;
                else
                {
                    if (IsEmpty)
                        throw new InvalidOperationException("CannotModifyEmptyRect");
                    _width = value.Width;
                    _height = value.Height;
                }
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                _x = value;
            }
        }
        double _x;

        public double Y
        {
            get { return _y; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                _y = value;
            }
        }
        double _y;

        public double Width
        {
            get { return _width; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                if (value < 0)
                    throw new ArgumentException("WidthCannotBeNegative");

                _width = value;
            }
        }
        double _width;

        public double Height
        {
            get { return _height; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptyRect");
                if (value < 0)
                    throw new ArgumentException("HeightCannotBeNegative");
                _height = value;
            }
        }
        double _height;

        public double Left
        {
            get { return _x; }
        }

        public double Top
        {
            get { return _y; }
        }

        public double Right
        {
            get
            {
                if (IsEmpty)
                    return double.NegativeInfinity;
                return _x + _width;
            }
        }

        public double Bottom
        {
            get
            {
                if (IsEmpty)
                    return double.NegativeInfinity;
                return _y + _height;
            }
        }

        public XPoint TopLeft
        {
            get { return new XPoint(Left, Top); }
        }

        public XPoint TopRight
        {
            get { return new XPoint(Right, Top); }
        }

        public XPoint BottomLeft
        {
            get { return new XPoint(Left, Bottom); }
        }

        public XPoint BottomRight
        {
            get { return new XPoint(Right, Bottom); }
        }

        public XPoint Center
        {
            get { return new XPoint(_x + _width / 2, _y + _height / 2); }
        }

        public bool Contains(XPoint point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(double x, double y)
        {
            if (IsEmpty)
                return false;
            return ContainsInternal(x, y);
        }

        public bool Contains(XRect rect)
        {
            return !IsEmpty && !rect.IsEmpty &&
              _x <= rect._x && _y <= rect._y &&
              _x + _width >= rect._x + rect._width && _y + _height >= rect._y + rect._height;
        }

        public bool IntersectsWith(XRect rect)
        {
            return !IsEmpty && !rect.IsEmpty &&
                rect.Left <= Right && rect.Right >= Left &&
                rect.Top <= Bottom && rect.Bottom >= Top;
        }

        public void Intersect(XRect rect)
        {
            if (!IntersectsWith(rect))
                this = Empty;
            else
            {
                double left = Math.Max(Left, rect.Left);
                double top = Math.Max(Top, rect.Top);
                _width = Math.Max(Math.Min(Right, rect.Right) - left, 0.0);
                _height = Math.Max(Math.Min(Bottom, rect.Bottom) - top, 0.0);
                _x = left;
                _y = top;
            }
        }

        public static XRect Intersect(XRect rect1, XRect rect2)
        {
            rect1.Intersect(rect2);
            return rect1;
        }

        public void Union(XRect rect)
        {
            if (IsEmpty)
                this = rect;
            else if (!rect.IsEmpty)
            {
                double left = Math.Min(Left, rect.Left);
                double top = Math.Min(Top, rect.Top);
                if (rect.Width == Double.PositiveInfinity || Width == Double.PositiveInfinity)
                    _width = Double.PositiveInfinity;
                else
                {
                    double right = Math.Max(Right, rect.Right);
                    _width = Math.Max(right - left, 0.0);
                }

                if (rect.Height == Double.PositiveInfinity || _height == Double.PositiveInfinity)
                    _height = Double.PositiveInfinity;
                else
                {
                    double bottom = Math.Max(Bottom, rect.Bottom);
                    _height = Math.Max(bottom - top, 0.0);
                }
                _x = left;
                _y = top;
            }
        }

        public static XRect Union(XRect rect1, XRect rect2)
        {
            rect1.Union(rect2);
            return rect1;
        }

        public void Union(XPoint point)
        {
            Union(new XRect(point, point));
        }

        public static XRect Union(XRect rect, XPoint point)
        {
            rect.Union(new XRect(point, point));
            return rect;
        }

        public void Offset(XVector offsetVector)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CannotCallMethod");
            _x += offsetVector.X;
            _y += offsetVector.Y;
        }

        public void Offset(double offsetX, double offsetY)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CannotCallMethod");
            _x += offsetX;
            _y += offsetY;
        }

        public static XRect Offset(XRect rect, XVector offsetVector)
        {
            rect.Offset(offsetVector.X, offsetVector.Y);
            return rect;
        }

        public static XRect Offset(XRect rect, double offsetX, double offsetY)
        {
            rect.Offset(offsetX, offsetY);
            return rect;
        }

        public static XRect operator +(XRect rect, XPoint point)
        {
            return new XRect(rect._x + point.X, rect.Y + point.Y, rect._width, rect._height);
        }

        public static XRect operator -(XRect rect, XPoint point)
        {
            return new XRect(rect._x - point.X, rect.Y - point.Y, rect._width, rect._height);
        }

        public void Inflate(XSize size)
        {
            Inflate(size.Width, size.Height);
        }

        public void Inflate(double width, double height)
        {
            if (IsEmpty)
                throw new InvalidOperationException("CannotCallMethod");
            _x -= width;
            _y -= height;
            _width += width;
            _width += width;
            _height += height;
            _height += height;
            if (_width < 0 || _height < 0)
                this = s_empty;
        }

        public static XRect Inflate(XRect rect, XSize size)
        {
            rect.Inflate(size.Width, size.Height);
            return rect;
        }

        public static XRect Inflate(XRect rect, double width, double height)
        {
            rect.Inflate(width, height);
            return rect;
        }

        public static XRect Transform(XRect rect, XMatrix matrix)
        {
            XMatrix.MatrixHelper.TransformRect(ref rect, ref matrix);
            return rect;
        }

        public void Transform(XMatrix matrix)
        {
            XMatrix.MatrixHelper.TransformRect(ref this, ref matrix);
        }

        public void Scale(double scaleX, double scaleY)
        {
            if (!IsEmpty)
            {
                _x *= scaleX;
                _y *= scaleY;
                _width *= scaleX;
                _height *= scaleY;
                if (scaleX < 0)
                {
                    _x += _width;
                    _width *= -1.0;
                }
                if (scaleY < 0)
                {
                    _y += _height;
                    _height *= -1.0;
                }
            }
        }


        bool ContainsInternal(double x, double y)
        {
            return x >= _x && x - _width <= _x && y >= _y && y - _height <= _y;
        }

        static XRect CreateEmptyRect()
        {
            XRect rect = new XRect();
            rect._x = double.PositiveInfinity;
            rect._y = double.PositiveInfinity;
            rect._width = double.NegativeInfinity;
            rect._height = double.NegativeInfinity;
            return rect;
        }

        static XRect()
        {
            s_empty = CreateEmptyRect();
        }

        static readonly XRect s_empty;

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture,
                    "rect=({0:" + format + "}, {1:" + format + "}, {2:" + format + "}, {3:" + format + "})",
                    _x, _y, _width, _height);
            }
        }
    }
    public struct XSize : IFormattable
    {
        public XSize(double width, double height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("WidthAndHeightCannotBeNegative");

            _width = width;
            _height = height;
        }

        public static bool operator ==(XSize size1, XSize size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
        }

        public static bool operator !=(XSize size1, XSize size2)
        {
            return !(size1 == size2);
        }

        public static bool Equals(XSize size1, XSize size2)
        {
            if (size1.IsEmpty)
                return size2.IsEmpty;
            return size1.Width.Equals(size2.Width) && size1.Height.Equals(size2.Height);
        }

        public override bool Equals(object o)
        {
            if (!(o is XSize))
                return false;
            return Equals(this, (XSize)o);
        }

        public bool Equals(XSize value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
                return 0;
            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        public static XSize Parse(string source)
        {
            XSize empty;
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            TokenizerHelper helper = new TokenizerHelper(source, cultureInfo);
            string str = helper.NextTokenRequired();
            if (str == "Empty")
                empty = Empty;
            else
                empty = new XSize(Convert.ToDouble(str, cultureInfo), Convert.ToDouble(helper.NextTokenRequired(), cultureInfo));
            helper.LastTokenRequired();
            return empty;
        }

        public XPoint ToXPoint()
        {
            return new XPoint(_width, _height);
        }

        public XVector ToXVector()
        {
            return new XVector(_width, _height);
        }


        public override string ToString()
        {
            return ConvertToString(null, null);
        }

        public string ToString(IFormatProvider provider)
        {
            return ConvertToString(null, provider);
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return ConvertToString(format, provider);
        }

        internal string ConvertToString(string format, IFormatProvider provider)
        {
            if (IsEmpty)
                return "Empty";

            char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
            provider = provider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", new object[] { numericListSeparator, _width, _height });
        }

        public static XSize Empty
        {
            get { return s_empty; }
        }
        static readonly XSize s_empty;

        public bool IsEmpty
        {
            get { return _width < 0; }
        }

        public double Width
        {
            get { return _width; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptySize");
                if (value < 0)
                    throw new ArgumentException("WidthCannotBeNegative");
                _width = value;
            }
        }
        double _width;

        public double Height
        {
            get { return _height; }
            set
            {
                if (IsEmpty)
                    throw new InvalidOperationException("CannotModifyEmptySize");
                if (value < 0)
                    throw new ArgumentException("HeightCannotBeNegative");
                _height = value;
            }
        }
        double _height;

        public static explicit operator XVector(XSize size)
        {
            return new XVector(size._width, size._height);
        }

        public static explicit operator XPoint(XSize size)
        {
            return new XPoint(size._width, size._height);
        }

        private static XSize CreateEmptySize()
        {
            XSize size = new XSize();
            size._width = double.NegativeInfinity;
            size._height = double.NegativeInfinity;
            return size;
        }

        static XSize()
        {
            s_empty = CreateEmptySize();
        }

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture,
                    "size=({2}{0:" + format + "}, {1:" + format + "})",
                    _width, _height, IsEmpty ? "Empty " : "");
            }
        }
    }
    public sealed class XSolidBrush : XBrush
    {
        public XSolidBrush()
        { }

        public XSolidBrush(XColor color)
            : this(color, false)
        { }

        internal XSolidBrush(XColor color, bool immutable)
        {
            _color = color;
            _immutable = immutable;
        }

        public XSolidBrush(XSolidBrush brush)
        {
            _color = brush.Color;
        }

        public XColor Color
        {
            get { return _color; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XSolidBrush"));
                _color = value;
            }
        }
        internal XColor _color;

        public bool Overprint
        {
            get { return _overprint; }
            set
            {
                if (_immutable)
                    throw new ArgumentException(PSSR.CannotChangeImmutableObject("XSolidBrush"));
                _overprint = value;
            }
        }
        internal bool _overprint;

        readonly bool _immutable;
    }
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
    public struct XUnit : IFormattable
    {
        internal const double PointFactor = 1;
        internal const double InchFactor = 72;
        internal const double MillimeterFactor = 72 / 25.4;
        internal const double CentimeterFactor = 72 / 2.54;
        internal const double PresentationFactor = 72 / 96.0;

        internal const double PointFactorWpf = 96 / 72.0;
        internal const double InchFactorWpf = 96;
        internal const double MillimeterFactorWpf = 96 / 25.4;
        internal const double CentimeterFactorWpf = 96 / 2.54;
        internal const double PresentationFactorWpf = 1;

        public XUnit(double point)
        {
            _value = point;
            _type = XGraphicsUnit.Point;
        }

        public XUnit(double value, XGraphicsUnit type)
        {
            if (!Enum.IsDefined(typeof(XGraphicsUnit), type))
#if !SILVERLIGHT && !NETFX_CORE && !UWP
                throw new System.ComponentModel.InvalidEnumArgumentException(nameof(type), (int)type, typeof(XGraphicsUnit));
#endif
            _value = value;
            _type = type;
        }

        public double Value
        {
            get { return _value; }
        }

        public XGraphicsUnit Type
        {
            get { return _type; }
        }

        public double Point
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value;

                    case XGraphicsUnit.Inch:
                        return _value * 72;

                    case XGraphicsUnit.Millimeter:
                        return _value * 72 / 25.4;

                    case XGraphicsUnit.Centimeter:
                        return _value * 72 / 2.54;

                    case XGraphicsUnit.Presentation:
                        return _value * 72 / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Point;
            }
        }

        public double Inch
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value / 72;

                    case XGraphicsUnit.Inch:
                        return _value;

                    case XGraphicsUnit.Millimeter:
                        return _value / 25.4;

                    case XGraphicsUnit.Centimeter:
                        return _value / 2.54;

                    case XGraphicsUnit.Presentation:
                        return _value / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Inch;
            }
        }

        public double Millimeter
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value * 25.4 / 72;

                    case XGraphicsUnit.Inch:
                        return _value * 25.4;

                    case XGraphicsUnit.Millimeter:
                        return _value;

                    case XGraphicsUnit.Centimeter:
                        return _value * 10;

                    case XGraphicsUnit.Presentation:
                        return _value * 25.4 / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Millimeter;
            }
        }

        public double Centimeter
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value * 2.54 / 72;

                    case XGraphicsUnit.Inch:
                        return _value * 2.54;

                    case XGraphicsUnit.Millimeter:
                        return _value / 10;

                    case XGraphicsUnit.Centimeter:
                        return _value;

                    case XGraphicsUnit.Presentation:
                        return _value * 2.54 / 96;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Centimeter;
            }
        }

        public double Presentation
        {
            get
            {
                switch (_type)
                {
                    case XGraphicsUnit.Point:
                        return _value * 96 / 72;

                    case XGraphicsUnit.Inch:
                        return _value * 96;

                    case XGraphicsUnit.Millimeter:
                        return _value * 96 / 25.4;

                    case XGraphicsUnit.Centimeter:
                        return _value * 96 / 2.54;

                    case XGraphicsUnit.Presentation:
                        return _value;

                    default:
                        throw new InvalidCastException();
                }
            }
            set
            {
                _value = value;
                _type = XGraphicsUnit.Point;
            }
        }

        public string ToString(IFormatProvider formatProvider)
        {
            string valuestring = _value.ToString(formatProvider) + GetSuffix();
            return valuestring;
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            string valuestring = _value.ToString(format, formatProvider) + GetSuffix();
            return valuestring;
        }

        public override string ToString()
        {
            string valuestring = _value.ToString(CultureInfo.InvariantCulture) + GetSuffix();
            return valuestring;
        }

        string GetSuffix()
        {
            switch (_type)
            {
                case XGraphicsUnit.Point:
                    return "pt";

                case XGraphicsUnit.Inch:
                    return "in";

                case XGraphicsUnit.Millimeter:
                    return "mm";

                case XGraphicsUnit.Centimeter:
                    return "cm";

                case XGraphicsUnit.Presentation:
                    return "pu";

                default:
                    throw new InvalidCastException();
            }
        }

        public static XUnit FromPoint(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Point;
            return unit;
        }

        public static XUnit FromInch(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Inch;
            return unit;
        }

        public static XUnit FromMillimeter(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Millimeter;
            return unit;
        }

        public static XUnit FromCentimeter(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Centimeter;
            return unit;
        }

        public static XUnit FromPresentation(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Presentation;
            return unit;
        }

        public static implicit operator XUnit(string value)
        {
            XUnit unit;
            value = value.Trim();

            value = value.Replace(',', '.');

            int count = value.Length;
            int valLen = 0;
            for (; valLen < count;)
            {
                char ch = value[valLen];
                if (ch == '.' || ch == '-' || ch == '+' || char.IsNumber(ch))
                    valLen++;
                else
                    break;
            }

            try
            {
                unit._value = Double.Parse(value.Substring(0, valLen).Trim(), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                unit._value = 1;
                string message = String.Format("String '{0}' is not a valid value for structure 'XUnit'.", value);
                throw new ArgumentException(message, ex);
            }

            string typeStr = value.Substring(valLen).Trim().ToLower();
            unit._type = XGraphicsUnit.Point;
            switch (typeStr)
            {
                case "cm":
                    unit._type = XGraphicsUnit.Centimeter;
                    break;

                case "in":
                    unit._type = XGraphicsUnit.Inch;
                    break;

                case "mm":
                    unit._type = XGraphicsUnit.Millimeter;
                    break;

                case "":
                case "pt":
                    unit._type = XGraphicsUnit.Point;
                    break;

                case "pu":
                    unit._type = XGraphicsUnit.Presentation;
                    break;

                default:
                    throw new ArgumentException("Unknown unit type: '" + typeStr + "'");
            }
            return unit;
        }

        public static implicit operator XUnit(int value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Point;
            return unit;
        }

        public static implicit operator XUnit(double value)
        {
            XUnit unit;
            unit._value = value;
            unit._type = XGraphicsUnit.Point;
            return unit;
        }

        public static implicit operator double(XUnit value)
        {
            return value.Point;
        }

        public static bool operator ==(XUnit value1, XUnit value2)
        {
            return value1._type == value2._type && value1._value == value2._value;
        }

        public static bool operator !=(XUnit value1, XUnit value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(Object obj)
        {
            if (obj is XUnit)
                return this == (XUnit)obj;
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode() ^ _type.GetHashCode();
        }

        public static XUnit Parse(string value)
        {
            XUnit unit = value;
            return unit;
        }

        public void ConvertType(XGraphicsUnit type)
        {
            if (_type == type)
                return;

            switch (type)
            {
                case XGraphicsUnit.Point:
                    _value = Point;
                    _type = XGraphicsUnit.Point;
                    break;

                case XGraphicsUnit.Inch:
                    _value = Inch;
                    _type = XGraphicsUnit.Inch;
                    break;

                case XGraphicsUnit.Centimeter:
                    _value = Centimeter;
                    _type = XGraphicsUnit.Centimeter;
                    break;

                case XGraphicsUnit.Millimeter:
                    _value = Millimeter;
                    _type = XGraphicsUnit.Millimeter;
                    break;

                case XGraphicsUnit.Presentation:
                    _value = Presentation;
                    _type = XGraphicsUnit.Presentation;
                    break;

                default:
                    throw new ArgumentException("Unknown unit type: '" + type + "'");
            }
        }

        public static readonly XUnit Zero = new XUnit();

        double _value;
        XGraphicsUnit _type;

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return String.Format(CultureInfo.InvariantCulture, "unit=({0:" + format + "} {1})", _value, GetSuffix());
            }
        }
    }
    public struct XVector : IFormattable
    {
        public XVector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public static bool operator ==(XVector vector1, XVector vector2)
        {
            return vector1._x == vector2._x && vector1._y == vector2._y;
        }

        public static bool operator !=(XVector vector1, XVector vector2)
        {
            return vector1._x != vector2._x || vector1._y != vector2._y;
        }

        public static bool Equals(XVector vector1, XVector vector2)
        {
            if (vector1.X.Equals(vector2.X))
                return vector1.Y.Equals(vector2.Y);
            return false;
        }

        public override bool Equals(object o)
        {
            if (!(o is XVector))
                return false;
            return Equals(this, (XVector)o);
        }

        public bool Equals(XVector value)
        {
            return Equals(this, value);
        }

        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode();
        }

        public static XVector Parse(string source)
        {
            TokenizerHelper helper = new TokenizerHelper(source, CultureInfo.InvariantCulture);
            string str = helper.NextTokenRequired();
            XVector vector = new XVector(Convert.ToDouble(str, CultureInfo.InvariantCulture), Convert.ToDouble(helper.NextTokenRequired(), CultureInfo.InvariantCulture));
            helper.LastTokenRequired();
            return vector;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        double _x;

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        double _y;

        public override string ToString()
        {
            return ConvertToString(null, null);
        }

        public string ToString(IFormatProvider provider)
        {
            return ConvertToString(null, provider);
        }

        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            return ConvertToString(format, provider);
        }

        internal string ConvertToString(string format, IFormatProvider provider)
        {
            const char numericListSeparator = ',';
            provider = provider ?? CultureInfo.InvariantCulture;
            return string.Format(provider, "{1:" + format + "}{0}{2:" + format + "}", numericListSeparator, _x, _y);
        }

        public double Length
        {
            get { return Math.Sqrt(_x * _x + _y * _y); }
        }

        public double LengthSquared
        {
            get { return _x * _x + _y * _y; }
        }

        public void Normalize()
        {
            this = this / Math.Max(Math.Abs(_x), Math.Abs(_y));
            this = this / Length;
        }

        public static double CrossProduct(XVector vector1, XVector vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }

        public static double AngleBetween(XVector vector1, XVector vector2)
        {
            double y = vector1._x * vector2._y - vector2._x * vector1._y;
            double x = vector1._x * vector2._x + vector1._y * vector2._y;
            return (Math.Atan2(y, x) * 57.295779513082323);
        }

        public static XVector operator -(XVector vector)
        {
            return new XVector(-vector._x, -vector._y);
        }

        public void Negate()
        {
            _x = -_x;
            _y = -_y;
        }

        public static XVector operator +(XVector vector1, XVector vector2)
        {
            return new XVector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        public static XVector Add(XVector vector1, XVector vector2)
        {
            return new XVector(vector1._x + vector2._x, vector1._y + vector2._y);
        }

        public static XVector operator -(XVector vector1, XVector vector2)
        {
            return new XVector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        public static XVector Subtract(XVector vector1, XVector vector2)
        {
            return new XVector(vector1._x - vector2._x, vector1._y - vector2._y);
        }

        public static XPoint operator +(XVector vector, XPoint point)
        {
            return new XPoint(point.X + vector._x, point.Y + vector._y);
        }

        public static XPoint Add(XVector vector, XPoint point)
        {
            return new XPoint(point.X + vector._x, point.Y + vector._y);
        }

        public static XVector operator *(XVector vector, double scalar)
        {
            return new XVector(vector._x * scalar, vector._y * scalar);
        }

        public static XVector Multiply(XVector vector, double scalar)
        {
            return new XVector(vector._x * scalar, vector._y * scalar);
        }

        public static XVector operator *(double scalar, XVector vector)
        {
            return new XVector(vector._x * scalar, vector._y * scalar);
        }

        public static XVector Multiply(double scalar, XVector vector)
        {
            return new XVector(vector._x * scalar, vector._y * scalar);
        }

        public static XVector operator /(XVector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        public static XVector Divide(XVector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }

        public static XVector operator *(XVector vector, XMatrix matrix)
        {
            return matrix.Transform(vector);
        }

        public static XVector Multiply(XVector vector, XMatrix matrix)
        {
            return matrix.Transform(vector);
        }

        public static double operator *(XVector vector1, XVector vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        public static double Multiply(XVector vector1, XVector vector2)
        {
            return vector1._x * vector2._x + vector1._y * vector2._y;
        }

        public static double Determinant(XVector vector1, XVector vector2)
        {
            return vector1._x * vector2._y - vector1._y * vector2._x;
        }

        public static explicit operator XSize(XVector vector)
        {
            return new XSize(Math.Abs(vector._x), Math.Abs(vector._y));
        }

        public static explicit operator XPoint(XVector vector)
        {
            return new XPoint(vector._x, vector._y);
        }

        string DebuggerDisplay
        {
            get
            {
                const string format = Config.SignificantFigures10;
                return string.Format(CultureInfo.InvariantCulture, "vector=({0:" + format + "}, {1:" + format + "})", _x, _y);
            }
        }
    }







}
