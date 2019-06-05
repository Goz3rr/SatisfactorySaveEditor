using System;
using System.IO;

using NLog;

namespace SatisfactorySaveParser.Save.Serialization
{
    public class SatisfactorySaveSerializer : ISaveSerializer
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public FGSaveSession Deserialize(Stream fileStream)
        {
            using (var reader = new BinaryReader(fileStream))
            {
                var save = new FGSaveSession
                {
                    Header = DeserializeHeader(reader)
                };

                return save;
            }
        }

        public void Serialize(FGSaveSession save, Stream fileStream)
        {
            using (var writer = new BinaryWriter(fileStream))
            {
                SerializeHeader(save.Header, writer);
            }
        }

        public FSaveHeader DeserializeHeader(BinaryReader reader)
        {
            var headerVersion = (FSaveHeaderVersion)reader.ReadInt32();
            var saveVersion = (FSaveCustomVersion)reader.ReadInt32();

            if (headerVersion > FSaveHeaderVersion.LatestVersion)
                throw new UnsupportedHeaderVersionException(headerVersion);

            if (saveVersion > FSaveCustomVersion.LatestVersion)
                throw new UnsupportedSaveVersionException(saveVersion);

            var header = new FSaveHeader
            {
                HeaderVersion = headerVersion,
                SaveVersion = saveVersion,
                BuildVersion = reader.ReadInt32(),

                MapName = reader.ReadLengthPrefixedString(),
                MapOptions = reader.ReadLengthPrefixedString(),
                SessionName = reader.ReadLengthPrefixedString(),

                PlayDuration = reader.ReadInt32(),
                SaveDateTime = reader.ReadInt64()
            };

            if (header.SupportsSessionVisibility)
            {
                header.SessionVisibility = (ESessionVisibility)reader.ReadByte();
                log.Debug($"Read save header: HeaderVersion={header.HeaderVersion}, SaveVersion={(int)header.SaveVersion}, BuildVersion={header.BuildVersion}, MapName={header.MapName}, MapOpts={header.MapOptions}, Session={header.SessionName}, PlayTime={header.PlayDuration}, SaveTime={header.SaveDateTime}, Visibility={header.SessionVisibility}");
            }
            else
            {
                log.Debug($"Read save header: HeaderVersion={header.HeaderVersion}, SaveVersion={(int)header.SaveVersion}, BuildVersion={header.BuildVersion}, MapName={header.MapName}, MapOpts={header.MapOptions}, Session={header.SessionName}, PlayTime={header.PlayDuration}, SaveTime={header.SaveDateTime}");
            }

            return header;
        }

        public void SerializeHeader(FSaveHeader header, BinaryWriter writer)
        {
            writer.Write((int)header.HeaderVersion);
            writer.Write((int)header.SaveVersion);
            writer.Write(header.BuildVersion);

            writer.WriteLengthPrefixedString(header.MapName);
            writer.WriteLengthPrefixedString(header.MapOptions);
            writer.WriteLengthPrefixedString(header.SessionName);

            writer.Write(header.PlayDuration);
            writer.Write(header.SaveDateTime);

            if (header.SupportsSessionVisibility)
                writer.Write((byte)header.SessionVisibility);
        }
    }
}
