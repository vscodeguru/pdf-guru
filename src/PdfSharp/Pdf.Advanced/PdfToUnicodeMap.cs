using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PdfSharp.Fonts;
using PdfSharp.Pdf.Filters;

namespace PdfSharp.Pdf.Advanced
{
    internal sealed class PdfToUnicodeMap : PdfDictionary
    {
        public PdfToUnicodeMap(PdfDocument document)
            : base(document)
        { }

        public PdfToUnicodeMap(PdfDocument document, CMapInfo cmapInfo)
            : base(document)
        {
            _cmapInfo = cmapInfo;
        }

        public CMapInfo CMapInfo
        {
            get { return _cmapInfo; }
            set { _cmapInfo = value; }
        }
        CMapInfo _cmapInfo;

        internal override void PrepareForSave()
        {
            base.PrepareForSave();

            string prefix =
              "/CIDInit /ProcSet findresource begin\n" +
              "12 dict begin\n" +
              "begincmap\n" +
              "/CIDSystemInfo << /Registry (Adobe)/Ordering (UCS)/Supplement 0>> def\n" +
              "/CMapName /Adobe-Identity-UCS def /CMapType 2 def\n";
            string suffix = "endcmap CMapName currentdict /CMap defineresource pop end end";

            Dictionary<int, char> glyphIndexToCharacter = new Dictionary<int, char>();
            int lowIndex = 65536, hiIndex = -1;
            foreach (KeyValuePair<char, int> entry in _cmapInfo.CharacterToGlyphIndex)
            {
                int index = (int)entry.Value;
                lowIndex = Math.Min(lowIndex, index);
                hiIndex = Math.Max(hiIndex, index);
                glyphIndexToCharacter[index] = entry.Key;
            }

            MemoryStream ms = new MemoryStream();
            StreamWriter wrt = new StreamWriter(ms, Encoding.ASCII);
            wrt.Write(prefix);
            wrt.WriteLine("1 begincodespacerange");
            wrt.WriteLine(String.Format("<{0:X4}><{1:X4}>", lowIndex, hiIndex));
            wrt.WriteLine("endcodespacerange");
            wrt.WriteLine(String.Format("{0} beginbfrange", glyphIndexToCharacter.Count));
            foreach (KeyValuePair<int, char> entry in glyphIndexToCharacter)
            wrt.WriteLine(String.Format("<{0:X4}><{0:X4}><{1:X4}>", entry.Key, (int)entry.Value));
            wrt.WriteLine("endbfrange");
            wrt.Write(suffix);
            wrt.Close();
            byte[] bytes = ms.ToArray();
            ms.Close();
            if (Owner.Options.CompressContentStreams)
            {
                Elements.SetName("/Filter", "/FlateDecode");
                bytes = Filtering.FlateDecode.Encode(bytes, _document.Options.FlateEncodeMode);
            }
            else
            {
                Elements.Remove("/Filter");
            }

            if (Stream == null)
                CreateStream(bytes);
            else
            {
                Stream.Value = bytes;
                Elements.SetInteger(PdfStream.Keys.Length, Stream.Length);
            }
        }

        public sealed class Keys : PdfStream.Keys
        {
        }
    }
}
