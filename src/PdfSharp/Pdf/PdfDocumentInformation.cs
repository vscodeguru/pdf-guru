using System;

namespace PdfSharp.Pdf
{
    public sealed class PdfDocumentInformation : PdfDictionary
    {
        public PdfDocumentInformation(PdfDocument document)
            : base(document)
        { }

        internal PdfDocumentInformation(PdfDictionary dict)
            : base(dict)
        { }

        public string Title
        {
            get { return Elements.GetString(Keys.Title); }
            set { Elements.SetString(Keys.Title, value); }
        }

        public string Author
        {
            get { return Elements.GetString(Keys.Author); }
            set { Elements.SetString(Keys.Author, value); }
        }

        public string Subject
        {
            get { return Elements.GetString(Keys.Subject); }
            set { Elements.SetString(Keys.Subject, value); }
        }

        public string Keywords
        {
            get { return Elements.GetString(Keys.Keywords); }
            set { Elements.SetString(Keys.Keywords, value); }
        }

        public string Creator
        {
            get { return Elements.GetString(Keys.Creator); }
            set { Elements.SetString(Keys.Creator, value); }
        }

        public string Producer
        {
            get { return Elements.GetString(Keys.Producer); }
        }

        public DateTime CreationDate
        {
            get { return Elements.GetDateTime(Keys.CreationDate, DateTime.MinValue); }
            set { Elements.SetDateTime(Keys.CreationDate, value); }
        }

        public DateTime ModificationDate
        {
            get { return Elements.GetDateTime(Keys.ModDate, DateTime.MinValue); }
            set { Elements.SetDateTime(Keys.ModDate, value); }
        }

        internal sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string Title = "/Title";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string Author = "/Author";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string Subject = "/Subject";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string Keywords = "/Keywords";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string Creator = "/Creator";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string Producer = "/Producer";

            [KeyInfo(KeyType.Date | KeyType.Optional)]
            public const string CreationDate = "/CreationDate";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string ModDate = "/ModDate";

            [KeyInfo("1.3", KeyType.Name | KeyType.Optional)]
            public const string Trapped = "/Trapped";

            public static DictionaryMeta Meta
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
