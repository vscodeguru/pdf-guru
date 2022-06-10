using System;

namespace PdfSharp
{
    static class Config
    {
        public const string SignificantFigures2 = "0.##";
        public const string SignificantFigures3 = "0.###";
        public const string SignificantFigures4 = "0.####";
        public const string SignificantFigures7 = "0.#######";
        public const string SignificantFigures10 = "0.##########";
        public const string SignificantFigures1Plus9 = "0.0#########";
    }

    static class Const
    {
        public const double Deg2Rad = Math.PI / 180;    

        public const double ItalicSkewAngleSinus = 0.34202014332566873304409961468226;    

        public const double BoldEmphasis = 0.02;

        public const double κ = 0.5522847498307933984022516322796;                    
    }
}
