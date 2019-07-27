using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     The SaveProperty attribute is used to indicate which serialized property should be mapped
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SavePropertyAttribute : Attribute
    {
        /// <summary>
        ///     Serialized name of the property, usually prefixed with m
        /// </summary>
        public string Name { get; set; }

        public SavePropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
