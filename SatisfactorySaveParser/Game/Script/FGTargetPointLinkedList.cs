using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGTargetPointLinkedList")]
    public class FGTargetPointLinkedList : SaveComponent
    {
        [SaveProperty("mFirst")]
        public ObjectReference First { get; set; }

        [SaveProperty("mLast")]
        public ObjectReference Last { get; set; }

        [SaveProperty("mCurrentTarget")]
        public ObjectReference CurrentTarget { get; set; }
    }
}
