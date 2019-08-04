using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    // TODO
    public class FBuilding : GameStruct
    {
        public override string StructName => throw new System.NotImplementedException();

        [StructProperty("Buildables")]
        public List<ObjectReference> Buildables { get; } = new List<ObjectReference>();
    }
}
