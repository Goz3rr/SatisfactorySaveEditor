using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    public class MappingTestActor : SaveActor
    {
        public const string TestFloatName = "mTestFloat";
        public const int TestFloatIndex = 0;
        public const float TestFloatValue = 123.45f;

        [SaveProperty(TestFloatName)]
        public float TestFloat { get; set; } = TestFloatValue;
    }
}
