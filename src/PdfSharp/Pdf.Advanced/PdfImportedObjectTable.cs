using System;
using System.Collections.Generic;

namespace PdfSharp.Pdf.Advanced
{
    internal sealed class PdfImportedObjectTable
    {
        public PdfImportedObjectTable(PdfDocument owner, PdfDocument externalDocument)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            if (externalDocument == null)
                throw new ArgumentNullException("externalDocument");
            _owner = owner;
            _externalDocumentHandle = externalDocument.Handle;
            _xObjects = new PdfFormXObject[externalDocument.PageCount];
        }
        readonly PdfFormXObject[] _xObjects;

        public PdfDocument Owner
        {
            get { return _owner; }
        }
        readonly PdfDocument _owner;

        public PdfDocument ExternalDocument
        {
            get { return _externalDocumentHandle.IsAlive ? _externalDocumentHandle.Target : null; }
        }
        readonly PdfDocument.DocumentHandle _externalDocumentHandle;

        public PdfFormXObject GetXObject(int pageNumber)
        {
            return _xObjects[pageNumber - 1];
        }

        public void SetXObject(int pageNumber, PdfFormXObject xObject)
        {
            _xObjects[pageNumber - 1] = xObject;
        }

        public bool Contains(PdfObjectID externalID)
        {
            return _externalIDs.ContainsKey(externalID.ToString());
        }

        public void Add(PdfObjectID externalID, PdfReference iref)
        {
            _externalIDs[externalID.ToString()] = iref;
        }

        public PdfReference this[PdfObjectID externalID]
        {
            get { return _externalIDs[externalID.ToString()]; }
        }

        readonly Dictionary<string, PdfReference> _externalIDs = new Dictionary<string, PdfReference>();
    }
}
