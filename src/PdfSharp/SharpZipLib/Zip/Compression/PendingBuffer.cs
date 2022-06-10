
namespace PdfSharp.SharpZipLib.Zip.Compression
{

    internal class PendingBuffer
    {
        byte[] buffer_;

        int start;
        int end;

        uint bits;
        int bitCount;
        public PendingBuffer()
            : this(4096)
        {
        }

        public PendingBuffer(int bufferSize)
        {
            buffer_ = new byte[bufferSize];
        }

        public void Reset()
        {
            start = end = bitCount = 0;
        }

        public void WriteByte(int value)
        {

            buffer_[end++] = unchecked((byte)value);
        }

        public void WriteShort(int value)
        {
            buffer_[end++] = unchecked((byte)value);
            buffer_[end++] = unchecked((byte)(value >> 8));
        }

        public void WriteInt(int value)
        {
            buffer_[end++] = unchecked((byte)value);
            buffer_[end++] = unchecked((byte)(value >> 8));
            buffer_[end++] = unchecked((byte)(value >> 16));
            buffer_[end++] = unchecked((byte)(value >> 24));
        }

        public void WriteBlock(byte[] block, int offset, int length)
        {

            System.Array.Copy(block, offset, buffer_, end, length);
            end += length;
        }

        public int BitCount
        {
            get
            {
                return bitCount;
            }
        }

        public void AlignToByte()
        {

            if (bitCount > 0)
            {
                buffer_[end++] = unchecked((byte)bits);
                if (bitCount > 8)
                {
                    buffer_[end++] = unchecked((byte)(bits >> 8));
                }
            }
            bits = 0;
            bitCount = 0;
        }

        public void WriteBits(int b, int count)
        {

            bits |= (uint)(b << bitCount);
            bitCount += count;
            if (bitCount >= 16)
            {
                buffer_[end++] = unchecked((byte)bits);
                buffer_[end++] = unchecked((byte)(bits >> 8));
                bits >>= 16;
                bitCount -= 16;
            }
        }

        public void WriteShortMSB(int s)
        {
            buffer_[end++] = unchecked((byte)(s >> 8));
            buffer_[end++] = unchecked((byte)s);
        }

        public bool IsFlushed
        {
            get
            {
                return end == 0;
            }
        }

        public int Flush(byte[] output, int offset, int length)
        {
            if (bitCount >= 8)
            {
                buffer_[end++] = unchecked((byte)bits);
                bits >>= 8;
                bitCount -= 8;
            }

            if (length > end - start)
            {
                length = end - start;
                System.Array.Copy(buffer_, start, output, offset, length);
                start = 0;
                end = 0;
            }
            else
            {
                System.Array.Copy(buffer_, start, output, offset, length);
                start += length;
            }
            return length;
        }

        public byte[] ToByteArray()
        {
            byte[] result = new byte[end - start];
            System.Array.Copy(buffer_, start, result, 0, result.Length);
            start = 0;
            end = 0;
            return result;
        }
    }
}
