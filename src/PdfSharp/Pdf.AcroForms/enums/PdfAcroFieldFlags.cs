using System;

namespace PdfSharp.Pdf.AcroForms
{
    [Flags]
    public enum PdfAcroFieldFlags
    {
        ReadOnly = 1 << (1 - 1),

        Required = 1 << (2 - 1),

        NoExport = 1 << (3 - 1),

        Pushbutton = 1 << (17 - 1),

        Radio = 1 << (16 - 1),

        NoToggleToOff = 1 << (15 - 1),

        Multiline = 1 << (13 - 1),

        Password = 1 << (14 - 1),

        FileSelect = 1 << (21 - 1),

        DoNotSpellCheckTextField = 1 << (23 - 1),

        DoNotScroll = 1 << (24 - 1),

        Combo = 1 << (18 - 1),

        Edit = 1 << (19 - 1),

        Sort = 1 << (20 - 1),

        MultiSelect = 1 << (22 - 1),

        DoNotSpellCheckChoiseField = 1 << (23 - 1),
    }
}
