namespace PdfSharp.Pdf.Internal
{
    class PdfDiagnostics
    {
        public static bool TraceCompressedObjects
        {
            get { return _traceCompressedObjects; }
            set { _traceCompressedObjects = value; }
        }
        static bool _traceCompressedObjects = true;

        public static bool TraceXrefStreams
        {
            get { return _traceXrefStreams && TraceCompressedObjects; }
            set { _traceXrefStreams = value; }
        }
        static bool _traceXrefStreams = true;

        public static bool TraceObjectStreams
        {
            get { return _traceObjectStreams && TraceCompressedObjects; }
            set { _traceObjectStreams = value; }
        }
        static bool _traceObjectStreams = true;
    }
}
