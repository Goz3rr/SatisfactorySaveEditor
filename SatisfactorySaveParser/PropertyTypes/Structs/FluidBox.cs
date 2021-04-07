using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class FluidBox : IStructData
    {
        public float Unknown { get; set; }

        public int SerializedLength => 4;
        public string Type => "FluidBox";

        public FluidBox(BinaryReader reader)
        {
            Unknown = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Unknown);
        }
    }
}
