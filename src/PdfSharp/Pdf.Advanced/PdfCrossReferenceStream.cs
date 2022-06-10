using System.Collections.Generic;

namespace PdfSharp.Pdf.Advanced
{
    internal sealed class PdfCrossReferenceStream : PdfTrailer          
    {
        public PdfCrossReferenceStream(PdfDocument document)
            : base(document)
        {

        }

        public readonly List<CrossReferenceStreamEntry> Entries = new List<CrossReferenceStreamEntry>();

        public struct CrossReferenceStreamEntry
        {
            public uint Type;      

            public uint Field2;

            public uint Field3;
        }

        public new class Keys : PdfTrailer.Keys                 
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "XRef")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public new const string Size = "/Size";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Index = "/Index";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public new const string Prev = "/Prev";

            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string W = "/W";

            public static new DictionaryMeta Meta
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
