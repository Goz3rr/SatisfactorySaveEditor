using System;
using System.Collections.Generic;
using System.IO;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Properties.ArrayValues;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save.Properties
{
    public class SetProperty : SerializedProperty
    {
        public const string TypeName = nameof(SetProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(List<IArrayElement>);
        public override object BackingObject => Elements;

        public override int SerializedLength => 0;

        /// <summary>
        ///     String representation of the Property type this set consists of
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Actual content of the set
        /// </summary>
        public List<IArrayElement> Elements { get; } = new List<IArrayElement>();

        public SetProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public static SetProperty Parse(BinaryReader reader, string propertyName, int index, int buildVersion, out int overhead)
        {
            var result = new SetProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.Length + 6;

            result.ReadPropertyGuid(reader);
            reader.AssertNullInt32();

            var count = reader.ReadInt32();

            switch (result.Type)
            {
                case StructProperty.TypeName:
                    {
                        var pos = reader.BaseStream.Position;
                        var unk = reader.ReadInt32();
                        var gameStruct = new DynamicGameStruct(null);
                        gameStruct.Deserialize(reader, buildVersion);
                        result.Elements.Add(new StructArrayValue()
                        {
                            Data = gameStruct
                        });
                        overhead += (int)(reader.BaseStream.Position - pos);
                    }
                    break;
                default:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(SatisfactorySaveSerializer.DeserializeArrayElement(result.Type, reader, buildVersion));
                        }
                    }
                    break;
            }

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
