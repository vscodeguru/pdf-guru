using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PdfSharp.Internal;

namespace PdfSharp.Fonts.OpenType
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class OpenTypeFontfaceCache
    {
        OpenTypeFontfaceCache()
        {
            _fontfaceCache = new Dictionary<string, OpenTypeFontface>(StringComparer.OrdinalIgnoreCase);
            _fontfacesByCheckSum = new Dictionary<ulong, OpenTypeFontface>();
        }

        public static bool TryGetFontface(string key, out OpenTypeFontface fontface)
        {
            try
            {
                Lock.EnterFontFactory();
                bool result = Singleton._fontfaceCache.TryGetValue(key, out fontface);
                return result;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static bool TryGetFontface(ulong checkSum, out OpenTypeFontface fontface)
        {
            try
            {
                Lock.EnterFontFactory();
                bool result = Singleton._fontfacesByCheckSum.TryGetValue(checkSum, out fontface);
                return result;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static OpenTypeFontface AddFontface(OpenTypeFontface fontface)
        {
            try
            {
                Lock.EnterFontFactory();
                OpenTypeFontface fontfaceCheck;
                if (TryGetFontface(fontface.FullFaceName, out fontfaceCheck))
                {
                    if (fontfaceCheck.CheckSum != fontface.CheckSum)
                        throw new InvalidOperationException("OpenTypeFontface with same signature but different bytes.");
                    return fontfaceCheck;
                }
                Singleton._fontfaceCache.Add(fontface.FullFaceName, fontface);
                Singleton._fontfacesByCheckSum.Add(fontface.CheckSum, fontface);
                return fontface;
            }
            finally { Lock.ExitFontFactory(); }
        }

        static OpenTypeFontfaceCache Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    try
                    {
                        Lock.EnterFontFactory();
                        if (_singleton == null)
                            _singleton = new OpenTypeFontfaceCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }
        static volatile OpenTypeFontfaceCache _singleton;

        internal static string GetCacheState()
        {
            StringBuilder state = new StringBuilder();
            state.Append("====================\n");
            state.Append("OpenType fontfaces by name\n");
            Dictionary<string, OpenTypeFontface>.KeyCollection familyKeys = Singleton._fontfaceCache.Keys;
            int count = familyKeys.Count;
            string[] keys = new string[count];
            familyKeys.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, Singleton._fontfaceCache[key].DebuggerDisplay);
            state.Append("\n");
            return state.ToString();
        }

        readonly Dictionary<string, OpenTypeFontface> _fontfaceCache;

        readonly Dictionary<ulong, OpenTypeFontface> _fontfacesByCheckSum;

        string DebuggerDisplay
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Fontfaces: {0}", _fontfaceCache.Count); }
        }
    }
}
