using System;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Annotations
{
    public sealed class PdfRubberStampAnnotation : PdfAnnotation
    {
        public PdfRubberStampAnnotation()
        {
            Initialize();
        }

        public PdfRubberStampAnnotation(PdfDocument document)
            : base(document)
        {
            Initialize();
        }

        void Initialize()
        {
            Elements.SetName(Keys.Subtype, "/Stamp");
            Color = XColors.Yellow;
        }

        public PdfRubberStampAnnotationIcon Icon
        {
            get
            {
                string value = Elements.GetName(Keys.Name);
                if (value == "")
                    return PdfRubberStampAnnotationIcon.NoIcon;
                value = value.Substring(1);
                if (!Enum.IsDefined(typeof(PdfRubberStampAnnotationIcon), value))
                    return PdfRubberStampAnnotationIcon.NoIcon;
                return (PdfRubberStampAnnotationIcon)Enum.Parse(typeof(PdfRubberStampAnnotationIcon), value, false);
            }
            set
            {
                if (Enum.IsDefined(typeof(PdfRubberStampAnnotationIcon), value) &&
                  PdfRubberStampAnnotationIcon.NoIcon != value)
                {
                    Elements.SetName(Keys.Name, "/" + value.ToString());
                }
                else
                    Elements.Remove(Keys.Name);
            }
        }

        internal new class Keys : PdfAnnotation.Keys
        {
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
