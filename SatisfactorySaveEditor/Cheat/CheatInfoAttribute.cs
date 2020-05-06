using System;

namespace SatisfactorySaveEditor.Cheat
{
    /// <summary>
    /// Defines user-presentable information about a cheat
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CheatInfoAttribute : Attribute
    {
        /// <summary>
        /// Human readable name of this cheat
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Short description of what this cheat does
        /// <para>This will be displayed in a tooltip when hovering over the cheat button</para>
        /// </summary>
        public string Description { get; }

        /// <summary></summary>
        /// <param name="name">Human readable name of this cheat</param>
        /// <param name="description">Short description of what this cheat does</param>
        public CheatInfoAttribute(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
