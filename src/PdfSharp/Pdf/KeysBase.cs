using System;

namespace PdfSharp.Pdf
{
    public class KeysBase
    {
        internal static DictionaryMeta CreateMeta(Type type)
        {
            return new DictionaryMeta(type);
        }
    }
}
