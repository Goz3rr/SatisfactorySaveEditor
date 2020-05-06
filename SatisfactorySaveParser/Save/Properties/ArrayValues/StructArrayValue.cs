using System;
using System.IO;

using SatisfactorySaveParser.Game.Structs;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class StructArrayValue : IStructPropertyValue, IArrayElement
    {
        public GameStruct Data { get; set; }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
