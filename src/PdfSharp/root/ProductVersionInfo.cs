
namespace PdfSharp
{
    public static class ProductVersionInfo
    {
        public const string Title = "PDFsharp";

        public const string Description = "A .NET library for processing PDF.";

        public const string Producer = Title + " " + VersionMajor + "." + VersionMinor + "." + VersionBuild + Technology + " (" + Url + ")";

        public const string Producer2 = Title + " " + VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch + Technology + " (" + Url + ")";

        public const string Version = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch;

        public const string Version2 = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionPatch + Technology;

        public const string Url = "www.pdfsharp.com";

        public const string Configuration = "";

        public const string Company = "empira Software GmbH, Cologne Area (Germany)";

        public const string Product = "PDFsharp";

        public const string Copyright = "Copyright © 2005-2019 empira Software GmbH.";

        public const string Trademark = "PDFsharp";

        public const string Culture = "";

        public const string VersionMajor = "1";

        public const string VersionMinor = "50";

        public const string VersionBuild = "5147";                 

        public const string VersionPatch = "0";

        public const string VersionPrerelease = "";                 

        public const string VersionReferenceDate = "2005-01-01";

        public const string NuGetID = "PDFsharp";

        public const string NuGetTitle = "PDFsharp";

        public const string NuGetAuthors = "empira Software GmbH";

        public const string NuGetOwners = "empira Software GmbH";

        public const string NuGetDescription = "PDFsharp is the Open Source .NET library that easily creates and processes PDF documents on the fly from any .NET language. The same drawing routines can be used to create PDF documents, draw on the screen, or send output to any printer.";

        public const string NuGetReleaseNotes = "";

        public const string NuGetSummary = "A .NET library for processing PDF.";

        public const string NuGetLanguage = "";

        public const string NuGetProjectUrl = "www.pdfsharp.net";

        public const string NuGetIconUrl = "http://www.pdfsharp.net/resources/PDFsharp-Logo-32x32.png";

        public const string NuGetLicenseUrl = "http://www.pdfsharp.net/PDFsharp_License.ashx";

        public const bool NuGetRequireLicenseAcceptance = false;

        public const string NuGetTags = "PDFsharp PDF creation";

#if CORE
        public const string Technology = "";    
#endif
    }
}
