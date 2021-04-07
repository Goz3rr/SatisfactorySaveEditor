using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;
using System.IO;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class StructPropertyTests
    {
        private const int BuildVersion = 139586;

        private static readonly string StructTypedName = "mPrimaryColor";
        private static readonly string StructTypedType = "LinearColor";
        private static readonly float StructTypedDataR = 1f;
        private static readonly float StructTypedDataG = 0.440705001354218f;
        private static readonly float StructTypedDataB = 0.156314000487328f;
        private static readonly float StructTypedDataA = 1f;
        private static readonly byte[] StructTypedBytes = new byte[] { 0x0E, 0x00, 0x00, 0x00, 0x6D, 0x50, 0x72, 0x69, 0x6D, 0x61, 0x72, 0x79, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x53, 0x74, 0x72, 0x75, 0x63, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x4C, 0x69, 0x6E, 0x65, 0x61, 0x72, 0x43, 0x6F, 0x6C, 0x6F, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x16, 0xA4, 0xE1, 0x3E, 0xC7, 0x10, 0x20, 0x3E, 0x00, 0x00, 0x80, 0x3F };

        [TestMethod]
        public void StructPropertyTypedRead()
        {
            using (var stream = new MemoryStream(StructTypedBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader, BuildVersion) as StructProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(StructTypedName, prop.PropertyName);
                Assert.AreEqual(StructProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(StructTypedType, prop.Type);

                Assert.AreEqual(0, prop.Unk1);
                Assert.AreEqual(0, prop.Unk2);
                Assert.AreEqual(0, prop.Unk3);
                Assert.AreEqual(0, prop.Unk4);
                Assert.AreEqual(0, prop.Unk5);

                var data = prop.Data as LinearColor;

                Assert.AreNotEqual(null, data);
                Assert.AreEqual(StructTypedDataR, data.R);
                Assert.AreEqual(StructTypedDataG, data.G);
                Assert.AreEqual(StructTypedDataB, data.B);
                Assert.AreEqual(StructTypedDataA, data.A);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void StructPropertyTypedWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new StructProperty(StructTypedName)
                {
                    Data = new LinearColor(StructTypedDataR, StructTypedDataG, StructTypedDataB, StructTypedDataA)
                };

                prop.Serialize(writer, BuildVersion);

                Assert.AreEqual(16, prop.SerializedLength);
                CollectionAssert.AreEqual(StructTypedBytes, stream.ToArray());
            }
        }
    }
}
