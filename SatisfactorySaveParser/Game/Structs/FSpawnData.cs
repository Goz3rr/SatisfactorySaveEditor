using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("SpawnData")]
    public class FSpawnData : GameStruct
    {
        public override string StructName => "SpawnData";

        [StructProperty("SpawnLocation")]
        public FVector SpawnLocation { get; set; }

        [StructProperty("Creature")]
        public ObjectReference Creature { get; set; }

        [StructProperty("WasKilled")]
        public bool WasKilled { get; set; }

        [StructProperty("KilledOnDayNr")]
        public int KilledOnDayNr { get; set; }

        [StructProperty("CreatureClassOverride")]
        public ObjectReference CreatureClassOverride { get; set; }

        [StructProperty("SpawnWeight")]
        public float SpawnWeight { get; set; }
    }
}
