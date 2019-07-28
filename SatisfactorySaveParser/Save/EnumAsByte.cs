using System;

namespace SatisfactorySaveParser.Save
{
    public class EnumAsByte<T> where T : Enum
    {
        public T Value { get; set; }

        public EnumAsByte(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"EnumAsByte {Value.ToString()}";
        }
    }
}
