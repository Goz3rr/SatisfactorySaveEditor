using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGRecipeManager")]
    public class FGRecipeManager : SaveActor
    {
        [SaveProperty("mAvailableRecipes")]
        public List<ObjectReference> AvailableRecipes { get; } = new List<ObjectReference>();
    }
}
