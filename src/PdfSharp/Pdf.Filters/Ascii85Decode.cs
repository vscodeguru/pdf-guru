using System;

namespace PdfSharp.Pdf.Filters
{
    public class Ascii85Decode : Filter
    {
        public override byte[] Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            int length = data.Length;              
            int words = length / 4;
            int rest = length - (words * 4);
            byte[] result = new byte[words * 5 + (rest == 0 ? 0 : rest + 1) + 2];

            int idxIn = 0, idxOut = 0;
            int wCount = 0;
            while (wCount < words)
            {
                uint val = ((uint)data[idxIn++] << 24) + ((uint)data[idxIn++] << 16) + ((uint)data[idxIn++] << 8) + data[idxIn++];
                if (val == 0)
                {
                    result[idxOut++] = (byte)'z';
                }
                else
                {
                    byte c5 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c4 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c3 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c2 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c1 = (byte)(val + '!');

                    result[idxOut++] = c1;
                    result[idxOut++] = c2;
                    result[idxOut++] = c3;
                    result[idxOut++] = c4;
                    result[idxOut++] = c5;
                }
                wCount++;
            }
            if (rest == 1)
            {
                uint val = (uint)data[idxIn] << 24;
                val /= 85 * 85 * 85;
                byte c2 = (byte)(val % 85 + '!');
                val /= 85;
                byte c1 = (byte)(val + '!');

                result[idxOut++] = c1;
                result[idxOut++] = c2;
            }
            else if (rest == 2)
            {
                uint val = ((uint)data[idxIn++] << 24) + ((uint)data[idxIn] << 16);
                val /= 85 * 85;
                byte c3 = (byte)(val % 85 + '!');
                val /= 85;
                byte c2 = (byte)(val % 85 + '!');
                val /= 85;
                byte c1 = (byte)(val + '!');

                result[idxOut++] = c1;
                result[idxOut++] = c2;
                result[idxOut++] = c3;
            }
            else if (rest == 3)
            {
                uint val = ((uint)data[idxIn++] << 24) + ((uint)data[idxIn++] << 16) + ((uint)data[idxIn] << 8);
                val /= 85;
                byte c4 = (byte)(val % 85 + '!');
                val /= 85;
                byte c3 = (byte)(val % 85 + '!');
                val /= 85;
                byte c2 = (byte)(val % 85 + '!');
                val /= 85;
                byte c1 = (byte)(val + '!');

                result[idxOut++] = c1;
                result[idxOut++] = c2;
                result[idxOut++] = c3;
                result[idxOut++] = c4;
            }
            result[idxOut++] = (byte)'~';
            result[idxOut++] = (byte)'>';

            if (idxOut < result.Length)
                Array.Resize(ref result, idxOut);

            return result;
        }

        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            int idx;
            int length = data.Length;
            int zCount = 0;
            int idxOut = 0;
            for (idx = 0; idx < length; idx++)
            {
                char ch = (char)data[idx];
                if (ch >= '!' && ch <= 'u')
                    data[idxOut++] = (byte)ch;
                else if (ch == 'z')
                {
                    data[idxOut++] = (byte)ch;
                    zCount++;
                }
                else if (ch == '~')
                {
                    if ((char)data[idx + 1] != '>')
                        throw new ArgumentException("Illegal character.", "data");
                    break;
                }
            }
            if (idx == length)
                throw new ArgumentException("Illegal character.", "data");

            length = idxOut;
            int nonZero = length - zCount;
            int byteCount = 4 * (zCount + (nonZero / 5));     

            int remainder = nonZero % 5;
            if (remainder == 1)
                throw new InvalidOperationException("Illegal character.");

            if (remainder != 0)
                byteCount += remainder - 1;

            byte[] output = new byte[byteCount];

            idxOut = 0;
            idx = 0;
            while (idx + 4 < length)
            {
                char ch = (char)data[idx];
                if (ch == 'z')
                {
                    idx++;
                    idxOut += 4;
                }
                else
                {
                    long value =
                      (long)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                      (uint)(data[idx++] - '!') * (85 * 85 * 85) +
                      (uint)(data[idx++] - '!') * (85 * 85) +
                      (uint)(data[idx++] - '!') * 85 +
                      (uint)(data[idx++] - '!');

                    if (value > UInt32.MaxValue)
                        throw new InvalidOperationException("Value of group greater than 2 power 32 - 1.");

                    output[idxOut++] = (byte)(value >> 24);
                    output[idxOut++] = (byte)(value >> 16);
                    output[idxOut++] = (byte)(value >> 8);
                    output[idxOut++] = (byte)value;
                }
            }

            if (remainder == 2)   
            {
                uint value =
                  (uint)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                  (uint)(data[idx] - '!') * (85 * 85 * 85);

                if (value != 0)
                    value += 0x01000000;

                output[idxOut] = (byte)(value >> 24);
            }
            else if (remainder == 3)   
            {
                int idxIn = idx;
                uint value =
                  (uint)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                  (uint)(data[idx++] - '!') * (85 * 85 * 85) +
                  (uint)(data[idx] - '!') * (85 * 85);

                if (value != 0)
                {
                    value &= 0xFFFF0000;
                    uint val = value / (85 * 85);
                    byte c3 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c2 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c1 = (byte)(val + '!');
                    if (c1 != data[idxIn] || c2 != data[idxIn + 1] || c3 != data[idxIn + 2])
                    {
                        value += 0x00010000;
                    }
                }
                output[idxOut++] = (byte)(value >> 24);
                output[idxOut] = (byte)(value >> 16);
            }
            else if (remainder == 4)   
            {
                int idxIn = idx;
                uint value =
                  (uint)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                  (uint)(data[idx++] - '!') * (85 * 85 * 85) +
                  (uint)(data[idx++] - '!') * (85 * 85) +
                  (uint)(data[idx] - '!') * 85;

                if (value != 0)
                {
                    value &= 0xFFFFFF00;
                    uint val = value / 85;
                    byte c4 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c3 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c2 = (byte)(val % 85 + '!');
                    val /= 85;
                    byte c1 = (byte)(val + '!');
                    if (c1 != data[idxIn] || c2 != data[idxIn + 1] || c3 != data[idxIn + 2] || c4 != data[idxIn + 3])
                    {
                        value += 0x00000100;
                    }
                }
                output[idxOut++] = (byte)(value >> 24);
                output[idxOut++] = (byte)(value >> 16);
                output[idxOut] = (byte)(value >> 8);
            }
            return output;
        }
    }
}
