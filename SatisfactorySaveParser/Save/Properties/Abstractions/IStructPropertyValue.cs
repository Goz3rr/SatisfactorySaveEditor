using SatisfactorySaveParser.Game.Structs;

namespace SatisfactorySaveParser.Save.Properties.Abstractions
{
    public interface IStructPropertyValue
    {
        GameStruct Data { get; set; }
    }
}
