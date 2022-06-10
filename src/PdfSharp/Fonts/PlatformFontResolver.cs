using System;
using System.Diagnostics;
using GdiFont = System.Drawing.Font;
using GdiFontStyle = System.Drawing.FontStyle;
using PdfSharp.Drawing;

namespace PdfSharp.Fonts
{
    public static class PlatformFontResolver
    {
        public static FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            FontResolvingOptions fontResolvingOptions = new FontResolvingOptions(FontHelper.CreateStyle(isBold, isItalic));
            return ResolveTypeface(familyName, fontResolvingOptions, XGlyphTypeface.ComputeKey(familyName, fontResolvingOptions));
        }

        internal static FontResolverInfo ResolveTypeface(string familyName, FontResolvingOptions fontResolvingOptions, string typefaceKey)
        {
            if (string.IsNullOrEmpty(typefaceKey))
                typefaceKey = XGlyphTypeface.ComputeKey(familyName, fontResolvingOptions);

            FontResolverInfo fontResolverInfo;
            if (FontFactory.TryGetFontResolverInfoByTypefaceKey(typefaceKey, out fontResolverInfo))
                return fontResolverInfo;

#if (CORE || GDI) && !WPF
            GdiFont gdiFont;
            XFontSource fontSource = CreateFontSource(familyName, fontResolvingOptions, out gdiFont, typefaceKey);
#endif

            if (fontSource == null)
                return null;

            if (fontResolvingOptions.OverrideStyleSimulations)
            {
#if (CORE || GDI) && !WPF
                fontResolverInfo = new PlatformFontResolverInfo(typefaceKey, fontResolvingOptions.MustSimulateBold, fontResolvingOptions.MustSimulateItalic, gdiFont);
#endif
            }
            else
            {
#if (CORE || GDI) && !WPF
                bool mustSimulateBold = gdiFont.Bold && !fontSource.Fontface.os2.IsBold;
                bool mustSimulateItalic = gdiFont.Italic && !fontSource.Fontface.os2.IsItalic;
                fontResolverInfo = new PlatformFontResolverInfo(typefaceKey, mustSimulateBold, mustSimulateItalic, gdiFont);
#endif
            }

            FontFactory.CacheFontResolverInfo(typefaceKey, fontResolverInfo);

            return fontResolverInfo;
        }

#if (CORE_WITH_GDI || GDI) && !WPF
        internal static XFontSource CreateFontSource(string familyName, FontResolvingOptions fontResolvingOptions, out GdiFont font, string typefaceKey)
        {
            if (string.IsNullOrEmpty(typefaceKey))
                typefaceKey = XGlyphTypeface.ComputeKey(familyName, fontResolvingOptions);
            GdiFontStyle gdiStyle = (GdiFontStyle)(fontResolvingOptions.FontStyle & XFontStyle.BoldItalic);

            XFontSource fontSource;
            font = FontHelper.CreateFont(familyName, 10, gdiStyle, out fontSource);

            if (fontSource != null)
            {
                Debug.Assert(font != null);
            }
            else
            {
                fontSource = XFontSource.GetOrCreateFromGdi(typefaceKey, font);
            }
            return fontSource;
        }
#endif

    }
}
