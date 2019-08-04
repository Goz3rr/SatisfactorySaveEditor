using System;
using System.IO;
using SatisfactorySaveParser.Game.Structs;

namespace SatisfactorySaveParser.Save.Properties
{
    public class StructProperty : SerializedProperty
    {
        public const string TypeName = nameof(StructProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(object);
        public override object BackingObject => null;

        public override int SerializedLength => 0;

        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }
        public byte Unk5 { get; set; }

        public GameStruct Data { get; set; }

        public StructProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Struct {PropertyName}";
        }

        public static StructProperty Deserialize(BinaryReader reader, string propertyName, int size, int index, out int overhead)
        {
            var result = new StructProperty(propertyName, index);
            var structType = reader.ReadLengthPrefixedString();
            overhead = structType.Length + 22;

            result.Unk1 = reader.ReadInt32();
            result.Unk2 = reader.ReadInt32();
            result.Unk3 = reader.ReadInt32();
            result.Unk4 = reader.ReadInt32();
            result.Unk5 = reader.ReadByte();

            var before = reader.BaseStream.Position;

            var structObj = GameStructFactory.CreateFromType(structType);
            structObj.Deserialize(reader);
            result.Data = structObj;

            var after = reader.BaseStream.Position;

            if (before + size != after)
                throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
