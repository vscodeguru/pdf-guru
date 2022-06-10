using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf.Internal
{
    internal class ThreadLocalStorage  
    {
        public ThreadLocalStorage()
        {
            _importedDocuments = new Dictionary<string, PdfDocument.DocumentHandle>(StringComparer.OrdinalIgnoreCase);
        }

        public void AddDocument(string path, PdfDocument document)
        {
            _importedDocuments.Add(path, document.Handle);
        }

        public void RemoveDocument(string path)
        {
            _importedDocuments.Remove(path);
        }

        public PdfDocument GetDocument(string path)
        {
            Debug.Assert(path.StartsWith("*") || Path.IsPathRooted(path), "Path must be full qualified.");

            PdfDocument document = null;
            PdfDocument.DocumentHandle handle;
            if (_importedDocuments.TryGetValue(path, out handle))
            {
                document = handle.Target;
                if (document == null)
                    RemoveDocument(path);
            }
            if (document == null)
            {
                document = PdfReader.Open(path, PdfDocumentOpenMode.Import);
                _importedDocuments.Add(path, document.Handle);
            }
            return document;
        }

        public PdfDocument[] Documents
        {
            get
            {
                List<PdfDocument> list = new List<PdfDocument>();
                foreach (PdfDocument.DocumentHandle handle in _importedDocuments.Values)
                {
                    if (handle.IsAlive)
                        list.Add(handle.Target);
                }
                return list.ToArray();
            }
        }

        public void DetachDocument(PdfDocument.DocumentHandle handle)
        {
            if (handle.IsAlive)
            {
                foreach (string path in _importedDocuments.Keys)
                {
                    if (_importedDocuments[path] == handle)
                    {
                        _importedDocuments.Remove(path);
                        break;
                    }
                }
            }

            bool itemRemoved = true;
            while (itemRemoved)
            {
                itemRemoved = false;
                foreach (string path in _importedDocuments.Keys)
                {
                    if (!_importedDocuments[path].IsAlive)
                    {
                        _importedDocuments.Remove(path);
                        itemRemoved = true;
                        break;
                    }
                }
            }
        }

        readonly Dictionary<string, PdfDocument.DocumentHandle> _importedDocuments;
    }
}