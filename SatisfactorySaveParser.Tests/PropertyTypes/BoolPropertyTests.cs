using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class BoolPropertyTests
    {
        private const string BoolTrueName = "mHasCompletedIntroTutorial";
        private const bool BoolTrueValue = true;
        private static readonly byte[] BoolTrueBytes = new byte[] { 0x1B, 0x00, 0x00, 0x00, 0x6D, 0x48, 0x61, 0x73, 0x43, 0x6F, 0x6D, 0x70, 0x6C, 0x65, 0x74, 0x65, 0x64, 0x49, 0x6E, 0x74, 0x72, 0x6F, 0x54, 0x75, 0x74, 0x6F, 0x72, 0x69, 0x61, 0x6C, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x42, 0x6F, 0x6F, 0x6C, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };

        [TestMethod]
        public void BoolPropertyRead()
        {
            using (var stream = new MemoryStream(BoolTrueBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SatisfactorySaveSerializer.DeserializeProperty(reader) as BoolProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(BoolTrueName, prop.PropertyName);
                Assert.AreEqual(BoolProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(BoolTrueValue, prop.Value);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void BoolPropertyWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new BoolProperty(BoolTrueName)
                {
                    Value = BoolTrueValue
                };

                SatisfactorySaveSerializer.SerializeProperty(prop, writer);

                Assert.AreEqual(0, prop.SerializedLength);
                CollectionAssert.AreEqual(BoolTrueBytes, stream.ToArray());
            }
        }
    }
}
