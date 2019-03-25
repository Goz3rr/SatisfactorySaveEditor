using System;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class DynamicStructData : IStructData
    {
        public List<SerializedProperty> Fields { get; set; } = new List<SerializedProperty>();

        public int SerializedLength => throw new NotImplementedException();

        public DynamicStructData(BinaryReader reader)
        {
            SerializedProperty prop;
            while ((prop = SerializedProperty.Parse(reader)) != null)
            {
                Fields.Add(prop);
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
