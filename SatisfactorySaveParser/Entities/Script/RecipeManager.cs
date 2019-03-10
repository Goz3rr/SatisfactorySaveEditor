using System;

namespace SatisfactorySaveParser.Entities.Script
{
    [SaveEntity("/Script/FactoryGame.FGRecipeManager")]
    public class RecipeManager : SaveEntity
    {
        public override void ParseData(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
