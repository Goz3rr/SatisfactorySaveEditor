using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class NamePropertyTests
    {
        private const string NameName = "mStartingPointTagName";
        private const string NameValue = "Grass Fields";
        private static readonly byte[] NameBytes = new byte[] { 0x16, 0x00, 0x00, 0x00, 0x6D, 0x53, 0x74, 0x61, 0x72, 0x74, 0x69, 0x6E, 0x67, 0x50, 0x6F, 0x69, 0x6E, 0x74, 0x54, 0x61, 0x67, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x4E, 0x61, 0x6D, 0x65, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x47, 0x72, 0x61, 0x73, 0x73, 0x20, 0x46, 0x69, 0x65, 0x6C, 0x64, 0x73, 0x00 };

        [TestMethod]
        public void NamePropertyRead()
        {
            using (var stream = new MemoryStream(NameBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SatisfactorySaveSerializer.DeserializeProperty(reader) as NameProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(NameName, prop.PropertyName);
                Assert.AreEqual(NameProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(NameValue, prop.Value);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void NamePropertyWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new NameProperty(NameName)
                {
                    Value = NameValue
                };

                SatisfactorySaveSerializer.SerializeProperty(prop, writer);

                Assert.AreEqual(17, prop.SerializedLength);
                CollectionAssert.AreEqual(NameBytes, stream.ToArray());
            }
        }
    }
}
