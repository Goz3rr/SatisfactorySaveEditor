using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatisfactorySaveParser.PropertyTypes;
using System.IO;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class TextPropertyTests
    {
        private static readonly string TextName = "mCompassText";
        private static readonly string TextValue = "Oil";
        private static readonly int TextUnknown = 2;
        private static readonly byte[] TextBytes = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x6D, 0x43, 0x6F, 0x6D, 0x70, 0x61, 0x73, 0x73, 0x54, 0x65, 0x78, 0x74, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x15, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x4F, 0x69, 0x6C, 0x00 };

        [TestMethod]
        public void TextPropertyRead()
        {
            using (var stream = new MemoryStream(TextBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader) as TextProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(TextName, prop.PropertyName);
                Assert.AreEqual(TextProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(TextValue, prop.Value);
                Assert.AreEqual(TextUnknown, prop.Unknown4);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void TextPropertyWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new TextProperty(TextName)
                {
                    Value = TextValue,
                    Unknown4 = TextUnknown
                };

                prop.Serialize(writer);

                Assert.AreEqual(21, prop.SerializedLength);
                CollectionAssert.AreEqual(TextBytes, stream.ToArray());
            }
        }
    }
}
