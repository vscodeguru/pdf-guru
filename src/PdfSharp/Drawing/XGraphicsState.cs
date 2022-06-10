

namespace PdfSharp.Drawing
{
    public sealed class XGraphicsState
    {
#if CORE
        internal XGraphicsState()
        { }
#endif
        internal InternalGraphicsState InternalState;
    }
}
