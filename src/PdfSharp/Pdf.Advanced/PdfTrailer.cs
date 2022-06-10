using System;
using System.Diagnostics;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.Advanced
{
    internal class PdfTrailer : PdfDictionary          
    {
        public PdfTrailer(PdfDocument document)
            : base(document)
        {
            _document = document;
        }

        public PdfTrailer(PdfCrossReferenceStream trailer)
            : base(trailer._document)
        {
            _document = trailer._document;

            PdfReference iref = trailer.Elements.GetReference(Keys.Info);
            if (iref != null)
                Elements.SetReference(Keys.Info, iref);

            Elements.SetReference(Keys.Root, trailer.Elements.GetReference(Keys.Root));

            Elements.SetInteger(Keys.Size, trailer.Elements.GetInteger(Keys.Size));

            PdfArray id = trailer.Elements.GetArray(Keys.ID);
            if (id != null)
                Elements.SetValue(Keys.ID, id);
        }

        public int Size
        {
            get { return Elements.GetInteger(Keys.Size); }
            set { Elements.SetInteger(Keys.Size, value); }
        }

        public PdfDocumentInformation Info
        {
            get { return (PdfDocumentInformation)Elements.GetValue(Keys.Info, VCF.CreateIndirect); }
        }

        public PdfCatalog Root
        {
            get { return (PdfCatalog)Elements.GetValue(PdfTrailer.Keys.Root, VCF.CreateIndirect); }
        }

        public string GetDocumentID(int index)
        {
            if (index < 0 || index > 1)
                throw new ArgumentOutOfRangeException("index", index, "Index must be 0 or 1.");

            PdfArray array = Elements[Keys.ID] as PdfArray;
            if (array == null || array.Elements.Count < 2)
                return "";
            PdfItem item = array.Elements[index];
            if (item is PdfString)
                return ((PdfString)item).Value;
            return "";
        }

        public void SetDocumentID(int index, string value)
        {
            if (index < 0 || index > 1)
                throw new ArgumentOutOfRangeException("index", index, "Index must be 0 or 1.");

            PdfArray array = Elements[Keys.ID] as PdfArray;
            if (array == null || array.Elements.Count < 2)
                array = CreateNewDocumentIDs();
            array.Elements[index] = new PdfString(value, PdfStringFlags.HexLiteral);
        }

        internal PdfArray CreateNewDocumentIDs()
        {
            PdfArray array = new PdfArray(_document);
            byte[] docID = Guid.NewGuid().ToByteArray();
            string id = PdfEncoders.RawEncoding.GetString(docID, 0, docID.Length);
            array.Elements.Add(new PdfString(id, PdfStringFlags.HexLiteral));
            array.Elements.Add(new PdfString(id, PdfStringFlags.HexLiteral));
            Elements[Keys.ID] = array;
            return array;
        }

        public PdfStandardSecurityHandler SecurityHandler
        {
            get
            {
                if (_securityHandler == null)
                    _securityHandler = (PdfStandardSecurityHandler)Elements.GetValue(Keys.Encrypt, VCF.CreateIndirect);
                return _securityHandler;
            }
        }
        internal PdfStandardSecurityHandler _securityHandler;

        internal override void WriteObject(PdfWriter writer)
        {
            _elements.Remove(Keys.XRefStm);

            PdfStandardSecurityHandler securityHandler = writer.SecurityHandler;
            writer.SecurityHandler = null;
            base.WriteObject(writer);
            writer.SecurityHandler = securityHandler;
        }

        internal void Finish()
        {
            PdfReference iref = _document._trailer.Elements[Keys.Root] as PdfReference;
            if (iref != null && iref.Value == null)
            {
                iref = _document._irefTable[iref.ObjectID];
                Debug.Assert(iref.Value != null);
                _document._trailer.Elements[Keys.Root] = iref;
            }

            iref = _document._trailer.Elements[PdfTrailer.Keys.Info] as PdfReference;
            if (iref != null && iref.Value == null)
            {
                iref = _document._irefTable[iref.ObjectID];
                Debug.Assert(iref.Value != null);
                _document._trailer.Elements[Keys.Info] = iref;
            }

            iref = _document._trailer.Elements[Keys.Encrypt] as PdfReference;
            if (iref != null)
            {
                iref = _document._irefTable[iref.ObjectID];
                Debug.Assert(iref.Value != null);
                _document._trailer.Elements[Keys.Encrypt] = iref;

                iref.Value = _document._trailer._securityHandler;
                _document._trailer._securityHandler.Reference = iref;
                iref.Value.Reference = iref;
            }

            Elements.Remove(Keys.Prev);

            Debug.Assert(_document._irefTable.IsUnderConstruction == false);
            _document._irefTable.IsUnderConstruction = false;
        }

        internal class Keys : KeysBase               
        {
            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string Size = "/Size";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string Prev = "/Prev";

            [KeyInfo(KeyType.Dictionary | KeyType.Required, typeof(PdfCatalog))]
            public const string Root = "/Root";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfStandardSecurityHandler))]
            public const string Encrypt = "/Encrypt";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfDocumentInformation))]
            public const string Info = "/Info";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string ID = "/ID";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string XRefStm = "/XRefStm";

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
