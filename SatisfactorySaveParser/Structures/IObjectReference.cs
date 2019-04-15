namespace SatisfactorySaveParser.Structures
{
    public interface IObjectReference
    {
        string LevelName { get; set; }
        string PathName { get; set; }

        SaveObject ReferencedObject { get; set; }
    }
}
