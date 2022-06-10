using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    internal sealed class PdfReferenceTable_old        
    {
        public PdfReferenceTable_old(PdfDocument document)
        {
            _document = document;
        }
        readonly PdfDocument _document;

        public Dictionary<PdfObjectID, PdfReference> ObjectTable = new Dictionary<PdfObjectID, PdfReference>();

        internal bool IsUnderConstruction
        {
            get { return _isUnderConstruction; }
            set { _isUnderConstruction = value; }
        }
        bool _isUnderConstruction;

        public void Add(PdfReference iref)
        {
            if (iref.ObjectID.IsEmpty)
                iref.ObjectID = new PdfObjectID(GetNewObjectNumber());

            if (ObjectTable.ContainsKey(iref.ObjectID))
                throw new InvalidOperationException("Object already in table.");

            ObjectTable.Add(iref.ObjectID, iref);
        }

        public void Add(PdfObject value)
        {
            if (value.Owner == null)
                value.Document = _document;
            else
                Debug.Assert(value.Owner == _document);

            if (value.ObjectID.IsEmpty)
                value.SetObjectID(GetNewObjectNumber(), 0);

            if (ObjectTable.ContainsKey(value.ObjectID))
                throw new InvalidOperationException("Object already in table.");

            ObjectTable.Add(value.ObjectID, value.Reference);
        }

        public void Remove(PdfReference iref)
        {
            ObjectTable.Remove(iref.ObjectID);
        }

        public PdfReference this[PdfObjectID objectID]
        {
            get
            {
                PdfReference iref;
                ObjectTable.TryGetValue(objectID, out iref);
                return iref;
            }
        }

        public bool Contains(PdfObjectID objectID)
        {
            return ObjectTable.ContainsKey(objectID);
        }

        public int GetNewObjectNumber()
        {
            return ++_maxObjectNumber;
        }
        internal int _maxObjectNumber;

        internal void WriteObject(PdfWriter writer)
        {
            writer.WriteRaw("xref\n");

            PdfReference[] irefs = AllReferences;

            int count = irefs.Length;
            writer.WriteRaw(String.Format("0 {0}\n", count + 1));
            writer.WriteRaw(String.Format("{0:0000000000} {1:00000} {2} \n", 0, 65535, "f"));
            for (int idx = 0; idx < count; idx++)
            {
                PdfReference iref = irefs[idx];

                writer.WriteRaw(String.Format("{0:0000000000} {1:00000} {2} \n", iref.Position, iref.GenerationNumber, "n"));
            }
        }

        internal PdfObjectID[] AllObjectIDs
        {
            get
            {
                ICollection collection = ObjectTable.Keys;
                PdfObjectID[] objectIDs = new PdfObjectID[collection.Count];
                collection.CopyTo(objectIDs, 0);
                return objectIDs;
            }
        }

        internal PdfReference[] AllReferences
        {
            get
            {
                Dictionary<PdfObjectID, PdfReference>.ValueCollection collection = ObjectTable.Values;
                List<PdfReference> list = new List<PdfReference>(collection);
                list.Sort(PdfReference.Comparer);
                PdfReference[] irefs = new PdfReference[collection.Count];
                list.CopyTo(irefs, 0);
                return irefs;
            }
        }

        internal void HandleOrphanedReferences()
        { }

        internal int Compact()
        {
            int removed = ObjectTable.Count;
            PdfReference[] irefs = TransitiveClosure(_document._trailer);



            _maxObjectNumber = 0;
            ObjectTable.Clear();
            foreach (PdfReference iref in irefs)
            {
                if (!ObjectTable.ContainsKey(iref.ObjectID))
                {
                    ObjectTable.Add(iref.ObjectID, iref);
                    _maxObjectNumber = Math.Max(_maxObjectNumber, iref.ObjectNumber);
                }
            }
            removed -= ObjectTable.Count;
            return removed;
        }

        internal void Renumber()
        {
            PdfReference[] irefs = AllReferences;
            ObjectTable.Clear();
            int count = irefs.Length;
            for (int idx = 0; idx < count; idx++)
            {
                PdfReference iref = irefs[idx];

                iref.ObjectID = new PdfObjectID(idx + 1);
                ObjectTable.Add(iref.ObjectID, iref);
            }
            _maxObjectNumber = count;
        }

        [Conditional("DEBUG_")]
        public void CheckConsistence()
        {
            Dictionary<PdfReference, object> ht1 = new Dictionary<PdfReference, object>();
            foreach (PdfReference iref in ObjectTable.Values)
            {
                Debug.Assert(!ht1.ContainsKey(iref), "Duplicate iref.");
                Debug.Assert(iref.Value != null);
                ht1.Add(iref, null);
            }

            Dictionary<PdfObjectID, object> ht2 = new Dictionary<PdfObjectID, object>();
            foreach (PdfReference iref in ObjectTable.Values)
            {
                Debug.Assert(!ht2.ContainsKey(iref.ObjectID), "Duplicate iref.");
                ht2.Add(iref.ObjectID, null);
            }

            ICollection collection = ObjectTable.Values;
            int count = collection.Count;
            PdfReference[] irefs = new PdfReference[count];
            collection.CopyTo(irefs, 0);
#if true
            for (int i = 0; i < count; i++)
                for (int j = 0; j < count; j++)
                    if (i != j)
                    {
                        Debug.Assert(ReferenceEquals(irefs[i].Document, _document));
                        Debug.Assert(irefs[i] != irefs[j]);
                        Debug.Assert(!ReferenceEquals(irefs[i], irefs[j]));
                        Debug.Assert(!ReferenceEquals(irefs[i].Value, irefs[j].Value));
                        Debug.Assert(!Equals(irefs[i].ObjectID, irefs[j].Value.ObjectID));
                        Debug.Assert(irefs[i].ObjectNumber != irefs[j].Value.ObjectNumber);
                        Debug.Assert(ReferenceEquals(irefs[i].Document, irefs[j].Document));
                        GetType();
                    }
#endif
        }

        public PdfReference[] TransitiveClosure(PdfObject pdfObject)
        {
            return TransitiveClosure(pdfObject, Int16.MaxValue);
        }

        public PdfReference[] TransitiveClosure(PdfObject pdfObject, int depth)
        {
            CheckConsistence();
            Dictionary<PdfItem, object> objects = new Dictionary<PdfItem, object>();
            _overflow = new Dictionary<PdfItem, object>();
            TransitiveClosureImplementation(objects, pdfObject   );
        TryAgain:
            if (_overflow.Count > 0)
            {
                PdfObject[] array = new PdfObject[_overflow.Count];
                _overflow.Keys.CopyTo(array, 0);
                _overflow = new Dictionary<PdfItem, object>();
                for (int idx = 0; idx < array.Length; idx++)
                {
                    PdfObject obj = array[idx];
                    TransitiveClosureImplementation(objects, obj   );
                }
                goto TryAgain;
            }

            CheckConsistence();

            ICollection collection = objects.Keys;
            int count = collection.Count;
            PdfReference[] irefs = new PdfReference[count];
            collection.CopyTo(irefs, 0);

            return irefs;
        }

        static int _nestingLevel;
        Dictionary<PdfItem, object> _overflow = new Dictionary<PdfItem, object>();

        void TransitiveClosureImplementation(Dictionary<PdfItem, object> objects, PdfObject pdfObject   )
        {
            try
            {
                _nestingLevel++;
                if (_nestingLevel >= 1000)
                {
                    if (!_overflow.ContainsKey(pdfObject))
                        _overflow.Add(pdfObject, null);
                    return;
                }

                IEnumerable enumerable = null; 
                PdfDictionary dict;
                PdfArray array;
                if ((dict = pdfObject as PdfDictionary) != null)
                    enumerable = dict.Elements.Values;
                else if ((array = pdfObject as PdfArray) != null)
                    enumerable = array.Elements;
                else
                    Debug.Assert(false, "Should not come here.");

                if (enumerable != null)
                {
                    foreach (PdfItem item in enumerable)
                    {
                        PdfReference iref = item as PdfReference;
                        if (iref != null)
                        {
                            if (!ReferenceEquals(iref.Document, _document))
                            {
                                GetType();
                                Debug.WriteLine(String.Format("Bad iref: {0}", iref.ObjectID.ToString()));
                            }
                            Debug.Assert(ReferenceEquals(iref.Document, _document) || iref.Document == null, "External object detected!");

                            if (!objects.ContainsKey(iref))
                            {
                                PdfObject value = iref.Value;

                                if (iref.Document != null)
                                {
                                    if (value == null)
                                    {
                                        iref = ObjectTable[iref.ObjectID];
                                        Debug.Assert(iref.Value != null);
                                        value = iref.Value;
                                    }
                                    Debug.Assert(ReferenceEquals(iref.Document, _document));
                                    objects.Add(iref, null);
                                    if (value is PdfArray || value is PdfDictionary)
                                        TransitiveClosureImplementation(objects, value   );
                                }
                            }
                        }
                        else
                        {
                            PdfObject pdfObject28 = item as PdfObject;
                            if (pdfObject28 != null && (pdfObject28 is PdfDictionary || pdfObject28 is PdfArray))
                                TransitiveClosureImplementation(objects, pdfObject28   );
                        }
                    }
                }
            }
            finally
            {
                _nestingLevel--;
            }
        }

        public PdfReference DeadObject
        {
            get
            {
                if (_deadObject == null)
                {
                    _deadObject = new PdfDictionary(_document);
                    Add(_deadObject);
                    _deadObject.Elements.Add("/DeadObjectCount", new PdfInteger());
                }
                return _deadObject.Reference;
            }
        }
        PdfDictionary _deadObject;
    }
}
