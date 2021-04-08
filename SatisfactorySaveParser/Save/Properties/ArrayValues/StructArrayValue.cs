using System;
using System.IO;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class StructArrayValue : IStructPropertyValue, IArrayElement
    {
        public Type BackingType => Data.GetType();

        public GameStruct Data { get; set; }

        public static StructArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
