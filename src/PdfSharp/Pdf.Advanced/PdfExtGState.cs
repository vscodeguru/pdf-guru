using System.Globalization;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfExtGState : PdfDictionary
    {
        public PdfExtGState(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/ExtGState");

        }

        internal void SetDefault1()
        {
            Elements.SetBoolean(Keys.AIS, false);
            if (Elements.ContainsKey(Keys.BM)) Elements.SetName(Keys.BM, "/Normal");
            StrokeAlpha = 1;
            NonStrokeAlpha = 1;
            Elements.SetBoolean(Keys.op, false);
            Elements.SetBoolean(Keys.OP, false);
            Elements.SetBoolean(Keys.SA, true);
            Elements.SetName(Keys.SMask, "/None");
        }

        internal void SetDefault2()
        {
            Elements.SetBoolean(Keys.AIS, false);
            Elements.SetName(Keys.BM, "/Normal");
            StrokeAlpha = 1;
            NonStrokeAlpha = 1;
            Elements.SetBoolean(Keys.op, true);
            Elements.SetBoolean(Keys.OP, true);
            Elements.SetInteger(Keys.OPM, 1);
            Elements.SetBoolean(Keys.SA, true);
            Elements.SetName(Keys.SMask, "/None");
        }

        public double StrokeAlpha
        {
            set
            {
                _strokeAlpha = value;
                Elements.SetReal(Keys.CA, value);
                UpdateKey();
            }
        }
        double _strokeAlpha;

        public double NonStrokeAlpha
        {
            set
            {
                _nonStrokeAlpha = value;
                Elements.SetReal(Keys.ca, value);
                UpdateKey();
            }
        }
        double _nonStrokeAlpha;

        public bool StrokeOverprint
        {
            set
            {
                _strokeOverprint = value;
                Elements.SetBoolean(Keys.OP, value);
                UpdateKey();
            }
        }
        bool _strokeOverprint;

        public bool NonStrokeOverprint
        {
            set
            {
                _nonStrokeOverprint = value;
                Elements.SetBoolean(Keys.op, value);
                UpdateKey();
            }
        }
        bool _nonStrokeOverprint;

        public PdfSoftMask SoftMask
        {
            set { Elements.SetReference(Keys.SMask, value); }
        }

        internal string Key
        {
            get { return _key; }
        }

        void UpdateKey()
        {
            _key = ((int)(1000 * _strokeAlpha)).ToString(CultureInfo.InvariantCulture) +
                         ((int)(1000 * _nonStrokeAlpha)).ToString(CultureInfo.InvariantCulture) +
                         (_strokeOverprint ? "S" : "s") + (_nonStrokeOverprint ? "N" : "n");
        }
        string _key;

        internal static string MakeKey(double alpha, bool overPaint)
        {
            string key = ((int)(1000 * alpha)).ToString(CultureInfo.InvariantCulture) + (overPaint ? "O" : "0");
            return key;
        }

        internal sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string LW = "/LW";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string LC = "/LC";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string LJ = "/LJ";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string ML = "/ML";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string D = "/D";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string RI = "/RI";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string OP = "/OP";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string op = "/op";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string OPM = "/OPM";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Font = "/Font";

            [KeyInfo(KeyType.Function | KeyType.Optional)]
            public const string BG = "/BG";

            [KeyInfo(KeyType.FunctionOrName | KeyType.Optional)]
            public const string BG2 = "/BG2";

            [KeyInfo(KeyType.Function | KeyType.Optional)]
            public const string UCR = "/UCR";

            [KeyInfo(KeyType.FunctionOrName | KeyType.Optional)]
            public const string UCR2 = "/UCR2";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string SA = "/SA";

            [KeyInfo(KeyType.NameOrArray | KeyType.Optional)]
            public const string BM = "/BM";

            [KeyInfo(KeyType.NameOrDictionary | KeyType.Optional)]
            public const string SMask = "/SMask";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string CA = "/CA";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string ca = "/ca";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string AIS = "/AIS";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string TK = "/TK";

            internal static DictionaryMeta Meta
            {
                get { return _meta ?? (_meta = CreateMeta(typeof(Keys))); }
            }
            static DictionaryMeta _meta;
        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
