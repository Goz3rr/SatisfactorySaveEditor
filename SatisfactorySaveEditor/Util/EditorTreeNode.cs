using SatisfactorySaveParser;
using System.Collections.Generic;
using System.Linq;

namespace SatisfactorySaveEditor.Util
{
    /// <summary>
    ///     Helper class to build editor node tree
    /// </summary>
    public class EditorTreeNode
    {
        /// <summary>
        ///     Name of this specific node in the tree
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     SaveObjects located at this specific node in the tree
        ///     May be empty if not at an end of the tree
        /// </summary>
        public List<SaveObject> Content { get; set; } = new List<SaveObject>();

        /// <summary>
        ///     Child nodes below this node in the tree
        /// </summary>
        public Dictionary<string, EditorTreeNode> Children { get; set; } = new Dictionary<string, EditorTreeNode>();

        public EditorTreeNode(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name} ({Children.Count})";
        }

        public void AddChild(IEnumerable<string> path, SaveObject entry)
        {
            if (path.Count() == 0)
            {
                Content.Add(entry);
                return;
            }

            var first = path.First();
            if (!Children.TryGetValue(first, out EditorTreeNode child))
            {
                child = Children[first] = new EditorTreeNode(first);
            }

            child.AddChild(path.Skip(1), entry);
        }
    }
}
