using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     The SaveObject attribute is used to register a class for object (de)serializing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SaveObjectClassAttribute : Attribute
    {
        public string Type { get; set; }

        public SaveObjectClassAttribute(string type)
        {
            Type = type;
        }
    }
}
