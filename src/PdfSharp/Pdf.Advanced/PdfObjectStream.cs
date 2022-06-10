
using System.IO;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf.Advanced
{
    public class PdfObjectStream : PdfDictionary
    {
        public PdfObjectStream(PdfDocument document)
            : base(document)
        {
        }

        internal PdfObjectStream(PdfDictionary dict)
            : base(dict)
        {
            int n = Elements.GetInteger(Keys.N);
            int first = Elements.GetInteger(Keys.First);
            Stream.TryUnfilter();

            Parser parser = new Parser(null, new MemoryStream(Stream.Value));
            _header = parser.ReadObjectStreamHeader(n, first);

        }

        internal void ReadReferences(PdfCrossReferenceTable xrefTable)
        {
            for (int idx = 0; idx < _header.Length; idx++)
            {
                int objectNumber = _header[idx][0];
                int offset = _header[idx][1];

                PdfObjectID objectID = new PdfObjectID(objectNumber);

                PdfReference iref = new PdfReference(objectID, -1);
                if (!xrefTable.Contains(iref.ObjectID))
                {
                    xrefTable.Add(iref);
                }
                else
                {
                    GetType();
                }
            }
        }

        internal PdfReference ReadCompressedObject(int index)
        {
            Parser parser = new Parser(_document, new MemoryStream(Stream.Value));
            int objectNumber = _header[index][0];
            int offset = _header[index][1];
            return parser.ReadCompressedObject(objectNumber, offset);
        }

        private readonly int[][] _header;     

        public class Keys : PdfStream.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "ObjStm")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string N = "/N";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string First = "/First";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string Extends = "/Extends";
        }
    }

}
