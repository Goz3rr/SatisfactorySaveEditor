using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class StrPropertyTests
    {
        private const int BuildVersion = 139586;

        private const string StrName = "mSaveSessionName";
        private const string StrValue = "space war";
        private static readonly byte[] StrBytes = new byte[] { 0x11, 0x00, 0x00, 0x00, 0x6D, 0x53, 0x61, 0x76, 0x65, 0x53, 0x65, 0x73, 0x73, 0x69, 0x6F, 0x6E, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x53, 0x74, 0x72, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x73, 0x70, 0x61, 0x63, 0x65, 0x20, 0x77, 0x61, 0x72, 0x00 };

        [TestMethod]
        public void StrPropertyRead()
        {
            using var stream = new MemoryStream(StrBytes);
            using var reader = new BinaryReader(stream);

            var prop = SatisfactorySaveSerializer.DeserializeProperty(reader, BuildVersion) as StrProperty;

            Assert.AreNotEqual(null, prop);

            Assert.AreEqual(StrName, prop.PropertyName);
            Assert.AreEqual(StrProperty.TypeName, prop.PropertyType);

            Assert.AreEqual(0, prop.Index);

            Assert.AreEqual(StrValue, prop.Value);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void StrPropertyWrite()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var prop = new StrProperty(StrName)
            {
                Value = StrValue
            };

            SatisfactorySaveSerializer.SerializeProperty(prop, writer);

            Assert.AreEqual(14, prop.SerializedLength);
            CollectionAssert.AreEqual(StrBytes, stream.ToArray());
        }
    }
}
