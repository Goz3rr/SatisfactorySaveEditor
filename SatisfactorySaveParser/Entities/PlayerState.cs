using System;

namespace SatisfactorySaveParser.Entities
{
    [SaveEntity("/Game/FactoryGame/Character/Player/BP_PlayerState.BP_PlayerState_C")]
    public class PlayerState : SaveEntity
    {
        public override void ParseData(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
