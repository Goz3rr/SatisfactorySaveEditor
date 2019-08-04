using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public class FConveyorBeltItem
    {
        // FInventoryItem
        public string ItemClass { get; set; }

        public ObjectReference ItemState { get; set; }

        public float Offset { get; set; }

        public FConveyorBeltItem(string item, ObjectReference state, float offset)
        {
            ItemClass = item;
            ItemState = state;
            Offset = offset;
        }

    }
}
