namespace PdfSharp.Fonts
{
    public interface IFontResolver
    {
        FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic);

        byte[] GetFont(string faceName);
    }
}