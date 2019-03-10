using System;

namespace SatisfactorySaveParser
{
    public class SaveEntityAttribute : Attribute
    {
        public string Name { get; set; }

        public SaveEntityAttribute(string name)
        {
            Name = name;
        }
    }
}
