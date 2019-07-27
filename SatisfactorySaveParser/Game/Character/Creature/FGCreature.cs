using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Character.Creature
{
    public abstract class FGCreature : FGCharacterBase
    {
        [SaveProperty("mSpline")]
        public ObjectReference Spline { get; set; }

        [SaveProperty("mIsPersistent")]
        public bool IsPersistent { get; set; }
    }
}
