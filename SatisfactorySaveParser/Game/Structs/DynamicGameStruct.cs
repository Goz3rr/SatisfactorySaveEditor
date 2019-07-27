namespace SatisfactorySaveParser.Game.Structs
{
    public class DynamicGameStruct : GameStruct
    {
        private readonly string structName;
        public override string StructName => structName;

        public DynamicGameStruct(string type)
        {
            structName = type;
        }
    }
}
