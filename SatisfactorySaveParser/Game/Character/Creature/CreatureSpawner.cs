using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Character.Creature
{
    [SaveObjectClass("/Game/FactoryGame/Character/Creature/BP_CreatureSpawner.BP_CreatureSpawner_C")]
    public class CreatureSpawner : SaveActor
    {
        [SaveProperty("mIsActive")]
        public bool IsActive { get; set; }

        [SaveProperty("mSpawnData")]
        public List<FSpawnData> SpawnData { get; } = new List<FSpawnData>();

        [SaveProperty("mRandomSeed")]
        public int RandomSeed { get; set; }
    }
}
