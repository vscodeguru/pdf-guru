using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Annotations;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("(PageCount={Count})")]
    public sealed class PdfPages : PdfDictionary, IEnumerable<PdfPage>
    {
        internal PdfPages(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Pages");
            Elements[Keys.Count] = new PdfInteger(0);
        }

        internal PdfPages(PdfDictionary dictionary)
            : base(dictionary)
        { }

        public int Count
        {
            get { return PagesArray.Elements.Count; }
        }

        public PdfPage this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.PageIndexOutOfRange);

                PdfDictionary dict = (PdfDictionary)((PdfReference)PagesArray.Elements[index]).Value;
                if (!(dict is PdfPage))
                    dict = new PdfPage(dict);
                return (PdfPage)dict;
            }
        }

        internal PdfPage FindPage(PdfObjectID id)    
        {
            PdfPage page = null;
            foreach (PdfItem item in PagesArray)
            {
                PdfReference reference = item as PdfReference;
                if (reference != null)
                {
                    PdfDictionary dictionary = reference.Value as PdfDictionary;
                    if (dictionary != null && dictionary.ObjectID == id)
                    {
                        page = dictionary as PdfPage ?? new PdfPage(dictionary);
                        break;
                    }
                }
            }
            return page;
        }

        public PdfPage Add()
        {
            PdfPage page = new PdfPage();
            Insert(Count, page);
            return page;
        }

        public PdfPage Add(PdfPage page)
        {
            return Insert(Count, page);
        }

        public PdfPage Insert(int index)
        {
            PdfPage page = new PdfPage();
            Insert(index, page);
            return page;
        }

        public PdfPage Insert(int index, PdfPage page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (page.Owner == Owner)
            {
                int count = Count;
                for (int idx = 0; idx < count; idx++)
                {
                    if (ReferenceEquals(this[idx], page))
                        throw new InvalidOperationException(PSSR.MultiplePageInsert);
                }

                Owner._irefTable.Add(page);
                Debug.Assert(page.Owner == Owner);

                PagesArray.Elements.Insert(index, page.Reference);

                Elements.SetInteger(Keys.Count, PagesArray.Elements.Count);

                return page;
            }

            if (page.Owner == null)
            {
                page.Document = Owner;

                Owner._irefTable.Add(page);
                Debug.Assert(page.Owner == Owner);
                PagesArray.Elements.Insert(index, page.Reference);
                Elements.SetInteger(Keys.Count, PagesArray.Elements.Count);
            }
            else
            {
                PdfPage importPage = page;
                page = ImportExternalPage(importPage);
                Owner._irefTable.Add(page);

                PdfImportedObjectTable importedObjectTable = Owner.FormTable.GetImportedObjectTable(importPage);
                importedObjectTable.Add(importPage.ObjectID, page.Reference);

                PagesArray.Elements.Insert(index, page.Reference);
                Elements.SetInteger(Keys.Count, PagesArray.Elements.Count);
                PdfAnnotations.FixImportedAnnotation(page);
            }
            if (Owner.Settings.TrimMargins.AreSet)
                page.TrimMargins = Owner.Settings.TrimMargins;
            
            return page;
        }

        public void InsertRange(int index, PdfDocument document, int startIndex, int pageCount)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException("index", "Argument 'index' out of range.");

            int importDocumentPageCount = document.PageCount;

            if (startIndex < 0 || startIndex + pageCount > importDocumentPageCount)
                throw new ArgumentOutOfRangeException("startIndex", "Argument 'startIndex' out of range.");

            if (pageCount > importDocumentPageCount)
                throw new ArgumentOutOfRangeException("pageCount", "Argument 'pageCount' out of range.");

            PdfPage[] insertPages = new PdfPage[pageCount];
            PdfPage[] importPages = new PdfPage[pageCount];

            for (int idx = 0, insertIndex = index, importIndex = startIndex;
                importIndex < startIndex + pageCount;
                idx++, insertIndex++, importIndex++)
            {
                PdfPage importPage = document.Pages[importIndex];
                PdfPage page = ImportExternalPage(importPage);
                insertPages[idx] = page;
                importPages[idx] = importPage;

                Owner._irefTable.Add(page);

                PdfImportedObjectTable importedObjectTable = Owner.FormTable.GetImportedObjectTable(importPage);
                importedObjectTable.Add(importPage.ObjectID, page.Reference);

                PagesArray.Elements.Insert(insertIndex, page.Reference);

                if (Owner.Settings.TrimMargins.AreSet)
                    page.TrimMargins = Owner.Settings.TrimMargins;
            }
            Elements.SetInteger(Keys.Count, PagesArray.Elements.Count);

            for (int idx = 0, importIndex = startIndex;
                importIndex < startIndex + pageCount;
                idx++, importIndex++)
            {
                PdfPage importPage = document.Pages[importIndex];
                PdfPage page = insertPages[idx];

                PdfArray annots = importPage.Elements.GetArray(PdfPage.Keys.Annots);
                if (annots != null)
                {
                    PdfAnnotations annotations = new PdfAnnotations(Owner);

                    int count = annots.Elements.Count;
                    for (int idxAnnotation = 0; idxAnnotation < count; idxAnnotation++)
                    {
                        PdfDictionary annot = annots.Elements.GetDictionary(idxAnnotation);
                        if (annot != null)
                        {
                            string subtype = annot.Elements.GetString(PdfAnnotation.Keys.Subtype);
                            if (subtype == "/Link")
                            {
                                bool addAnnotation = false;
                                PdfLinkAnnotation newAnnotation = new PdfLinkAnnotation(Owner);

                                PdfName[] importAnnotationKeyNames = annot.Elements.KeyNames;
                                foreach (PdfName pdfItem in importAnnotationKeyNames)
                                {
                                    PdfItem impItem;
                                    switch (pdfItem.Value)
                                    {
                                        case "/BS":
                                            newAnnotation.Elements.Add("/BS", new PdfLiteral("<</W 0>>"));
                                            break;

                                        case "/F":    
                                            impItem = annot.Elements.GetValue("/F");
                                            Debug.Assert(impItem is PdfInteger);
                                            newAnnotation.Elements.Add("/F", impItem.Clone());
                                            break;

                                        case "/Rect":       
                                            impItem = annot.Elements.GetValue("/Rect");
                                            Debug.Assert(impItem is PdfArray);
                                            newAnnotation.Elements.Add("/Rect", impItem.Clone());
                                            break;

                                        case "/StructParent":    
                                            impItem = annot.Elements.GetValue("/StructParent");
                                            Debug.Assert(impItem is PdfInteger);
                                            newAnnotation.Elements.Add("/StructParent", impItem.Clone());
                                            break;

                                        case "/Subtype":    
                                            break;

                                        case "/Dest":          
                                            impItem = annot.Elements.GetValue("/Dest");
                                            impItem = impItem.Clone();

                                            PdfArray destArray = impItem as PdfArray;
                                            if (destArray != null && destArray.Elements.Count == 5)
                                            {
                                                PdfReference iref = destArray.Elements[0] as PdfReference;
                                                if (iref != null)
                                                {
                                                    iref = RemapReference(insertPages, importPages, iref);
                                                    if (iref != null)
                                                    {
                                                        destArray.Elements[0] = iref;
                                                        newAnnotation.Elements.Add("/Dest", destArray);
                                                        addAnnotation = true;
                                                    }
                                                }
                                            }
                                            break;

                                        default:
                                            break;

                                    }
                                }
                                if (addAnnotation)
                                    annotations.Add(newAnnotation);
                            }
                        }
                    }

                    if (annotations.Count > 0)
                    {
                        page.Elements.Add(PdfPage.Keys.Annots, annotations);
                    }
                }

            }
        }

        public void InsertRange(int index, PdfDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            InsertRange(index, document, 0, document.PageCount);
        }

        public void InsertRange(int index, PdfDocument document, int startIndex)
        {
            if (document == null)
                throw new ArgumentNullException("document");

            InsertRange(index, document, startIndex, document.PageCount - startIndex);
        }

        public void Remove(PdfPage page)
        {
            PagesArray.Elements.Remove(page.Reference);
            Elements.SetInteger(Keys.Count, PagesArray.Elements.Count);
        }

        public void RemoveAt(int index)
        {
            PagesArray.Elements.RemoveAt(index);
            Elements.SetInteger(Keys.Count, PagesArray.Elements.Count);
        }

        public void MovePage(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= Count)
                throw new ArgumentOutOfRangeException("oldIndex");
            if (newIndex < 0 || newIndex >= Count)
                throw new ArgumentOutOfRangeException("newIndex");
            if (oldIndex == newIndex)
                return;

            PdfReference page = (PdfReference)_pagesArray.Elements[oldIndex];
            _pagesArray.Elements.RemoveAt(oldIndex);
            _pagesArray.Elements.Insert(newIndex, page);
        }

        PdfPage ImportExternalPage(PdfPage importPage)
        {
            if (importPage.Owner._openMode != PdfDocumentOpenMode.Import)
                throw new InvalidOperationException("A PDF document must be opened with PdfDocumentOpenMode.Import to import pages from it.");

            PdfPage page = new PdfPage(_document);

            CloneElement(page, importPage, PdfPage.Keys.Resources, false);
            CloneElement(page, importPage, PdfPage.Keys.Contents, false);
            CloneElement(page, importPage, PdfPage.Keys.MediaBox, true);
            CloneElement(page, importPage, PdfPage.Keys.CropBox, true);
            CloneElement(page, importPage, PdfPage.Keys.Rotate, true);
            CloneElement(page, importPage, PdfPage.Keys.BleedBox, true);
            CloneElement(page, importPage, PdfPage.Keys.TrimBox, true);
            CloneElement(page, importPage, PdfPage.Keys.ArtBox, true);
#if true
            CloneElement(page, importPage, PdfPage.Keys.Annots, false);
#endif
            return page;
        }

        void CloneElement(PdfPage page, PdfPage importPage, string key, bool deepcopy)
        {
            Debug.Assert(page != null);
            Debug.Assert(page.Owner == _document);
            Debug.Assert(importPage.Owner != null);
            Debug.Assert(importPage.Owner != _document);

            PdfItem item = importPage.Elements[key];
            if (item != null)
            {
                PdfImportedObjectTable importedObjectTable = null;
                if (!deepcopy)
                    importedObjectTable = Owner.FormTable.GetImportedObjectTable(importPage);

                if (item is PdfReference)
                    item = ((PdfReference)item).Value;
                if (item is PdfObject)
                {
                    PdfObject root = (PdfObject)item;
                    if (deepcopy)
                    {
                        Debug.Assert(root.Owner != null, "See 'else' case for details");
                        root = DeepCopyClosure(_document, root);
                    }
                    else
                    {
                        if (root.Owner == null)
                            root.Document = importPage.Owner;
                        root = ImportClosure(importedObjectTable, page.Owner, root);
                    }

                    if (root.Reference == null)
                        page.Elements[key] = root;
                    else
                        page.Elements[key] = root.Reference;
                }
                else
                {
                    page.Elements[key] = item.Clone();
                }
            }
        }

        static PdfReference RemapReference(PdfPage[] newPages, PdfPage[] impPages, PdfReference iref)
        {
            for (int idx = 0; idx < newPages.Length; idx++)
            {
                if (impPages[idx].Reference == iref)
                    return newPages[idx].Reference;
            }
            return null;
        }

        public PdfArray PagesArray
        {
            get
            {
                if (_pagesArray == null)
                    _pagesArray = (PdfArray)Elements.GetValue(Keys.Kids, VCF.Create);
                return _pagesArray;
            }
        }
        PdfArray _pagesArray;

        internal void FlattenPageTree()
        {
            PdfPage.InheritedValues values = new PdfPage.InheritedValues();
            PdfPage.InheritValues(this, ref values);
            PdfDictionary[] pages = GetKids(Reference, values, null);

            PdfArray array = new PdfArray(Owner);
            foreach (PdfDictionary page in pages)
            {
                page.Elements[PdfPage.Keys.Parent] = Reference;
                array.Elements.Add(page.Reference);
            }

            Elements.SetName(Keys.Type, "/Pages");
#if true
            Elements.SetValue(Keys.Kids, array);
#endif
            Elements.SetInteger(Keys.Count, array.Elements.Count);
        }

        PdfDictionary[] GetKids(PdfReference iref, PdfPage.InheritedValues values, PdfDictionary parent)
        {
            PdfDictionary kid = (PdfDictionary)iref.Value;

#if true
            string type = kid.Elements.GetName(Keys.Type);
            if (type == "/Page")
            {
                PdfPage.InheritValues(kid, values);
                return new PdfDictionary[] { kid };
            }

            if (string.IsNullOrEmpty(type))
            {
                PdfPage.InheritValues(kid, values);
                return new PdfDictionary[] { kid };
            }

#endif

            Debug.Assert(kid.Elements.GetName(Keys.Type) == "/Pages");
            PdfPage.InheritValues(kid, ref values);
            List<PdfDictionary> list = new List<PdfDictionary>();
            PdfArray kids = kid.Elements["/Kids"] as PdfArray;

            if (kids == null)
            {
                PdfReference xref3 = kid.Elements["/Kids"] as PdfReference;
                if (xref3 != null)
                    kids = xref3.Value as PdfArray;
            }

            foreach (PdfReference xref2 in kids)
                list.AddRange(GetKids(xref2, values, kid));
            int count = list.Count;
            Debug.Assert(count == kid.Elements.GetInteger("/Count"));
            return list.ToArray();
        }

        internal override void PrepareForSave()
        {
            int count = _pagesArray.Elements.Count;
            for (int idx = 0; idx < count; idx++)
            {
                PdfPage page = this[idx];
                page.PrepareForSave();
            }
        }

        public new IEnumerator<PdfPage> GetEnumerator()
        {
            return new PdfPagesEnumerator(this);
        }

        class PdfPagesEnumerator : IEnumerator<PdfPage>
        {
            internal PdfPagesEnumerator(PdfPages list)
            {
                _list = list;
                _index = -1;
            }

            public bool MoveNext()
            {
                if (_index < _list.Count - 1)
                {
                    _index++;
                    _currentElement = _list[_index];
                    return true;
                }
                _index = _list.Count;
                return false;
            }

            public void Reset()
            {
                _currentElement = null;
                _index = -1;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public PdfPage Current
            {
                get
                {
                    if (_index == -1 || _index >= _list.Count)
                        throw new InvalidOperationException(PSSR.ListEnumCurrentOutOfRange);
                    return _currentElement;
                }
            }

            public void Dispose()
            {
            }

            PdfPage _currentElement;
            int _index;
            readonly PdfPages _list;
        }

        internal sealed class Keys : PdfPage.InheritablePageKeys
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Pages")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string Parent = "/Parent";

            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string Kids = "/Kids";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string Count = "/Count";

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
