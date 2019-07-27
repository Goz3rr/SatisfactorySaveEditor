using System;

namespace SatisfactorySaveParser.Game.Structs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GameStructAttribute : Attribute
    {
        public string StructName { get; set; }

        public GameStructAttribute(string structName)
        {
            StructName = structName;
        }
    }
}
