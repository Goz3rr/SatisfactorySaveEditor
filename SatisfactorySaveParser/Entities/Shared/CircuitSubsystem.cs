using System.IO;

namespace SatisfactorySaveParser.Entities.Shared
{
    [SaveEntity("/Game/FactoryGame/-Shared/Blueprint/BP_CircuitSubsystem.BP_CircuitSubsystem_C")]
    public class CircuitSubsystem : SaveEntity
    {
        public int DataInt6 { get; set; }

        public override void ParseData(uint length, BinaryReader reader)
        {
            base.ParseData(length, reader);

            DataInt6 = reader.ReadInt32();
        }
    }
}
