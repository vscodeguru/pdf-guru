using System;

namespace PdfSharp.Pdf.Filters
{
    public class AsciiHexDecode : Filter
    {
        public override byte[] Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            int count = data.Length;
            byte[] bytes = new byte[2 * count];
            for (int i = 0, j = 0; i < count; i++)
            {
                byte b = data[i];
                bytes[j++] = (byte)((b >> 4) + ((b >> 4) < 10 ? (byte)'0' : (byte)('A' - 10)));
                bytes[j++] = (byte)((b & 0xF) + ((b & 0xF) < 10 ? (byte)'0' : (byte)('A' - 10)));
            }
            return bytes;
        }

        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            data = RemoveWhiteSpace(data);
            int count = data.Length;
            if (count > 0 && data[count - 1] == '>')
                --count;
            if (count % 2 == 1)
            {
                count++;
                byte[] temp = data;
                data = new byte[count];
                temp.CopyTo(data, 0);
            }
            count >>= 1;
            byte[] bytes = new byte[count];
            for (int i = 0, j = 0; i < count; i++)
            {
                byte hi = data[j++];
                byte lo = data[j++];
                if (hi >= 'a' && hi <= 'f')
                    hi -= 32;
                if (lo >= 'a' && lo <= 'f')
                    lo -= 32;
                bytes[i] = (byte)((hi > '9' ? hi - '7'  : hi - '0') * 16 + (lo > '9' ? lo - '7'  : lo - '0'));
            }
            return bytes;
        }
    }
}
