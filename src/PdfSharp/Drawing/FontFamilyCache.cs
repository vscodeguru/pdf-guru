using PdfSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;
namespace PdfSharp.Drawing

{
    internal sealed class FontFamilyCache
    {
        FontFamilyCache()
        {
            _familiesByName = new Dictionary<string, FontFamilyInternal>(StringComparer.OrdinalIgnoreCase);
        }

        public static FontFamilyInternal GetFamilyByName(string familyName)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal family;
                Singleton._familiesByName.TryGetValue(familyName, out family);
                return family;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static FontFamilyInternal CacheOrGetFontFamily(FontFamilyInternal fontFamily)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal existingFontFamily;
                if (Singleton._familiesByName.TryGetValue(fontFamily.Name, out existingFontFamily))
                {

                    return existingFontFamily;
                }
                Singleton._familiesByName.Add(fontFamily.Name, fontFamily);
                return fontFamily;
            }
            finally { Lock.ExitFontFactory(); }
        }

        static FontFamilyCache Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    try
                    {
                        Lock.EnterFontFactory();
                        if (_singleton == null)
                            _singleton = new FontFamilyCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }
        static volatile FontFamilyCache _singleton;

        internal static string GetCacheState()
        {
            StringBuilder state = new StringBuilder();
            state.Append("====================\n");
            state.Append("Font families by name\n");
            Dictionary<string, FontFamilyInternal>.KeyCollection familyKeys = Singleton._familiesByName.Keys;
            int count = familyKeys.Count;
            string[] keys = new string[count];
            familyKeys.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, Singleton._familiesByName[key].DebuggerDisplay);
            state.Append("\n");
            return state.ToString();
        }

        readonly Dictionary<string, FontFamilyInternal> _familiesByName;
    }
}
