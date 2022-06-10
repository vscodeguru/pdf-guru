using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace PdfSharp.Pdf
{
    internal sealed class KeyDescriptor
    {
        public KeyDescriptor(KeyInfoAttribute attribute)
        {
            _version = attribute.Version;
            _keyType = attribute.KeyType;
            _fixedValue = attribute.FixedValue;
            _objectType = attribute.ObjectType;

            if (_version == "")
                _version = "1.0";
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        string _version;

        public KeyType KeyType
        {
            get { return _keyType; }
            set { _keyType = value; }
        }
        KeyType _keyType;

        public string KeyValue
        {
            get { return _keyValue; }
            set { _keyValue = value; }
        }
        string _keyValue;

        public string FixedValue
        {
            get { return _fixedValue; }
        }
        readonly string _fixedValue;

        public Type ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }
        Type _objectType;

        public bool CanBeIndirect
        {
            get { return (_keyType & KeyType.MustNotBeIndirect) == 0; }
        }

        public Type GetValueType()
        {
            Type type = _objectType;
            if (type == null)
            {
                switch (_keyType & KeyType.TypeMask)
                {
                    case KeyType.Name:
                        type = typeof(PdfName);
                        break;

                    case KeyType.String:
                        type = typeof(PdfString);
                        break;

                    case KeyType.Boolean:
                        type = typeof(PdfBoolean);
                        break;

                    case KeyType.Integer:
                        type = typeof(PdfInteger);
                        break;

                    case KeyType.Real:
                        type = typeof(PdfReal);
                        break;

                    case KeyType.Date:
                        type = typeof(PdfDate);
                        break;

                    case KeyType.Rectangle:
                        type = typeof(PdfRectangle);
                        break;

                    case KeyType.Array:
                        type = typeof(PdfArray);
                        break;

                    case KeyType.Dictionary:
                        type = typeof(PdfDictionary);
                        break;

                    case KeyType.Stream:
                        type = typeof(PdfDictionary);
                        break;

                    case KeyType.NumberTree:
                        throw new NotImplementedException("KeyType.NumberTree");

                    case KeyType.NameOrArray:
                        throw new NotImplementedException("KeyType.NameOrArray");

                    case KeyType.ArrayOrDictionary:
                        throw new NotImplementedException("KeyType.ArrayOrDictionary");

                    case KeyType.StreamOrArray:
                        throw new NotImplementedException("KeyType.StreamOrArray");

                    case KeyType.ArrayOrNameOrString:
                        return null;     
                    default:
                        Debug.Assert(false, "Invalid KeyType: " + _keyType);
                        break;
                }
            }
            return type;
        }
    }

    internal class DictionaryMeta
    {
        public DictionaryMeta(Type type)
        {
#if !NETFX_CORE && !UWP
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (FieldInfo field in fields)
            {
                object[] attributes = field.GetCustomAttributes(typeof(KeyInfoAttribute), false);
                if (attributes.Length == 1)
                {
                    KeyInfoAttribute attribute = (KeyInfoAttribute)attributes[0];
                    KeyDescriptor descriptor = new KeyDescriptor(attribute);
                    descriptor.KeyValue = (string)field.GetValue(null);
                    _keyDescriptors[descriptor.KeyValue] = descriptor;
                }
            }
#endif
        }

        public KeyDescriptor this[string key]
        {
            get
            {
                KeyDescriptor keyDescriptor;
                _keyDescriptors.TryGetValue(key, out keyDescriptor);
                return keyDescriptor;
            }
        }

        readonly Dictionary<string, KeyDescriptor> _keyDescriptors = new Dictionary<string, KeyDescriptor>();
    }
}
