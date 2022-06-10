using System.IO;

namespace PdfSharp.Fonts
{
    internal class FontWriter
    {
        public FontWriter(Stream stream)
        {
            _stream = stream;
        }

        public void Close(bool closeUnderlyingStream)
        {
            if (_stream != null && closeUnderlyingStream)
            {
#if !UWP
                _stream.Close();
#endif
                _stream.Dispose();
            }
            _stream = null;
        }

        public void Close()
        {
            Close(true);
        }

        public int Position
        {
            get { return (int)_stream.Position; }
            set { _stream.Position = value; }
        }

        public void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        public void WriteByte(int value)
        {
            _stream.WriteByte((byte)value);
        }

        public void WriteShort(short value)
        {
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)value);
        }

        public void WriteShort(int value)
        {
            WriteShort((short)value);
        }

        public void WriteUShort(ushort value)
        {
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)value);
        }

        public void WriteUShort(int value)
        {
            WriteUShort((ushort)value);
        }

        public void WriteInt(int value)
        {
            _stream.WriteByte((byte)(value >> 24));
            _stream.WriteByte((byte)(value >> 16));
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)value);
        }

        public void WriteUInt(uint value)
        {
            _stream.WriteByte((byte)(value >> 24));
            _stream.WriteByte((byte)(value >> 16));
            _stream.WriteByte((byte)(value >> 8));
            _stream.WriteByte((byte)value);
        }

        public void Write(byte[] buffer)
        {
            _stream.Write(buffer, 0, buffer.Length);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        internal Stream Stream
        {
            get { return _stream; }
        }
        Stream _stream;
    }
}
