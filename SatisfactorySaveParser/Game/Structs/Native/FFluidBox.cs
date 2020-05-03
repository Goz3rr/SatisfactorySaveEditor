using System.IO;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("FluidBox")]
    public class FFluidBox : GameStruct
    {
        public override string StructName => "FluidBox";
        public override int SerializedLength => 4;

        public float Unknown { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Unknown = reader.ReadSingle();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Unknown);
        }
    }
}
