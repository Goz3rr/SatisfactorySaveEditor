using System.IO;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/C4Dispenser/BP_DestructibleLargeRock.BP_DestructibleLargeRock_C")]
    public class DestructibleLargeRock : SaveActor
    {
        public int UnknownInt { get; set; }

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            UnknownInt = reader.ReadInt32();
        }

        public override void SerializeNativeData(BinaryWriter writer)
        {
            writer.Write(UnknownInt);
        }
    }
}
