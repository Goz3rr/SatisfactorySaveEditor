using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Character.Creature
{
    [SaveObjectClass("/Game/FactoryGame/Character/Creature/BP_CreatureSpawner.BP_CreatureSpawner_C")]
    public class CreatureSpawner : SaveActor
    {
        /// <summary>
        ///     Indicates if we have spawned our enemies
        /// </summary>
        [SaveProperty("mIsActive")]
        public bool IsActive { get; set; }

        /// <summary>
        ///     Structure for keeping all data saved about enemies spawned
        /// </summary>
        [SaveProperty("mSpawnData")]
        public List<FSpawnData> SpawnData { get; } = new List<FSpawnData>();

        /// <summary>
        ///     cached value to see if spawner is near a base 
        /// </summary>
        [SaveProperty("mCachedIsNearBase")]
        public bool CachedIsNearBase { get; set; }

        /// <summary>
        ///     Indicates that this spawner has been deactivated and want to destroy its creatures
        /// </summary>
        [SaveProperty("mIsPendingDestroy")]
        public bool IsPendingDestroy { get; set; }

        [SaveProperty("mRandomSeed")]
        public int RandomSeed { get; set; }
    }
}
