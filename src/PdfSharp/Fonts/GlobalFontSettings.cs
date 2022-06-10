using System;
using PdfSharp.Internal;
using PdfSharp.Pdf;

namespace PdfSharp.Fonts
{
    public static class GlobalFontSettings
    {
        public const string DefaultFontName = "PlatformDefault";

        public static IFontResolver FontResolver
        {
            get { return _fontResolver; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                try
                {
                    Lock.EnterFontFactory();
                    if (ReferenceEquals(_fontResolver, value))
                        return;

                    if (FontFactory.HasFontSources)
                        throw new InvalidOperationException("Must not change font resolver after is was once used.");

                    _fontResolver = value;
                }
                finally { Lock.ExitFontFactory(); }
            }
        }
        static IFontResolver _fontResolver;

        public static PdfFontEncoding DefaultFontEncoding
        {
            get
            {
                if (!_fontEncodingInitialized)
                    DefaultFontEncoding = PdfFontEncoding.Unicode;
                return _fontEncoding;
            }
            set
            {
                try
                {
                    Lock.EnterFontFactory();
                    if (_fontEncodingInitialized)
                    {
                        if (_fontEncoding == value)
                            return;
                        throw new InvalidOperationException("Must not change DefaultFontEncoding after is was set once.");
                    }

                    _fontEncoding = value;
                    _fontEncodingInitialized = true;
                }
                finally { Lock.ExitFontFactory(); }
            }
        }
        static PdfFontEncoding _fontEncoding;
        static bool _fontEncodingInitialized;
    }
}