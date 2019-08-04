using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("SchematicCost")]
    public class FSchematicCost : GameStruct
    {
        public override string StructName => "SchematicCost";

        [StructProperty("Schematic")]
        public ObjectReference Schematic { get; set; }

        [StructProperty("ItemCost")]
        public List<FItemAmount> ItemCost { get; } = new List<FItemAmount>();
    }
}
