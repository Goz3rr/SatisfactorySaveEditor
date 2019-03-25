using SatisfactorySaveParser.PropertyTypes.Structs;
using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class StructProperty : SerializedProperty
    {
        public const string TypeName = nameof(StructProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => Data.SerializedLength;

        public string Type { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }
        public byte Unk5 { get; set; }

        public IStructData Data { get; set; }

        public StructProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"struct {Type}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);

            writer.WriteLengthPrefixedString(Type);
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Unk5);
            Data.Serialize(writer);
        }

        private static IStructData ParseStructData(string type, BinaryReader reader)
        {
            switch (type)
            {
                case "LinearColor":
                    return new LinearColor(reader);
                case "Rotator":
                case "Vector":
                    return new Vector(reader);
                case "Box":
                    return new Box(reader);
                case "Quat":
                    return new Quat(reader);
                case "InventoryItem":
                    return new InventoryItem(reader);
                case "RailroadTrackPosition":
                    return new RailroadTrackPosition(reader);
                /*
                case "InventoryStack":
                case "InventoryItem":
                case "PhaseCost":
                case "ItemAmount":
                case "ResearchCost":
                case "CompletedResearch":
                case "ResearchRecipeReward":
                case "ItemFoundData":
                case "RecipeAmountStruct":
                case "MessageData":
                case "SplinePointData":
                    return new DynamicStructData(reader);
                */
                default:
                    return new DynamicStructData(reader);
                    //throw new NotImplementedException($"Can't deserialize struct {type}");
            }
        }

        public static StructProperty[] ParseArray(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            StructProperty[] result = new StructProperty[count];

            var name = reader.ReadLengthPrefixedString();
            var propertyType = reader.ReadLengthPrefixedString();
            var size = reader.ReadInt32();
            var index = reader.ReadInt32();

            var structType = reader.ReadLengthPrefixedString();


            var unk1 = reader.ReadInt32();
            //Trace.Assert(unk1 == 0);

            var unk2 = reader.ReadInt32();
            //Trace.Assert(unk2 == 0);

            var unk3 = reader.ReadInt32();
            //Trace.Assert(unk3 == 0);

            var unk4 = reader.ReadInt32();
            //Trace.Assert(unk4 == 0);

            var unk5 = reader.ReadByte();
            Trace.Assert(unk5 == 0);

            for(var i = 0; i < count; i++)
            {
                result[i] = new StructProperty(name, index)
                {
                    Data = ParseStructData(structType, reader)
                };
            }


            return result;
        }

        public static StructProperty Parse(string propertyName, int index, BinaryReader reader, int size, out int overhead)
        {
            var result = new StructProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.Length + 22;

            result.Unk1 = reader.ReadInt32();
            Trace.Assert(result.Unk1 == 0);

            result.Unk2 = reader.ReadInt32();
            Trace.Assert(result.Unk2 == 0);

            result.Unk3 = reader.ReadInt32();
            Trace.Assert(result.Unk3 == 0);

            result.Unk4 = reader.ReadInt32();
            Trace.Assert(result.Unk4 == 0);

            result.Unk5 = reader.ReadByte();
            Trace.Assert(result.Unk5 == 0);

            var before = reader.BaseStream.Position;
            result.Data = ParseStructData(result.Type, reader);
            var after = reader.BaseStream.Position;

            if (before + size != after)
            {
                throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");
            }

            return result;
        }
    }
}
