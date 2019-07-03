namespace SatisfactorySaveParser.Structures
{
    public class Vector4
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public override string ToString()
        {
            return $"X: {X} Y: {Y} Z: {Z} W: {W}";
        }
    }
}
