using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class ObjectArrayValue : IObjectPropertyValue, IArrayElement
    {
        public Type BackingType => typeof(ObjectReference);

        public ObjectReference Reference { get; set; }

        public static ObjectArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            return new ObjectArrayValue()
            {
                Reference = reader.ReadObjectReference()
            };
        }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
