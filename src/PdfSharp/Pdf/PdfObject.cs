using System;
using System.Diagnostics;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    public abstract class PdfObject : PdfItem
    {
        protected PdfObject()
        { }

        protected PdfObject(PdfDocument document)
        {
            Document = document;
        }

        protected PdfObject(PdfObject obj)
            : this(obj.Owner)
        {
            if (obj._iref != null)
                obj._iref.Value = this;
        }

        public new PdfObject Clone()
        {
            return (PdfObject)Copy();
        }

        protected override object Copy()
        {
            PdfObject obj = (PdfObject)base.Copy();
            obj._document = null;
            obj._iref = null;
            return obj;
        }


        internal void SetObjectID(int objectNumber, int generationNumber)
        {
            PdfObjectID objectID = new PdfObjectID(objectNumber, generationNumber);

            if (_iref == null)
                _iref = _document._irefTable[objectID];
            if (_iref == null)
            {
                new PdfReference(this);
                Debug.Assert(_iref != null);
                _iref.ObjectID = objectID;
            }
            _iref.Value = this;
            _iref.Document = _document;
        }

        public virtual PdfDocument Owner
        {
            get { return _document; }
        }

        internal virtual PdfDocument Document
        {
            set
            {
                if (!ReferenceEquals(_document, value))
                {
                    if (_document != null)
                        throw new InvalidOperationException("Cannot change document.");
                    _document = value;
                    if (_iref != null)
                        _iref.Document = value;
                }
            }
        }
        internal PdfDocument _document;

        public bool IsIndirect
        {
            get { return _iref != null; }
        }

        public PdfObjectInternals Internals
        {
            get { return _internals ?? (_internals = new PdfObjectInternals(this)); }
        }
        PdfObjectInternals _internals;

        internal virtual void PrepareForSave()
        { }

        internal override void WriteObject(PdfWriter writer)
        {
            Debug.Assert(false, "Must not come here!");
        }

        internal PdfObjectID ObjectID
        {
            get { return _iref != null ? _iref.ObjectID : PdfObjectID.Empty; }
        }

        internal int ObjectNumber
        {
            get { return ObjectID.ObjectNumber; }
        }

        internal int GenerationNumber
        {
            get { return ObjectID.GenerationNumber; }
        }

        internal static PdfObject DeepCopyClosure(PdfDocument owner, PdfObject externalObject)
        {
            PdfObject[] elements = externalObject.Owner.Internals.GetClosure(externalObject);
            int count = elements.Length;
            PdfImportedObjectTable iot = new PdfImportedObjectTable(owner, externalObject.Owner);
            for (int idx = 0; idx < count; idx++)
            {
                PdfObject obj = elements[idx];
                PdfObject clone = obj.Clone();
                Debug.Assert(clone.Reference == null);
                clone.Document = owner;
                if (obj.Reference != null)
                {
                    owner._irefTable.Add(clone);
                    Debug.Assert(clone.Reference != null);
                    iot.Add(obj.ObjectID, clone.Reference);
                }
                else
                {
                    Debug.Assert(idx == 0);
                }
                elements[idx] = clone;
            }

            for (int idx = 0; idx < count; idx++)
            {
                PdfObject obj = elements[idx];
                Debug.Assert(obj.Owner == owner);
                FixUpObject(iot, owner, obj);
            }

            return elements[0];
        }

        internal static PdfObject ImportClosure(PdfImportedObjectTable importedObjectTable, PdfDocument owner, PdfObject externalObject)
        {
            Debug.Assert(ReferenceEquals(importedObjectTable.Owner, owner), "importedObjectTable does not belong to the owner.");
            Debug.Assert(ReferenceEquals(importedObjectTable.ExternalDocument, externalObject.Owner),
                "The ExternalDocument of the importedObjectTable does not belong to the owner of object to be imported.");

            PdfObject[] elements = externalObject.Owner.Internals.GetClosure(externalObject);
            int count = elements.Length;

            for (int idx = 0; idx < count; idx++)
            {
                PdfObject obj = elements[idx];
                Debug.Assert(!ReferenceEquals(obj.Owner, owner));
                if (importedObjectTable.Contains(obj.ObjectID))
                {
                    PdfReference iref = importedObjectTable[obj.ObjectID];
                    Debug.Assert(iref != null);
                    Debug.Assert(iref.Value != null);
                    Debug.Assert(iref.Document == owner);
                    elements[idx] = iref.Value;
                }
                else
                {
                    PdfObject clone = obj.Clone();
                    Debug.Assert(clone.Reference == null);
                    clone.Document = owner;
                    if (obj.Reference != null)
                    {
                        owner._irefTable.Add(clone);
                        Debug.Assert(clone.Reference != null);
                        importedObjectTable.Add(obj.ObjectID, clone.Reference);
                    }
                    else
                    {
                        Debug.Assert(idx == 0);
                    }
                    elements[idx] = clone;
                }
            }
            for (int idx = 0; idx < count; idx++)
            {
                PdfObject obj = elements[idx];
                Debug.Assert(owner != null);
                FixUpObject(importedObjectTable, importedObjectTable.Owner, obj);
            }

            return elements[0];
        }

        static void FixUpObject(PdfImportedObjectTable iot, PdfDocument owner, PdfObject value)
        {
            Debug.Assert(ReferenceEquals(iot.Owner, owner));

            PdfDictionary dict;
            PdfArray array;
            if ((dict = value as PdfDictionary) != null)
            {
                if (dict.Owner == null)
                {
                    dict.Document = owner;
                }
                else
                {
                    Debug.Assert(dict.Owner == owner);
                }

                PdfName[] names = dict.Elements.KeyNames;
                foreach (PdfName name in names)
                {
                    PdfItem item = dict.Elements[name];
                    Debug.Assert(item != null, "A dictionary element cannot be null.");

                    PdfReference iref = item as PdfReference;
                    if (iref != null)
                    {
                        if (iref.Document == owner)
                        {
                            continue;
                        }

                        PdfReference newXRef = iot[iref.ObjectID];           
                        Debug.Assert(newXRef != null);
                        Debug.Assert(newXRef.Document == owner);
                        dict.Elements[name] = newXRef;
                    }
                    else
                    {
                        PdfObject pdfObject = item as PdfObject;
                        if (pdfObject != null)
                        {
                            FixUpObject(iot, owner, pdfObject);
                        }
                        else
                        {
                            DebugCheckNonObjects(item);
                        }
                    }
                }
            }
            else if ((array = value as PdfArray) != null)
            {
                if (array.Owner == null)
                {
                    array.Document = owner;
                }
                else
                {
                    Debug.Assert(array.Owner == owner);
                }

                int count = array.Elements.Count;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfItem item = array.Elements[idx];
                    Debug.Assert(item != null, "An array element cannot be null.");

                    PdfReference iref = item as PdfReference;
                    if (iref != null)
                    {
                        if (iref.Document == owner)
                        {
                            continue;
                        }

                        Debug.Assert(iref.Document == iot.ExternalDocument);
                        PdfReference newXRef = iot[iref.ObjectID];
                        Debug.Assert(newXRef != null);
                        Debug.Assert(newXRef.Document == owner);
                        array.Elements[idx] = newXRef;
                    }
                    else
                    {
                        PdfObject pdfObject = item as PdfObject;
                        if (pdfObject != null)
                        {
                            FixUpObject(iot, owner, pdfObject);
                        }
                        else
                        {
                            DebugCheckNonObjects(item);
                        }
                    }
                }
            }
            else
            {
                if (value is PdfNameObject || value is PdfStringObject || value is PdfBooleanObject || value is PdfIntegerObject || value is PdfNumberObject)
                {
                    Debug.Assert(value.IsIndirect);
                    Debug.Assert(value.Owner == owner);
                }
                else
                    Debug.Assert(false, "Should not come here. Object is neither a dictionary nor an array.");
            }
        }

        [Conditional("DEBUG")]
        static void DebugCheckNonObjects(PdfItem item)
        {
            if (item is PdfName)
                return;
            if (item is PdfBoolean)
                return;
            if (item is PdfInteger)
                return;
            if (item is PdfNumber)
                return;
            if (item is PdfString)
                return;
            if (item is PdfRectangle)
                return;
            if (item is PdfNull)
                return;

            Type type = item.GetType();
            Debug.Assert(type != null, string.Format("CheckNonObjects: Add {0} to the list.", type.Name));
        }

        public PdfReference Reference
        {
            get { return _iref; }

            internal set { _iref = value; }
        }
        PdfReference _iref;
    }
}