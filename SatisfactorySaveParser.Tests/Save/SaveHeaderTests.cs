using Microsoft.VisualStudio.TestTools.UnitTesting;
using SatisfactorySaveParser.Save;
using System.IO;

namespace SatisfactorySaveParser.Tests.Save
{
    [TestClass]
    public class SaveHeaderTests
    {
        private static readonly byte[] SaveHeaderV5Bytes = new byte[] { 0x05, 0x00, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0xF9, 0x02, 0x01, 0x00, 0x11, 0x00, 0x00, 0x00, 0x50, 0x65, 0x72, 0x73, 0x69, 0x73, 0x74, 0x65, 0x6E, 0x74, 0x5F, 0x4C, 0x65, 0x76, 0x65, 0x6C, 0x00, 0x47, 0x00, 0x00, 0x00, 0x3F, 0x73, 0x74, 0x61, 0x72, 0x74, 0x6C, 0x6F, 0x63, 0x3D, 0x47, 0x72, 0x61, 0x73, 0x73, 0x20, 0x46, 0x69, 0x65, 0x6C, 0x64, 0x73, 0x3F, 0x73, 0x65, 0x73, 0x73, 0x69, 0x6F, 0x6E, 0x4E, 0x61, 0x6D, 0x65, 0x3D, 0x73, 0x70, 0x61, 0x63, 0x65, 0x20, 0x77, 0x61, 0x72, 0x3F, 0x56, 0x69, 0x73, 0x69, 0x62, 0x69, 0x6C, 0x69, 0x74, 0x79, 0x3D, 0x53, 0x56, 0x5F, 0x46, 0x72, 0x69, 0x65, 0x6E, 0x64, 0x73, 0x4F, 0x6E, 0x6C, 0x79, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x73, 0x70, 0x61, 0x63, 0x65, 0x20, 0x77, 0x61, 0x72, 0x00, 0xC5, 0xAB, 0x00, 0x00, 0xD0, 0xDA, 0x51, 0x19, 0x8E, 0xA4, 0xD6, 0x08, 0x01, 0x9E, 0x32, 0x00, 0x00 };
        private static readonly int SaveHeaderV5SaveVersion = 0x5;
        private static readonly int SaveHeaderV5BuildVersion = 0x11;
        private static readonly int SaveHeaderV5Magic = 0x000102F9;
        private static readonly string SaveHeaderV5MapName = "Persistent_Level";
        private static readonly string SaveHeaderV5MapOptions = "?startloc=Grass Fields?sessionName=space war?Visibility=SV_FriendsOnly";
        private static readonly string SaveHeaderV5SessionName = "space war";
        private static readonly int SaveHeaderV5PlayDuration = 0x0000ABC5;
        private static readonly int SaveHeaderV5Padding_0 = 0x1951DAD0;
        private static readonly long SaveHeaderV5SaveDateTime = 0x00329E0108D6A48E;
        private static readonly ESessionVisibility SaveHeaderV5SessionVisibility = ESessionVisibility.SV_Private;

        [TestMethod]
        public void TestSaveHeaderV5Reading()
        {
            using (var stream = new MemoryStream(SaveHeaderV5Bytes))
            using (var reader = new BinaryReader(stream))
            {
                var header = SaveHeader.Parse(reader);

                Assert.AreEqual(SaveHeaderV5SaveVersion, header.SaveVersion);
                Assert.AreEqual(SaveHeaderV5BuildVersion, header.BuildVersion);
                Assert.AreEqual(SaveHeaderV5Magic, header.Magic);

                Assert.AreEqual(SaveHeaderV5MapName, header.MapName);
                Assert.AreEqual(SaveHeaderV5MapOptions, header.MapOptions);
                Assert.AreEqual(SaveHeaderV5SessionName, header.SessionName);

                Assert.AreEqual(SaveHeaderV5PlayDuration, header.PlayDuration);
                Assert.AreEqual(SaveHeaderV5Padding_0, header.Padding_0);
                Assert.AreEqual(SaveHeaderV5SaveDateTime, header.SaveDateTime);
                Assert.AreEqual(SaveHeaderV5SessionVisibility, header.SessionVisibility);

                Assert.AreEqual(stream.Length, stream.Position);
            }
        }

        [TestMethod]
        public void TestSaveHeaderV5Writing()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                var header = new SaveHeader
                {
                    SaveVersion = SaveHeaderV5SaveVersion,
                    BuildVersion = SaveHeaderV5BuildVersion,
                    Magic = SaveHeaderV5Magic,

                    MapName = SaveHeaderV5MapName,
                    MapOptions = SaveHeaderV5MapOptions,
                    SessionName = SaveHeaderV5SessionName,

                    PlayDuration = SaveHeaderV5PlayDuration,
                    Padding_0 = SaveHeaderV5Padding_0,
                    SaveDateTime = SaveHeaderV5SaveDateTime,
                    SessionVisibility = SaveHeaderV5SessionVisibility
                };

                header.Serialize(writer);

                CollectionAssert.AreEqual(SaveHeaderV5Bytes, stream.ToArray());
            }
        }
    }
}
