using System;
using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Internal;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.Security;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("(Name={Name})")]      
    public sealed class PdfDocument : PdfObject, IDisposable
    {
        internal DocumentState _state;
        internal PdfDocumentOpenMode _openMode;

        public PdfDocument()
        {
            _creation = DateTime.Now;
            _state = DocumentState.Created;
            _version = 14;
            Initialize();
            Info.CreationDate = _creation;
        }

        public PdfDocument(string filename)
        {
            _creation = DateTime.Now;
            _state = DocumentState.Created;
            _version = 14;
            Initialize();
            Info.CreationDate = _creation;

#if !NETFX_CORE
            _outStream = new FileStream(filename, FileMode.Create);
#endif
        }

        public PdfDocument(Stream outputStream)
        {
            _creation = DateTime.Now;
            _state = DocumentState.Created;
            Initialize();
            Info.CreationDate = _creation;

            _outStream = outputStream;
        }

        internal PdfDocument(Lexer lexer)
        {
            _creation = DateTime.Now;
            _state = DocumentState.Imported;

            _irefTable = new PdfCrossReferenceTable(this);
            _lexer = lexer;
        }

        void Initialize()
        {
            _fontTable = new PdfFontTable(this);
            _imageTable = new PdfImageTable(this);
            _trailer = new PdfTrailer(this);
            _irefTable = new PdfCrossReferenceTable(this);
            _trailer.CreateNewDocumentIDs();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool disposing)
        {
            if (_state != DocumentState.Disposed)
            {
                if (disposing)
                {
                }
            }
            _state = DocumentState.Disposed;
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
        object _tag;

        string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        string _name = NewName();

        static string NewName()
        {
            return "Document " + _nameCount++;
        }
        static int _nameCount;

        internal bool CanModify
        {
            get { return true; }
        }

        public void Close()
        {
            if (!CanModify)
                throw new InvalidOperationException(PSSR.CannotModify);

            if (_outStream != null)
            {
                PdfStandardSecurityHandler securityHandler = null;
                if (SecuritySettings.DocumentSecurityLevel != PdfDocumentSecurityLevel.None)
                    securityHandler = SecuritySettings.SecurityHandler;

                PdfWriter writer = new PdfWriter(_outStream, securityHandler);
                try
                {
                    DoSave(writer);
                }
                finally
                {
                    writer.Close();
                }
            }
        }

#if true 
        public void Save(string path)
        {
            if (!CanModify)
                throw new InvalidOperationException(PSSR.CannotModify);

#if !NETFX_CORE
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Save(stream);
            }
#endif
        }
#endif
        public void Save(Stream stream, bool closeStream)
        {
            if (!CanModify)
                throw new InvalidOperationException(PSSR.CannotModify);

            string message = "";
            if (!CanSave(ref message))
                throw new PdfSharpException(message);

            PdfStandardSecurityHandler securityHandler = null;
            if (SecuritySettings.DocumentSecurityLevel != PdfDocumentSecurityLevel.None)
                securityHandler = SecuritySettings.SecurityHandler;

            PdfWriter writer = null;
            try
            {
                writer = new PdfWriter(stream, securityHandler);
                DoSave(writer);
            }
            finally
            {
                if (stream != null)
                {
                    if (closeStream)
#if UWP
 
#else
                        stream.Close();
#endif
                    else
                    {
                        if (stream.CanRead && stream.CanSeek)
                            stream.Position = 0;           
                    }
                }
                if (writer != null)
                    writer.Close(closeStream);
            }
        }

        public void Save(Stream stream)
        {
            Save(stream, false);
        }

        void DoSave(PdfWriter writer)
        {
            if (_pages == null || _pages.Count == 0)
            {
                if (_outStream != null)
                {
                    throw new InvalidOperationException("Cannot save a PDF document with no pages. Do not use \"public PdfDocument(string filename)\" or \"public PdfDocument(Stream outputStream)\" if you want to open an existing PDF document from a file or stream; use PdfReader.Open() for that purpose.");
                }
                throw new InvalidOperationException("Cannot save a PDF document with no pages.");
            }

            try
            {
                if (_trailer is PdfCrossReferenceStream)
                {
                    PdfStandardSecurityHandler securityHandler = _securitySettings.SecurityHandler;
                    _trailer = new PdfTrailer((PdfCrossReferenceStream)_trailer);
                    _trailer._securityHandler = securityHandler;
                }

                bool encrypt = _securitySettings.DocumentSecurityLevel != PdfDocumentSecurityLevel.None;
                if (encrypt)
                {
                    PdfStandardSecurityHandler securityHandler = _securitySettings.SecurityHandler;
                    if (securityHandler.Reference == null)
                        _irefTable.Add(securityHandler);
                    else
                        Debug.Assert(_irefTable.Contains(securityHandler.ObjectID));
                    _trailer.Elements[PdfTrailer.Keys.Encrypt] = _securitySettings.SecurityHandler.Reference;
                }
                else
                    _trailer.Elements.Remove(PdfTrailer.Keys.Encrypt);

                PrepareForSave();

                if (encrypt)
                    _securitySettings.SecurityHandler.PrepareEncryption();

                writer.WriteFileHeader(this);
                PdfReference[] irefs = _irefTable.AllReferences;
                int count = irefs.Length;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfReference iref = irefs[idx];
                    iref.Position = writer.Position;
                    iref.Value.WriteObject(writer);
                }
                int startxref = writer.Position;
                _irefTable.WriteObject(writer);
                writer.WriteRaw("trailer\n");
                _trailer.Elements.SetInteger("/Size", count + 1);
                _trailer.WriteObject(writer);
                writer.WriteEof(this, startxref);

            }
            finally
            {
                if (writer != null)
                {
                    writer.Stream.Flush();
                }
            }
        }

        internal override void PrepareForSave()
        {
            PdfDocumentInformation info = Info;

            string pdfSharpProducer = VersionInfo.Producer;
            if (!ProductVersionInfo.VersionPatch.Equals("0"))
                pdfSharpProducer = ProductVersionInfo.Producer2;

            if (info.Elements[PdfDocumentInformation.Keys.Creator] == null)
                info.Creator = pdfSharpProducer;

            string producer = info.Producer;
            if (producer.Length == 0)
                producer = pdfSharpProducer;
            else
            {
                if (!producer.StartsWith(VersionInfo.Title))
                    producer = pdfSharpProducer + " (Original: " + producer + ")";
            }
            info.Elements.SetString(PdfDocumentInformation.Keys.Producer, producer);

            if (_fontTable != null)
                _fontTable.PrepareForSave();

            Catalog.PrepareForSave();

#if true
            int removed = _irefTable.Compact();
            if (removed != 0)
                Debug.WriteLine("PrepareForSave: Number of deleted unreachable objects: " + removed);
            _irefTable.Renumber();
#endif
        }

        public bool CanSave(ref string message)
        {
            if (!SecuritySettings.CanSave(ref message))
                return false;

            return true;
        }

        internal bool HasVersion(string version)
        {
            return String.Compare(Catalog.Version, version) >= 0;
        }

        public PdfDocumentOptions Options
        {
            get
            {
                if (_options == null)
                    _options = new PdfDocumentOptions(this);
                return _options;
            }
        }
        PdfDocumentOptions _options;

        public PdfDocumentSettings Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new PdfDocumentSettings(this);
                return _settings;
            }
        }
        PdfDocumentSettings _settings;

        internal bool EarlyWrite
        {
            get { return false; }
        }

        public int Version
        {
            get { return _version; }
            set
            {
                if (!CanModify)
                    throw new InvalidOperationException(PSSR.CannotModify);
                if (value < 12 || value > 17)     
                    throw new ArgumentException(PSSR.InvalidVersionNumber, "value");
                _version = value;
            }
        }
        internal int _version;

        public int PageCount
        {
            get
            {
                if (CanModify)
                    return Pages.Count;
                PdfDictionary pageTreeRoot = (PdfDictionary)Catalog.Elements.GetObject(PdfCatalog.Keys.Pages);
                return pageTreeRoot.Elements.GetInteger(PdfPages.Keys.Count);
            }
        }

        public long FileSize
        {
            get { return _fileSize; }
        }
        internal long _fileSize;    

        public string FullPath
        {
            get { return _fullPath; }
        }
        internal string _fullPath = String.Empty;    

        public Guid Guid
        {
            get { return _guid; }
        }
        Guid _guid = Guid.NewGuid();

        internal DocumentHandle Handle
        {
            get
            {
                if (_handle == null)
                    _handle = new DocumentHandle(this);
                return _handle;
            }
        }
        DocumentHandle _handle;

        public bool IsImported
        {
            get { return (_state & DocumentState.Imported) != 0; }
        }

        public bool IsReadOnly
        {
            get { return (_openMode != PdfDocumentOpenMode.Modify); }
        }

        internal Exception DocumentNotImported()
        {
            return new InvalidOperationException("Document not imported.");
        }

        public PdfDocumentInformation Info
        {
            get
            {
                if (_info == null)
                    _info = _trailer.Info;
                return _info;
            }
        }
        PdfDocumentInformation _info;       

        public PdfCustomValues CustomValues
        {
            get
            {
                if (_customValues == null)
                    _customValues = PdfCustomValues.Get(Catalog.Elements);
                return _customValues;
            }
            set
            {
                if (value != null)
                    throw new ArgumentException("Only null is allowed to clear all custom values.");
                PdfCustomValues.Remove(Catalog.Elements);
                _customValues = null;
            }
        }
        PdfCustomValues _customValues;

        public PdfPages Pages
        {
            get
            {
                if (_pages == null)
                    _pages = Catalog.Pages;
                return _pages;
            }
        }
        PdfPages _pages;       

        public PdfPageLayout PageLayout
        {
            get { return Catalog.PageLayout; }
            set
            {
                if (!CanModify)
                    throw new InvalidOperationException(PSSR.CannotModify);
                Catalog.PageLayout = value;
            }
        }

        public PdfPageMode PageMode
        {
            get { return Catalog.PageMode; }
            set
            {
                if (!CanModify)
                    throw new InvalidOperationException(PSSR.CannotModify);
                Catalog.PageMode = value;
            }
        }

        public PdfViewerPreferences ViewerPreferences
        {
            get { return Catalog.ViewerPreferences; }
        }

        public PdfOutlineCollection Outlines
        {
            get { return Catalog.Outlines; }
        }

        public PdfAcroForm AcroForm
        {
            get { return Catalog.AcroForm; }
        }

        public string Language
        {
            get { return Catalog.Language; }
            set { Catalog.Language = value; }
        }

        public PdfSecuritySettings SecuritySettings
        {
            get { return _securitySettings ?? (_securitySettings = new PdfSecuritySettings(this)); }
        }
        internal PdfSecuritySettings _securitySettings;

        internal PdfFontTable FontTable
        {
            get { return _fontTable ?? (_fontTable = new PdfFontTable(this)); }
        }
        PdfFontTable _fontTable;

        internal PdfImageTable ImageTable
        {
            get
            {
                if (_imageTable == null)
                    _imageTable = new PdfImageTable(this);
                return _imageTable;
            }
        }
        PdfImageTable _imageTable;

        internal PdfFormXObjectTable FormTable      
        {
            get { return _formTable ?? (_formTable = new PdfFormXObjectTable(this)); }
        }
        PdfFormXObjectTable _formTable;

        internal PdfExtGStateTable ExtGStateTable
        {
            get { return _extGStateTable ?? (_extGStateTable = new PdfExtGStateTable(this)); }
        }
        PdfExtGStateTable _extGStateTable;

        internal PdfCatalog Catalog
        {
            get { return _catalog ?? (_catalog = _trailer.Root); }
        }
        PdfCatalog _catalog;       

        public new PdfInternals Internals
        {
            get { return _internals ?? (_internals = new PdfInternals(this)); }
        }
        PdfInternals _internals;

        public PdfPage AddPage()
        {
            if (!CanModify)
                throw new InvalidOperationException(PSSR.CannotModify);
            return Catalog.Pages.Add();
        }

        public PdfPage AddPage(PdfPage page)
        {
            if (!CanModify)
                throw new InvalidOperationException(PSSR.CannotModify);
            return Catalog.Pages.Add(page);
        }

        public PdfPage InsertPage(int index)
        {
            if (!CanModify)
                throw new InvalidOperationException(PSSR.CannotModify);
            return Catalog.Pages.Insert(index);
        }

        public PdfPage InsertPage(int index, PdfPage page)
        {
            if (!CanModify)
                throw new InvalidOperationException(PSSR.CannotModify);
            return Catalog.Pages.Insert(index, page);
        }

        public void Flatten()
        {
            for (int idx = 0; idx < AcroForm.Fields.Count; idx++)
            {
                AcroForm.Fields[idx].ReadOnly = true;
            }
        }

        public PdfStandardSecurityHandler SecurityHandler
        {
            get { return _trailer.SecurityHandler; }
        }

        internal PdfTrailer _trailer;
        internal PdfCrossReferenceTable _irefTable;
        internal Stream _outStream;

        internal Lexer _lexer;

        internal DateTime _creation;

        internal void OnExternalDocumentFinalized(PdfDocument.DocumentHandle handle)
        {
            if (tls != null)
            {
                tls.DetachDocument(handle);
            }

            if (_formTable != null)
                _formTable.DetachDocument(handle);
        }

        internal static ThreadLocalStorage Tls
        {
            get { return tls ?? (tls = new ThreadLocalStorage()); }
        }
        [ThreadStatic]
        static ThreadLocalStorage tls;

        [DebuggerDisplay("(ID={ID}, alive={IsAlive})")]
        internal class DocumentHandle
        {
            public DocumentHandle(PdfDocument document)
            {
                _weakRef = new WeakReference(document);
                ID = document._guid.ToString("B").ToUpper();
            }

            public bool IsAlive
            {
                get { return _weakRef.IsAlive; }
            }

            public PdfDocument Target
            {
                get { return _weakRef.Target as PdfDocument; }
            }
            readonly WeakReference _weakRef;

            public string ID;

            public override bool Equals(object obj)
            {
                DocumentHandle handle = obj as DocumentHandle;
                if (!ReferenceEquals(handle, null))
                    return ID == handle.ID;
                return false;
            }

            public override int GetHashCode()
            {
                return ID.GetHashCode();
            }

            public static bool operator ==(DocumentHandle left, DocumentHandle right)
            {
                if (ReferenceEquals(left, null))
                    return ReferenceEquals(right, null);
                return left.Equals(right);
            }

            public static bool operator !=(DocumentHandle left, DocumentHandle right)
            {
                return !(left == right);
            }
        }
    }
}