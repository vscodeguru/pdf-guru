using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Filters;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf
{
    public enum VCF
    {
        None,

        Create,

        CreateIndirect,
    }

    [DebuggerDisplay("{DebuggerDisplay}")]
    public class PdfDictionary : PdfObject, IEnumerable<KeyValuePair<string, PdfItem>>
    {
        public PdfDictionary()
        { }

        public PdfDictionary(PdfDocument document)
            : base(document)
        { }

        protected PdfDictionary(PdfDictionary dict)
            : base(dict)
        {
            if (dict._elements != null)
                dict._elements.ChangeOwner(this);
            if (dict._stream != null)
                dict._stream.ChangeOwner(this);
        }

        public new PdfDictionary Clone()
        {
            return (PdfDictionary)Copy();
        }

        protected override object Copy()
        {
            PdfDictionary dict = (PdfDictionary)base.Copy();
            if (dict._elements != null)
            {
                dict._elements = dict._elements.Clone();
                dict._elements.ChangeOwner(dict);
                PdfName[] names = dict._elements.KeyNames;
                foreach (PdfName name in names)
                {
                    PdfObject obj = dict._elements[name] as PdfObject;
                    if (obj != null)
                    {
                        obj = obj.Clone();
                        dict._elements[name] = obj;
                    }
                }
            }
            if (dict._stream != null)
            {
                dict._stream = dict._stream.Clone();
                dict._stream.ChangeOwner(dict);
            }
            return dict;
        }

        public DictionaryElements Elements
        {
            get { return _elements ?? (_elements = new DictionaryElements(this)); }
        }

        internal DictionaryElements _elements;

        public IEnumerator<KeyValuePair<string, PdfItem>> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            PdfName[] keys = Elements.KeyNames;
            List<PdfName> list = new List<PdfName>(keys);
            list.Sort(PdfName.Comparer);
            list.CopyTo(keys, 0);

            StringBuilder pdf = new StringBuilder();
            pdf.Append("<< ");
            foreach (PdfName key in keys)
                pdf.Append(key + " " + Elements[key] + " ");
            pdf.Append(">>");

            return pdf.ToString();
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteBeginObject(this);
            PdfName[] keys = Elements.KeyNames;

            foreach (PdfName key in keys)
                WriteDictionaryElement(writer, key);
            if (Stream != null)
                WriteDictionaryStream(writer);
            writer.WriteEndObject();
        }

        internal virtual void WriteDictionaryElement(PdfWriter writer, PdfName key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            PdfItem item = Elements[key];
            key.WriteObject(writer);
            item.WriteObject(writer);
            writer.NewLine();
        }

        internal virtual void WriteDictionaryStream(PdfWriter writer)
        {
            writer.WriteStream(this, (writer.Options & PdfWriterOptions.OmitStream) == PdfWriterOptions.OmitStream);
        }

        public PdfStream Stream
        {
            get { return _stream; }
            set { _stream = value; }
        }
        PdfStream _stream;

        public PdfStream CreateStream(byte[] value)
        {
            if (_stream != null)
                throw new InvalidOperationException("The dictionary already has a stream.");

            _stream = new PdfStream(value, this);
            Elements[PdfStream.Keys.Length] = new PdfInteger(_stream.Length);
            return _stream;
        }

        internal virtual DictionaryMeta Meta
        {
            get { return null; }
        }

        [DebuggerDisplay("{DebuggerDisplay}")]
        public sealed class DictionaryElements : IDictionary<string, PdfItem>, ICloneable
        {
            internal DictionaryElements(PdfDictionary ownerDictionary)
            {
                _elements = new Dictionary<string, PdfItem>();
                _ownerDictionary = ownerDictionary;
            }

            object ICloneable.Clone()
            {
                DictionaryElements dictionaryElements = (DictionaryElements)MemberwiseClone();
                dictionaryElements._elements = new Dictionary<string, PdfItem>(dictionaryElements._elements);
                dictionaryElements._ownerDictionary = null;
                return dictionaryElements;
            }

            public DictionaryElements Clone()
            {
                return (DictionaryElements)((ICloneable)this).Clone();
            }

            internal void ChangeOwner(PdfDictionary ownerDictionary)
            {
                if (_ownerDictionary != null)
                {
                }

                _ownerDictionary = ownerDictionary;

                ownerDictionary._elements = this;
            }

            internal PdfDictionary Owner
            {
                get { return _ownerDictionary; }
            }

            public bool GetBoolean(string key, bool create)
            {
                object obj = this[key];
                if (obj == null)
                {
                    if (create)
                        this[key] = new PdfBoolean();
                    return false;
                }

                if (obj is PdfReference)
                    obj = ((PdfReference)obj).Value;

                PdfBoolean boolean = obj as PdfBoolean;
                if (boolean != null)
                    return boolean.Value;

                PdfBooleanObject booleanObject = obj as PdfBooleanObject;
                if (booleanObject != null)
                    return booleanObject.Value;
                throw new InvalidCastException("GetBoolean: Object is not a boolean.");
            }

            public bool GetBoolean(string key)
            {
                return GetBoolean(key, false);
            }

            public void SetBoolean(string key, bool value)
            {
                this[key] = new PdfBoolean(value);
            }

            public int GetInteger(string key, bool create)
            {
                object obj = this[key];
                if (obj == null)
                {
                    if (create)
                        this[key] = new PdfInteger();
                    return 0;
                }
                PdfReference reference = obj as PdfReference;
                if (reference != null)
                    obj = reference.Value;

                PdfInteger integer = obj as PdfInteger;
                if (integer != null)
                    return integer.Value;

                PdfIntegerObject integerObject = obj as PdfIntegerObject;
                if (integerObject != null)
                    return integerObject.Value;

                throw new InvalidCastException("GetInteger: Object is not an integer.");
            }

            public int GetInteger(string key)
            {
                return GetInteger(key, false);
            }

            public void SetInteger(string key, int value)
            {
                this[key] = new PdfInteger(value);
            }

            public double GetReal(string key, bool create)
            {
                object obj = this[key];
                if (obj == null)
                {
                    if (create)
                        this[key] = new PdfReal();
                    return 0;
                }

                PdfReference reference = obj as PdfReference;
                if (reference != null)
                    obj = reference.Value;

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

            public double GetReal(string key)
            {
                return GetReal(key, false);
            }

            public void SetReal(string key, double value)
            {
                this[key] = new PdfReal(value);
            }

            public string GetString(string key, bool create)
            {
                object obj = this[key];
                if (obj == null)
                {
                    if (create)
                        this[key] = new PdfString();
                    return "";
                }

                PdfReference reference = obj as PdfReference;
                if (reference != null)
                    obj = reference.Value;

                PdfString str = obj as PdfString;
                if (str != null)
                    return str.Value;

                PdfStringObject strObject = obj as PdfStringObject;
                if (strObject != null)
                    return strObject.Value;

                PdfName name = obj as PdfName;
                if (name != null)
                    return name.Value;

                PdfNameObject nameObject = obj as PdfNameObject;
                if (nameObject != null)
                    return nameObject.Value;

                throw new InvalidCastException("GetString: Object is not a string.");
            }

            public string GetString(string key)
            {
                return GetString(key, false);
            }

            public bool TryGetString(string key, out string value)
            {
                value = null;
                object obj = this[key];
                if (obj == null)
                    return false;

                PdfReference reference = obj as PdfReference;
                if (reference != null)
                    obj = reference.Value;

                PdfString str = obj as PdfString;
                if (str != null)
                {
                    value = str.Value;
                    return true;
                }

                PdfStringObject strObject = obj as PdfStringObject;
                if (strObject != null)
                {
                    value = strObject.Value;
                    return true;
                }

                PdfName name = obj as PdfName;
                if (name != null)
                {
                    value = name.Value;
                    return true;
                }

                PdfNameObject nameObject = obj as PdfNameObject;
                if (nameObject != null)
                {
                    value = nameObject.Value;
                    return true;
                }

                return false;
            }

            public void SetString(string key, string value)
            {
                this[key] = new PdfString(value);
            }

            public string GetName(string key)
            {
                object obj = this[key];
                if (obj == null)
                {
                    return String.Empty;
                }

                PdfReference reference = obj as PdfReference;
                if (reference != null)
                    obj = reference.Value;

                PdfName name = obj as PdfName;
                if (name != null)
                    return name.Value;

                PdfNameObject nameObject = obj as PdfNameObject;
                if (nameObject != null)
                    return nameObject.Value;

                throw new InvalidCastException("GetName: Object is not a name.");
            }

            public void SetName(string key, string value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value.Length == 0 || value[0] != '/')
                    value = "/" + value;

                this[key] = new PdfName(value);
            }

            public PdfRectangle GetRectangle(string key, bool create)
            {
                PdfRectangle value = new PdfRectangle();
                object obj = this[key];
                if (obj == null)
                {
                    if (create)
                        this[key] = value = new PdfRectangle();
                    return value;
                }
                if (obj is PdfReference)
                    obj = ((PdfReference)obj).Value;

                PdfArray array = obj as PdfArray;
                if (array != null && array.Elements.Count == 4)
                {
                    value = new PdfRectangle(array.Elements.GetReal(0), array.Elements.GetReal(1),
                      array.Elements.GetReal(2), array.Elements.GetReal(3));
                    this[key] = value;
                }
                else
                    value = (PdfRectangle)obj;
                return value;
            }

            public PdfRectangle GetRectangle(string key)
            {
                return GetRectangle(key, false);
            }

            public void SetRectangle(string key, PdfRectangle rect)
            {
                _elements[key] = rect;
            }

            public XMatrix GetMatrix(string key, bool create)
            {
                XMatrix value = new XMatrix();
                object obj = this[key];
                if (obj == null)
                {
                    if (create)
                        this[key] = new PdfLiteral("[1 0 0 1 0 0]");        
                    return value;
                }
                PdfReference reference = obj as PdfReference;
                if (reference != null)
                    obj = reference.Value;

                PdfArray array = obj as PdfArray;
                if (array != null && array.Elements.Count == 6)
                {
                    value = new XMatrix(array.Elements.GetReal(0), array.Elements.GetReal(1), array.Elements.GetReal(2),
                      array.Elements.GetReal(3), array.Elements.GetReal(4), array.Elements.GetReal(5));
                }
                else if (obj is PdfLiteral)
                {
                    throw new NotImplementedException("Parsing matrix from literal.");
                }
                else
                    throw new InvalidCastException("Element is not an array with 6 values.");
                return value;
            }

            public XMatrix GetMatrix(string key)
            {
                return GetMatrix(key, false);
            }

            public void SetMatrix(string key, XMatrix matrix)
            {
                _elements[key] = PdfLiteral.FromMatrix(matrix);
            }

            public DateTime GetDateTime(string key, DateTime defaultValue)
            {
                object obj = this[key];
                if (obj == null)
                {
                    return defaultValue;
                }

                PdfReference reference = obj as PdfReference;
                if (reference != null)
                    obj = reference.Value;

                PdfDate date = obj as PdfDate;
                if (date != null)
                    return date.Value;

                string strDate;
                PdfString pdfString = obj as PdfString;
                if (pdfString != null)
                    strDate = pdfString.Value;
                else
                {
                    PdfStringObject stringObject = obj as PdfStringObject;
                    if (stringObject != null)
                        strDate = stringObject.Value;
                    else
                        throw new InvalidCastException("GetName: Object is not a name.");
                }

                if (strDate != "")
                {
                    try
                    {
                        defaultValue = Parser.ParseDateTime(strDate, defaultValue);
                    }
                    catch { }
                }
                return defaultValue;
            }

            public void SetDateTime(string key, DateTime value)
            {
                _elements[key] = new PdfDate(value);
            }

            internal int GetEnumFromName(string key, object defaultValue, bool create)
            {
                if (!(defaultValue is Enum))
                    throw new ArgumentException("defaultValue");

                object obj = this[key];
                if (obj == null)
                {
                    if (create)
                        this[key] = new PdfName(defaultValue.ToString());

                    return (int)defaultValue;
                }
                Debug.Assert(obj is Enum);
                return (int)Enum.Parse(defaultValue.GetType(), obj.ToString().Substring(1), false);
            }

            internal int GetEnumFromName(string key, object defaultValue)
            {
                return GetEnumFromName(key, defaultValue, false);
            }

            internal void SetEnumAsName(string key, object value)
            {
                if (!(value is Enum))
                    throw new ArgumentException("value");
                _elements[key] = new PdfName("/" + value);
            }

            public PdfItem GetValue(string key, VCF options)
            {
                PdfObject obj;
                PdfDictionary dict;
                PdfArray array;
                PdfReference iref;
                PdfItem value = this[key];
                if (value == null ||
                    value is PdfNull ||
                    value is PdfReference && ((PdfReference)value).Value is PdfNullObject)
                {
                    if (options != VCF.None)
                    {
                        Type type = GetValueType(key);
                        if (type != null)
                        {
                            Debug.Assert(typeof(PdfItem).IsAssignableFrom(type), "Type not allowed.");
                            if (typeof(PdfDictionary).IsAssignableFrom(type))
                            {
                                value = obj = CreateDictionary(type, null);
                            }
                            else if (typeof(PdfArray).IsAssignableFrom(type))
                            {
                                value = obj = CreateArray(type, null);
                            }
                            else
                                throw new NotImplementedException("Type other than array or dictionary.");

                            if (options == VCF.CreateIndirect)
                            {
                                _ownerDictionary.Owner._irefTable.Add(obj);
                                this[key] = obj.Reference;
                            }
                            else
                                this[key] = obj;
                        }
                        else
                            throw new NotImplementedException("Cannot create value for key: " + key);
                    }
                }
                else
                {
                    if ((iref = value as PdfReference) != null)
                    {
                        value = iref.Value;
                        if (value == null)
                        {
                            throw new InvalidOperationException("Indirect reference without value.");
                        }

                        if (true)   
                        {
                            Type type = GetValueType(key);
                            Debug.Assert(type != null, "No value type specified in meta information. Please send this file to PDFsharp support.");

                            if (type != null && type != value.GetType())
                            {
                                if (typeof(PdfDictionary).IsAssignableFrom(type))
                                {
                                    Debug.Assert(value is PdfDictionary, "Bug in PDFsharp. Please send this file to PDFsharp support.");
                                    value = CreateDictionary(type, (PdfDictionary)value);
                                }
                                else if (typeof(PdfArray).IsAssignableFrom(type))
                                {
                                    Debug.Assert(value is PdfArray, "Bug in PDFsharp. Please send this file to PDFsharp support.");
                                    value = CreateArray(type, (PdfArray)value);
                                }
                                else
                                    throw new NotImplementedException("Type other than array or dictionary.");
                            }

                        }
                        return value;
                    }

                    if (true)   
                    {
                        if ((dict = value as PdfDictionary) != null)
                        {
                            Debug.Assert(!dict.IsIndirect);

                            Type type = GetValueType(key);
                            Debug.Assert(type != null, "No value type specified in meta information. Please send this file to PDFsharp support.");
                            if (dict.GetType() != type)
                                dict = CreateDictionary(type, dict);
                            return dict;
                        }

                        if ((array = value as PdfArray) != null)
                        {
                            Debug.Assert(!array.IsIndirect);

                            Type type = GetValueType(key);
                            if (type != null && type != array.GetType())
                                array = CreateArray(type, array);
                            return array;
                        }
                    }
                }
                return value;
            }

            public PdfItem GetValue(string key)
            {
                return GetValue(key, VCF.None);
            }

            Type GetValueType(string key)      
            {
                Type type = null;
                DictionaryMeta meta = _ownerDictionary.Meta;
                if (meta != null)
                {
                    KeyDescriptor kd = meta[key];
                    if (kd != null)
                        type = kd.GetValueType();
                }
                return type;
            }

            PdfArray CreateArray(Type type, PdfArray oldArray)
            {

                ConstructorInfo ctorInfo;
                PdfArray array;
                if (oldArray == null)
                {
                    ctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null, new Type[] { typeof(PdfDocument) }, null);
                    Debug.Assert(ctorInfo != null, "No appropriate constructor found for type: " + type.Name);
                    array = ctorInfo.Invoke(new object[] { _ownerDictionary.Owner }) as PdfArray;
                }
                else
                {
                    ctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null, new Type[] { typeof(PdfArray) }, null);
                    Debug.Assert(ctorInfo != null, "No appropriate constructor found for type: " + type.Name);
                    array = ctorInfo.Invoke(new object[] { oldArray }) as PdfArray;
                }
                return array;

            }

            PdfDictionary CreateDictionary(Type type, PdfDictionary oldDictionary)
            {

                ConstructorInfo ctorInfo;
                PdfDictionary dict;
                if (oldDictionary == null)
                {
                    ctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null, new Type[] { typeof(PdfDocument) }, null);
                    Debug.Assert(ctorInfo != null, "No appropriate constructor found for type: " + type.Name);
                    dict = ctorInfo.Invoke(new object[] { _ownerDictionary.Owner }) as PdfDictionary;
                }
                else
                {
                    ctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                      null, new Type[] { typeof(PdfDictionary) }, null);
                    Debug.Assert(ctorInfo != null, "No appropriate constructor found for type: " + type.Name);
                    dict = ctorInfo.Invoke(new object[] { oldDictionary }) as PdfDictionary;
                }
                return dict;

            }

            PdfItem CreateValue(Type type, PdfDictionary oldValue)
            {

                ConstructorInfo ctorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null, new Type[] { typeof(PdfDocument) }, null);
                PdfObject obj = ctorInfo.Invoke(new object[] { _ownerDictionary.Owner }) as PdfObject;
                if (oldValue != null)
                {
                    obj.Reference = oldValue.Reference;
                    obj.Reference.Value = obj;
                    if (obj is PdfDictionary)
                    {
                        PdfDictionary dict = (PdfDictionary)obj;
                        dict._elements = oldValue._elements;
                    }
                }
                return obj;

            }

            public void SetValue(string key, PdfItem value)
            {
                Debug.Assert((value is PdfObject && ((PdfObject)value).Reference == null) | !(value is PdfObject),
                    "You try to set an indirect object directly into a dictionary.");

                _elements[key] = value;
            }

            public PdfObject GetObject(string key)
            {
                PdfItem item = this[key];
                PdfReference reference = item as PdfReference;
                if (reference != null)
                    return reference.Value;
                return item as PdfObject;
            }

            public PdfDictionary GetDictionary(string key)
            {
                return GetObject(key) as PdfDictionary;
            }

            public PdfArray GetArray(string key)
            {
                return GetObject(key) as PdfArray;
            }

            public PdfReference GetReference(string key)
            {
                PdfItem item = this[key];
                return item as PdfReference;
            }

            public void SetObject(string key, PdfObject obj)
            {
                if (obj.Reference != null)
                    throw new ArgumentException("PdfObject must not be an indirect object.", "obj");
                this[key] = obj;
            }

            public void SetReference(string key, PdfObject obj)
            {
                if (obj.Reference == null)
                    throw new ArgumentException("PdfObject must be an indirect object.", "obj");
                this[key] = obj.Reference;
            }

            public void SetReference(string key, PdfReference iref)
            {
                if (iref == null)
                    throw new ArgumentNullException("iref");
                this[key] = iref;
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public IEnumerator<KeyValuePair<string, PdfItem>> GetEnumerator()
            {
                return _elements.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((ICollection)_elements).GetEnumerator();
            }

            public PdfItem this[string key]
            {
                get
                {
                    PdfItem item;
                    _elements.TryGetValue(key, out item);
                    return item;
                }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException("value");
                    PdfObject obj = value as PdfObject;
                    if (obj != null && obj.IsIndirect)
                        value = obj.Reference;
                    _elements[key] = value;
                }
            }

            public PdfItem this[PdfName key]
            {
                get { return this[key.Value]; }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException("value");

                    PdfObject obj = value as PdfObject;
                    if (obj != null && obj.IsIndirect)
                        value = obj.Reference;
                    _elements[key.Value] = value;
                }
            }

            public bool Remove(string key)
            {
                return _elements.Remove(key);
            }

            public bool Remove(KeyValuePair<string, PdfItem> item)
            {
                throw new NotImplementedException();
            }

            public bool ContainsKey(string key)
            {
                return _elements.ContainsKey(key);
            }

            public bool Contains(KeyValuePair<string, PdfItem> item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                _elements.Clear();
            }

            public void Add(string key, PdfItem value)
            {
                if (String.IsNullOrEmpty(key))
                    throw new ArgumentNullException("key");

                if (key[0] != '/')
                    throw new ArgumentException("The key must start with a slash '/'.");

                PdfObject obj = value as PdfObject;
                if (obj != null && obj.IsIndirect)
                    value = obj.Reference;

                _elements.Add(key, value);
            }

            public void Add(KeyValuePair<string, PdfItem> item)
            {
                Add(item.Key, item.Value);
            }

            public PdfName[] KeyNames
            {
                get
                {
                    ICollection values = _elements.Keys;
                    int count = values.Count;
                    string[] strings = new string[count];
                    values.CopyTo(strings, 0);
                    PdfName[] names = new PdfName[count];
                    for (int idx = 0; idx < count; idx++)
                        names[idx] = new PdfName(strings[idx]);
                    return names;
                }
            }

            public ICollection<string> Keys
            {
                get
                {
                    ICollection values = _elements.Keys;
                    int count = values.Count;
                    string[] keys = new string[count];
                    values.CopyTo(keys, 0);
                    return keys;
                }
            }

            public bool TryGetValue(string key, out PdfItem value)
            {
                return _elements.TryGetValue(key, out value);
            }

            public ICollection<PdfItem> Values
            {
                get
                {
                    ICollection values = _elements.Values;
                    PdfItem[] items = new PdfItem[values.Count];
                    values.CopyTo(items, 0);
                    return items;
                }
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

            public void CopyTo(KeyValuePair<string, PdfItem>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public object SyncRoot
            {
                get { return null; }
            }

            internal string DebuggerDisplay
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(CultureInfo.InvariantCulture, "key={0}:(", _elements.Count);
                    bool addSpace = false;
                    ICollection<string> keys = _elements.Keys;
                    foreach (string key in keys)
                    {
                        if (addSpace)
                            sb.Append(' ');
                        addSpace = true;
                        sb.Append(key);
                    }
                    sb.Append(")");
                    return sb.ToString();
                }
            }

            Dictionary<string, PdfItem> _elements;

            PdfDictionary _ownerDictionary;
        }

        public sealed class PdfStream
        {
            internal PdfStream(PdfDictionary ownerDictionary)
            {
                if (ownerDictionary == null)
                    throw new ArgumentNullException("ownerDictionary");
                _ownerDictionary = ownerDictionary;
            }

            internal PdfStream(byte[] value, PdfDictionary owner)
                : this(owner)
            {
                _value = value;
            }

            public PdfStream Clone()
            {
                PdfStream stream = (PdfStream)MemberwiseClone();
                stream._ownerDictionary = null;
                if (stream._value != null)
                {
                    stream._value = new byte[stream._value.Length];
                    _value.CopyTo(stream._value, 0);
                }
                return stream;
            }

            internal void ChangeOwner(PdfDictionary dict)
            {
                if (_ownerDictionary != null)
                {
                }

                _ownerDictionary = dict;

                _ownerDictionary._stream = this;
            }

            PdfDictionary _ownerDictionary;

            public int Length
            {
                get { return _value != null ? _value.Length : 0; }
            }

            internal bool HasDecodeParams
            {
                get
                {
                    PdfDictionary dictionary = _ownerDictionary.Elements.GetDictionary(Keys.DecodeParms);
                    if (dictionary != null)
                    {
                        return true;
                    }
                    return false;
                }
            }

            internal int DecodePredictor           
            {
                get
                {
                    PdfDictionary dictionary = _ownerDictionary.Elements.GetDictionary(Keys.DecodeParms);
                    if (dictionary != null)
                    {
                        return dictionary.Elements.GetInteger("/Predictor");
                    }
                    return 0;
                }
            }

            internal int DecodeColumns           
            {
                get
                {
                    PdfDictionary dictionary = _ownerDictionary.Elements.GetDictionary(Keys.DecodeParms);
                    if (dictionary != null)
                    {
                        return dictionary.Elements.GetInteger("/Columns");
                    }
                    return 0;
                }
            }

            public byte[] Value
            {
                get { return _value; }
                set
                {
                    if (value == null)
                        throw new ArgumentNullException("value");
                    _value = value;
                    _ownerDictionary.Elements.SetInteger(Keys.Length, value.Length);
                }
            }
            byte[] _value;

            public byte[] UnfilteredValue
            {
                get
                {
                    byte[] bytes = null;
                    if (_value != null)
                    {
                        PdfItem filter = _ownerDictionary.Elements["/Filter"];
                        if (filter != null)
                        {
                            bytes = Filtering.Decode(_value, filter);
                            if (bytes == null)
                            {
                                string message = String.Format("«Cannot decode filter '{0}'»", filter);
                                bytes = PdfEncoders.RawEncoding.GetBytes(message);
                            }
                        }
                        else
                        {
                            bytes = new byte[_value.Length];
                            _value.CopyTo(bytes, 0);
                        }
                    }
                    return bytes ?? new byte[0];
                }
            }

            public bool TryUnfilter()       
            {
                if (_value != null)
                {
                    PdfItem filter = _ownerDictionary.Elements["/Filter"];
                    if (filter != null)
                    {
                        byte[] bytes = Filtering.Decode(_value, filter);
                        if (bytes != null)
                        {
                            _ownerDictionary.Elements.Remove(Keys.Filter);
                            Value = bytes;
                        }
                        else
                            return false;
                    }
                }
                return true;
            }

            public void Zip()
            {
                if (_value == null)
                    return;

                if (!_ownerDictionary.Elements.ContainsKey("/Filter"))
                {
                    _value = Filtering.FlateDecode.Encode(_value, _ownerDictionary._document.Options.FlateEncodeMode);
                    _ownerDictionary.Elements["/Filter"] = new PdfName("/FlateDecode");
                    _ownerDictionary.Elements["/Length"] = new PdfInteger(_value.Length);
                }
            }

            public override string ToString()
            {
                if (_value == null)
                    return "«null»";

                string stream;
                PdfItem filter = _ownerDictionary.Elements["/Filter"];
                if (filter != null)
                {

                    byte[] bytes = Filtering.Decode(_value, filter);
                    if (bytes != null)
                        stream = PdfEncoders.RawEncoding.GetString(bytes, 0, bytes.Length);

                    else
                        throw new NotImplementedException("Unknown filter");
                }
                else
                    stream = PdfEncoders.RawEncoding.GetString(_value, 0, _value.Length);

                return stream;
            }

            public class Keys : KeysBase
            {
                [KeyInfo(KeyType.Integer | KeyType.Required)]
                public const string Length = "/Length";

                [KeyInfo(KeyType.NameOrArray | KeyType.Optional)]
                public const string Filter = "/Filter";

                [KeyInfo(KeyType.ArrayOrDictionary | KeyType.Optional)]
                public const string DecodeParms = "/DecodeParms";

                [KeyInfo("1.2", KeyType.String | KeyType.Optional)]
                public const string F = "/F";

                [KeyInfo("1.2", KeyType.NameOrArray | KeyType.Optional)]
                public const string FFilter = "/FFilter";

                [KeyInfo("1.2", KeyType.ArrayOrDictionary | KeyType.Optional)]
                public const string FDecodeParms = "/FDecodeParms";

                [KeyInfo("1.5", KeyType.Integer | KeyType.Optional)]
                public const string DL = "/DL";

            }
        }

        string DebuggerDisplay
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, "dictionary({0},[{1}])={2}", 
                    ObjectID.DebuggerDisplay, 
                    Elements.Count,
                    _elements.DebuggerDisplay);
            }
        }
    }
}
