using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class InterfaceArrayValue : IInterfacePropertyValue, IArrayElement
    {
        public Type BackingType => typeof(ObjectReference);

        public ObjectReference Reference { get; set; }

        public static InterfaceArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            return new InterfaceArrayValue()
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
