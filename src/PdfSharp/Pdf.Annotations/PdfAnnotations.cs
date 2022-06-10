using System;
using System.Diagnostics;
using System.Collections;
using PdfSharp.Pdf.Advanced;
using System.Collections.Generic;

namespace PdfSharp.Pdf.Annotations
{
    public sealed class PdfAnnotations : PdfArray
    {
        internal PdfAnnotations(PdfDocument document)
            : base(document)
        { }

        internal PdfAnnotations(PdfArray array)
            : base(array)
        { }

        public void Add(PdfAnnotation annotation)
        {
            annotation.Document = Owner;
            Owner._irefTable.Add(annotation);
            Elements.Add(annotation.Reference);
        }

        public void Remove(PdfAnnotation annotation)
        {
            if (annotation.Owner != Owner)
                throw new InvalidOperationException("The annotation does not belong to this document.");

            Owner.Internals.RemoveObject(annotation);
            Elements.Remove(annotation.Reference);
        }

        public void Clear()
        {
            for (int idx = Count - 1; idx >= 0; idx--)
                Page.Annotations.Remove(_page.Annotations[idx]);
        }

        public int Count
        {
            get { return Elements.Count; }
        }

        public PdfAnnotation this[int index]
        {
            get
            {
                PdfReference iref;
                PdfDictionary dict;
                PdfItem item = Elements[index];
                if ((iref = item as PdfReference) != null)
                {
                    Debug.Assert(iref.Value is PdfDictionary, "Reference to dictionary expected.");
                    dict = (PdfDictionary)iref.Value;
                }
                else
                {
                    Debug.Assert(item is PdfDictionary, "Dictionary expected.");
                    dict = (PdfDictionary)item;
                }
                PdfAnnotation annotation = dict as PdfAnnotation;
                if (annotation == null)
                {
                    annotation = new PdfGenericAnnotation(dict);
                    if (iref == null)
                        Elements[index] = annotation;
                }
                return annotation;
            }
        }

        internal PdfPage Page
        {
            get { return _page; }
            set { _page = value; }
        }
        PdfPage _page;

        internal static void FixImportedAnnotation(PdfPage page)
        {
            PdfArray annots = page.Elements.GetArray(PdfPage.Keys.Annots);
            if (annots != null)
            {
                int count = annots.Elements.Count;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfDictionary annot = annots.Elements.GetDictionary(idx);
                    if (annot != null && annot.Elements.ContainsKey("/P"))
                        annot.Elements["/P"] = page.Reference;
                }
            }
        }

        public override IEnumerator<PdfItem> GetEnumerator()
        {
            return (IEnumerator<PdfItem>)new AnnotationsIterator(this);
        }
        class AnnotationsIterator : IEnumerator<PdfItem>
        {
            public AnnotationsIterator(PdfAnnotations annotations)
            {
                _annotations = annotations;
                _index = -1;
            }

            public PdfItem Current
            {
                get { return _annotations[_index]; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return ++_index < _annotations.Count;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
            }

            readonly PdfAnnotations _annotations;
            int _index;
        }
    }
}
