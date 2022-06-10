using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

namespace PdfSharp.Pdf.Content.Objects       
{
    public abstract class CObject : ICloneable
    {
        protected CObject()
        { }

        object ICloneable.Clone()
        {
            return Copy();
        }

        public CObject Clone()
        {
            return Copy();
        }

        protected virtual CObject Copy()
        {
            return (CObject)MemberwiseClone();
        }

        internal abstract void WriteObject(ContentWriter writer);
    }

    [DebuggerDisplay("({Text})")]
    public class CComment : CObject
    {
        public new CComment Clone()
        {
            return (CComment)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        string _text;

        public override string ToString()
        {
            return "% " + _text;
        }

        internal override void WriteObject(ContentWriter writer)
        {
            writer.WriteLineRaw(ToString());
        }
    }

    [DebuggerDisplay("(count={Count})")]
    public class CSequence : CObject, IList<CObject>     
    {
        public new CSequence Clone()
        {
            return (CSequence)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            _items = new List<CObject>(_items);
            for (int idx = 0; idx < _items.Count; idx++)
                _items[idx] = _items[idx].Clone();
            return obj;
        }

        public void Add(CSequence sequence)
        {
            int count = sequence.Count;
            for (int idx = 0; idx < count; idx++)
                _items.Add(sequence[idx]);
        }

        public void Add(CObject value)
        {
            _items.Add(value);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(CObject value)
        {
            return _items.Contains(value);
        }

        public int IndexOf(CObject value)
        {
            return _items.IndexOf(value);
        }

        public void Insert(int index, CObject value)
        {
            _items.Insert(index, value);
        }

        public bool Remove(CObject value)
        {
            return _items.Remove(value);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public CObject this[int index]
        {
            get { return (CObject)_items[index]; }
            set { _items[index] = value; }
        }
        public void CopyTo(CObject[] array, int index)
        {
            _items.CopyTo(array, index);
        }


        public int Count
        {
            get { return _items.Count; }
        }

        public IEnumerator<CObject> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public byte[] ToContent()
        {
            Stream stream = new MemoryStream();
            ContentWriter writer = new ContentWriter(stream);
            WriteObject(writer);
            writer.Close(false);

            stream.Position = 0;
            int count = (int)stream.Length;
            byte[] bytes = new byte[count];
            stream.Read(bytes, 0, count);
#if !UWP
            stream.Close();
#endif
            return bytes;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            for (int idx = 0; idx < _items.Count; idx++)
                s.Append(_items[idx]);

            return s.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal override void WriteObject(ContentWriter writer)
        {
            for (int idx = 0; idx < _items.Count; idx++)
                _items[idx].WriteObject(writer);
        }

        int IList<CObject>.IndexOf(CObject item)
        {
            throw new NotImplementedException();
        }

        void IList<CObject>.Insert(int index, CObject item)
        {
            throw new NotImplementedException();
        }

        void IList<CObject>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        CObject IList<CObject>.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void ICollection<CObject>.Add(CObject item)
        {
            throw new NotImplementedException();
        }

        void ICollection<CObject>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<CObject>.Contains(CObject item)
        {
            throw new NotImplementedException();
        }

        void ICollection<CObject>.CopyTo(CObject[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<CObject>.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<CObject>.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection<CObject>.Remove(CObject item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<CObject> IEnumerable<CObject>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        List<CObject> _items = new List<CObject>();
    }

    public abstract class CNumber : CObject
    {
        public new CNumber Clone()
        {
            return (CNumber)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

    }

    [DebuggerDisplay("({Value})")]
    public class CInteger : CNumber
    {
        public new CInteger Clone()
        {
            return (CInteger)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        int _value;

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        internal override void WriteObject(ContentWriter writer)
        {
            writer.WriteRaw(ToString() + " ");
        }
    }

    [DebuggerDisplay("({Value})")]
    public class CReal : CNumber
    {
        public new CReal Clone()
        {
            return (CReal)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }
        double _value;

        public override string ToString()
        {
            const string format = Config.SignificantFigures1Plus9;
            return _value.ToString(format, CultureInfo.InvariantCulture);
        }

        internal override void WriteObject(ContentWriter writer)
        {
            writer.WriteRaw(ToString() + " ");
        }
    }

    public enum CStringType
    {
        String,

        HexString,

        UnicodeString,

        UnicodeHexString,

        Dictionary,
    }

    [DebuggerDisplay("({Value})")]
    public class CString : CObject
    {
        public new CString Clone()
        {
            return (CString)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        string _value;

        public CStringType CStringType
        {
            get { return _cStringType; }
            set { _cStringType = value; }
        }
        CStringType _cStringType;

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            switch (CStringType)
            {
                case CStringType.String:
                    s.Append("(");
                    int length = _value.Length;
                    for (int ich = 0; ich < length; ich++)
                    {
                        char ch = _value[ich];
                        switch (ch)
                        {
                            case Chars.LF:
                                s.Append("\\n");
                                break;

                            case Chars.CR:
                                s.Append("\\r");
                                break;

                            case Chars.HT:
                                s.Append("\\t");
                                break;

                            case Chars.BS:
                                s.Append("\\b");
                                break;

                            case Chars.FF:
                                s.Append("\\f");
                                break;

                            case Chars.ParenLeft:
                                s.Append("\\(");
                                break;

                            case Chars.ParenRight:
                                s.Append("\\)");
                                break;

                            case Chars.BackSlash:
                                s.Append("\\\\");
                                break;

                            default:

                                s.Append(ch);
                                break;
                        }
                    }
                    s.Append(')');
                    break;


                case CStringType.HexString:
                    throw new NotImplementedException();
                case CStringType.UnicodeString:
                    throw new NotImplementedException();
                case CStringType.UnicodeHexString:
                    throw new NotImplementedException();
                case CStringType.Dictionary:
                    s.Append(_value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return s.ToString();
        }

        internal override void WriteObject(ContentWriter writer)
        {
            writer.WriteRaw(ToString());
        }
    }

    [DebuggerDisplay("({Name})")]
    public class CName : CObject
    {
        public CName()
        {
            _name = "/";
        }

        public CName(string name)
        {
            Name = name;
        }

        public new CName Clone()
        {
            return (CName)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrEmpty(_name))
                    throw new ArgumentNullException(nameof(value));
                if (_name[0] != '/')
                    throw new ArgumentException(PSSR.NameMustStartWithSlash);
                _name = value;
            }
        }
        string _name;

        public override string ToString()
        {
            return _name;
        }

        internal override void WriteObject(ContentWriter writer)
        {
            writer.WriteRaw(ToString() + " ");
        }
    }

    [DebuggerDisplay("(count={Count})")]
    public class CArray : CSequence
    {
        public new CArray Clone()
        {
            return (CArray)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

        public override string ToString()
        {
            return "[" + base.ToString() + "]";
        }

        internal override void WriteObject(ContentWriter writer)
        {
            writer.WriteRaw(ToString());
        }
    }

    [DebuggerDisplay("({Name}, operands={Operands.Count})")]
    public class COperator : CObject
    {
        protected COperator()
        { }

        internal COperator(OpCode opcode)
        {
            _opcode = opcode;
        }

        public new COperator Clone()
        {
            return (COperator)Copy();
        }

        protected override CObject Copy()
        {
            CObject obj = base.Copy();
            return obj;
        }

        public virtual string Name
        {
            get { return _opcode.Name; }
        }

        public CSequence Operands
        {
            get { return _seqence ?? (_seqence = new CSequence()); }
        }
        CSequence _seqence;

        public OpCode OpCode
        {
            get { return _opcode; }
        }
        readonly OpCode _opcode;


        public override string ToString()
        {
            if (_opcode.OpCodeName == OpCodeName.Dictionary)
                return " ";

            return Name;
        }

        internal override void WriteObject(ContentWriter writer)
        {
            int count = _seqence != null ? _seqence.Count : 0;
            for (int idx = 0; idx < count; idx++)
            {
                _seqence[idx].WriteObject(writer);
            }
            writer.WriteLineRaw(ToString());
        }
    }
}
