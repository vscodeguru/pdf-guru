
using System.IO;
using PdfSharp.SharpZipLib.Zip.Compression;
using PdfSharp.SharpZipLib.Zip.Compression.Streams;


namespace PdfSharp.Pdf.Filters
{
    public class FlateDecode : Filter
    {
        public override byte[] Encode(byte[] data)
        {
            return Encode(data, PdfFlateEncodeMode.Default);
        }

        public byte[] Encode(byte[] data, PdfFlateEncodeMode mode)
        {
            MemoryStream ms = new MemoryStream();


            int level = Deflater.DEFAULT_COMPRESSION;
            switch (mode)
            {
                case PdfFlateEncodeMode.BestCompression:
                    level = Deflater.BEST_COMPRESSION;
                    break;
                case PdfFlateEncodeMode.BestSpeed:
                    level = Deflater.BEST_SPEED;
                    break;
            }
            DeflaterOutputStream zip = new DeflaterOutputStream(ms, new Deflater(level, false));
            zip.Write(data, 0, data.Length);
            zip.Finish();
            ms.Capacity = (int)ms.Length;
            return ms.GetBuffer();
        }

        public override byte[] Decode(byte[] data, FilterParms parms)
        {
            MemoryStream msInput = new MemoryStream(data);
            MemoryStream msOutput = new MemoryStream();

            InflaterInputStream iis = new InflaterInputStream(msInput, new Inflater(false));
            int cbRead;
            byte[] abResult = new byte[32768];
            do
            {
                cbRead = iis.Read(abResult, 0, abResult.Length);
                if (cbRead > 0)
                    msOutput.Write(abResult, 0, cbRead);
            }
            while (cbRead > 0);

            iis.Close();
            msOutput.Flush();
            if (msOutput.Length >= 0)
            {
                msOutput.Capacity = (int)msOutput.Length;
                return msOutput.GetBuffer();
            }
            return null;
        }
    }
}
