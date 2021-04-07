using Microsoft.VisualStudio.TestTools.UnitTesting;

using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.Save;

using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser.Tests.PropertyTypes
{
    [TestClass]
    public class TextPropertyTests
    {
        private const int BuildVersion = 139586;

        private const string TextName = "mCompassText";
        private const string TextValue = "Oil";
        private const int TextFlags = 2;
        private static readonly byte[] TextBytes = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x6D, 0x43, 0x6F, 0x6D, 0x70, 0x61, 0x73, 0x73, 0x54, 0x65, 0x78, 0x74, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x15, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x4F, 0x69, 0x6C, 0x00 };

        private const string AltTextName = "mCompassText";
        private const string AltTextValue = "Beacon";
        private const int AltTextFlags = 8;
        private const string AltTextKey = "B8D68B1B46E1D68DCB0BE39EC800B74E";
        private static readonly byte[] AltTextBytes = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x6D, 0x43, 0x6F, 0x6D, 0x70, 0x61, 0x73, 0x73, 0x54, 0x65, 0x78, 0x74, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x39, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x00, 0x00, 0x42, 0x38, 0x44, 0x36, 0x38, 0x42, 0x31, 0x42, 0x34, 0x36, 0x45, 0x31, 0x44, 0x36, 0x38, 0x44, 0x43, 0x42, 0x30, 0x42, 0x45, 0x33, 0x39, 0x45, 0x43, 0x38, 0x30, 0x30, 0x42, 0x37, 0x34, 0x45, 0x00, 0x07, 0x00, 0x00, 0x00, 0x42, 0x65, 0x61, 0x63, 0x6F, 0x6E, 0x00 };

        private const string Alt2TextName = "mMapText";
        private const string Alt2TextValue = "Radar Tower: {Name}";
        private const int Alt2TextFlags = 1;
        private const string Alt2TextKey = "1A1BB1E24E24B63BAEA1059177C85F97";
        private static readonly ArgumentFormat Alt2ArgumentFormat = new ArgumentFormat()
        {
            Name = "Name",
            ValueType = EFormatArgumentType.Text,
            Value = new BaseTextEntry(18)
            {
                Namespace = "",
                Key = "",
                Value = "Base"
            }
        };
        private const int Alt2TextSourceFlags = 8;
        private static readonly byte[] Alt2TextBytes = new byte[] { 0x09, 0x00, 0x00, 0x00, 0x6D, 0x4D, 0x61, 0x70, 0x54, 0x65, 0x78, 0x74, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x6F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x03, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x21, 0x00, 0x00, 0x00, 0x31, 0x41, 0x31, 0x42, 0x42, 0x31, 0x45, 0x32, 0x34, 0x45, 0x32, 0x34, 0x42, 0x36, 0x33, 0x42, 0x41, 0x45, 0x41, 0x31, 0x30, 0x35, 0x39, 0x31, 0x37, 0x37, 0x43, 0x38, 0x35, 0x46, 0x39, 0x37, 0x00, 0x14, 0x00, 0x00, 0x00, 0x52, 0x61, 0x64, 0x61, 0x72, 0x20, 0x54, 0x6F, 0x77, 0x65, 0x72, 0x3A, 0x20, 0x7B, 0x4E, 0x61, 0x6D, 0x65, 0x7D, 0x00, 0x01, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x04, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x42, 0x61, 0x73, 0x65, 0x00 };

        private const string NoneTextName = "mStationName";
        private const int NoneTextFlags = 18;
        private static readonly byte[] NoneTextBytes = new byte[] { 0x0D, 0x00, 0x00, 0x00, 0x6D, 0x53, 0x74, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x0D, 0x00, 0x00, 0x00, 0x54, 0x65, 0x78, 0x74, 0x50, 0x72, 0x6F, 0x70, 0x65, 0x72, 0x74, 0x79, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0xFF };


        [TestMethod]
        public void TextPropertyRead()
        {
            using (var stream = new MemoryStream(TextBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader, BuildVersion) as TextProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(TextName, prop.PropertyName);
                Assert.AreEqual(TextProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                var text = prop.Text as BaseTextEntry;
                Assert.AreNotEqual(null, text);

                Assert.AreEqual(TextValue, text.Value);
                Assert.AreEqual(TextFlags, text.Flags);

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
                    Text = new BaseTextEntry(TextFlags)
                    {
                        Value = TextValue
                    }
                };

                prop.Serialize(writer, BuildVersion);

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
                var prop = SerializedProperty.Parse(reader, BuildVersion) as TextProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(AltTextName, prop.PropertyName);
                Assert.AreEqual(TextProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                var text = prop.Text as BaseTextEntry;
                Assert.AreNotEqual(null, text);

                Assert.AreEqual(AltTextKey, text.Key);
                Assert.AreEqual(AltTextValue, text.Value);
                Assert.AreEqual(AltTextFlags, text.Flags);

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
                    Text = new BaseTextEntry(AltTextFlags)
                    {
                        Key = AltTextKey,
                        Value = AltTextValue
                    }
                };

                prop.Serialize(writer, BuildVersion);

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
                var prop = SerializedProperty.Parse(reader, BuildVersion) as TextProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(Alt2TextName, prop.PropertyName);
                Assert.AreEqual(TextProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                var text = prop.Text as ArgumentFormatTextEntry;
                Assert.AreNotEqual(null, text);

                Assert.AreEqual(Alt2TextFlags, text.Flags);

                Assert.AreEqual(Alt2TextKey, text.SourceFormat.Key);
                Assert.AreEqual(Alt2TextValue, text.SourceFormat.Value);
                Assert.AreEqual(Alt2TextSourceFlags, text.SourceFormat.Flags);

                var eq = Alt2ArgumentFormat.Equals(text.Arguments[0]);

                CollectionAssert.AreEqual(new List<ArgumentFormat>(new[] { Alt2ArgumentFormat }), text.Arguments);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void TextPropertyAlt2Write()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var text = new ArgumentFormatTextEntry(Alt2TextFlags)
                {
                    SourceFormat = new BaseTextEntry(Alt2TextSourceFlags)
                    {
                        Key = Alt2TextKey,
                        Value = Alt2TextValue
                    }
                };
                text.Arguments.Add(Alt2ArgumentFormat);

                var prop = new TextProperty(Alt2TextName)
                {
                    Text = text
                };

                prop.Serialize(writer, BuildVersion);

                Assert.AreEqual(111, prop.SerializedLength);
                CollectionAssert.AreEqual(Alt2TextBytes, stream.ToArray());
            }
        }

        [TestMethod]
        public void TextPropertyNoneRead()
        {
            using (var stream = new MemoryStream(NoneTextBytes))
            using (var reader = new BinaryReader(stream))
            {
                var prop = SerializedProperty.Parse(reader, BuildVersion) as TextProperty;

                Assert.AreNotEqual(null, prop);

                Assert.AreEqual(NoneTextName, prop.PropertyName);
                Assert.AreEqual(TextProperty.TypeName, prop.PropertyType);

                Assert.AreEqual(0, prop.Index);

                var text = prop.Text as NoneTextEntry;
                Assert.AreNotEqual(null, text);

                Assert.AreEqual(NoneTextFlags, text.Flags);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void TextPropertyNoneWrite()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var prop = new TextProperty(NoneTextName)
                {
                    Text = new NoneTextEntry(NoneTextFlags)
                };

                prop.Serialize(writer, BuildVersion);

                Assert.AreEqual(5, prop.SerializedLength);
                CollectionAssert.AreEqual(NoneTextBytes, stream.ToArray());
            }
        }
    }
}
