using System.Collections.Generic;
using System.Linq;

namespace SatisfactorySaveParser
{
    public class SaveTreeNode
    {
        public string Name { get; set; }

        public List<SaveObject> Content { get; set; } = new List<SaveObject>();

        public Dictionary<string, SaveTreeNode> Children { get; set; } = new Dictionary<string, SaveTreeNode>();

        public SaveTreeNode(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} ({Children.Count})";
        }

        public void AddChild(IEnumerable<string> path, SaveObject entry)
        {
            if(path.Count() == 0)
            {
                Content.Add(entry);
                return;
            }

            var first = path.First();
            if(!Children.TryGetValue(first, out SaveTreeNode child))
            {
                child = Children[first] = new SaveTreeNode(first);
            }

            child.AddChild(path.Skip(1), entry);
        }
    }
}
