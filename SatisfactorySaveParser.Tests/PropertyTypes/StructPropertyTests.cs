using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class StructPropertyTests
    {
        private const string StructTypedName = "mPrimaryColor";
        private const string StructTypedType = "LinearColor";
        private const float StructTypedDataR = 1f;
        private const float StructTypedDataG = 0.440705001354218f;
        private const float StructTypedDataB = 0.156314000487328f;
        private const float StructTypedDataA = 1f;
        private static readonly byte[] StructTypedBytes = new byte[] { 0x0E, 0x00, 0x00, 0x00, 0x6D, 0x50, 0x72, 0x69, 0x6D, 0x61, 0x72, 0x79, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x53, 0x74, 0x72, 0x75, 0x63, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x4C, 0x69, 0x6E, 0x65, 0x61, 0x72, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x16, 0xA4, 0xE1, 0x3E, 0xC7, 0x10, 0x20, 0x3E, 0x00, 0x00, 0x80, 0x3F };

        [TestMethod]
        public void StructPropertyTypedRead()
        {
            using (var stream = new MemoryStream(StructTypedBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SatisfactorySaveSerializer.DeserializeProperty(reader) as StructProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(StructTypedName, prop.PropertyName);
                Assert.AreEqual(StructProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                // todo

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void StructPropertyTypedWrite()
        {
            Assert.Fail();
        }
    }
}
