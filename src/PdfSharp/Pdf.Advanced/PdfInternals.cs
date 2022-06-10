using System;
using System.Text;
using System.IO;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf.Advanced
{
    public class PdfInternals      
    {
        internal PdfInternals(PdfDocument document)
        {
            _document = document;
        }
        readonly PdfDocument _document;

        public string FirstDocumentID
        {
            get { return _document._trailer.GetDocumentID(0); }
            set { _document._trailer.SetDocumentID(0, value); }
        }

        public Guid FirstDocumentGuid
        {
            get { return GuidFromString(_document._trailer.GetDocumentID(0)); }
        }

        public string SecondDocumentID
        {
            get { return _document._trailer.GetDocumentID(1); }
            set { _document._trailer.SetDocumentID(1, value); }
        }

        public Guid SecondDocumentGuid
        {
            get { return GuidFromString(_document._trailer.GetDocumentID(0)); }
        }

        Guid GuidFromString(string id)
        {
            if (id == null || id.Length != 16)
                return Guid.Empty;

            StringBuilder guid = new StringBuilder();
            for (int idx = 0; idx < 16; idx++)
                guid.AppendFormat("{0:X2}", (byte)id[idx]);

            return new Guid(guid.ToString());
        }

        public PdfCatalog Catalog
        {
            get { return _document.Catalog; }
        }

        public PdfExtGStateTable ExtGStateTable
        {
            get { return _document.ExtGStateTable; }
        }

        public PdfObject GetObject(PdfObjectID objectID)
        {
            return _document._irefTable[objectID].Value;
        }

        public PdfObject MapExternalObject(PdfObject externalObject)
        {
            PdfFormXObjectTable table = _document.FormTable;
            PdfImportedObjectTable iot = table.GetImportedObjectTable(externalObject.Owner);
            PdfReference reference = iot[externalObject.ObjectID];
            return reference == null ? null : reference.Value;
        }

        public static PdfReference GetReference(PdfObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            return obj.Reference;
        }

        public static PdfObjectID GetObjectID(PdfObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            return obj.ObjectID;
        }

        public static int GetObjectNumber(PdfObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            return obj.ObjectNumber;
        }

        public static int GenerationNumber(PdfObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            return obj.GenerationNumber;
        }

        public PdfObject[] GetAllObjects()
        {
            PdfReference[] irefs = _document._irefTable.AllReferences;
            int count = irefs.Length;
            PdfObject[] objects = new PdfObject[count];
            for (int idx = 0; idx < count; idx++)
                objects[idx] = irefs[idx].Value;
            return objects;
        }

        [Obsolete("Use GetAllObjects.")]       
        public PdfObject[] AllObjects
        {
            get { return GetAllObjects(); }
        }

        public T CreateIndirectObject<T>() where T : PdfObject
        {
#if true
            T obj = Activator.CreateInstance<T>();
            _document._irefTable.Add(obj);
#endif
            return obj;
        }

        public void AddObject(PdfObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (obj.Owner == null)
                obj.Document = _document;
            else if (obj.Owner != _document)
                throw new InvalidOperationException("Object does not belong to this document.");
            _document._irefTable.Add(obj);
        }

        public void RemoveObject(PdfObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (obj.Reference == null)
                throw new InvalidOperationException("Only indirect objects can be removed.");
            if (obj.Owner != _document)
                throw new InvalidOperationException("Object does not belong to this document.");

            _document._irefTable.Remove(obj.Reference);
        }

        public PdfObject[] GetClosure(PdfObject obj)
        {
            return GetClosure(obj, Int32.MaxValue);
        }

        public PdfObject[] GetClosure(PdfObject obj, int depth)
        {
            PdfReference[] references = _document._irefTable.TransitiveClosure(obj, depth);
            int count = references.Length + 1;
            PdfObject[] objects = new PdfObject[count];
            objects[0] = obj;
            for (int idx = 1; idx < count; idx++)
                objects[idx] = references[idx - 1].Value;
            return objects;
        }

        public void WriteObject(Stream stream, PdfItem item)
        {
            PdfWriter writer = new PdfWriter(stream, null);
            writer.Options = PdfWriterOptions.OmitStream;
            item.WriteObject(writer);
        }

        public string CustomValueKey = "/PdfSharp.CustomValue";
    }
}
