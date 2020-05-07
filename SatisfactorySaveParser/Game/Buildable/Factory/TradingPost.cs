using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/TradingPost/Build_TradingPost.Build_TradingPost_C")]
    public class TradingPost : FGBuildableFactory
    {
        [SaveProperty("mGenerators")]
        public List<ObjectReference> Generators { get; } = new List<ObjectReference>();

        [SaveProperty("mStorage")]
        public ObjectReference Storage { get; set; }

        [SaveProperty("mMAM")]
        public ObjectReference MAM { get; set; }

        [SaveProperty("mHubTerminal")]
        public ObjectReference HubTerminal { get; set; }

        [SaveProperty("mWorkBench")]
        public ObjectReference WorkBench { get; set; }

        [SaveProperty("mInputInventory")]
        public ObjectReference InputInventory { get; set; }

        [SaveProperty("mStorageInventory")]
        public ObjectReference StorageInventory { get; set; }
    }
}
