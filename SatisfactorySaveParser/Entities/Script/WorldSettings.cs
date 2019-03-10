using System;

namespace SatisfactorySaveParser.Entities.Script
{
    [SaveEntity("/Script/FactoryGame.FGWorldSettings")]
    public class WorldSettings : SaveEntity
    {
        public override void ParseData(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
