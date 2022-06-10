using System;
using System.Collections.Generic;
using System.Collections;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf
{
    public class PdfOutlineCollection : PdfObject, ICollection<PdfOutline>, IList<PdfOutline>
    {
        internal PdfOutlineCollection(PdfDocument document, PdfOutline parent)
            : base(document)
        {
            _parent = parent;
        }

        [Obsolete("Use 'Count > 0' - HasOutline will throw exception.")]
        public bool HasOutline    
        {
            get
            {
                throw new InvalidOperationException("Use 'Count > 0'");
            }
        }

        public bool Remove(PdfOutline item)
        {
            if (_outlines.Remove(item))
            {
                RemoveFromOutlinesTree(item);
                return true;
            }
            return false;
        }

        public int Count
        {
            get { return _outlines.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Add(PdfOutline outline)
        {
            if (outline == null)
                throw new ArgumentNullException("outline");

            if (outline.DestinationPage != null && !ReferenceEquals(Owner, outline.DestinationPage.Owner))
                throw new ArgumentException("Destination page must belong to this document.");

            AddToOutlinesTree(outline);
            _outlines.Add(outline);

            if (outline.Opened)
            {
                outline = _parent;
                while (outline != null)
                {
                    outline.OpenCount++;
                    outline = outline.Parent;
                }
            }
        }

        public void Clear()
        {
            if (Count > 0)
            {
                PdfOutline[] array = new PdfOutline[Count];
                _outlines.CopyTo(array);
                _outlines.Clear();
                foreach (PdfOutline item in array)
                {
                    RemoveFromOutlinesTree(item);
                }
            }
        }

        public bool Contains(PdfOutline item)
        {
            return _outlines.Contains(item);
        }

        public void CopyTo(PdfOutline[] array, int arrayIndex)
        {
            _outlines.CopyTo(array, arrayIndex);
        }

        public PdfOutline Add(string title, PdfPage destinationPage, bool opened, PdfOutlineStyle style, XColor textColor)
        {
            PdfOutline outline = new PdfOutline(title, destinationPage, opened, style, textColor);
            Add(outline);
            return outline;
        }

        public PdfOutline Add(string title, PdfPage destinationPage, bool opened, PdfOutlineStyle style)
        {
            PdfOutline outline = new PdfOutline(title, destinationPage, opened, style);
            Add(outline);
            return outline;
        }

        public PdfOutline Add(string title, PdfPage destinationPage, bool opened)
        {
            PdfOutline outline = new PdfOutline(title, destinationPage, opened);
            Add(outline);
            return outline;
        }

        public PdfOutline Add(string title, PdfPage destinationPage)
        {
            PdfOutline outline = new PdfOutline(title, destinationPage);
            Add(outline);
            return outline;
        }

        public int IndexOf(PdfOutline item)
        {
            return _outlines.IndexOf(item);
        }

        public void Insert(int index, PdfOutline outline)
        {
            if (outline == null)
                throw new ArgumentNullException("outline");
            if (index < 0 || index >= _outlines.Count)
                throw new ArgumentOutOfRangeException("index", index, PSSR.OutlineIndexOutOfRange);

            AddToOutlinesTree(outline);
            _outlines.Insert(index, outline);
        }

        public void RemoveAt(int index)
        {
            PdfOutline outline = _outlines[index];
            _outlines.RemoveAt(index);
            RemoveFromOutlinesTree(outline);
        }

        public PdfOutline this[int index]
        {
            get
            {
                if (index < 0 || index >= _outlines.Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.OutlineIndexOutOfRange);
                return _outlines[index];
            }
            set
            {
                if (index < 0 || index >= _outlines.Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.OutlineIndexOutOfRange);
                if (value == null)
                    throw new ArgumentOutOfRangeException("value", null, PSSR.SetValueMustNotBeNull);

                AddToOutlinesTree(value);
                _outlines[index] = value;
            }
        }

        public IEnumerator<PdfOutline> GetEnumerator()
        {
            return _outlines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal int CountOpen()
        {
            int count = 0;
            return count;
        }

        void AddToOutlinesTree(PdfOutline outline)
        {
            if (outline == null)
                throw new ArgumentNullException("outline");

            if (outline.DestinationPage != null && !ReferenceEquals(Owner, outline.DestinationPage.Owner))
                throw new ArgumentException("Destination page must belong to this document.");

            outline.Document = Owner;
            outline.Parent = _parent;

            if (!Owner._irefTable.Contains(outline.ObjectID))
                Owner._irefTable.Add(outline);
            else
            {
                outline.GetType();
            }

        }

        void RemoveFromOutlinesTree(PdfOutline outline)
        {
            if (outline == null)
                throw new ArgumentNullException("outline");

            outline.Parent = null;

            Owner._irefTable.Remove(outline.Reference);
        }

        readonly PdfOutline _parent;

        readonly List<PdfOutline> _outlines = new List<PdfOutline>();
    }
}
