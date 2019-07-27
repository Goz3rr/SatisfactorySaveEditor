using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    //[GameStruct("")]
    public class FBuilding : GameStruct
    {
        public override string StructName => "";

        [StructProperty("Buildables")]
        public List<ObjectReference> Buildables { get; } = new List<ObjectReference>();
    }
}
