using SatisfactorySaveParser.Structures;
using System;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Box : IStructData
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }

        public byte UnknownByte { get; set; }

        public int SerializedLength => 25;
        public string Type => "Box";

        public Box(BinaryReader reader)
        {
            Min = reader.ReadVector3();
            Max = reader.ReadVector3();

            UnknownByte = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Min);
            writer.Write(Max);
            writer.Write(UnknownByte);
        }
    }
}
