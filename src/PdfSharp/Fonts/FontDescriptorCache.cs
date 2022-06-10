using System;
using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Internal;

namespace PdfSharp.Fonts
{
    internal sealed class FontDescriptorCache
    {
        FontDescriptorCache()
        {
            _cache = new Dictionary<string, FontDescriptor>();
        }

        public static FontDescriptor GetOrCreateDescriptorFor(XFont font)
        {
            if (font == null)
                throw new ArgumentNullException("font");

            string fontDescriptorKey = FontDescriptor.ComputeKey(font);
            try
            {
                Lock.EnterFontFactory();
                FontDescriptor descriptor;
                if (!Singleton._cache.TryGetValue(fontDescriptorKey, out descriptor))
                {
                    descriptor = new OpenTypeDescriptor(fontDescriptorKey, font);
                    Singleton._cache.Add(fontDescriptorKey, descriptor);
                }
                return descriptor;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static FontDescriptor GetOrCreateDescriptor(string fontFamilyName, XFontStyle style)
        {
            if (string.IsNullOrEmpty(fontFamilyName))
                throw new ArgumentNullException("fontFamilyName");

            string fontDescriptorKey = FontDescriptor.ComputeKey(fontFamilyName, style);
            try
            {
                Lock.EnterFontFactory();
                FontDescriptor descriptor;
                if (!Singleton._cache.TryGetValue(fontDescriptorKey, out descriptor))
                {
                    XFont font = new XFont(fontFamilyName, 10, style);
                    descriptor = GetOrCreateDescriptorFor(font);
                    if (Singleton._cache.ContainsKey(fontDescriptorKey))
                        Singleton.GetType();
                    else
                        Singleton._cache.Add(fontDescriptorKey, descriptor);
                }
                return descriptor;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static FontDescriptor GetOrCreateDescriptor(string idName, byte[] fontData)
        {
            string fontDescriptorKey = FontDescriptor.ComputeKey(idName);
            try
            {
                Lock.EnterFontFactory();
                FontDescriptor descriptor;
                if (!Singleton._cache.TryGetValue(fontDescriptorKey, out descriptor))
                {
                    descriptor = GetOrCreateOpenTypeDescriptor(fontDescriptorKey, idName, fontData);
                    Singleton._cache.Add(fontDescriptorKey, descriptor);
                }
                return descriptor;
            }
            finally { Lock.ExitFontFactory(); }
        }

        static OpenTypeDescriptor GetOrCreateOpenTypeDescriptor(string fontDescriptorKey, string idName, byte[] fontData)
        {
            return new OpenTypeDescriptor(fontDescriptorKey, idName, fontData);
        }

        static FontDescriptorCache Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    try
                    {
                        Lock.EnterFontFactory();
                        if (_singleton == null)
                            _singleton = new FontDescriptorCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }
        static volatile FontDescriptorCache _singleton;

        readonly Dictionary<string, FontDescriptor> _cache;
    }
}
