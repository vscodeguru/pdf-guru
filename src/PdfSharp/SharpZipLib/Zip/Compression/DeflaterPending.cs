namespace PdfSharp.SharpZipLib.Zip.Compression
{

    internal class DeflaterPending : PendingBuffer
    {
        public DeflaterPending()
            : base(DeflaterConstants.PENDING_BUF_SIZE)
        {
        }
    }
}
