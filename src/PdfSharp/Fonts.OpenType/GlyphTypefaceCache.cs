using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Internal;

namespace PdfSharp.Fonts.OpenType
{
    internal class GlyphTypefaceCache
    {
        GlyphTypefaceCache()
        {
            _glyphTypefacesByKey = new Dictionary<string, XGlyphTypeface>();
        }

        public static bool TryGetGlyphTypeface(string key, out XGlyphTypeface glyphTypeface)
        {
            try
            {
                Lock.EnterFontFactory();
                bool result = Singleton._glyphTypefacesByKey.TryGetValue(key, out glyphTypeface);
                return result;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static void AddGlyphTypeface(XGlyphTypeface glyphTypeface)
        {
            try
            {
                Lock.EnterFontFactory();
                GlyphTypefaceCache cache = Singleton;
                Debug.Assert(!cache._glyphTypefacesByKey.ContainsKey(glyphTypeface.Key));
                cache._glyphTypefacesByKey.Add(glyphTypeface.Key, glyphTypeface);
            }
            finally { Lock.ExitFontFactory(); }
        }

        static GlyphTypefaceCache Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    try
                    {
                        Lock.EnterFontFactory();
                        if (_singleton == null)
                            _singleton = new GlyphTypefaceCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }
        static volatile GlyphTypefaceCache _singleton;

        internal static string GetCacheState()
        {
            StringBuilder state = new StringBuilder();
            state.Append("====================\n");
            state.Append("Glyph typefaces by name\n");
            Dictionary<string, XGlyphTypeface>.KeyCollection familyKeys = Singleton._glyphTypefacesByKey.Keys;
            int count = familyKeys.Count;
            string[] keys = new string[count];
            familyKeys.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, Singleton._glyphTypefacesByKey[key].DebuggerDisplay);
            state.Append("\n");
            return state.ToString();
        }

        readonly Dictionary<string, XGlyphTypeface> _glyphTypefacesByKey;
    }
}
