using System;
using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class FINNetworkTrace : IStructData
    {
        public ObjectReference Reference { get; set; }
        public bool HasPrev { get; set; }
        public FINNetworkTrace Prev { get; set; }
        public bool HasStep { get; set; }
        public string Step { get; set; }

        public int SerializedLength => 0;
        public string Type => "FINNetworkTrace";

        public FINNetworkTrace(BinaryReader reader)
        {
            Reference = new ObjectReference(reader);

            HasPrev = reader.ReadInt32() != 0;
            if (HasPrev)
                Prev = new FINNetworkTrace(reader);

            HasStep = reader.ReadInt32() != 0;
            if (HasStep)
                Step = reader.ReadLengthPrefixedString();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.WriteLengthPrefixedString(Reference.LevelName);
            writer.WriteLengthPrefixedString(Reference.PathName);

            writer.Write(HasPrev ? 1 : 0);
            if (HasPrev)
                Prev.Serialize(writer, buildVersion);

            writer.Write(HasStep ? 1 : 0);
            if (HasStep)
                writer.WriteLengthPrefixedString(Step);
        }
    }
}
