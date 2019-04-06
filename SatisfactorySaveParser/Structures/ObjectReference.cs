namespace SatisfactorySaveParser.Structures
{
    public class ObjectReference
    {
        public string Root { get; set; }
        public string Name { get; set; }

        public ObjectReference(string root, string name)
        {
            Root = root;
            Name = name;
        }

        public override string ToString()
        {
            return $"Root: {Root} Name: {Name}";
        }
    }
}
