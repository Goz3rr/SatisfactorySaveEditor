using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Character
{
    public abstract class FGCharacterBase : SaveActor
    {
        [SaveProperty("mHealthComponent")]
        public ObjectReference HealthComponent { get; set; }
    }
}
