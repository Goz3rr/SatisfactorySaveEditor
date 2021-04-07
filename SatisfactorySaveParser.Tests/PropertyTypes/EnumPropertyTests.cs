using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatisfactorySaveParser.PropertyTypes;
using System.IO;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class EnumPropertyTests
    {
        private const int BuildVersion = 139586;

        private static readonly string EnumName = "mPendingTutorial";
        private static readonly string EnumType = "EIntroTutorialSteps";
        private static readonly string EnumValue = "EIntroTutorialSteps::ITS_DONE";
        private static readonly byte[] EnumBytes = new byte[] { 0x11, 0x00, 0x00, 0x00, 0x6D, 0x50, 0x65, 0x6E, 0x64, 0x69, 0x6E, 0x67, 0x54, 0x75, 0x74, 0x6F, 0x72, 0x69, 0x61, 0x6C, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x45, 0x6E, 0x75, 0x6D, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x22, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x45, 0x49, 0x6E, 0x74, 0x72, 0x6F, 0x54, 0x75, 0x74, 0x6F, 0x72, 0x69, 0x61, 0x6C, 0x53, 0x74, 0x65, 0x70, 0x73, 0x00, 0x00, 0x1E, 0x00, 0x00, 0x00, 0x45, 0x49, 0x6E, 0x74, 0x72, 0x6F, 0x54, 0x75, 0x74, 0x6F, 0x72, 0x69, 0x61, 0x6C, 0x53, 0x74, 0x65, 0x70, 0x73, 0x3A, 0x3A, 0x49, 0x54, 0x53, 0x5F, 0x44, 0x4F, 0x4E, 0x45, 0x00 };

        [TestMethod]
        public void EnumPropertyRead()
        {
            using (var stream = new MemoryStream(EnumBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader, BuildVersion) as EnumProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(EnumName, prop.PropertyName);
                Assert.AreEqual(EnumProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(EnumType, prop.Type);
                Assert.AreEqual(EnumValue, prop.Name);

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
                    Name = EnumValue
                };

                prop.Serialize(writer, BuildVersion);

                Assert.AreEqual(34, prop.SerializedLength);
                CollectionAssert.AreEqual(EnumBytes, stream.ToArray());
            }
        }
    }
}
