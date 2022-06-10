using System;
using System.Diagnostics;
using System.Globalization;
using PdfSharp.Drawing;

namespace PdfSharp.Fonts
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class FontResolverInfo
    {
        private const string KeyPrefix = "frik:";      

        public FontResolverInfo(string faceName) :
            this(faceName, false, false, 0)
        { }

        internal FontResolverInfo(string faceName, bool mustSimulateBold, bool mustSimulateItalic, int collectionNumber)
        {
            if (String.IsNullOrEmpty(faceName))
                throw new ArgumentNullException("faceName");
            if (collectionNumber != 0)
                throw new NotImplementedException("collectionNumber is not yet implemented and must be 0.");

            _faceName = faceName;
            _mustSimulateBold = mustSimulateBold;
            _mustSimulateItalic = mustSimulateItalic;
            _collectionNumber = collectionNumber;
        }

        public FontResolverInfo(string faceName, bool mustSimulateBold, bool mustSimulateItalic)
            : this(faceName, mustSimulateBold, mustSimulateItalic, 0)
        { }

        public FontResolverInfo(string faceName, XStyleSimulations styleSimulations)
            : this(faceName,
                  (styleSimulations & XStyleSimulations.BoldSimulation) == XStyleSimulations.BoldSimulation,
                  (styleSimulations & XStyleSimulations.ItalicSimulation) == XStyleSimulations.ItalicSimulation, 0)
        { }

        internal string Key
        {
            get
            {
                return _key ?? (_key = KeyPrefix + _faceName.ToLowerInvariant()
                                       + '/' + (_mustSimulateBold ? "b+" : "b-") + (_mustSimulateItalic ? "i+" : "i-"));
            }
        }
        string _key;

        public string FaceName
        {
            get { return _faceName; }
        }
        readonly string _faceName;

        public bool MustSimulateBold
        {
            get { return _mustSimulateBold; }
        }
        readonly bool _mustSimulateBold;

        public bool MustSimulateItalic
        {
            get { return _mustSimulateItalic; }
        }
        readonly bool _mustSimulateItalic;

        public XStyleSimulations StyleSimulations
        {
            get { return (_mustSimulateBold ? XStyleSimulations.BoldSimulation : 0) | (_mustSimulateItalic ? XStyleSimulations.ItalicSimulation : 0); }
        }

        internal int CollectionNumber        
        {
            get { return _collectionNumber; }
        }
        readonly int _collectionNumber;

        internal string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "FontResolverInfo: '{0}',{1}{2}", FaceName,
                    MustSimulateBold ? " simulate Bold" : "",
                    MustSimulateItalic ? " simulate Italic" : "");
            }
        }
    }
}
