using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class Int64PropertyTests
    {
        private const string Int64Name = "mTotalResourceSinkPoints";
        private const long Int64Value = 808375760;
        private static readonly byte[] Int64Bytes = new byte[] { 0x19, 0x00, 0x00, 0x00, 0x6D, 0x54, 0x6F, 0x74, 0x61, 0x6C, 0x52, 0x65, 0x73, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x53, 0x69, 0x6E, 0x6B, 0x50, 0x6F, 0x69, 0x6E, 0x74, 0x73, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x49, 0x6E, 0x74, 0x36, 0x34, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xD0, 0xD5, 0x2E, 0x30, 0x00, 0x00, 0x00, 0x00 };

        [TestMethod]
        public void Int64PropertyRead()
        {
            using var stream = new MemoryStream(Int64Bytes);
            using var reader = new BinaryReader(stream);

            var prop = SatisfactorySaveSerializer.DeserializeProperty(reader) as Int64Property;

            Assert.AreNotEqual(null, prop);

            Assert.AreEqual(Int64Name, prop.PropertyName);
            Assert.AreEqual(Int64Property.TypeName, prop.PropertyType);

            Assert.AreEqual(0, prop.Index);

            Assert.AreEqual(Int64Value, prop.Value);

            Assert.AreEqual(stream.Length, stream.Position);
        }

        [TestMethod]
        public void Int64PropertyWrite()
        {
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);

            var prop = new Int64Property(Int64Name)
            {
                Value = Int64Value
            };

            SatisfactorySaveSerializer.SerializeProperty(prop, writer);

            Assert.AreEqual(8, prop.SerializedLength);
            CollectionAssert.AreEqual(Int64Bytes, stream.ToArray());
        }
    }
}
