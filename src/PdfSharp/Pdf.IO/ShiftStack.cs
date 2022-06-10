using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PdfSharp.Pdf.IO
{
    internal class ShiftStack
    {
        public ShiftStack()
        {
            _items = new List<PdfItem>();
        }

        public PdfItem[] ToArray(int start, int length)
        {
            PdfItem[] items = new PdfItem[length];
            for (int i = 0, j = start; i < length; i++, j++)
                items[i] = _items[j];
            return items;
        }

        public int SP
        {
            get { return _sp; }
        }

        public PdfItem this[int index]
        {
            get
            {
                if (index >= _sp)
                    throw new ArgumentOutOfRangeException("index", index, "Value greater than stack index.");
                return _items[index];
            }
        }

        public PdfItem GetItem(int relativeIndex)
        {
            if (relativeIndex >= 0 || -relativeIndex > _sp)
                throw new ArgumentOutOfRangeException("relativeIndex", relativeIndex, "Value out of stack range.");
            return _items[_sp + relativeIndex];
        }

        public int GetInteger(int relativeIndex)
        {
            if (relativeIndex >= 0 || -relativeIndex > _sp)
                throw new ArgumentOutOfRangeException("relativeIndex", relativeIndex, "Value out of stack range.");
            return ((PdfInteger)_items[_sp + relativeIndex]).Value;
        }

        public void Shift(PdfItem item)
        {
            Debug.Assert(item != null);
            _items.Add(item);
            _sp++;
        }

        public void Reduce(int count)
        {
            if (count > _sp)
                throw new ArgumentException("count causes stack underflow.");
            _items.RemoveRange(_sp - count, count);
            _sp -= count;
        }

        public void Reduce(PdfItem item, int count)
        {
            Debug.Assert(item != null);
            Reduce(count);
            _items.Add(item);
            _sp++;
        }

        int _sp;

        readonly List<PdfItem> _items;
    }
}
