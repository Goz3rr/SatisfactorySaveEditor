using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveEditor.Message
{
    public class CoordinateAddedMessage
    {
        public Vector3 Point { get; }

        public CoordinateAddedMessage(Vector3 point)
        {
            Point = point;
        }
    }
}
