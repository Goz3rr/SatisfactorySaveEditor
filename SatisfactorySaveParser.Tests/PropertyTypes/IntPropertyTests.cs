using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class IntPropertyTests
    {
        private const string IntName = "mNumberOfPassedDays";
        private const int IntValue = 0;
        private static readonly byte[] IntBytes = new byte[] { 0x14, 0x00, 0x00, 0x00, 0x6D, 0x4E, 0x75, 0x6D, 0x62, 0x65, 0x72, 0x4F, 0x66, 0x50, 0x61, 0x73, 0x73, 0x65, 0x64, 0x44, 0x61, 0x79, 0x73, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x49, 0x6E, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        [TestMethod]
        public void IntPropertyRead()
        {
            using var stream = new MemoryStream(IntBytes);
            using var reader = new BinaryReader(stream);

            var prop = SatisfactorySaveSerializer.DeserializeProperty(reader) as IntProperty;

            Assert.AreNotEqual(null, prop);

            Assert.AreEqual(IntName, prop.PropertyName);
            Assert.AreEqual(IntProperty.TypeName, prop.PropertyType);

            Assert.AreEqual(0, prop.Index);

            Assert.AreEqual(IntValue, prop.Value);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void IntPropertyWrite()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var prop = new IntProperty(IntName)
            {
                Value = IntValue
            };

            SatisfactorySaveSerializer.SerializeProperty(prop, writer);

            Assert.AreEqual(4, prop.SerializedLength);
            CollectionAssert.AreEqual(IntBytes, stream.ToArray());
        }
    }
}
