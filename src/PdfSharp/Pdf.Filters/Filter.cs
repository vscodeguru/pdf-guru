using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.Filters
{
    public class FilterParms
    {
    }

    public abstract class Filter
    {
        public abstract byte[] Encode(byte[] data);

        public virtual byte[] Encode(string rawString)
        {
            byte[] bytes = PdfEncoders.RawEncoding.GetBytes(rawString);
            bytes = Encode(bytes);
            return bytes;
        }

        public abstract byte[] Decode(byte[] data, FilterParms parms);

        public byte[] Decode(byte[] data)
        {
            return Decode(data, null);
        }

        public virtual string DecodeToString(byte[] data, FilterParms parms)
        {
            byte[] bytes = Decode(data, parms);
            string text = PdfEncoders.RawEncoding.GetString(bytes, 0, bytes.Length);
            return text;
        }

        public string DecodeToString(byte[] data)
        {
            return DecodeToString(data, null);
        }

        protected byte[] RemoveWhiteSpace(byte[] data)
        {
            int count = data.Length;
            int j = 0;
            for (int i = 0; i < count; i++, j++)
            {
                switch (data[i])
                {
                    case (byte)Chars.NUL:    
                    case (byte)Chars.HT:     
                    case (byte)Chars.LF:      
                    case (byte)Chars.FF:      
                    case (byte)Chars.CR:      
                    case (byte)Chars.SP:     
                        j--;
                        break;

                    default:
                        if (i != j)
                            data[j] = data[i];
                        break;
                }
            }
            if (j < count)
            {
                byte[] temp = data;
                data = new byte[j];
                for (int idx = 0; idx < j; idx++)
                    data[idx] = temp[idx];
            }
            return data;
        }
    }
}
