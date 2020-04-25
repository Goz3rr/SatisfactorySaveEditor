using System;
using System.IO;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("Guid")]
    public class FGuid : GameStruct
    {
        public override string StructName => "Guid";
        public override int SerializedLength => 16;

        public Guid Data { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Data = new Guid(reader.ReadBytes(16));
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Data.ToByteArray());
        }
    }
}
