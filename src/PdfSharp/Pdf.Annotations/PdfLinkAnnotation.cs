using System;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.Annotations
{
    public sealed class PdfLinkAnnotation : PdfAnnotation
    {
        enum LinkType
        {
            None, Document, Web, File
        }

        public PdfLinkAnnotation()
        {
            _linkType = LinkType.None;
            Elements.SetName(PdfAnnotation.Keys.Subtype, "/Link");
        }

        public PdfLinkAnnotation(PdfDocument document)
            : base(document)
        {
            _linkType = LinkType.None;
            Elements.SetName(PdfAnnotation.Keys.Subtype, "/Link");
        }

        public static PdfLinkAnnotation CreateDocumentLink(PdfRectangle rect, int destinationPage)
        {
            if (destinationPage < 1)
                throw new ArgumentException("Invalid destination page in call to CreateDocumentLink: page number is one-based and must be 1 or higher.", "destinationPage");

            PdfLinkAnnotation link = new PdfLinkAnnotation();
            link._linkType = LinkType.Document;
            link.Rectangle = rect;
            link._destPage = destinationPage;
            return link;
        }
        int _destPage;
        LinkType _linkType;
        string _url;

        public static PdfLinkAnnotation CreateWebLink(PdfRectangle rect, string url)
        {
            PdfLinkAnnotation link = new PdfLinkAnnotation();
            link._linkType = PdfLinkAnnotation.LinkType.Web;
            link.Rectangle = rect;
            link._url = url;
            return link;
        }

        public static PdfLinkAnnotation CreateFileLink(PdfRectangle rect, string fileName)
        {
            PdfLinkAnnotation link = new PdfLinkAnnotation();
            link._linkType = LinkType.File;
            link.Rectangle = rect;
            link._url = fileName;
            return link;
        }

        internal override void WriteObject(PdfWriter writer)
        {
            PdfPage dest = null;
            if (Elements[PdfAnnotation.Keys.BS] == null)
                Elements[PdfAnnotation.Keys.BS] = new PdfLiteral("<</Type/Border/W 0>>");

            if (Elements[PdfAnnotation.Keys.Border] == null)
                Elements[PdfAnnotation.Keys.Border] = new PdfLiteral("[0 0 0]");

            switch (_linkType)
            {
                case LinkType.None:
                    break;

                case LinkType.Document:
                    int destIndex = _destPage;
                    if (destIndex > Owner.PageCount)
                        destIndex = Owner.PageCount;
                    destIndex--;
                    dest = Owner.Pages[destIndex];
                    Elements[Keys.Dest] = new PdfLiteral("[{0} 0 R/XYZ null null 0]", dest.ObjectNumber);
                    break;

                case LinkType.Web:
                    Elements[PdfAnnotation.Keys.A] = new PdfLiteral("<</S/URI/URI{0}>>", 
                        PdfEncoders.ToStringLiteral(_url, PdfStringEncoding.WinAnsiEncoding, writer.SecurityHandler));
                    break;

                case LinkType.File:
                    Elements[PdfAnnotation.Keys.A] = new PdfLiteral("<</Type/Action/S/Launch/F<</Type/Filespec/F{0}>> >>",
                        PdfEncoders.ToStringLiteral(_url, PdfStringEncoding.WinAnsiEncoding, writer.SecurityHandler));
                    break;
            }
            base.WriteObject(writer);
        }

        internal new class Keys : PdfAnnotation.Keys
        {
            [KeyInfo(KeyType.ArrayOrNameOrString | KeyType.Optional)]
            public const string Dest = "/Dest";

            [KeyInfo("1.2", KeyType.Name | KeyType.Optional)]
            public const string H = "/H";

            [KeyInfo("1.3", KeyType.Dictionary | KeyType.Optional)]
            public const string PA = "/PA";

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
