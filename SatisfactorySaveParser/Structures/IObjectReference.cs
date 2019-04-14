namespace SatisfactorySaveParser.Structures
{
    public interface IObjectReference
    {
        string Root { get; set; }
        string Name { get; set; }

        SaveObject ReferencedObject { get; set; }
    }
}
