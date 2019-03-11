using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class StructProperty : ISerializedField
    {

        public override string ToString()
        {
            return $"";
        }

        public static StructProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new StructProperty();

            return result;
        }
    }
}
