using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PdfSharp.Internal;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Security;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf.IO
{
    public class PdfPasswordProviderArgs
    {
        public string Password;

        public bool Abort;
    }

    public delegate void PdfPasswordProvider(PdfPasswordProviderArgs args);

    public static class PdfReader
    {
        public static int TestPdfFile(string path)
        {
#if !NETFX_CORE
            FileStream stream = null;
            try
            {
                int pageNumber;
                string realPath = Drawing.XPdfForm.ExtractPageNumber(path, out pageNumber);
                if (File.Exists(realPath))      
                {
                    stream = new FileStream(realPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    byte[] bytes = new byte[1024];
                    stream.Read(bytes, 0, 1024);
                    return GetPdfFileVersion(bytes);
                }
            }
            catch { }
            finally
            {
                try
                {
                    if (stream != null)
                    {
#if UWP
#else
                        stream.Close();
#endif
                    }
                }
                catch
                {
                }
            }
#endif
            return 0;
        }

        public static int TestPdfFile(Stream stream)
        {
            long pos = -1;
            try
            {
                pos = stream.Position;
                byte[] bytes = new byte[1024];
                stream.Read(bytes, 0, 1024);
                return GetPdfFileVersion(bytes);
            }
            catch { }
            finally
            {
                try
                {
                    if (pos != -1)
                        stream.Position = pos;
                }
                catch { }
            }
            return 0;
        }

        public static int TestPdfFile(byte[] data)
        {
            return GetPdfFileVersion(data);
        }

        internal static int GetPdfFileVersion(byte[] bytes)
        {
            try
            {
                string header = PdfEncoders.RawEncoding.GetString(bytes, 0, bytes.Length);   
                if (header[0] == '%' || header.IndexOf("%PDF", StringComparison.Ordinal) >= 0)
                {
                    int ich = header.IndexOf("PDF-", StringComparison.Ordinal);
                    if (ich > 0 && header[ich + 5] == '.')
                    {
                        char major = header[ich + 4];
                        char minor = header[ich + 6];
                        if (major >= '1' && major < '2' && minor >= '0' && minor <= '9')
                            return (major - '0') * 10 + (minor - '0');
                    }
                }
            }
            catch { }
            return 0;
        }

        public static PdfDocument Open(string path, PdfDocumentOpenMode openmode)
        {
            return Open(path, null, openmode, null);
        }

        public static PdfDocument Open(string path, PdfDocumentOpenMode openmode, PdfPasswordProvider provider)
        {
            return Open(path, null, openmode, provider);
        }

        public static PdfDocument Open(string path, string password, PdfDocumentOpenMode openmode)
        {
            return Open(path, password, openmode, null);
        }

        public static PdfDocument Open(string path, string password, PdfDocumentOpenMode openmode, PdfPasswordProvider provider)
        {
#if !NETFX_CORE
            PdfDocument document;
            Stream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                document = Open(stream, password, openmode, provider);
                if (document != null)
                {
                    document._fullPath = Path.GetFullPath(path);
                }
            }
            finally
            {
                if (stream != null)
#if !UWP
                    stream.Close();

#endif
            }
            return document;

#endif
        }

        public static PdfDocument Open(string path)
        {
            return Open(path, null, PdfDocumentOpenMode.Modify, null);
        }

        public static PdfDocument Open(string path, string password)
        {
            return Open(path, password, PdfDocumentOpenMode.Modify, null);
        }

        public static PdfDocument Open(Stream stream, PdfDocumentOpenMode openmode)
        {
            return Open(stream, null, openmode);
        }

        public static PdfDocument Open(Stream stream, PdfDocumentOpenMode openmode, PdfPasswordProvider passwordProvider)
        {
            return Open(stream, null, openmode, passwordProvider);
        }
        public static PdfDocument Open(Stream stream, string password, PdfDocumentOpenMode openmode)
        {
            return Open(stream, password, openmode, null);
        }

        public static PdfDocument Open(Stream stream, string password, PdfDocumentOpenMode openmode, PdfPasswordProvider passwordProvider)
        {
            PdfDocument document;
            try
            {
                Lexer lexer = new Lexer(stream);
                document = new PdfDocument(lexer);
                document._state |= DocumentState.Imported;
                document._openMode = openmode;
                document._fileSize = stream.Length;

                byte[] header = new byte[1024];
                stream.Position = 0;
                stream.Read(header, 0, 1024);
                document._version = GetPdfFileVersion(header);
                if (document._version == 0)
                    throw new InvalidOperationException(PSSR.InvalidPdf);

                document._irefTable.IsUnderConstruction = true;
                Parser parser = new Parser(document);
                document._trailer = parser.ReadTrailer();
                if (document._trailer == null)
                    ParserDiagnostics.ThrowParserException("Invalid PDF file: no trailer found.");     

                Debug.Assert(document._irefTable.IsUnderConstruction);
                document._irefTable.IsUnderConstruction = false;

                PdfReference xrefEncrypt = document._trailer.Elements[PdfTrailer.Keys.Encrypt] as PdfReference;
                if (xrefEncrypt != null)
                {
                    PdfObject encrypt = parser.ReadObject(null, xrefEncrypt.ObjectID, false, false);

                    encrypt.Reference = xrefEncrypt;
                    xrefEncrypt.Value = encrypt;
                    PdfStandardSecurityHandler securityHandler = document.SecurityHandler;
                    TryAgain:
                    PasswordValidity validity = securityHandler.ValidatePassword(password);
                    if (validity == PasswordValidity.Invalid)
                    {
                        if (passwordProvider != null)
                        {
                            PdfPasswordProviderArgs args = new PdfPasswordProviderArgs();
                            passwordProvider(args);
                            if (args.Abort)
                                return null;
                            password = args.Password;
                            goto TryAgain;
                        }
                        else
                        {
                            if (password == null)
                                throw new PdfReaderException(PSSR.PasswordRequired);
                            else
                                throw new PdfReaderException(PSSR.InvalidPassword);
                        }
                    }
                    else if (validity == PasswordValidity.UserPassword && openmode == PdfDocumentOpenMode.Modify)
                    {
                        if (passwordProvider != null)
                        {
                            PdfPasswordProviderArgs args = new PdfPasswordProviderArgs();
                            passwordProvider(args);
                            if (args.Abort)
                                return null;
                            password = args.Password;
                            goto TryAgain;
                        }
                        else
                            throw new PdfReaderException(PSSR.OwnerPasswordRequired);
                    }
                }
                else
                {
                    if (password != null)
                    {
                    }
                }

                PdfReference[] irefs2 = document._irefTable.AllReferences;
                int count2 = irefs2.Length;

                Dictionary<int, object> objectStreams = new Dictionary<int, object>();
                for (int idx = 0; idx < count2; idx++)
                {
                    PdfReference iref = irefs2[idx];
                    PdfCrossReferenceStream xrefStream = iref.Value as PdfCrossReferenceStream;
                    if (xrefStream != null)
                    {
                        for (int idx2 = 0; idx2 < xrefStream.Entries.Count; idx2++)
                        {
                            PdfCrossReferenceStream.CrossReferenceStreamEntry item = xrefStream.Entries[idx2];
                            if (item.Type == 2)
                            {
                                int objectNumber = (int)item.Field2;
                                if (!objectStreams.ContainsKey(objectNumber))
                                {
                                    objectStreams.Add(objectNumber, null);
                                    PdfObjectID objectID = new PdfObjectID((int)item.Field2);
                                    parser.ReadIRefsFromCompressedObject(objectID);
                                }
                            }
                        }
                    }
                }

                for (int idx = 0; idx < count2; idx++)
                {
                    PdfReference iref = irefs2[idx];
                    PdfCrossReferenceStream xrefStream = iref.Value as PdfCrossReferenceStream;
                    if (xrefStream != null)
                    {
                        for (int idx2 = 0; idx2 < xrefStream.Entries.Count; idx2++)
                        {
                            PdfCrossReferenceStream.CrossReferenceStreamEntry item = xrefStream.Entries[idx2];
                            if (item.Type == 2)
                            {
                                PdfReference irefNew = parser.ReadCompressedObject(new PdfObjectID((int)item.Field2),
                                    (int)item.Field3);
                                Debug.Assert(document._irefTable.Contains(iref.ObjectID));
                            }
                        }
                    }
                }


                PdfReference[] irefs = document._irefTable.AllReferences;
                int count = irefs.Length;

                for (int idx = 0; idx < count; idx++)
                {
                    PdfReference iref = irefs[idx];
                    if (iref.Value == null)
                    {

                        try
                        {
                            Debug.Assert(document._irefTable.Contains(iref.ObjectID));
                            PdfObject pdfObject = parser.ReadObject(null, iref.ObjectID, false, false);
                            Debug.Assert(pdfObject.Reference == iref);
                            pdfObject.Reference = iref;
                            Debug.Assert(pdfObject.Reference.Value != null, "Something went wrong.");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            throw;
                        }
                    }
                    else
                    {
                        Debug.Assert(document._irefTable.Contains(iref.ObjectID));
                    }
                    document._irefTable._maxObjectNumber = Math.Max(document._irefTable._maxObjectNumber,
                        iref.ObjectNumber);
                }

                if (xrefEncrypt != null)
                {
                    document.SecurityHandler.EncryptDocument();
                }

                document._trailer.Finish();


                if (openmode == PdfDocumentOpenMode.Modify)
                {
                    if (document.Internals.SecondDocumentID == "")
                        document._trailer.CreateNewDocumentIDs();
                    else
                    {
                        byte[] agTemp = Guid.NewGuid().ToByteArray();
                        document.Internals.SecondDocumentID = PdfEncoders.RawEncoding.GetString(agTemp, 0, agTemp.Length);
                    }

                    document.Info.ModificationDate = DateTime.Now;

                    int removed = document._irefTable.Compact();
                    if (removed != 0)
                        Debug.WriteLine("Number of deleted unreachable objects: " + removed);

                    PdfPages pages = document.Pages;
                    Debug.Assert(pages != null);

                    document._irefTable.CheckConsistence();
                    document._irefTable.Renumber();
                    document._irefTable.CheckConsistence();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return document;
        }

        public static PdfDocument Open(Stream stream)
        {
            return Open(stream, PdfDocumentOpenMode.Modify);
        }
    }
}
