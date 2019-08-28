namespace SatisfactorySaveEditor.Model
{
    public class ResourceType
    {
        public string ItemPath { get; }
        public string Name { get; }
        public int Quantity { get; set; }

        public string Image => $"pack://application:,,,/Icon/{Name.Replace(' ', '_')}.png";

        public ResourceType(string itemPath, string name, int qty)
        {
            ItemPath = itemPath;
            Name = name;
            Quantity = qty;
        }
    }
}
