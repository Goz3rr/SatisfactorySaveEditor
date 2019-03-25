using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class DynamicStructData : IStructData
    {
        public List<SerializedProperty> Fields { get; set; } = new List<SerializedProperty>();

        public int SerializedLength => Fields.Sum(f => f.SerializedLength);

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
            foreach(var field in Fields)
            {
                field.Serialize(writer);
            }
        }
    }
}
