using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Properties.ArrayValues;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ArrayProperty : SerializedProperty
    {
        public const string TypeName = nameof(ArrayProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(List<IArrayElement>);
        public override object BackingObject => Elements;

        public override int SerializedLength => 0;

        /// <summary>
        ///     String representation of the Property type this array consists of
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Actual content of the array
        /// </summary>
        public List<IArrayElement> Elements { get; } = new List<IArrayElement>();

        public ArrayProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public static ArrayProperty Parse(BinaryReader reader, string propertyName, int index, out int overhead)
        {
            var result = new ArrayProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            reader.AssertNullByte();

            overhead = result.Type.GetSerializedLength() + 1;

            var count = reader.ReadInt32();

            switch (result.Type)
            {
                case ByteProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new ByteArrayValue()
                            {
                                ByteValue = reader.ReadByte()
                            });
                        }
                    }
                    break;

                case EnumProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            var str = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new EnumArrayValue()
                            {
                                Type = str.Split(':')[0],
                                Value = str
                            });
                        }
                    }
                    break;

                case FloatProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            var value = reader.ReadSingle();
                            result.Elements.Add(new FloatArrayValue()
                            {
                                Value = value
                            });
                        }
                    }
                    break;

                case IntProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new IntArrayValue()
                            {
                                Value = reader.ReadInt32()
                            });
                        }
                    }
                    break;

                case InterfaceProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new InterfaceArrayValue()
                            {
                                Reference = reader.ReadObjectReference()
                            });
                        }
                    }
                    break;

                case ObjectProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new ObjectArrayValue()
                            {
                                Reference = reader.ReadObjectReference()
                            });
                        }
                    }
                    break;

                case StrProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new StrArrayValue()
                            {
                                Value = reader.ReadLengthPrefixedString()
                            });
                        }
                    }
                    break;

                case StructProperty.TypeName:
                    {
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

                            result.Elements.Add(new StructArrayValue()
                            {
                                Data = structObj
                            });
                        }
                    }
                    break;

                case TextProperty.TypeName:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(new TextArrayValue()
                            {
                                Text = TextProperty.ParseTextEntry(reader)
                            });
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException($"Unimplemented Array type: {result.Type}");
            }

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
