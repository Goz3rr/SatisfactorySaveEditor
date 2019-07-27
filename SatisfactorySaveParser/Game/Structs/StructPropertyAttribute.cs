using System;

namespace SatisfactorySaveParser.Game.Structs
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StructPropertyAttribute : Attribute
    {
        /// <summary>
        ///     Serialized name of the property
        /// </summary>
        public string Name { get; set; }

        public StructPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
