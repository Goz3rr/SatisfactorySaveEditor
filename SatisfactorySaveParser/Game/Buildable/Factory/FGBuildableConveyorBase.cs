using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableConveyorBase : FGBuildable
    {
        public List<FConveyorBeltItem> Items { get; } = new List<FConveyorBeltItem>();

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            var itemCount = reader.ReadInt32();
            for (var i = 0; i < itemCount; i++)
            {
                reader.AssertNullInt32(); // probably a string
                Items.Add(new FConveyorBeltItem(item: reader.ReadLengthPrefixedString(), state: reader.ReadObjectReference(), offset: reader.ReadSingle()));
            }
        }
    }
}
