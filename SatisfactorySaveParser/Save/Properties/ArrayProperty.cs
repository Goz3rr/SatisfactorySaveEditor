using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using SatisfactorySaveParser.Game.Structs;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ArrayProperty : SerializedProperty
    {
        public const string TypeName = nameof(ArrayProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(List<SerializedProperty>);
        public override object BackingObject => Elements;

        public override int SerializedLength => 0;

        /// <summary>
        ///     String representation of the Property type this array consists of
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Actual content of the array
        /// </summary>
        public List<SerializedProperty> Elements { get; } = new List<SerializedProperty>();

        public ArrayProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public static ArrayProperty Parse(BinaryReader reader, string propertyName, int size, int index, out int overhead)
        {
            var result = new ArrayProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            overhead = result.Type.GetSerializedLength() + 1;

            switch (result.Type)
            {
                case StructProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        var name = reader.ReadLengthPrefixedString();

                        var propertyType = reader.ReadLengthPrefixedString();
                        Trace.Assert(propertyType == "StructProperty");

                        var structSize = reader.ReadInt32();
                        var structIndex = reader.ReadInt32();

                        var structType = reader.ReadLengthPrefixedString();

                        var unk1 = reader.ReadInt32();
                        var unk2 = reader.ReadInt32();
                        var unk3 = reader.ReadInt32();
                        var unk4 = reader.ReadInt32();
                        var unk5 = reader.ReadByte();

                        for (var i = 0; i < count; i++)
                        {
                            var structObj = GameStructFactory.CreateFromType(structType);
                            structObj.Deserialize(reader);
                        }
                    }
                    break;

                case ObjectProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new ObjectProperty(null)
                            {
                                Reference = reader.ReadObjectReference()
                            });
                        }
                    }
                    break;

                case IntProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new IntProperty(null)
                            {
                                Value = reader.ReadInt32()
                            });
                        }
                    }
                    break;

                case ByteProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new ByteProperty(null)
                            {
                                ByteValue = reader.ReadByte()
                            });
                        }
                    }
                    break;

                case EnumProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var str = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new EnumProperty(null)
                            {
                                Type = str.Split(':')[0],
                                Value = str
                            });
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
