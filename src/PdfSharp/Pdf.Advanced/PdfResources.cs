using System.Collections.Generic;

namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfResources : PdfDictionary
    {
        public PdfResources(PdfDocument document)
            : base(document)
        {
            Elements[Keys.ProcSet] = new PdfLiteral("[/PDF/Text/ImageB/ImageC/ImageI]");
        }

        internal PdfResources(PdfDictionary dict)
            : base(dict)
        { }

        public string AddFont(PdfFont font)
        {
            string name;
            if (!_resources.TryGetValue(font, out name))
            {
                name = NextFontName;
                _resources[font] = name;
                if (font.Reference == null)
                    Owner._irefTable.Add(font);
                Fonts.Elements[name] = font.Reference;
            }
            return name;
        }

        public string AddImage(PdfImage image)
        {
            string name;
            if (!_resources.TryGetValue(image, out name))
            {
                name = NextImageName;
                _resources[image] = name;
                if (image.Reference == null)
                    Owner._irefTable.Add(image);
                XObjects.Elements[name] = image.Reference;
            }
            return name;
        }

        public string AddForm(PdfFormXObject form)
        {
            string name;
            if (!_resources.TryGetValue(form, out name))
            {
                name = NextFormName;
                _resources[form] = name;
                if (form.Reference == null)
                    Owner._irefTable.Add(form);
                XObjects.Elements[name] = form.Reference;
            }
            return name;
        }

        public string AddExtGState(PdfExtGState extGState)
        {
            string name;
            if (!_resources.TryGetValue(extGState, out name))
            {
                name = NextExtGStateName;
                _resources[extGState] = name;
                if (extGState.Reference == null)
                    Owner._irefTable.Add(extGState);
                ExtGStates.Elements[name] = extGState.Reference;
            }
            return name;
        }

        public string AddPattern(PdfShadingPattern pattern)
        {
            string name;
            if (!_resources.TryGetValue(pattern, out name))
            {
                name = NextPatternName;
                _resources[pattern] = name;
                if (pattern.Reference == null)
                    Owner._irefTable.Add(pattern);
                Patterns.Elements[name] = pattern.Reference;
            }
            return name;
        }

        public string AddPattern(PdfTilingPattern pattern)
        {
            string name;
            if (!_resources.TryGetValue(pattern, out name))
            {
                name = NextPatternName;
                _resources[pattern] = name;
                if (pattern.Reference == null)
                    Owner._irefTable.Add(pattern);
                Patterns.Elements[name] = pattern.Reference;
            }
            return name;
        }

        public string AddShading(PdfShading shading)
        {
            string name;
            if (!_resources.TryGetValue(shading, out name))
            {
                name = NextShadingName;
                _resources[shading] = name;
                if (shading.Reference == null)
                    Owner._irefTable.Add(shading);
                Shadings.Elements[name] = shading.Reference;
            }
            return name;
        }

        internal PdfResourceMap Fonts
        {
            get { return _fonts ?? (_fonts = (PdfResourceMap)Elements.GetValue(Keys.Font, VCF.Create)); }
        }
        PdfResourceMap _fonts;

        internal PdfResourceMap XObjects
        {
            get { return _xObjects ?? (_xObjects = (PdfResourceMap)Elements.GetValue(Keys.XObject, VCF.Create)); }
        }
        PdfResourceMap _xObjects;

        internal PdfResourceMap ExtGStates
        {
            get
            {
                return _extGStates ?? (_extGStates = (PdfResourceMap)Elements.GetValue(Keys.ExtGState, VCF.Create));
            }
        }
        PdfResourceMap _extGStates;

        internal PdfResourceMap ColorSpaces
        {
            get { return _colorSpaces ?? (_colorSpaces = (PdfResourceMap)Elements.GetValue(Keys.ColorSpace, VCF.Create)); }
        }
        PdfResourceMap _colorSpaces;

        internal PdfResourceMap Patterns
        {
            get { return _patterns ?? (_patterns = (PdfResourceMap) Elements.GetValue(Keys.Pattern, VCF.Create)); }
        }
        PdfResourceMap _patterns;

        internal PdfResourceMap Shadings
        {
            get { return _shadings ?? (_shadings = (PdfResourceMap) Elements.GetValue(Keys.Shading, VCF.Create)); }
        }
        PdfResourceMap _shadings;

        internal PdfResourceMap Properties
        {
            get {return _properties ?? (_properties = (PdfResourceMap) Elements.GetValue(Keys.Properties, VCF.Create));}
        }
        PdfResourceMap _properties;

        string NextFontName
        {
            get
            {
                string name;
                while (ExistsResourceNames(name = string.Format("/F{0}", _fontNumber++))) { }
                return name;
            }
        }
        int _fontNumber;

        string NextImageName
        {
            get
            {
                string name;
                while (ExistsResourceNames(name = string.Format("/I{0}", _imageNumber++))) { }
                return name;
            }
        }
        int _imageNumber;

        string NextFormName
        {
            get
            {
                string name;
                while (ExistsResourceNames(name = string.Format("/Fm{0}", _formNumber++))) { }
                return name;
            }
        }
        int _formNumber;

        string NextExtGStateName
        {
            get
            {
                string name;
                while (ExistsResourceNames(name = string.Format("/GS{0}", _extGStateNumber++))) { }
                return name;
            }
        }
        int _extGStateNumber;

        string NextPatternName
        {
            get
            {
                string name;
                while (ExistsResourceNames(name = string.Format("/Pa{0}", _patternNumber++))) ;
                return name;
            }
        }
        int _patternNumber;

        string NextShadingName
        {
            get
            {
                string name;
                while (ExistsResourceNames(name = string.Format("/Sh{0}", _shadingNumber++))) ;
                return name;
            }
        }
        int _shadingNumber;

        internal bool ExistsResourceNames(string name)
        {
            if (_importedResourceNames == null)
            {
                _importedResourceNames = new Dictionary<string, object>();

                if (Elements[Keys.Font] != null)
                    Fonts.CollectResourceNames(_importedResourceNames);

                if (Elements[Keys.XObject] != null)
                    XObjects.CollectResourceNames(_importedResourceNames);

                if (Elements[Keys.ExtGState] != null)
                    ExtGStates.CollectResourceNames(_importedResourceNames);

                if (Elements[Keys.ColorSpace] != null)
                    ColorSpaces.CollectResourceNames(_importedResourceNames);

                if (Elements[Keys.Pattern] != null)
                    Patterns.CollectResourceNames(_importedResourceNames);

                if (Elements[Keys.Shading] != null)
                    Shadings.CollectResourceNames(_importedResourceNames);

                if (Elements[Keys.Properties] != null)
                    Properties.CollectResourceNames(_importedResourceNames);
            }
            return _importedResourceNames.ContainsKey(name);
        }

        Dictionary<string, object> _importedResourceNames;

        readonly Dictionary<PdfObject, string> _resources = new Dictionary<PdfObject, string>();

        public sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResourceMap))]
            public const string ExtGState = "/ExtGState";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResourceMap))]
            public const string ColorSpace = "/ColorSpace";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResourceMap))]
            public const string Pattern = "/Pattern";

            [KeyInfo("1.3", KeyType.Dictionary | KeyType.Optional, typeof(PdfResourceMap))]
            public const string Shading = "/Shading";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResourceMap))]
            public const string XObject = "/XObject";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResourceMap))]
            public const string Font = "/Font";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string ProcSet = "/ProcSet";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional, typeof(PdfResourceMap))]
            public const string Properties = "/Properties";

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
