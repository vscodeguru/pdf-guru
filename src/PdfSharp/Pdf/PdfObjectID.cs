using System;
using System.Diagnostics;
using System.Globalization;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public struct PdfObjectID : IComparable
    {
        public PdfObjectID(int objectNumber)
        {
            Debug.Assert(objectNumber >= 1, "Object number out of range.");
            _objectNumber = objectNumber;
            _generationNumber = 0;
        }

        public PdfObjectID(int objectNumber, int generationNumber)
        {
            Debug.Assert(objectNumber >= 1, "Object number out of range.");
            _objectNumber = objectNumber;
            _generationNumber = (ushort)generationNumber;
        }

        public int ObjectNumber
        {
            get { return _objectNumber; }
        }
        readonly int _objectNumber;

        public int GenerationNumber
        {
            get { return _generationNumber; }
        }
        readonly ushort _generationNumber;

        public bool IsEmpty
        {
            get { return _objectNumber == 0; }
        }

        public override bool Equals(object obj)
        {
            if (obj is PdfObjectID)
            {
                PdfObjectID id = (PdfObjectID)obj;
                if (_objectNumber == id._objectNumber)
                    return _generationNumber == id._generationNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _objectNumber ^ _generationNumber;
        }

        public static bool operator ==(PdfObjectID left, PdfObjectID right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PdfObjectID left, PdfObjectID right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return _objectNumber.ToString(CultureInfo.InvariantCulture) + " " + _generationNumber.ToString(CultureInfo.InvariantCulture);
        }

        public static PdfObjectID Empty
        {
            get { return new PdfObjectID(); }
        }

        public int CompareTo(object obj)
        {
            if (obj is PdfObjectID)
            {
                PdfObjectID id = (PdfObjectID)obj;
                if (_objectNumber == id._objectNumber)
                    return _generationNumber - id._generationNumber;
                return _objectNumber - id._objectNumber;
            }
            return 1;
        }

        internal string DebuggerDisplay
        {
            get { return String.Format("id=({0})", ToString()); }
        }
    }
}
