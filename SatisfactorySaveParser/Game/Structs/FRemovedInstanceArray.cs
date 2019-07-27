using System.Collections.Generic;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("RemovedInstanceArray")]
    public class FRemovedInstanceArray : GameStruct
    {
        public override string StructName => "RemovedInstanceArray";

        [StructProperty("Items")]
        public List<FRemovedInstance> Items { get; } = new List<FRemovedInstance>();
    }
}
