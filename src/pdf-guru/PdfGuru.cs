using System;
using System;
using System.Collections.Generic;
using System.Text;
using System;
using System;
using System;


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

}
