namespace PdfSharp.Pdf
{
    public sealed class PdfViewerPreferences : PdfDictionary
    {
        internal PdfViewerPreferences(PdfDocument document)
            : base(document)
        { }

        PdfViewerPreferences(PdfDictionary dict)
            : base(dict)
        { }

        public bool HideToolbar
        {
            get { return Elements.GetBoolean(Keys.HideToolbar); }
            set { Elements.SetBoolean(Keys.HideToolbar, value); }
        }

        public bool HideMenubar
        {
            get { return Elements.GetBoolean(Keys.HideMenubar); }
            set { Elements.SetBoolean(Keys.HideMenubar, value); }
        }

        public bool HideWindowUI
        {
            get { return Elements.GetBoolean(Keys.HideWindowUI); }
            set { Elements.SetBoolean(Keys.HideWindowUI, value); }
        }

        public bool FitWindow
        {
            get { return Elements.GetBoolean(Keys.FitWindow); }
            set { Elements.SetBoolean(Keys.FitWindow, value); }
        }

        public bool CenterWindow
        {
            get { return Elements.GetBoolean(Keys.CenterWindow); }
            set { Elements.SetBoolean(Keys.CenterWindow, value); }
        }

        public bool DisplayDocTitle
        {
            get { return Elements.GetBoolean(Keys.DisplayDocTitle); }
            set { Elements.SetBoolean(Keys.DisplayDocTitle, value); }
        }

        public PdfReadingDirection? Direction
        {
            get
            {
                switch (Elements.GetName(Keys.Direction))
                {
                    case "L2R":
                        return PdfReadingDirection.LeftToRight;

                    case "R2L":
                        return PdfReadingDirection.RightToLeft;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    switch (value.Value)
                    {
                        case PdfReadingDirection.RightToLeft:
                            Elements.SetName(Keys.Direction, "R2L");
                            break;

                        default:
                            Elements.SetName(Keys.Direction, "L2R");
                            break;
                    }
                }
                else
                    Elements.Remove(Keys.Direction);
            }
        }

        internal sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string HideToolbar = "/HideToolbar";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string HideMenubar = "/HideMenubar";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string HideWindowUI = "/HideWindowUI";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string FitWindow = "/FitWindow";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string CenterWindow = "/CenterWindow";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string DisplayDocTitle = "/DisplayDocTitle";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string NonFullScreenPageMode = "/NonFullScreenPageMode";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Direction = "/Direction";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string ViewArea = "/ViewArea";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string ViewClip = "/ViewClip";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string PrintArea = "/PrintArea";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string PrintClip = "/PrintClip";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string PrintScaling = "/PrintScaling";

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