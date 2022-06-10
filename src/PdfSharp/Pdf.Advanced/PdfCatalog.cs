using System;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.AcroForms;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfCatalog : PdfDictionary
    {
        public PdfCatalog(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Catalog");

            _version = "1.4";     
        }

        internal PdfCatalog(PdfDictionary dictionary)
            : base(dictionary)
        { }

        public string Version
        {
            get { return _version; }
            set
            {
                switch (value)
                {
                    case "1.0":
                    case "1.1":
                    case "1.2":
                        throw new InvalidOperationException("Unsupported PDF version.");

                    case "1.3":
                    case "1.4":
                        _version = value;
                        break;

                    case "1.5":
                    case "1.6":
                        throw new InvalidOperationException("Unsupported PDF version.");

                    default:
                        throw new ArgumentException("Invalid version.");
                }
            }
        }
        string _version = "1.3";

        public PdfPages Pages
        {
            get
            {
                if (_pages == null)
                {
                    _pages = (PdfPages)Elements.GetValue(Keys.Pages, VCF.CreateIndirect);
                    if (Owner.IsImported)
                        _pages.FlattenPageTree();
                }
                return _pages;
            }
        }
        PdfPages _pages;

        internal PdfPageLayout PageLayout
        {
            get { return (PdfPageLayout)Elements.GetEnumFromName(Keys.PageLayout, PdfPageLayout.SinglePage); }
            set { Elements.SetEnumAsName(Keys.PageLayout, value); }
        }

        internal PdfPageMode PageMode
        {
            get { return (PdfPageMode)Elements.GetEnumFromName(Keys.PageMode, PdfPageMode.UseNone); }
            set { Elements.SetEnumAsName(Keys.PageMode, value); }
        }

        internal PdfViewerPreferences ViewerPreferences
        {
            get
            {
                if (_viewerPreferences == null)
                    _viewerPreferences = (PdfViewerPreferences)Elements.GetValue(Keys.ViewerPreferences, VCF.CreateIndirect);
                return _viewerPreferences;
            }
        }
        PdfViewerPreferences _viewerPreferences;

        internal PdfOutlineCollection Outlines
        {
            get
            {
               if (_outline == null)
                {
                    _outline = (PdfOutline)Elements.GetValue(Keys.Outlines, VCF.CreateIndirect);
                }
               return _outline.Outlines;
            }
        }
        PdfOutline _outline;

        public PdfAcroForm AcroForm
        {
            get
            {
                if (_acroForm == null)
                    _acroForm = (PdfAcroForm)Elements.GetValue(Keys.AcroForm);
                return _acroForm;
            }
        }
        PdfAcroForm _acroForm;

        public string Language
        {
            get { return Elements.GetString(Keys.Lang); }
            set
            {
                if (value == null)
                    Elements.Remove(Keys.Lang);
                else
                    Elements.SetString(Keys.Lang, value);
            }
        }

        internal override void PrepareForSave()
        {
            if (_pages != null)
                _pages.PrepareForSave();

            if (_outline != null && _outline.Outlines.Count > 0)
            {
                if (Elements[Keys.PageMode] == null)
                    PageMode = PdfPageMode.UseOutlines;
                _outline.PrepareForSave();
            }
        }

        internal override void WriteObject(PdfWriter writer)
        {
            if (_outline != null && _outline.Outlines.Count > 0)
            {
                if (Elements[Keys.PageMode] == null)
                    PageMode = PdfPageMode.UseOutlines;
            }
            base.WriteObject(writer);
        }

        internal sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Catalog")]
            public const string Type = "/Type";

            [KeyInfo("1.4", KeyType.Name | KeyType.Optional)]
            public const string Version = "/Version";

            [KeyInfo(KeyType.Dictionary | KeyType.Required | KeyType.MustBeIndirect, typeof(PdfPages))]
            public const string Pages = "/Pages";

            [KeyInfo("1.3", KeyType.NumberTree | KeyType.Optional)]
            public const string PageLabels = "/PageLabels";

            [KeyInfo("1.2", KeyType.Dictionary | KeyType.Optional)]
            public const string Names = "/Names";

            [KeyInfo("1.1", KeyType.Dictionary | KeyType.Optional)]
            public const string Dests = "/Dests";

            [KeyInfo("1.2", KeyType.Dictionary | KeyType.Optional, typeof(PdfViewerPreferences))]
            public const string ViewerPreferences = "/ViewerPreferences";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string PageLayout = "/PageLayout";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string PageMode = "/PageMode";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfOutline))]
            public const string Outlines = "/Outlines";

            [KeyInfo("1.1", KeyType.Array | KeyType.Optional)]
            public const string Threads = "/Threads";

            [KeyInfo("1.1", KeyType.ArrayOrDictionary | KeyType.Optional)]
            public const string OpenAction = "/OpenAction";

            [KeyInfo("1.4", KeyType.Dictionary | KeyType.Optional)]
            public const string AA = "/AA";

            [KeyInfo("1.1", KeyType.Dictionary | KeyType.Optional)]
            public const string URI = "/URI";

            [KeyInfo("1.2", KeyType.Dictionary | KeyType.Optional, typeof(PdfAcroForm))]
            public const string AcroForm = "/AcroForm";

            [KeyInfo("1.4", KeyType.Dictionary | KeyType.Optional | KeyType.MustBeIndirect)]
            public const string Metadata = "/Metadata";

            [KeyInfo("1.3", KeyType.Dictionary | KeyType.Optional)]
            public const string StructTreeRoot = "/StructTreeRoot";

            [KeyInfo("1.4", KeyType.Dictionary | KeyType.Optional)]
            public const string MarkInfo = "/MarkInfo";

            [KeyInfo("1.4", KeyType.String | KeyType.Optional)]
            public const string Lang = "/Lang";

            [KeyInfo("1.3", KeyType.Dictionary | KeyType.Optional)]
            public const string SpiderInfo = "/SpiderInfo";

            [KeyInfo("1.4", KeyType.Array | KeyType.Optional)]
            public const string OutputIntents = "/OutputIntents";

            [KeyInfo("1.4", KeyType.Dictionary | KeyType.Optional)]
            public const string PieceInfo = "/PieceInfo";

            [KeyInfo("1.5", KeyType.Dictionary | KeyType.Optional)]
            public const string OCProperties = "/OCProperties";

            [KeyInfo("1.5", KeyType.Dictionary | KeyType.Optional)]
            public const string Perms = "/Perms";

            [KeyInfo("1.5", KeyType.Dictionary | KeyType.Optional)]
            public const string Legal = "/Legal";

            [KeyInfo("1.7", KeyType.Array | KeyType.Optional)]
            public const string Requirements = "/Requirements";

            [KeyInfo("1.7", KeyType.Dictionary | KeyType.Optional)]
            public const string Collection = "/Collection";

            [KeyInfo("1.7", KeyType.Boolean | KeyType.Optional)]
            public const string NeedsRendering = "/NeedsRendering";

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
