using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class EnumPropertyTests
    {
        private const string EnumName = "mPendingTutorial";
        private const string EnumType = "EIntroTutorialSteps";
        private const string EnumValue = "EIntroTutorialSteps::ITS_DONE";
        private static readonly byte[] EnumBytes = new byte[] { 0x11, 0x00, 0x00, 0x00, 0x6D, 0x50, 0x65, 0x6E, 0x64, 0x69, 0x6E, 0x67, 0x54, 0x75, 0x74, 0x6F, 0x72, 0x69, 0x61, 0x6C, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x45, 0x6E, 0x75, 0x6D, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x22, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x45, 0x49, 0x6E, 0x74, 0x72, 0x6F, 0x54, 0x75, 0x74, 0x6F, 0x72, 0x69, 0x61, 0x6C, 0x53, 0x74, 0x65, 0x70, 0x73, 0x00, 0x00, 0x1E, 0x00, 0x00, 0x00, 0x45, 0x49, 0x6E, 0x74, 0x72, 0x6F, 0x54, 0x75, 0x74, 0x6F, 0x72, 0x69, 0x61, 0x6C, 0x53, 0x74, 0x65, 0x70, 0x73, 0x3A, 0x3A, 0x49, 0x54, 0x53, 0x5F, 0x44, 0x4F, 0x4E, 0x45, 0x00 };

        [TestMethod]
        public void EnumPropertyRead()
        {
            using (var stream = new MemoryStream(EnumBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SatisfactorySaveSerializer.DeserializeProperty(reader) as EnumProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(EnumName, prop.PropertyName);
                Assert.AreEqual(EnumProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(EnumType, prop.Type);
                Assert.AreEqual(EnumValue, prop.Value);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void EnumPropertyWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new EnumProperty(EnumName)
                {
                    Type = EnumType,
                    Value = EnumValue
                };

                SatisfactorySaveSerializer.SerializeProperty(prop, writer);

                Assert.AreEqual(34, prop.SerializedLength);
                CollectionAssert.AreEqual(EnumBytes, stream.ToArray());
            }
        }
    }
}
