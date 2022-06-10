using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using PdfSharp.Drawing;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Internal;


namespace PdfSharp.Fonts
{
    internal static class FontFactory
    {
        public static FontResolverInfo ResolveTypeface(string familyName, FontResolvingOptions fontResolvingOptions, string typefaceKey)
        {
            if (string.IsNullOrEmpty(typefaceKey))
                typefaceKey = XGlyphTypeface.ComputeKey(familyName, fontResolvingOptions);

            try
            {
                Lock.EnterFontFactory();
                FontResolverInfo fontResolverInfo;
                if (FontResolverInfosByName.TryGetValue(typefaceKey, out fontResolverInfo))
                    return fontResolverInfo;

                IFontResolver customFontResolver = GlobalFontSettings.FontResolver;
                if (customFontResolver != null)
                {
                    fontResolverInfo = customFontResolver.ResolveTypeface(familyName, fontResolvingOptions.IsBold, fontResolvingOptions.IsItalic);

                    if (fontResolverInfo != null && !(fontResolverInfo is PlatformFontResolverInfo))
                    {
                        if (fontResolvingOptions.OverrideStyleSimulations)
                        {
                            fontResolverInfo = new FontResolverInfo(fontResolverInfo.FaceName, fontResolvingOptions.MustSimulateBold, fontResolvingOptions.MustSimulateItalic, fontResolverInfo.CollectionNumber);
                        }

                        string resolverInfoKey = fontResolverInfo.Key;
                        FontResolverInfo existingFontResolverInfo;
                        if (FontResolverInfosByName.TryGetValue(resolverInfoKey, out existingFontResolverInfo))
                        {
                            fontResolverInfo = existingFontResolverInfo;
                            FontResolverInfosByName.Add(typefaceKey, fontResolverInfo);
                        }
                        else
                        {
                            FontResolverInfosByName.Add(typefaceKey, fontResolverInfo);
                            Debug.Assert(resolverInfoKey == fontResolverInfo.Key);
                            FontResolverInfosByName.Add(resolverInfoKey, fontResolverInfo);

                            XFontSource previousFontSource;
                            if (FontSourcesByName.TryGetValue(fontResolverInfo.FaceName, out previousFontSource))
                            {
                            }
                            else
                            {
                                byte[] bytes = customFontResolver.GetFont(fontResolverInfo.FaceName);
                                XFontSource fontSource = XFontSource.GetOrCreateFrom(bytes);

                                if (string.Compare(fontResolverInfo.FaceName, fontSource.FontName, StringComparison.OrdinalIgnoreCase) != 0)
                                    FontSourcesByName.Add(fontResolverInfo.FaceName, fontSource);
                            }
                        }
                    }
                }
                else
                {
                    fontResolverInfo = PlatformFontResolver.ResolveTypeface(familyName, fontResolvingOptions, typefaceKey);
                }

                return fontResolverInfo;
            }
            finally { Lock.ExitFontFactory(); }
        }


        public static XFontSource GetFontSourceByFontName(string fontName)
        {
            XFontSource fontSource;
            if (FontSourcesByName.TryGetValue(fontName, out fontSource))
                return fontSource;

            Debug.Assert(false, string.Format("An XFontSource with the name '{0}' does not exists.", fontName));
            return null;
        }

        public static XFontSource GetFontSourceByTypefaceKey(string typefaceKey)
        {
            XFontSource fontSource;
            if (FontSourcesByName.TryGetValue(typefaceKey, out fontSource))
                return fontSource;

            Debug.Assert(false, string.Format("An XFontSource with the typeface key '{0}' does not exists.", typefaceKey));
            return null;
        }

        public static bool TryGetFontSourceByKey(ulong key, out XFontSource fontSource)
        {
            return FontSourcesByKey.TryGetValue(key, out fontSource);
        }

        public static bool HasFontSources
        {
            get { return FontSourcesByName.Count > 0; }
        }

        public static bool TryGetFontResolverInfoByTypefaceKey(string typeFaceKey, out FontResolverInfo info)
        {
            return FontResolverInfosByName.TryGetValue(typeFaceKey, out info);
        }

        public static bool TryGetFontSourceByTypefaceKey(string typefaceKey, out XFontSource source)
        {
            return FontSourcesByName.TryGetValue(typefaceKey, out source);
        }

        internal static void CacheFontResolverInfo(string typefaceKey, FontResolverInfo fontResolverInfo)
        {
            FontResolverInfo existingfFontResolverInfo;
            if (FontResolverInfosByName.TryGetValue(typefaceKey, out existingfFontResolverInfo))
            {
                throw new InvalidOperationException(string.Format("A font file with different content already exists with the specified face name '{0}'.", typefaceKey));
            }
            if (FontResolverInfosByName.TryGetValue(fontResolverInfo.Key, out existingfFontResolverInfo))
            {
                throw new InvalidOperationException(string.Format("A font resolver already exists with the specified key '{0}'.", fontResolverInfo.Key));
            }
            FontResolverInfosByName.Add(typefaceKey, fontResolverInfo);
            FontResolverInfosByName.Add(fontResolverInfo.Key, fontResolverInfo);
        }

        public static XFontSource CacheFontSource(XFontSource fontSource)
        {
            try
            {
                Lock.EnterFontFactory();
                XFontSource existingFontSource;
                if (FontSourcesByKey.TryGetValue(fontSource.Key, out existingFontSource))
                {
                    return existingFontSource;

                }

                OpenTypeFontface fontface = fontSource.Fontface;
                if (fontface == null)
                {
                    fontSource.Fontface = new OpenTypeFontface(fontSource);
                }
                FontSourcesByKey.Add(fontSource.Key, fontSource);
                FontSourcesByName.Add(fontSource.FontName, fontSource);
                return fontSource;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static XFontSource CacheNewFontSource(string typefaceKey, XFontSource fontSource)
        {
            XFontSource existingFontSource;
            if (FontSourcesByKey.TryGetValue(fontSource.Key, out existingFontSource))
            {
                return existingFontSource;

            }

            OpenTypeFontface fontface = fontSource.Fontface;
            if (fontface == null)
            {
                fontface = new OpenTypeFontface(fontSource);
                fontSource.Fontface = fontface;         
            }

            FontSourcesByName.Add(typefaceKey, fontSource);
            FontSourcesByName.Add(fontSource.FontName, fontSource);
            FontSourcesByKey.Add(fontSource.Key, fontSource);

            return fontSource;
        }

        public static void CacheExistingFontSourceWithNewTypefaceKey(string typefaceKey, XFontSource fontSource)
        {
            try
            {
                Lock.EnterFontFactory();
                FontSourcesByName.Add(typefaceKey, fontSource);
            }
            finally { Lock.ExitFontFactory(); }
        }

        internal static string GetFontCachesState()
        {
            StringBuilder state = new StringBuilder();
            string[] keys;
            int count;

            state.Append("====================\n");
            state.Append("Font resolver info by name\n");
            Dictionary<string, FontResolverInfo>.KeyCollection keyCollection = FontResolverInfosByName.Keys;
            count = keyCollection.Count;
            keys = new string[count];
            keyCollection.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, FontResolverInfosByName[key].DebuggerDisplay);
            state.Append("\n");

            state.Append("Font source by key and name\n");
            Dictionary<ulong, XFontSource>.KeyCollection fontSourceKeys = FontSourcesByKey.Keys;
            count = fontSourceKeys.Count;
            ulong[] ulKeys = new ulong[count];
            fontSourceKeys.CopyTo(ulKeys, 0);
            Array.Sort(ulKeys, delegate (ulong x, ulong y) { return x == y ? 0 : (x > y ? 1 : -1); });
            foreach (ulong ul in ulKeys)
                state.AppendFormat("  {0}: {1}\n", ul, FontSourcesByKey[ul].DebuggerDisplay);
            Dictionary<string, XFontSource>.KeyCollection fontSourceNames = FontSourcesByName.Keys;
            count = fontSourceNames.Count;
            keys = new string[count];
            fontSourceNames.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, FontSourcesByName[key].DebuggerDisplay);
            state.Append("--------------------\n\n");

            state.Append(FontFamilyCache.GetCacheState());
            state.Append(GlyphTypefaceCache.GetCacheState());
            state.Append(OpenTypeFontfaceCache.GetCacheState());
            return state.ToString();
        }

        static readonly Dictionary<string, FontResolverInfo> FontResolverInfosByName = new Dictionary<string, FontResolverInfo>(StringComparer.OrdinalIgnoreCase);

        static readonly Dictionary<string, XFontSource> FontSourcesByName = new Dictionary<string, XFontSource>(StringComparer.OrdinalIgnoreCase);

        static readonly Dictionary<ulong, XFontSource> FontSourcesByKey = new Dictionary<ulong, XFontSource>();
    }
}
