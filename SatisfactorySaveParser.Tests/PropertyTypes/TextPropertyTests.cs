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

        private static readonly string Alt2TextName = "mMapText";
        private static readonly string Alt2TextValue = "Radar Tower: {Name}";
        private static readonly int Alt2TextUnknown = 1;
        private static readonly string Alt2TextUnknown8 = "1A1BB1E24E24B63BAEA1059177C85F97";
        private static readonly byte Alt2TextUnknown5 = 3;
        private static readonly int Alt2TextUnknown6 = 8;
        private static readonly string Alt2TextFormatName = "Name";
        private static readonly byte Alt2TextFormatUnknown = 4;
        private static readonly byte[] Alt2TextFormatData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x42, 0x61, 0x73, 0x65, 0x00 };
        private static readonly byte[] Alt2TextBytes = new byte[] { 0x09, 0x00, 0x00, 0x00, 0x6D, 0x4D, 0x61, 0x70, 0x54, 0x65, 0x78, 0x74, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x6F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x00, 0x00, 0x31, 0x41, 0x31, 0x42, 0x42, 0x31, 0x45, 0x32, 0x34, 0x45, 0x32, 0x34, 0x42, 0x36, 0x33, 0x42, 0x41, 0x45, 0x41, 0x31, 0x30, 0x35, 0x39, 0x31, 0x37, 0x37, 0x43, 0x38, 0x35, 0x46, 0x39, 0x37, 0x00, 0x14, 0x00, 0x00, 0x00, 0x52, 0x61, 0x64, 0x61, 0x72, 0x20, 0x54, 0x6F, 0x77, 0x65, 0x72, 0x3A, 0x20, 0x7B, 0x4E, 0x61, 0x6D, 0x65, 0x7D, 0x00, 0x01, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x04, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x42, 0x61, 0x73, 0x65, 0x00 };

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

        [TestMethod]
        public void TextPropertyAlt2Read()
        {
            using (var stream = new MemoryStream(Alt2TextBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader) as TextProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(Alt2TextName, prop.PropertyName);
                Assert.AreEqual(TextProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                Assert.AreEqual(Alt2TextValue, prop.Value);
                Assert.AreEqual(Alt2TextUnknown, prop.Unknown4);
                Assert.AreEqual(Alt2TextUnknown5, prop.Unknown5);
                Assert.AreEqual(Alt2TextUnknown6, prop.Unknown6);
                Assert.AreEqual(0, prop.Unknown7);

                Assert.AreEqual(1, prop.FormatData.Count);
                var formatData = prop.FormatData[0];
                Assert.AreEqual(Alt2TextFormatName, formatData.Name);
                Assert.AreEqual(Alt2TextFormatUnknown, formatData.Unknown);
                CollectionAssert.AreEqual(Alt2TextFormatData, formatData.Data);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void TextPropertyAlt2Write()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new TextProperty(Alt2TextName)
                {
                    Unknown8 = Alt2TextUnknown8,
                    Value = Alt2TextValue,
                    Unknown4 = Alt2TextUnknown,
                    Unknown5 = Alt2TextUnknown5,
                    Unknown6 = Alt2TextUnknown6
                };

                prop.FormatData.Add(new TextFormatData()
                {
                    Name = Alt2TextFormatName,
                    Unknown = Alt2TextFormatUnknown,
                    Data = Alt2TextFormatData
                });

                prop.Serialize(writer);

                Assert.AreEqual(111, prop.SerializedLength);
                CollectionAssert.AreEqual(Alt2TextBytes, stream.ToArray());
            }
        }
    }
}
