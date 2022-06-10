using System.Diagnostics;
using System.IO;

namespace PdfSharp.Fonts.OpenType
{
    internal class OpenTypeFontWriter : FontWriter
    {
        public OpenTypeFontWriter(Stream stream)
            : base(stream)
        { }

        public void WriteTag(string tag)
        {
            Debug.Assert(tag.Length == 4);
            WriteByte((byte)(tag[0]));
            WriteByte((byte)(tag[1]));
            WriteByte((byte)(tag[2]));
            WriteByte((byte)(tag[3]));
        }
    }
}
