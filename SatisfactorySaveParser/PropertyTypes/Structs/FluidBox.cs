using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class FluidBox : IStructData
    {
        public float Unknown { get; set; }

        public int SerializedLength => 25;
        public string Type => "FluidBox";

        public FluidBox(BinaryReader reader)
        {
            Unknown = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Unknown);
        }
    }
}
