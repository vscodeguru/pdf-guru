using System;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Annotations;

namespace PdfSharp.Pdf
{
    public sealed class PdfPage : PdfDictionary, IContentStream
    {
        public PdfPage()
        {
            Elements.SetName(Keys.Type, "/Page");
            Initialize();
        }

        public PdfPage(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Page");
            Elements[Keys.Parent] = document.Pages.Reference;
            Initialize();
        }

        internal PdfPage(PdfDictionary dict)
            : base(dict)
        {
            int rotate = Elements.GetInteger(InheritablePageKeys.Rotate);
            if (Math.Abs((rotate / 90)) % 2 == 1)
            {
#if true
                _orientation = PageOrientation.Landscape;
                _orientationSetByCodeForRotatedDocument = true;
#endif
            }
        }

        void Initialize()
        {
            Size = RegionInfo.CurrentRegion.IsMetric ? PageSize.A4 : PageSize.Letter;
            PdfRectangle rect = MediaBox;
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        object _tag;

        public void Close()
        {
            _closed = true;
        }
        bool _closed;

        internal bool IsClosed
        {
            get { return _closed; }
        }

        internal override PdfDocument Document
        {
            set
            {
                if (!ReferenceEquals(_document, value))
                {
                    if (_document != null)
                        throw new InvalidOperationException("Cannot change document.");
                    _document = value;
                    if (Reference != null)
                        Reference.Document = value;
                    Elements[Keys.Parent] = _document.Pages.Reference;
                }
            }
        }

        public PageOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                _orientationSetByCodeForRotatedDocument = false;
            }
        }
        PageOrientation _orientation;
        bool _orientationSetByCodeForRotatedDocument;
        public PageSize Size
        {
            get { return _pageSize; }
            set
            {
                if (!Enum.IsDefined(typeof(PageSize), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(PageSize));

                XSize size = PageSizeConverter.ToSize(value);
                MediaBox = new PdfRectangle(0, 0, size.Width, size.Height);
                _pageSize = value;
            }
        }
        PageSize _pageSize;

        public TrimMargins TrimMargins
        {
            get
            {
                if (_trimMargins == null)
                    _trimMargins = new TrimMargins();
                return _trimMargins;
            }
            set
            {
                if (_trimMargins == null)
                    _trimMargins = new TrimMargins();
                if (value != null)
                {
                    _trimMargins.Left = value.Left;
                    _trimMargins.Right = value.Right;
                    _trimMargins.Top = value.Top;
                    _trimMargins.Bottom = value.Bottom;
                }
                else
                    _trimMargins.All = 0;
            }
        }
        TrimMargins _trimMargins = new TrimMargins();

        public PdfRectangle MediaBox
        {
            get { return Elements.GetRectangle(Keys.MediaBox, true); }
            set { Elements.SetRectangle(Keys.MediaBox, value); }
        }

        public PdfRectangle CropBox
        {
            get { return Elements.GetRectangle(Keys.CropBox, true); }
            set { Elements.SetRectangle(Keys.CropBox, value); }
        }

        public PdfRectangle BleedBox
        {
            get { return Elements.GetRectangle(Keys.BleedBox, true); }
            set { Elements.SetRectangle(Keys.BleedBox, value); }
        }

        public PdfRectangle ArtBox
        {
            get { return Elements.GetRectangle(Keys.ArtBox, true); }
            set { Elements.SetRectangle(Keys.ArtBox, value); }
        }

        public PdfRectangle TrimBox
        {
            get { return Elements.GetRectangle(Keys.TrimBox, true); }
            set { Elements.SetRectangle(Keys.TrimBox, value); }
        }

        public XUnit Height
        {
            get
            {
                PdfRectangle rect = MediaBox;
                return _orientation == PageOrientation.Portrait ? rect.Height : rect.Width;
            }
            set
            {
                PdfRectangle rect = MediaBox;
                if (_orientation == PageOrientation.Portrait)
                    MediaBox = new PdfRectangle(rect.X1, 0, rect.X2, value);
                else
                    MediaBox = new PdfRectangle(0, rect.Y1, value, rect.Y2);
                _pageSize = PageSize.Undefined;
            }
        }

        public XUnit Width
        {
            get
            {
                PdfRectangle rect = MediaBox;
                return _orientation == PageOrientation.Portrait ? rect.Width : rect.Height;
            }
            set
            {
                PdfRectangle rect = MediaBox;
                if (_orientation == PageOrientation.Portrait)
                    MediaBox = new PdfRectangle(0, rect.Y1, value, rect.Y2);
                else
                    MediaBox = new PdfRectangle(rect.X1, 0, rect.X2, value);
                _pageSize = PageSize.Undefined;
            }
        }

        public int Rotate
        {
            get { return _elements.GetInteger(InheritablePageKeys.Rotate); }
            set
            {
                if (value % 90 != 0)
                    throw new ArgumentException("Value must be a multiple of 90.");
                _elements.SetInteger(InheritablePageKeys.Rotate, value);
            }
        }

        internal PdfContent RenderContent;

        public PdfContents Contents
        {
            get
            {
                if (_contents == null)
                {
                    if (true)   
                    {
                        PdfItem item = Elements[Keys.Contents];
                        if (item == null)
                        {
                            _contents = new PdfContents(Owner);
                        }
                        else
                        {
                            if (item is PdfReference)
                                item = ((PdfReference)item).Value;

                            PdfArray array = item as PdfArray;
                            if (array != null)
                            {
                                if (array.IsIndirect)
                                {
                                    array = array.Clone();
                                    array.Document = Owner;
                                }
                                _contents = new PdfContents(array);
                            }
                            else
                            {
                                _contents = new PdfContents(Owner);
                                PdfContent content = new PdfContent((PdfDictionary)item);
                                _contents.Elements.Add(content.Reference);
                            }
                        }
                    }
                    Debug.Assert(_contents.Reference == null);
                    Elements[Keys.Contents] = _contents;
                }
                return _contents;
            }
        }
        PdfContents _contents;

        public bool HasAnnotations
        {
            get
            {
                if (_annotations == null)
                {
                    _annotations = (PdfAnnotations)Elements.GetValue(Keys.Annots);
                    _annotations.Page = this;
                }
                return _annotations != null;
            }
        }

        public PdfAnnotations Annotations
        {
            get
            {
                if (_annotations == null)
                {
                    _annotations = (PdfAnnotations)Elements.GetValue(Keys.Annots, VCF.Create);
                    _annotations.Page = this;
                }
                return _annotations;
            }
        }
        PdfAnnotations _annotations;

        public PdfLinkAnnotation AddDocumentLink(PdfRectangle rect, int destinationPage)
        {
            PdfLinkAnnotation annotation = PdfLinkAnnotation.CreateDocumentLink(rect, destinationPage);
            Annotations.Add(annotation);
            return annotation;
        }

        public PdfLinkAnnotation AddWebLink(PdfRectangle rect, string url)
        {
            PdfLinkAnnotation annotation = PdfLinkAnnotation.CreateWebLink(rect, url);
            Annotations.Add(annotation);
            return annotation;
        }

        public PdfLinkAnnotation AddFileLink(PdfRectangle rect, string fileName)
        {
            PdfLinkAnnotation annotation = PdfLinkAnnotation.CreateFileLink(rect, fileName);
            Annotations.Add(annotation);
            return annotation;
        }

        public PdfCustomValues CustomValues
        {
            get
            {
                if (_customValues == null)
                    _customValues = PdfCustomValues.Get(Elements);
                return _customValues;
            }
            set
            {
                if (value != null)
                    throw new ArgumentException("Only null is allowed to clear all custom values.");
                PdfCustomValues.Remove(Elements);
                _customValues = null;
            }
        }
        PdfCustomValues _customValues;

        public PdfResources Resources
        {
            get
            {
                if (_resources == null)
                    _resources = (PdfResources)Elements.GetValue(Keys.Resources, VCF.Create); 
                return _resources;
            }
        }
        PdfResources _resources;

        PdfResources IContentStream.Resources
        {
            get { return Resources; }
        }

        internal string GetFontName(XFont font, out PdfFont pdfFont)
        {
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
            pdfFont = _document.FontTable.TryGetFont(idName);
            string name = null;
            if (pdfFont != null)
                name = Resources.AddFont(pdfFont);
            return name;
        }

        internal string GetFontName(string idName, byte[] fontData, out PdfFont pdfFont)
        {
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
            PdfImage pdfImage = _document.ImageTable.GetImage(image);
            Debug.Assert(pdfImage != null);
            string name = Resources.AddImage(pdfImage);
            return name;
        }

        string IContentStream.GetImageName(XImage image)
        {
            return GetImageName(image);
        }

        internal string GetFormName(XForm form)
        {
            PdfFormXObject pdfForm = _document.FormTable.GetForm(form);
            Debug.Assert(pdfForm != null);
            string name = Resources.AddForm(pdfForm);
            return name;
        }

        string IContentStream.GetFormName(XForm form)
        {
            return GetFormName(form);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            PdfRectangle mediaBox = MediaBox;
            if (_orientation == PageOrientation.Landscape && !_orientationSetByCodeForRotatedDocument)
                MediaBox = new PdfRectangle(mediaBox.X1, mediaBox.Y1, mediaBox.Y2, mediaBox.X2);

#if true
            TransparencyUsed = true;    
            if (TransparencyUsed && !Elements.ContainsKey(Keys.Group) &&
                _document.Options.ColorMode != PdfColorMode.Undefined)
            {
                PdfDictionary group = new PdfDictionary();
                _elements["/Group"] = group;
                if (_document.Options.ColorMode != PdfColorMode.Cmyk)
                    group.Elements.SetName("/CS", "/DeviceRGB");
                else
                    group.Elements.SetName("/CS", "/DeviceCMYK");
                group.Elements.SetName("/S", "/Transparency");
            }
#endif

            base.WriteObject(writer);

            if (_orientation == PageOrientation.Landscape && !_orientationSetByCodeForRotatedDocument)
                MediaBox = mediaBox;
        }

        internal bool TransparencyUsed;

        internal static void InheritValues(PdfDictionary page, InheritedValues values)
        {
            if (values.Resources != null)
            {
                PdfDictionary resources;
                PdfItem res = page.Elements[InheritablePageKeys.Resources];
                if (res is PdfReference)
                {
                    resources = (PdfDictionary)((PdfReference)res).Value.Clone();
                    resources.Document = page.Owner;
                }
                else
                    resources = (PdfDictionary)res;

                if (resources == null)
                {
                    resources = values.Resources.Clone();
                    resources.Document = page.Owner;
                    page.Elements.Add(InheritablePageKeys.Resources, resources);
                }
                else
                {
                    foreach (PdfName name in values.Resources.Elements.KeyNames)
                    {
                        if (!resources.Elements.ContainsKey(name.Value))
                        {
                            PdfItem item = values.Resources.Elements[name];
                            if (item is PdfObject)
                                item = item.Clone();
                            resources.Elements.Add(name.ToString(), item);
                        }
                    }
                }
            }

            if (values.MediaBox != null && page.Elements[InheritablePageKeys.MediaBox] == null)
                page.Elements[InheritablePageKeys.MediaBox] = values.MediaBox;

            if (values.CropBox != null && page.Elements[InheritablePageKeys.CropBox] == null)
                page.Elements[InheritablePageKeys.CropBox] = values.CropBox;

            if (values.Rotate != null && page.Elements[InheritablePageKeys.Rotate] == null)
                page.Elements[InheritablePageKeys.Rotate] = values.Rotate;
        }

        internal static void InheritValues(PdfDictionary page, ref InheritedValues values)
        {
            PdfItem item = page.Elements[InheritablePageKeys.Resources];
            if (item != null)
            {
                PdfReference reference = item as PdfReference;
                if (reference != null)
                    values.Resources = (PdfDictionary)(reference.Value);
                else
                    values.Resources = (PdfDictionary)item;
            }

            item = page.Elements[InheritablePageKeys.MediaBox];
            if (item != null)
                values.MediaBox = new PdfRectangle(item);

            item = page.Elements[InheritablePageKeys.CropBox];
            if (item != null)
                values.CropBox = new PdfRectangle(item);

            item = page.Elements[InheritablePageKeys.Rotate];
            if (item != null)
            {
                if (item is PdfReference)
                    item = ((PdfReference)item).Value;
                values.Rotate = (PdfInteger)item;
            }
        }

        internal override void PrepareForSave()
        {
            if (_trimMargins.AreSet)
            {
                double width = _trimMargins.Left.Point + Width.Point + _trimMargins.Right.Point;
                double height = _trimMargins.Top.Point + Height.Point + _trimMargins.Bottom.Point;

                MediaBox = new PdfRectangle(0, 0, width, height);
                CropBox = new PdfRectangle(0, 0, width, height);
                BleedBox = new PdfRectangle(0, 0, width, height);

                PdfRectangle rect = new PdfRectangle(_trimMargins.Left.Point, _trimMargins.Top.Point,
                  width - _trimMargins.Right.Point, height - _trimMargins.Bottom.Point);
                TrimBox = rect;
                ArtBox = rect.Clone();
            }
        }

        internal sealed class Keys : InheritablePageKeys
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Page")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Dictionary | KeyType.Required | KeyType.MustBeIndirect)]
            public const string Parent = "/Parent";

            [KeyInfo(KeyType.Date)]
            public const string LastModified = "/LastModified";

            [KeyInfo("1.3", KeyType.Rectangle | KeyType.Optional)]
            public const string BleedBox = "/BleedBox";

            [KeyInfo("1.3", KeyType.Rectangle | KeyType.Optional)]
            public const string TrimBox = "/TrimBox";

            [KeyInfo("1.3", KeyType.Rectangle | KeyType.Optional)]
            public const string ArtBox = "/ArtBox";

            [KeyInfo("1.4", KeyType.Dictionary | KeyType.Optional)]
            public const string BoxColorInfo = "/BoxColorInfo";

            [KeyInfo(KeyType.Array | KeyType.Stream | KeyType.Optional)]
            public const string Contents = "/Contents";

            [KeyInfo("1.4", KeyType.Dictionary | KeyType.Optional)]
            public const string Group = "/Group";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string Thumb = "/Thumb";

            [KeyInfo("1.1", KeyType.Array | KeyType.Optional)]
            public const string B = "/B";

            [KeyInfo("1.1", KeyType.Real | KeyType.Optional)]
            public const string Dur = "/Dur";

            [KeyInfo("1.1", KeyType.Dictionary | KeyType.Optional)]
            public const string Trans = "/Trans";

            [KeyInfo(KeyType.Array | KeyType.Optional, typeof(PdfAnnotations))]
            public const string Annots = "/Annots";

            [KeyInfo("1.2", KeyType.Dictionary | KeyType.Optional)]
            public const string AA = "/AA";

            [KeyInfo("1.4", KeyType.Stream | KeyType.Optional)]
            public const string Metadata = "/Metadata";

            [KeyInfo("1.3", KeyType.Dictionary | KeyType.Optional)]
            public const string PieceInfo = "/PieceInfo";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string StructParents = "/StructParents";

            [KeyInfo("1.3", KeyType.String | KeyType.Optional)]
            public const string ID = "/ID";

            [KeyInfo("1.3", KeyType.Real | KeyType.Optional)]
            public const string PZ = "/PZ";

            [KeyInfo("1.3", KeyType.Dictionary | KeyType.Optional)]
            public const string SeparationInfo = "/SeparationInfo";

            [KeyInfo("1.5", KeyType.Name | KeyType.Optional)]
            public const string Tabs = "/Tabs";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string TemplateInstantiated = "/TemplateInstantiated";

            [KeyInfo("1.5", KeyType.Dictionary | KeyType.Optional)]
            public const string PresSteps = "/PresSteps";

            [KeyInfo("1.6", KeyType.Real | KeyType.Optional)]
            public const string UserUnit = "/UserUnit";

            [KeyInfo("1.6", KeyType.Dictionary | KeyType.Optional)]
            public const string VP = "/VP";

            internal static DictionaryMeta Meta
            {
                get { return _meta ?? (_meta = CreateMeta(typeof(Keys))); }
            }
            static DictionaryMeta _meta;
        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }

        internal class InheritablePageKeys : KeysBase
        {
            [KeyInfo(KeyType.Dictionary | KeyType.Required | KeyType.Inheritable, typeof(PdfResources))]
            public const string Resources = "/Resources";

            [KeyInfo(KeyType.Rectangle | KeyType.Required | KeyType.Inheritable)]
            public const string MediaBox = "/MediaBox";

            [KeyInfo(KeyType.Rectangle | KeyType.Optional | KeyType.Inheritable)]
            public const string CropBox = "/CropBox";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string Rotate = "/Rotate";
        }

        internal struct InheritedValues
        {
            public PdfDictionary Resources;
            public PdfRectangle MediaBox;
            public PdfRectangle CropBox;
            public PdfInteger Rotate;
        }
    }
}
