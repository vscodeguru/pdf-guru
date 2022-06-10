using System;
using System.Diagnostics;
using PdfSharp.Drawing;
using PdfSharp.Fonts;

namespace PdfSharp.Internal
{
    internal static class DiagnosticsHelper
    {
        public static void HandleNotImplemented(string message)
        {
            string text = "Not implemented: " + message;
            switch (Diagnostics.NotImplementedBehaviour)
            {
                case NotImplementedBehaviour.DoNothing:
                    break;

                case NotImplementedBehaviour.Log:
                    Logger.Log(text);
                    break;

                case NotImplementedBehaviour.Throw:
                    ThrowNotImplementedException(text);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void ThrowNotImplementedException(string message)
        {
            throw new NotImplementedException(message);
        }
    }

    internal static class Logger
    {
        public static void Log(string format, params object[] args)
        {
            Debug.WriteLine("Log...");
        }
    }

    class Logging
    {

    }

    class Tracing
    {
        [Conditional("DEBUG")]
        public void Foo()
        { }
    }

    public static class DebugBreak
    {
        public static void Break()
        {
            Break(false);
        }

        public static void Break(bool always)
        {
        }
    }

    public static class FontsDevHelper
    {
        public static XFont CreateSpecialFont(string familyName, double emSize, XFontStyle style,
            XPdfFontOptions pdfOptions, XStyleSimulations styleSimulations)
        {
            return new XFont(familyName, emSize, style, pdfOptions, styleSimulations);
        }

        public static string GetFontCachesState()
        {
            return FontFactory.GetFontCachesState();
        }
    }
}
