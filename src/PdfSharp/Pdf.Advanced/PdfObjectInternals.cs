namespace PdfSharp.Pdf.Advanced
{
    public class PdfObjectInternals
    {
        internal PdfObjectInternals(PdfObject obj)
        {
            _obj = obj;
        }
        readonly PdfObject _obj;

        public PdfObjectID ObjectID
        {
            get { return _obj.ObjectID; }
        }

        public int ObjectNumber
        {
            get { return _obj.ObjectID.ObjectNumber; }
        }

        public int GenerationNumber
        {
            get { return _obj.ObjectID.GenerationNumber; }
        }

        public string TypeID
        {
            get
            {
                if (_obj is PdfArray)
                    return "array";
                if (_obj is PdfDictionary)
                    return "dictionary";
                return _obj.GetType().Name;
            }
        }
    }
}
