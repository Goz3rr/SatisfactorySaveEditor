using System.IO;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableWire : FGBuildable
    {
        public ObjectReference NextConnection { get; set; }
        public ObjectReference PreviousConnection { get; set; }

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            NextConnection = reader.ReadObjectReference();
            PreviousConnection = reader.ReadObjectReference();
        }
    }
}
