
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf.Advanced
{
    [DebuggerDisplay("iref({ObjectNumber}, {GenerationNumber})")]
    public sealed class PdfReference : PdfItem
    {
        public PdfReference(PdfObject pdfObject)
        {
            if (pdfObject.Reference != null)
                throw new InvalidOperationException("Must not create iref for an object that already has one.");
            _value = pdfObject;
            pdfObject.Reference = this;

        }

        public PdfReference(PdfObjectID objectID, int position)
        {
            _objectID = objectID;
            _position = position;

        }

        internal void WriteXRefEnty(PdfWriter writer)
        {
            string text = String.Format("{0:0000000000} {1:00000} n\n",
              _position, _objectID.GenerationNumber);      
            writer.WriteRaw(text);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }

        public PdfObjectID ObjectID
        {
            get { return _objectID; }
            set
            {
                if (_objectID == value)
                    return;

                _objectID = value;
                if (Document != null)
                {
                }
            }
        }
        PdfObjectID _objectID;

        public int ObjectNumber
        {
            get { return _objectID.ObjectNumber; }
        }

        public int GenerationNumber
        {
            get { return _objectID.GenerationNumber; }
        }

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }
        int _position;                 

        public PdfObject Value
        {
            get { return _value; }
            set
            {
                Debug.Assert(value != null, "The value of a PdfReference must never be null.");
                Debug.Assert(value.Reference == null || ReferenceEquals(value.Reference, this), "The reference of the value must be null or this.");
                _value = value;
                value.Reference = this;
            }
        }
        PdfObject _value;

        internal void SetObject(PdfObject value)
        {
            _value = value;
        }

        public PdfDocument Document
        {
            get { return _document; }
            set { _document = value; }
        }
        PdfDocument _document;

        public override string ToString()
        {
            return _objectID + " R";
        }

        internal static PdfReferenceComparer Comparer
        {
            get { return new PdfReferenceComparer(); }
        }

        internal class PdfReferenceComparer : IComparer<PdfReference>
        {
            public int Compare(PdfReference x, PdfReference y)
            {
                PdfReference l = x;
                PdfReference r = y;
                if (l != null)
                {
                    if (r != null)
                        return l._objectID.CompareTo(r._objectID);
                    return -1;
                }
                if (r != null)
                    return 1;
                return 0;
            }
        }
    }
}
