using System;

namespace PdfSharp.Drawing
{
    [Flags]
    public enum XStyleSimulations     
    {
        None = 0,

        BoldSimulation = 1,

        ItalicSimulation = 2,

        BoldItalicSimulation = ItalicSimulation | BoldSimulation,
    }
}
