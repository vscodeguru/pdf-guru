using PdfSharp.Drawing;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Annotations;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfTextField : PdfAcroField
    {
        internal PdfTextField(PdfDocument document)
            : base(document)
        { }

        internal PdfTextField(PdfDictionary dict)
            : base(dict)
        { }

        public string Text
        {
            get { return Elements.GetString(Keys.V); }
            set { Elements.SetString(Keys.V, value); RenderAppearance(); }   
        }

        public XFont Font
        {
            get { return _font; }
            set { _font = value; }
        }
        XFont _font = new XFont("Courier New", 10);

        public XColor ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }
        XColor _foreColor = XColors.Black;

        public XColor BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }
        XColor _backColor = XColor.Empty;

        public int MaxLength
        {
            get { return Elements.GetInteger(Keys.MaxLen); }
            set { Elements.SetInteger(Keys.MaxLen, value); }
        }

        public bool MultiLine
        {
            get { return (Flags & PdfAcroFieldFlags.Multiline) != 0; }
            set
            {
                if (value)
                    SetFlags |= PdfAcroFieldFlags.Multiline;
                else
                    SetFlags &= ~PdfAcroFieldFlags.Multiline;
            }
        }

        public bool Password
        {
            get { return (Flags & PdfAcroFieldFlags.Password) != 0; }
            set
            {
                if (value)
                    SetFlags |= PdfAcroFieldFlags.Password;
                else
                    SetFlags &= ~PdfAcroFieldFlags.Password;
            }
        }

        void RenderAppearance()
        {
            PdfRectangle rect = Elements.GetRectangle(PdfAnnotation.Keys.Rect);
            XForm form = new XForm(_document, rect.Size);
            XGraphics gfx = XGraphics.FromForm(form);

            if (_backColor != XColor.Empty)
                gfx.DrawRectangle(new XSolidBrush(BackColor), rect.ToXRect() - rect.Location);

            string text = Text;
            if (text.Length > 0)
                gfx.DrawString(Text, Font, new XSolidBrush(ForeColor),
                  rect.ToXRect() - rect.Location + new XPoint(2, 0), XStringFormats.TopLeft);

            form.DrawingFinished();
            form.PdfForm.Elements.Add("/FormType", new PdfLiteral("1"));

            PdfDictionary ap = Elements[PdfAnnotation.Keys.AP] as PdfDictionary;
            if (ap == null)
            {
                ap = new PdfDictionary(_document);
                Elements[PdfAnnotation.Keys.AP] = ap;
            }

            ap.Elements["/N"] = form.PdfForm.Reference;

            PdfFormXObject xobj = form.PdfForm;
            string s = xobj.Stream.ToString();
            s = "/Tx BMC\n" + s + "\nEMC";
            xobj.Stream.Value = new RawEncoding().GetBytes(s);
        }

        internal override void PrepareForSave()
        {
            base.PrepareForSave();
            RenderAppearance();
        }

        public new class Keys : PdfAcroField.Keys
        {
            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string MaxLen = "/MaxLen";

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
    }
}
