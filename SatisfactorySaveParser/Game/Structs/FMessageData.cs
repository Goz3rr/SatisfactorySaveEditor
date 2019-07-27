using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("MessageData")]
    public class FMessageData : GameStruct
    {
        public override string StructName => "MessageData";

        [StructProperty("WasRead")]
        public bool WasRead { get; set; }

        [StructProperty("MessageClass")]
        public ObjectReference MessageClass { get; set; }
    }
}
