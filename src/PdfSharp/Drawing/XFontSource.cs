using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using PdfSharp.Fonts;
using GdiFont = System.Drawing.Font;
using PdfSharp.Internal;
using PdfSharp.Fonts.OpenType;

namespace PdfSharp.Drawing
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class XFontSource
    {
        const uint ttcf = 0x66637474;

        XFontSource(byte[] bytes, ulong key)
        {
            _fontName = null;
            _bytes = bytes;
            _key = key;
        }

        public static XFontSource GetOrCreateFrom(byte[] bytes)
        {
            ulong key = FontHelper.CalcChecksum(bytes);
            XFontSource fontSource;
            if (!FontFactory.TryGetFontSourceByKey(key, out fontSource))
            {
                fontSource = new XFontSource(bytes, key);
                fontSource = FontFactory.CacheFontSource(fontSource);
            }
            return fontSource;
        }

#if CORE || GDI
        internal static XFontSource GetOrCreateFromGdi(string typefaceKey, GdiFont gdiFont)
        {
            byte[] bytes = ReadFontBytesFromGdi(gdiFont);
            XFontSource fontSource = GetOrCreateFrom(typefaceKey, bytes);
            return fontSource;
        }

        static byte[] ReadFontBytesFromGdi(GdiFont gdiFont)
        {
            int error = Marshal.GetLastWin32Error();
            error = Marshal.GetLastWin32Error();
            IntPtr hfont = gdiFont.ToHfont();
#if true
            IntPtr hdc = NativeMethods.GetDC(IntPtr.Zero);
#endif
            error = Marshal.GetLastWin32Error();
            IntPtr oldFont = NativeMethods.SelectObject(hdc, hfont);
            error = Marshal.GetLastWin32Error();
            bool isTtcf = false;
            int size = NativeMethods.GetFontData(hdc, 0, 0, null, 0);

            if ((uint)size == 0xc0000022)
                throw new InvalidOperationException("Microsoft Azure returns STATUS_ACCESS_DENIED ((NTSTATUS)0xC0000022L) from GetFontData. This is a bug in Azure. You must implement a FontResolver to circumvent this issue.");

            if (size == NativeMethods.GDI_ERROR)
            {
                size = NativeMethods.GetFontData(hdc, ttcf, 0, null, 0);
                isTtcf = true;
            }
            error = Marshal.GetLastWin32Error();
            if (size == 0)
                throw new InvalidOperationException("Cannot retrieve font data.");

            byte[] bytes = new byte[size];
            int effectiveSize = NativeMethods.GetFontData(hdc, isTtcf ? ttcf : 0, 0, bytes, size);
            Debug.Assert(size == effectiveSize);
            NativeMethods.SelectObject(hdc, oldFont);
            NativeMethods.ReleaseDC(IntPtr.Zero, hdc);

            return bytes;
        }
#endif
        static XFontSource GetOrCreateFrom(string typefaceKey, byte[] fontBytes)
        {
            XFontSource fontSource;
            ulong key = FontHelper.CalcChecksum(fontBytes);
            if (FontFactory.TryGetFontSourceByKey(key, out fontSource))
            {
                FontFactory.CacheExistingFontSourceWithNewTypefaceKey(typefaceKey, fontSource);
            }
            else
            {
                fontSource = new XFontSource(fontBytes, key);
                FontFactory.CacheNewFontSource(typefaceKey, fontSource);
            }
            return fontSource;
        }

        public static XFontSource CreateCompiledFont(byte[] bytes)
        {
            XFontSource fontSource = new XFontSource(bytes, 0);
            return fontSource;
        }

        internal OpenTypeFontface Fontface
        {
            get { return _fontface; }
            set
            {
                _fontface = value;
                _fontName = value.name.FullFontName;
            }
        }
        OpenTypeFontface _fontface;

        internal ulong Key
        {
            get
            {
                if (_key == 0)
                    _key = FontHelper.CalcChecksum(Bytes);
                return _key;
            }
        }
        ulong _key;

        public void IncrementKey()
        {
            _key += 1ul << 32;
        }

        public string FontName
        {
            get { return _fontName; }
        }
        string _fontName;

        public byte[] Bytes
        {
            get { return _bytes; }
        }
        readonly byte[] _bytes;

        public override int GetHashCode()
        {
            return (int)((Key >> 32) ^ Key);
        }

        public override bool Equals(object obj)
        {
            XFontSource fontSource = obj as XFontSource;
            if (fontSource == null)
                return false;
            return Key == fontSource.Key;
        }

        internal string DebuggerDisplay
        {
            get { return String.Format(CultureInfo.InvariantCulture, "XFontSource: '{0}', keyhash={1}", FontName, Key % 99991        ); }
        }
    }
}
