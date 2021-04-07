using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatisfactorySaveParser.PropertyTypes;
using System.IO;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class ObjectPropertyTests
    {
        private const int BuildVersion = 139586;

        private static readonly string ObjectName = "mOwnedPawn";
        private static readonly string ObjectStr1 = "Persistent_Level";
        private static readonly string ObjectStr2 = "Persistent_Level:PersistentLevel.Char_Player_C_0";
        private static readonly byte[] ObjectBytes = new byte[] { 0x0B, 0x00, 0x00, 0x00, 0x6D, 0x4F, 0x77, 0x6E, 0x65, 0x64, 0x50, 0x61, 0x77, 0x6E, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x4A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x00, 0x31, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x3A, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x2E, 0x43, 0x68, 0x61, 0x72, 0x5F, 0x50, 0x6C, 0x61, 0x79, 0x65, 0x72, 0x5F, 0x43, 0x5F, 0x30, 0x00 };

        [TestMethod]
        public void ObjectPropertyRead()
        {
            using (var stream = new MemoryStream(ObjectBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader, BuildVersion) as ObjectProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(ObjectName, prop.PropertyName);
                Assert.AreEqual(ObjectProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(ObjectStr1, prop.LevelName);
                Assert.AreEqual(ObjectStr2, prop.PathName);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void ObjectPropertyWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new ObjectProperty(ObjectName)
                {
                    LevelName = ObjectStr1,
                    PathName = ObjectStr2
                };

                prop.Serialize(writer, BuildVersion);

                Assert.AreEqual(74, prop.SerializedLength);
                CollectionAssert.AreEqual(ObjectBytes, stream.ToArray());
            }
        }
    }
}
