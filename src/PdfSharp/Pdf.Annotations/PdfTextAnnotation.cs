using System;

namespace PdfSharp.Pdf.Annotations
{
    public sealed class PdfTextAnnotation : PdfAnnotation
    {
        public PdfTextAnnotation()
        {
            Initialize();
        }

        public PdfTextAnnotation(PdfDocument document)
            : base(document)
        {
            Initialize();
        }

        void Initialize()
        {
            Elements.SetName(Keys.Subtype, "/Text");
            Icon = PdfTextAnnotationIcon.Comment;
        }

        public bool Open
        {
            get { return Elements.GetBoolean(Keys.Open); }
            set { Elements.SetBoolean(Keys.Open, value); }
        }

        public PdfTextAnnotationIcon Icon
        {
            get
            {
                string value = Elements.GetName(Keys.Name);
                if (value == "")
                    return PdfTextAnnotationIcon.NoIcon;
                value = value.Substring(1);
                if (!Enum.IsDefined(typeof(PdfTextAnnotationIcon), value))
                    return PdfTextAnnotationIcon.NoIcon;
                return (PdfTextAnnotationIcon)Enum.Parse(typeof(PdfTextAnnotationIcon), value, false);
            }
            set
            {
                if (Enum.IsDefined(typeof(PdfTextAnnotationIcon), value) &&
                  PdfTextAnnotationIcon.NoIcon != value)
                {
                    Elements.SetName(Keys.Name, "/" + value.ToString());
                }
                else
                    Elements.Remove(Keys.Name);
            }
        }

        internal new class Keys : PdfAnnotation.Keys
        {
            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string Open = "/Open";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Name = "/Name";

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
