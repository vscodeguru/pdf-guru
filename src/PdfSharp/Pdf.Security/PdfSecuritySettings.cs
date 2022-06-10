using System;

namespace PdfSharp.Pdf.Security
{
    public sealed class PdfSecuritySettings
    {
        internal PdfSecuritySettings(PdfDocument document)
        {
            _document = document;
        }
        readonly PdfDocument _document;

        public bool HasOwnerPermissions
        {
            get { return _hasOwnerPermissions; }
        }
        internal bool _hasOwnerPermissions = true;

        public PdfDocumentSecurityLevel DocumentSecurityLevel
        {
            get { return _documentSecurityLevel; }
            set { _documentSecurityLevel = value; }
        }
        PdfDocumentSecurityLevel _documentSecurityLevel;

        public string UserPassword
        {
            set { SecurityHandler.UserPassword = value; }
        }

        public string OwnerPassword
        {
            set { SecurityHandler.OwnerPassword = value; }
        }

        internal bool CanSave(ref string message)
        {
            if (_documentSecurityLevel != PdfDocumentSecurityLevel.None)
            {
                if (String.IsNullOrEmpty(SecurityHandler._userPassword) && String.IsNullOrEmpty(SecurityHandler._ownerPassword))
                {
                    message = PSSR.UserOrOwnerPasswordRequired;
                    return false;
                }
            }
            return true;
        }

        public bool PermitPrint
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitPrint) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitPrint;
                else
                    permission &= ~PdfUserAccessPermission.PermitPrint;
                SecurityHandler.Permission = permission;
            }
        }

        public bool PermitModifyDocument
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitModifyDocument) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitModifyDocument;
                else
                    permission &= ~PdfUserAccessPermission.PermitModifyDocument;
                SecurityHandler.Permission = permission;
            }
        }

        public bool PermitExtractContent
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitExtractContent) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitExtractContent;
                else
                    permission &= ~PdfUserAccessPermission.PermitExtractContent;
                SecurityHandler.Permission = permission;
            }
        }

        public bool PermitAnnotations
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitAnnotations) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitAnnotations;
                else
                    permission &= ~PdfUserAccessPermission.PermitAnnotations;
                SecurityHandler.Permission = permission;
            }
        }

        public bool PermitFormsFill
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitFormsFill) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitFormsFill;
                else
                    permission &= ~PdfUserAccessPermission.PermitFormsFill;
                SecurityHandler.Permission = permission;
            }
        }

        public bool PermitAccessibilityExtractContent
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitAccessibilityExtractContent) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitAccessibilityExtractContent;
                else
                    permission &= ~PdfUserAccessPermission.PermitAccessibilityExtractContent;
                SecurityHandler.Permission = permission;
            }
        }

        public bool PermitAssembleDocument
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitAssembleDocument) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitAssembleDocument;
                else
                    permission &= ~PdfUserAccessPermission.PermitAssembleDocument;
                SecurityHandler.Permission = permission;
            }
        }

        public bool PermitFullQualityPrint
        {
            get { return (SecurityHandler.Permission & PdfUserAccessPermission.PermitFullQualityPrint) != 0; }
            set
            {
                PdfUserAccessPermission permission = SecurityHandler.Permission;
                if (value)
                    permission |= PdfUserAccessPermission.PermitFullQualityPrint;
                else
                    permission &= ~PdfUserAccessPermission.PermitFullQualityPrint;
                SecurityHandler.Permission = permission;
            }
        }
        internal PdfStandardSecurityHandler SecurityHandler
        {
            get { return _document._trailer.SecurityHandler; }
        }
    }
}
