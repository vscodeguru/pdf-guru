using System.Collections.Generic;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfExtGStateTable : PdfResourceTable
    {
        public PdfExtGStateTable(PdfDocument document)
            : base(document)
        { }


        public PdfExtGState GetExtGStateStroke(double alpha, bool overprint)
        {
            string key = PdfExtGState.MakeKey(alpha, overprint);
            PdfExtGState extGState;
            if (!_strokeAlphaValues.TryGetValue(key, out extGState))
            {
                extGState = new PdfExtGState(Owner);
                extGState.StrokeAlpha = alpha;
                if (overprint)
                {
                    extGState.StrokeOverprint = true;
                    extGState.Elements.SetInteger(PdfExtGState.Keys.OPM, 1);
                }
                _strokeAlphaValues[key] = extGState;
            }
            return extGState;
        }

        public PdfExtGState GetExtGStateNonStroke(double alpha, bool overprint)
        {
            string key = PdfExtGState.MakeKey(alpha, overprint);
            PdfExtGState extGState;
            if (!_nonStrokeStates.TryGetValue(key, out extGState))
            {
                extGState = new PdfExtGState(Owner);
                extGState.NonStrokeAlpha = alpha;
                if (overprint)
                {
                    extGState.NonStrokeOverprint = true;
                    extGState.Elements.SetInteger(PdfExtGState.Keys.OPM, 1);
                }

                _nonStrokeStates[key] = extGState;
            }
            return extGState;
        }

        readonly Dictionary<string, PdfExtGState> _strokeAlphaValues = new Dictionary<string, PdfExtGState>();
        readonly Dictionary<string, PdfExtGState> _nonStrokeStates = new Dictionary<string, PdfExtGState>();
    }
}