using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGRecipeShortcut")]
    public class FGRecipeShortcut : SaveComponent
    {
        [SaveProperty("mRecipeToActivate")]
        public ObjectReference RecipeToActivate { get; set; }
    }
}
