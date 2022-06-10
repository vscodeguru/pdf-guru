using System;

namespace PdfSharp.Pdf.Security
{
    [Flags]
    internal enum PdfUserAccessPermission
    {
        PermitAll = -3,   

        PermitPrint = 0x00000004,      

        PermitModifyDocument = 0x00000008,      

        PermitExtractContent = 0x00000010,      

        PermitAnnotations = 0x00000020,      

        PermitFormsFill = 0x00000100,      

        PermitAccessibilityExtractContent = 0x00000200,      

        PermitAssembleDocument = 0x00000400,      

        PermitFullQualityPrint = 0x00000800,      

    }
}
