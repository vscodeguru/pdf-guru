using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Text;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class PdfArray : PdfObject, IEnumerable<PdfItem>
    {
        public PdfArray()
        { }

        public PdfArray(PdfDocument document)
            : base(document)
        { }

        public PdfArray(PdfDocument document, params PdfItem[] items)
            : base(document)
        {
            foreach (PdfItem item in items)
                Elements.Add(item);
        }

        protected PdfArray(PdfArray array)
            : base(array)
        {
            if (array._elements != null)
                array._elements.ChangeOwner(this);
        }

        public new PdfArray Clone()
        {
            return (PdfArray)Copy();
        }

        protected override object Copy()
        {
            PdfArray array = (PdfArray)base.Copy();
            if (array._elements != null)
            {
                array._elements = array._elements.Clone();
                int count = array._elements.Count;
                for (int idx = 0; idx < count; idx++)
                {
                    PdfItem item = array._elements[idx];
                    if (item is PdfObject)
                        array._elements[idx] = item.Clone();
                }
            }
            return array;
        }

        public ArrayElements Elements
        {
            get { return _elements ?? (_elements = new ArrayElements(this)); }
        }

        public virtual IEnumerator<PdfItem> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder pdf = new StringBuilder();
            pdf.Append("[ ");
            int count = Elements.Count;
            for (int idx = 0; idx < count; idx++)
                pdf.Append(Elements[idx] + " ");
            pdf.Append("]");
            return pdf.ToString();
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteBeginObject(this);
            int count = Elements.Count;
            for (int idx = 0; idx < count; idx++)
            {
                PdfItem value = Elements[idx];
                value.WriteObject(writer);
            }
            writer.WriteEndObject();
        }

        public sealed class ArrayElements : IList<PdfItem>, ICloneable
        {
            internal ArrayElements(PdfArray array)
            {
                _elements = new List<PdfItem>();
                _ownerArray = array;
            }

            object ICloneable.Clone()
            {
                ArrayElements elements = (ArrayElements)MemberwiseClone();
                elements._elements = new List<PdfItem>(elements._elements);
                elements._ownerArray = null;
                return elements;
            }

            public ArrayElements Clone()
            {
                return (ArrayElements)((ICloneable)this).Clone();
            }

            internal void ChangeOwner(PdfArray array)
            {
                if (_ownerArray != null)
                {
                }

                _ownerArray = array;

                array._elements = this;
            }

            public bool GetBoolean(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                object obj = this[index];
                if (obj == null)
                    return false;

                PdfBoolean boolean = obj as PdfBoolean;
                if (boolean != null)
                    return boolean.Value;

                PdfBooleanObject booleanObject = obj as PdfBooleanObject;
                if (booleanObject != null)
                    return booleanObject.Value;

                throw new InvalidCastException("GetBoolean: Object is not a boolean.");
            }

            public int GetInteger(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                object obj = this[index];
                if (obj == null)
                    return 0;

                PdfInteger integer = obj as PdfInteger;
                if (integer != null)
                    return integer.Value;

                PdfIntegerObject integerObject = obj as PdfIntegerObject;
                if (integerObject != null)
                    return integerObject.Value;

                throw new InvalidCastException("GetInteger: Object is not an integer.");
            }

            public double GetReal(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                object obj = this[index];
                if (obj == null)
                    return 0;

                PdfReal real = obj as PdfReal;
                if (real != null)
                    return real.Value;

                PdfRealObject realObject = obj as PdfRealObject;
                if (realObject != null)
                    return realObject.Value;

                PdfInteger integer = obj as PdfInteger;
                if (integer != null)
                    return integer.Value;

                PdfIntegerObject integerObject = obj as PdfIntegerObject;
                if (integerObject != null)
                    return integerObject.Value;

                throw new InvalidCastException("GetReal: Object is not a number.");
            }

            public double? GetNullableReal(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                object obj = this[index];
                if (obj == null)
                    return null;

                PdfNull @null = obj as PdfNull;
                if (@null != null)
                    return null;

                PdfNullObject nullObject = obj as PdfNullObject;
                if (nullObject != null)
                    return null;

                PdfReal real = obj as PdfReal;
                if (real != null)
                    return real.Value;

                PdfRealObject realObject = obj as PdfRealObject;
                if (realObject != null)
                    return realObject.Value;

                PdfInteger integer = obj as PdfInteger;
                if (integer != null)
                    return integer.Value;

                PdfIntegerObject integerObject = obj as PdfIntegerObject;
                if (integerObject != null)
                    return integerObject.Value;

                throw new InvalidCastException("GetReal: Object is not a number.");
            }

            public string GetString(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                object obj = this[index];
                if (obj == null)
                    return String.Empty;

                PdfString str = obj as PdfString;
                if (str != null)
                    return str.Value;

                PdfStringObject strObject = obj as PdfStringObject;
                if (strObject != null)
                    return strObject.Value;

                throw new InvalidCastException("GetString: Object is not a string.");
            }

            public string GetName(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                object obj = this[index];
                if (obj == null)
                    return String.Empty;

                PdfName name = obj as PdfName;
                if (name != null)
                    return name.Value;

                PdfNameObject nameObject = obj as PdfNameObject;
                if (nameObject != null)
                    return nameObject.Value;

                throw new InvalidCastException("GetName: Object is not a name.");
            }

            [Obsolete("Use GetObject, GetDictionary, GetArray, or GetReference")]
            public PdfObject GetIndirectObject(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                PdfReference reference = this[index] as PdfReference;
                if (reference != null)
                    return reference.Value;

                return null;
            }

            public PdfObject GetObject(int index)
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index", index, PSSR.IndexOutOfRange);

                PdfItem item = this[index];
                PdfReference reference = item as PdfReference;
                if (reference != null)
                    return reference.Value;

                return item as PdfObject;
            }

            public PdfDictionary GetDictionary(int index)
            {
                return GetObject(index) as PdfDictionary;
            }

            public PdfArray GetArray(int index)
            {
                return GetObject(index) as PdfArray;
            }

            public PdfReference GetReference(int index)
            {
                PdfItem item = this[index];
                return item as PdfReference;
            }

            public PdfItem[] Items
            {
                get { return _elements.ToArray(); }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public PdfItem this[int index]
            {
                get { return _elements[index]; }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException("value");
                    _elements[index] = value;
                }
            }

            public void RemoveAt(int index)
            {
                _elements.RemoveAt(index);
            }

            public bool Remove(PdfItem item)
            {
                return _elements.Remove(item);
            }

            public void Insert(int index, PdfItem value)
            {
                _elements.Insert(index, value);
            }

            public bool Contains(PdfItem value)
            {
                return _elements.Contains(value);
            }

            public void Clear()
            {
                _elements.Clear();
            }

            public int IndexOf(PdfItem value)
            {
                return _elements.IndexOf(value);
            }

            public void Add(PdfItem value)
            {
                PdfObject obj = value as PdfObject;
                if (obj != null && obj.IsIndirect)
                    _elements.Add(obj.Reference);
                else
                    _elements.Add(value);
            }

            public bool IsFixedSize
            {
                get { return false; }
            }

            public bool IsSynchronized
            {
                get { return false; }
            }

            public int Count
            {
                get { return _elements.Count; }
            }

            public void CopyTo(PdfItem[] array, int index)
            {
                _elements.CopyTo(array, index);
            }

            public object SyncRoot
            {
                get { return null; }
            }

            public IEnumerator<PdfItem> GetEnumerator()
            {
                return _elements.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _elements.GetEnumerator();
            }

            List<PdfItem> _elements;

            PdfArray _ownerArray;
        }

        ArrayElements _elements;

        string DebuggerDisplay
        {
            get
            {
#if true
                return String.Format(CultureInfo.InvariantCulture, "array({0},[{1}])", ObjectID.DebuggerDisplay, _elements == null ? 0 : _elements.Count);
#endif
            }
        }
    }
}
