using System;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Annotations
{
    public abstract class PdfAnnotation : PdfDictionary
    {
        protected PdfAnnotation()
        {
            Initialize();
        }

        protected PdfAnnotation(PdfDocument document)
            : base(document)
        {
            Initialize();
        }

        internal PdfAnnotation(PdfDictionary dict)
            : base(dict)
        { }

        void Initialize()
        {
            Elements.SetName(Keys.Type, "/Annot");
            Elements.SetString(Keys.NM, Guid.NewGuid().ToString("D"));
            Elements.SetDateTime(Keys.M, DateTime.Now);
        }

        [Obsolete("Use 'Parent.Remove(this)'")]
        public void Delete()
        {
            Parent.Remove(this);
        }

        public PdfAnnotationFlags Flags
        {
            get { return (PdfAnnotationFlags)Elements.GetInteger(Keys.F); }
            set
            {
                Elements.SetInteger(Keys.F, (int)value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        public PdfAnnotations Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        PdfAnnotations _parent;

        public PdfRectangle Rectangle
        {
            get { return Elements.GetRectangle(Keys.Rect, true); }
            set
            {
                Elements.SetRectangle(Keys.Rect, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        public string Title
        {
            get { return Elements.GetString(Keys.T, true); }
            set
            {
                Elements.SetString(Keys.T, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        public string Subject
        {
            get { return Elements.GetString(Keys.Subj, true); }
            set
            {
                Elements.SetString(Keys.Subj, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        public string Contents
        {
            get { return Elements.GetString(Keys.Contents, true); }
            set
            {
                Elements.SetString(Keys.Contents, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        public XColor Color
        {
            get
            {
                PdfItem item = Elements[Keys.C];
                PdfArray array = item as PdfArray;
                if (array != null)      
                {
                    if (array.Elements.Count == 3)
                    {
                        return XColor.FromArgb(
                            (int)(array.Elements.GetReal(0) * 255),
                            (int)(array.Elements.GetReal(1) * 255),
                            (int)(array.Elements.GetReal(2) * 255));
                    }
                }
                return XColors.Black;
            }
            set
            {
                PdfArray array = new PdfArray(Owner, new PdfReal[] { new PdfReal(value.R / 255.0), new PdfReal(value.G / 255.0), new PdfReal(value.B / 255.0) });
                Elements[Keys.C] = array;
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        public double Opacity
        {
            get
            {
                if (!Elements.ContainsKey(Keys.CA))
                    return 1;
                return Elements.GetReal(Keys.CA, true);
            }
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException("value", value, "Opacity must be a value in the range from 0 to 1.");
                Elements.SetReal(Keys.CA, value);
                Elements.SetDateTime(Keys.M, DateTime.Now);
            }
        }

        public class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Optional, FixedValue = "Annot")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string Subtype = "/Subtype";

            [KeyInfo(KeyType.Rectangle | KeyType.Required)]
            public const string Rect = "/Rect";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string Contents = "/Contents";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string NM = "/NM";

            [KeyInfo(KeyType.Date | KeyType.Optional)]
            public const string M = "/M";

            [KeyInfo("1.1", KeyType.Integer | KeyType.Optional)]
            public const string F = "/F";

            [KeyInfo("1.2", KeyType.Dictionary | KeyType.Optional)]
            public const string BS = "/BS";

            [KeyInfo("1.2", KeyType.Dictionary | KeyType.Optional)]
            public const string AP = "/AP";

            [KeyInfo("1.2", KeyType.Dictionary | KeyType.Optional)]
            public const string AS = "/AS";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Border = "/Border";

            [KeyInfo("1.1", KeyType.Array | KeyType.Optional)]
            public const string C = "/C";

            [KeyInfo("1.3", KeyType.Integer | KeyType.Optional)]
            public const string StructParent = "/StructParent";

            [KeyInfo("1.1", KeyType.Dictionary | KeyType.Optional)]
            public const string A = "/A";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string T = "/T";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string Popup = "/Popup";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string CA = "/CA";

            [KeyInfo("1.5", KeyType.TextString | KeyType.Optional)]
            public const string Subj = "/Subj";

        }
    }
}
