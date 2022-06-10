

namespace PdfSharp.Drawing
{
    public sealed class XGraphicsPathInternals
    {
        internal XGraphicsPathInternals(XGraphicsPath path)
        {
            _path = path;
        }
        XGraphicsPath _path;
    }
}