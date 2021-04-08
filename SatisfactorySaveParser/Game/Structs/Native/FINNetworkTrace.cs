using System;
using System.IO;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("FINNetworkTrace")]
    public class FINNetworkTrace : GameStruct
    {
        public override string StructName => "FINNetworkTrace";
        public override int SerializedLength => throw new NotImplementedException();

        public bool IsValid { get; set; }
        public ObjectReference Reference { get; set; }
        public bool HasPrev { get; set; }
        public FINNetworkTrace Prev { get; set; }
        public bool HasStep { get; set; }
        public string Step { get; set; }

        public override void Deserialize(BinaryReader reader, int buildVersion)
        {
            IsValid = reader.ReadBooleanFromInt32();

            if (IsValid)
            {
                Reference = reader.ReadObjectReference();

                HasPrev = reader.ReadBooleanFromInt32();
                if (HasPrev)
                {
                    Prev = (FINNetworkTrace)GameStructFactory.CreateFromType(StructName);
                    Prev.Deserialize(reader, buildVersion);
                }

                HasStep = reader.ReadBooleanFromInt32();
                if (HasStep)
                    Step = reader.ReadLengthPrefixedString();
            }
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteBoolAsInt32(IsValid);

            if (IsValid)
            {
                writer.Write(Reference);

                writer.WriteBoolAsInt32(HasPrev);
                if (HasPrev)
                    Prev.Serialize(writer);

                writer.WriteBoolAsInt32(HasStep);
                if (HasStep)
                    writer.WriteLengthPrefixedString(Step);
            }
        }
    }
}
