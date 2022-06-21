using System;
using System.Diagnostics;
using System.Globalization;
using PdfSharp.Drawing;
using PdfSharp.Pdf.Actions;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf
{
    public sealed class PdfOutline : PdfDictionary
    {
        public PdfOutline()
        {
        }

        internal PdfOutline(PdfDocument document)
            : base(document)
        {
        }

        public PdfOutline(PdfDictionary dict)
            : base(dict)
        {
            Initialize();
        }

        public PdfOutline(string title, PdfPage destinationPage, bool opened, PdfOutlineStyle style, XColor textColor)
        {
            Title = title;
            DestinationPage = destinationPage;
            Opened = opened;
            Style = style;
            TextColor = textColor;
        }

        public PdfOutline(string title, PdfPage destinationPage, bool opened, PdfOutlineStyle style)
        {
            Title = title;
            DestinationPage = destinationPage;
            Opened = opened;
            Style = style;
        }

        public PdfOutline(string title, PdfPage destinationPage, bool opened)
        {
            Title = title;
            DestinationPage = destinationPage;
            Opened = opened;
        }

        public PdfOutline(string title, PdfPage destinationPage)
        {
            Title = title;
            DestinationPage = destinationPage;
        }

        internal int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        int _count;

        internal int OpenCount;

        internal int CountOpen()
        {
            int count = _opened ? 1 : 0;
            if (_outlines != null)
                count += _outlines.CountOpen();
            return count;
        }

        public PdfOutline Parent
        {
            get { return _parent; }
            internal set { _parent = value; }
        }
        PdfOutline _parent;

        public string Title
        {
            get { return Elements.GetString(Keys.Title); }
            set
            {
                PdfString s = new PdfString(value, PdfStringEncoding.Unicode);
                Elements.SetValue(Keys.Title, s);
            }
        }

        public PdfPage DestinationPage
        {
            get { return _destinationPage; }
            set { _destinationPage = value; }
        }
        PdfPage _destinationPage;

        public double? Left
        {
            get { return _left; }
            set { _left = value; }
        }
        double? _left = null;

        public double? Top
        {
            get { return _top; }
            set { _top = value; }
        }
        double? _top = null;

        public double Right         
        {
            get { return _right; }
            set { _right = value; }
        }
        double _right = double.NaN;

        public double Bottom         
        {
            get { return _bottom; }
            set { _bottom = value; }
        }
        double _bottom = double.NaN;

        public double? Zoom
        {
            get { return _zoom; }
            set
            {
                if (value.HasValue && value.Value == 0)
                    _zoom = null;
                else
                    _zoom = value;
            }
        }
        double? _zoom;       

        public bool Opened
        {
            get { return _opened; }
            set { _opened = value; }

        }
        bool _opened;

        public PdfOutlineStyle Style
        {
            get { return (PdfOutlineStyle)Elements.GetInteger(Keys.F); }
            set { Elements.SetInteger(Keys.F, (int)value); }
        }

        public PdfPageDestinationType PageDestinationType
        {
            get { return _pageDestinationType; }
            set { _pageDestinationType = value; }
        }
        PdfPageDestinationType _pageDestinationType = PdfPageDestinationType.Xyz;

        public XColor TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }
        XColor _textColor;

        public bool HasChildren
        {
            get { return _outlines != null && _outlines.Count > 0; }
        }

        public PdfOutlineCollection Outlines
        {
            get { return _outlines ?? (_outlines = new PdfOutlineCollection(Owner, this)); }
        }
        PdfOutlineCollection _outlines;

        void Initialize()
        {
            string title;
            if (Elements.TryGetString(Keys.Title, out title))
                Title = title;

            PdfReference parentRef = Elements.GetReference(Keys.Parent);
            if (parentRef != null)
            {
                PdfOutline parent = parentRef.Value as PdfOutline;
                if (parent != null)
                    Parent = parent;
            }

            Count = Elements.GetInteger(Keys.Count);

            PdfArray colors = Elements.GetArray(Keys.C);
            if (colors != null && colors.Elements.Count == 3)
            {
                double r = colors.Elements.GetReal(0);
                double g = colors.Elements.GetReal(1);
                double b = colors.Elements.GetReal(2);
                TextColor = XColor.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
            }

            PdfItem dest = Elements.GetValue(Keys.Dest);
            PdfItem a = Elements.GetValue(Keys.A);
            Debug.Assert(dest == null || a == null, "Either destination or goto action.");

            PdfArray destArray = null;
            if (dest != null)
            {
                destArray = dest as PdfArray;
                if (destArray != null)
                {
                    SplitDestinationPage(destArray);
                }
                else
                {
                    Debug.Assert(false, "See what to do when this happened.");
                }
            }
            else if (a != null)
            {
                PdfDictionary action = a as PdfDictionary;
                if (action != null && action.Elements.GetName(PdfAction.Keys.S) == "/GoTo")
                {
                    dest = action.Elements[PdfGoToAction.Keys.D];
                    destArray = dest as PdfArray;
                    if (destArray != null)
                    {
                        Elements.Remove(Keys.A);
                        Elements.Add(Keys.Dest, destArray);
                        SplitDestinationPage(destArray);
                    }
                    else
                    {
                        throw new Exception("Destination Array expected.");
                    }
                }
                else
                {
                    Debug.Assert(false, "See what to do when this happened.");
                }
            }
            else
            {
            }

            InitializeChildren();
        }

        void SplitDestinationPage(PdfArray destination)         
        {

            PdfDictionary destPage = (PdfDictionary)((PdfReference)destination.Elements[0]).Value;
            PdfPage page = destPage as PdfPage;
            if (page == null)
                page = new PdfPage(destPage);

            DestinationPage = page;
            PdfName type = destination.Elements[1] as PdfName;
            if (type != null)
            {
                PageDestinationType = (PdfPageDestinationType)Enum.Parse(typeof(PdfPageDestinationType), type.Value.Substring(1), true);
                switch (PageDestinationType)
                {
                    case PdfPageDestinationType.Xyz:
                        Left = destination.Elements.GetNullableReal(2);
                        Top = destination.Elements.GetNullableReal(3);
                        Zoom = destination.Elements.GetNullableReal(4);           
                        break;

                    case PdfPageDestinationType.Fit:
                        break;

                    case PdfPageDestinationType.FitH:
                        Top = destination.Elements.GetNullableReal(2);
                        break;

                    case PdfPageDestinationType.FitV:
                        Left = destination.Elements.GetNullableReal(2);
                        break;

                    case PdfPageDestinationType.FitR:
                        Left = destination.Elements.GetReal(2);
                        Bottom = destination.Elements.GetReal(3);
                        Right = destination.Elements.GetReal(4);
                        Top = destination.Elements.GetReal(5);
                        break;

                    case PdfPageDestinationType.FitB:
                        break;

                    case PdfPageDestinationType.FitBH:
                        Top = destination.Elements.GetReal(2);
                        break;

                    case PdfPageDestinationType.FitBV:
                        Left = destination.Elements.GetReal(2);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

        }

        void InitializeChildren()
        {
            PdfReference firstRef = Elements.GetReference(Keys.First);
            PdfReference lastRef = Elements.GetReference(Keys.Last);
            PdfReference current = firstRef;
            while (current != null)
            {
                PdfOutline item = new PdfOutline((PdfDictionary)current.Value);
                Outlines.Add(item);

                current = item.Elements.GetReference(Keys.Next);
            }
        }

        internal override void PrepareForSave()
        {
            bool hasKids = HasChildren;
            if (_parent != null || hasKids)
            {
                if (_parent == null)
                {
                    Debug.Assert(_outlines != null && _outlines.Count > 0 && _outlines[0] != null);
                    Elements[Keys.First] = _outlines[0].Reference;
                    Elements[Keys.Last] = _outlines[_outlines.Count - 1].Reference;

                    if (OpenCount > 0)
                        Elements[Keys.Count] = new PdfInteger(OpenCount);
                }
                else
                {
                    Elements[Keys.Parent] = _parent.Reference;

                    int count = _parent._outlines.Count;
                    int index = _parent._outlines.IndexOf(this);
                    Debug.Assert(index != -1);

                    if (DestinationPage != null)
                        Elements[Keys.Dest] = CreateDestArray();

                    if (index > 0)
                        Elements[Keys.Prev] = _parent._outlines[index - 1].Reference;

                    if (index < count - 1)
                        Elements[Keys.Next] = _parent._outlines[index + 1].Reference;

                    if (hasKids)
                    {
                        Elements[Keys.First] = _outlines[0].Reference;
                        Elements[Keys.Last] = _outlines[_outlines.Count - 1].Reference;
                    }
                    if (OpenCount > 0)
                        Elements[Keys.Count] = new PdfInteger((_opened ? 1 : -1) * OpenCount);

                    if (_textColor != XColor.Empty && Owner.HasVersion("1.4"))
                        Elements[Keys.C] = new PdfLiteral("[{0}]", PdfEncoders.ToString(_textColor, PdfColorMode.Rgb));

                }

                if (hasKids)
                {
                    foreach (PdfOutline outline in _outlines)
                        outline.PrepareForSave();
                }
            }
        }

        PdfArray CreateDestArray()
        {
            PdfArray dest = null;
            switch (PageDestinationType)
            {
                case PdfPageDestinationType.Xyz:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral(String.Format("/XYZ {0} {1} {2}", Fd(Left), Fd(Top), Fd(Zoom))));
                    break;

                case PdfPageDestinationType.Fit:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral("/Fit"));
                    break;

                case PdfPageDestinationType.FitH:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral(String.Format("/FitH {0}", Fd(Top))));
                    break;

                case PdfPageDestinationType.FitV:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral(String.Format("/FitV {0}", Fd(Left))));
                    break;

                case PdfPageDestinationType.FitR:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral(String.Format("/FitR {0} {1} {2} {3}", Fd(Left), Fd(Bottom), Fd(Right), Fd(Top))));
                    break;

                case PdfPageDestinationType.FitB:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral("/FitB"));
                    break;

                case PdfPageDestinationType.FitBH:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral(String.Format("/FitBH {0}", Fd(Top))));
                    break;

                case PdfPageDestinationType.FitBV:
                    dest = new PdfArray(Owner,
                        DestinationPage.Reference, new PdfLiteral(String.Format("/FitBV {0}", Fd(Left))));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return dest;
        }

        string Fd(double value)
        {
            if (Double.IsNaN(value))
                throw new InvalidOperationException("Value is not a valid Double.");
            return value.ToString("#.##", CultureInfo.InvariantCulture);

        }

        string Fd(double? value)
        {
            return value.HasValue ? value.Value.ToString("#.##", CultureInfo.InvariantCulture) : "null";
        }

        internal override void WriteObject(PdfWriter writer)
        {
            bool hasKids = HasChildren;
            if (_parent != null || hasKids)
            {
                base.WriteObject(writer);
            }
        }


        internal sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Optional, FixedValue = "Outlines")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.String | KeyType.Required)]
            public const string Title = "/Title";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string Parent = "/Parent";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string Prev = "/Prev";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string Next = "/Next";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string First = "/First";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string Last = "/Last";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string Count = "/Count";

            [KeyInfo(KeyType.ArrayOrNameOrString | KeyType.Optional)]
            public const string Dest = "/Dest";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string A = "/A";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string SE = "/SE";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string C = "/C";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string F = "/F";

            public static DictionaryMeta Meta
            {
                get { return _meta ?? (_meta = CreateMeta(typeof(Keys))); }
            }
            static DictionaryMeta _meta;

        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
