using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser
{
    /// <summary>
    ///     SatisfactorySave is the main class for parsing a savegame
    /// </summary>
    public class SatisfactorySave
    {
        /// <summary>
        ///     Path to save on disk
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        ///     Header part of the save containing things like the version and metadata
        /// </summary>
        public SaveHeader Header { get; private set; }

        /// <summary>
        ///     Main content of the save game
        /// </summary>
        public List<SaveObject> Entries { get; set; } = new List<SaveObject>();

        /// <summary>
        ///     Unknown optional map of strings
        /// </summary>
        public List<ObjectReference> UnknownMap { get; set; } = new List<ObjectReference>();

        /// <summary>
        ///     Open a savefile from disk
        /// </summary>
        /// <param name="file">Full path to the .sav file, usually found in Documents/My Games/FactoryGame/SaveGame/</param>
        public SatisfactorySave(string file)
        {
            FileName = Environment.ExpandEnvironmentVariables(file);
            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new BinaryReader(stream))
            {
                Header = SaveHeader.Parse(reader);

                // Does not need to be a public property because it's equal to Entries.Count
                var totalEntries = reader.ReadUInt32();

                // Saved entities loop
                for(int i = 0; i < totalEntries; i++)
                {
                    var type = reader.ReadInt32();
                    switch (type)
                    {
                        case SaveEntity.TypeID:
                            Entries.Add(new SaveEntity(reader));
                            break;
                        case SaveComponent.TypeID:
                            Entries.Add(new SaveComponent(reader));
                            break;
                        default:
                            throw new InvalidOperationException($"Unexpected type {type}");
                    }
                }

                var totalEntries2 = reader.ReadInt32();
                Trace.Assert(Entries.Count == totalEntries);
                Trace.Assert(Entries.Count == totalEntries2);

                for (int i = 0; i < Entries.Count; i++)
                {
                    var len = reader.ReadInt32();
                    var before = reader.BaseStream.Position;
                    Entries[i].ParseData(len, reader);
                    var after = reader.BaseStream.Position;

                    if (before + len != after)
                    {
                        throw new InvalidOperationException($"Expected {len} bytes read but got {after - before}");
                    }
                }

                var unk10 = reader.ReadInt32();
                for (int i = 0; i < unk10; i++)
                {
                    var str1 = reader.ReadLengthPrefixedString();
                    var str2 = reader.ReadLengthPrefixedString();
                    UnknownMap.Add(new ObjectReference(str1, str2));
                }

                Trace.Assert(reader.BaseStream.Position == reader.BaseStream.Length);
            }
        }

        public void Save()
        {
            Save(FileName);
        }

        public void Save(string file)
        {
            file = Environment.ExpandEnvironmentVariables(file);

            using (var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                stream.SetLength(0); // Clear any original content

                Header.Serialize(writer);

                writer.Write(Entries.Count);

                var entities = Entries.Where(e => e is SaveEntity).ToArray();
                for (var i = 0; i < entities.Length; i++)
                {
                    writer.Write(SaveEntity.TypeID);
                    entities[i].SerializeHeader(writer);
                }

                var components = Entries.Where(e => e is SaveComponent).ToArray();
                for (var i = 0; i < components.Length; i++)
                {
                    writer.Write(SaveComponent.TypeID);
                    components[i].SerializeHeader(writer);
                }

                writer.Write(entities.Length + components.Length);

                using (var ms = new MemoryStream())
                using (var dataWriter = new BinaryWriter(ms))
                {
                    for (int i = 0; i < Entries.Count; i++)
                    {
                        Entries[i].SerializeData(dataWriter);

                        var bytes = ms.ToArray();
                        writer.Write(bytes.Length);
                        writer.Write(bytes);

                        ms.SetLength(0);
                    }
                }

                writer.Write(UnknownMap.Count);
                foreach(var unkMap in UnknownMap)
                {
                    writer.WriteLengthPrefixedString(unkMap.Root);
                    writer.WriteLengthPrefixedString(unkMap.Name);
                }
            }
        }
    }
}
