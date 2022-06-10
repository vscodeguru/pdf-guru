using System;
using GdiFont = System.Drawing.Font;
using PdfSharp.Fonts;
using PdfSharp.Fonts.OpenType;

namespace PdfSharp.Drawing
{
    public sealed class XFontFamily
    {
        public XFontFamily(string familyName)
        {
            FamilyInternal = FontFamilyInternal.GetOrCreateFromName(familyName, true);
        }

        internal XFontFamily(string familyName, bool createPlatformObjects)
        {
            FamilyInternal = FontFamilyInternal.GetOrCreateFromName(familyName, createPlatformObjects);
        }

        XFontFamily(FontFamilyInternal fontFamilyInternal)
        {
            FamilyInternal = fontFamilyInternal;
        }


        internal static XFontFamily CreateFromName_not_used(string name, bool createPlatformFamily)
        {
            XFontFamily fontFamily = new XFontFamily(name);
            if (createPlatformFamily)
            {

            }
            return fontFamily;
        }

        internal static XFontFamily GetOrCreateFontFamily(string name)
        {
            FontFamilyInternal fontFamilyInternal = FontFamilyCache.GetFamilyByName(name);
            if (fontFamilyInternal == null)
            {
                fontFamilyInternal = FontFamilyInternal.GetOrCreateFromName(name, false);
                fontFamilyInternal = FontFamilyCache.CacheOrGetFontFamily(fontFamilyInternal);
            }

            return new XFontFamily(fontFamilyInternal);
        }

#if CORE || GDI
        internal static XFontFamily GetOrCreateFromGdi(GdiFont font)
        {
            FontFamilyInternal fontFamilyInternal = FontFamilyInternal.GetOrCreateFromGdi(font.FontFamily);
            return new XFontFamily(fontFamilyInternal);
        }
#endif


        public string Name
        {
            get { return FamilyInternal.Name; }
        }

        public int GetCellAscent(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.Ascender;
            return result;
        }

        public int GetCellDescent(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.Descender;
            return result;
        }

        public int GetEmHeight(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.UnitsPerEm;
            return result;
        }

        public int GetLineSpacing(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.LineSpacing;
            return result;
        }

        public bool IsStyleAvailable(XFontStyle style)
        {
            XGdiFontStyle xStyle = ((XGdiFontStyle)style) & XGdiFontStyle.BoldItalic;
#if CORE
            throw new InvalidOperationException("In CORE build it is the responsibility of the developer to provide all required font faces.");
#endif
        }

        [Obsolete("Use platform API directly.")]
        public static XFontFamily[] Families
        {
            get
            {
                throw new InvalidOperationException("Obsolete and not implemted any more.");
            }
        }

        [Obsolete("Use platform API directly.")]
        public static XFontFamily[] GetFamilies(XGraphics graphics)
        {
            throw new InvalidOperationException("Obsolete and not implemted any more.");
        }
        internal FontFamilyInternal FamilyInternal;
    }
}
