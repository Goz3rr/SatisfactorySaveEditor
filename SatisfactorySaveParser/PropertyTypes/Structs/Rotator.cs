using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Rotator : Vector
    {
        public new string Type => "Rotator";

        public Rotator(BinaryReader reader) : base(reader)
        {
        }
    }
}
