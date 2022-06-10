using System;
using System.Diagnostics;
using PdfSharp.Drawing.Pdf;
using PdfSharp.Pdf.Filters;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfContent : PdfDictionary
    {
        public PdfContent(PdfDocument document)
            : base(document)
        { }

        internal PdfContent(PdfPage page)
            : base(page != null ? page.Owner : null)
        {
        }

        public PdfContent(PdfDictionary dict)   
            : base(dict)
        {
            Decode();
        }

        public bool Compressed
        {
            set
            {
                if (value)
                {
                    PdfItem filter = Elements["/Filter"];
                    if (filter == null)
                    {
                        byte[] bytes = Filtering.FlateDecode.Encode(Stream.Value, _document.Options.FlateEncodeMode);
                        Stream.Value = bytes;
                        Elements.SetInteger("/Length", Stream.Length);
                        Elements.SetName("/Filter", "/FlateDecode");
                    }
                }
            }
        }

        void Decode()
        {
            if (Stream != null && Stream.Value != null)
            {
                PdfItem item = Elements["/Filter"];
                if (item != null)
                {
                    byte[] bytes = Filtering.Decode(Stream.Value, item);
                    if (bytes != null)
                    {
                        Stream.Value = bytes;
                        Elements.Remove("/Filter");
                        Elements.SetInteger("/Length", Stream.Length);
                    }
                }
            }
        }

        internal void PreserveGraphicsState()
        {
            if (Stream != null)
            {
                byte[] value = Stream.Value;
                int length = value.Length;
                if (length != 0 && ((value[0] != (byte)'q' || value[1] != (byte)'\n')))
                {
                    byte[] newValue = new byte[length + 2 + 3];
                    newValue[0] = (byte)'q';
                    newValue[1] = (byte)'\n';
                    Array.Copy(value, 0, newValue, 2, length);
                    newValue[length + 2] = (byte)' ';
                    newValue[length + 3] = (byte)'Q';
                    newValue[length + 4] = (byte)'\n';
                    Stream.Value = newValue;
                    Elements.SetInteger("/Length", Stream.Length);
                }
            }
        }

        internal override void WriteObject(PdfWriter writer)
        {
            if (_pdfRenderer != null)
            {
                _pdfRenderer.Close();
                Debug.Assert(_pdfRenderer == null);
            }

            if (Stream != null)
            {
                if (Owner.Options.CompressContentStreams && Elements.GetName("/Filter").Length == 0)
                {
                    Stream.Value = Filtering.FlateDecode.Encode(Stream.Value, _document.Options.FlateEncodeMode);
                    Elements.SetName("/Filter", "/FlateDecode");
                }
                Elements.SetInteger("/Length", Stream.Length);
            }

            base.WriteObject(writer);
        }

        internal XGraphicsPdfRenderer _pdfRenderer;

        internal sealed class Keys : PdfStream.Keys
        {
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
