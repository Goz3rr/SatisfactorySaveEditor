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

        private static readonly string AltTextName = "mCompassText";
        private static readonly string AltTextValue = "Beacon";
        private static readonly int AltTextUnknown = 8;
        private static readonly string AltTextUnknown8 = "B8D68B1B46E1D68DCB0BE39EC800B74E";
        private static readonly byte[] AltTextBytes = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x6D, 0x43, 0x6F, 0x6D, 0x70, 0x61, 0x73, 0x73, 0x54, 0x65, 0x78, 0x74, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x39, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x00, 0x00, 0x42, 0x38, 0x44, 0x36, 0x38, 0x42, 0x31, 0x42, 0x34, 0x36, 0x45, 0x31, 0x44, 0x36, 0x38, 0x44, 0x43, 0x42, 0x30, 0x42, 0x45, 0x33, 0x39, 0x45, 0x43, 0x38, 0x30, 0x30, 0x42, 0x37, 0x34, 0x45, 0x00, 0x07, 0x00, 0x00, 0x00, 0x42, 0x65, 0x61, 0x63, 0x6F, 0x6E, 0x00 };


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

        [TestMethod]
        public void TextPropertyAltRead()
        {
            using (var stream = new MemoryStream(AltTextBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader) as TextProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(AltTextName, prop.PropertyName);
                Assert.AreEqual(TextProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(AltTextValue, prop.Value);
                Assert.AreEqual(AltTextUnknown, prop.Unknown4);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void TextPropertyAltWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new TextProperty(AltTextName)
                {
                    Unknown8 = AltTextUnknown8,
                    Value = AltTextValue,
                    Unknown4 = AltTextUnknown
                };

                prop.Serialize(writer);

                Assert.AreEqual(57, prop.SerializedLength);
                CollectionAssert.AreEqual(AltTextBytes, stream.ToArray());
            }
        }
    }
}
