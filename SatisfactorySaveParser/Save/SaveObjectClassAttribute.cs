using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     The SaveObject attribute is used to register a class for object (de)serializing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SaveObjectClassAttribute : Attribute
    {
        /// <summary>
        ///     The UE4 class this object belongs to
        /// </summary>
        public string Type { get; set; }

        public SaveObjectClassAttribute(string type)
        {
            Type = type;
        }
    }
}
