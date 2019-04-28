using NLog;
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
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

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
        ///     List of object references of all collected objects in the world (Nut/berry bushes, slugs, etc)
        /// </summary>
        public List<ObjectReference> CollectedObjects { get; set; } = new List<ObjectReference>();

        /// <summary>
        ///     Open a savefile from disk
        /// </summary>
        /// <param name="file">Full path to the .sav file, usually found in %localappdata%/FactoryGame/Saved/SaveGames</param>
        public SatisfactorySave(string file)
        {
            log.Info($"Opening save file: {file}");

            FileName = Environment.ExpandEnvironmentVariables(file);
            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new BinaryReader(stream))
            {
                Header = SaveHeader.Parse(reader);

                // Does not need to be a public property because it's equal to Entries.Count
                var totalSaveObjects = reader.ReadUInt32();
                log.Info($"Save contains {totalSaveObjects} object headers");

                // Saved entities loop
                for (int i = 0; i < totalSaveObjects; i++)
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

                var totalSaveObjectData = reader.ReadInt32();
                log.Info($"Save contains {totalSaveObjectData} object data");
                Trace.Assert(Entries.Count == totalSaveObjects);
                Trace.Assert(Entries.Count == totalSaveObjectData);

                for (int i = 0; i < Entries.Count; i++)
                {
                    var len = reader.ReadInt32();
                    var before = reader.BaseStream.Position;

#if DEBUG
                    //log.Trace($"Reading {len} bytes @ {before} for {Entries[i].TypePath}");
#endif

                    Entries[i].ParseData(len, reader);
                    var after = reader.BaseStream.Position;

                    if (before + len != after)
                    {
                        throw new InvalidOperationException($"Expected {len} bytes read but got {after - before}");
                    }
                }

                var collectedObjectsCount = reader.ReadInt32();
                log.Info($"Save contains {collectedObjectsCount} collected objects");
                for (int i = 0; i < collectedObjectsCount; i++)
                {
                    CollectedObjects.Add(new ObjectReference(reader));
                }

                log.Debug($"Read {reader.BaseStream.Position} of total {reader.BaseStream.Length} bytes");
                Trace.Assert(reader.BaseStream.Position == reader.BaseStream.Length);
            }
        }

        public void Save()
        {
            Save(FileName);
        }

        public void Save(string file)
        {
            log.Info($"Writing save file: {file}");

            FileName = Environment.ExpandEnvironmentVariables(file);
            using (var stream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write))
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
                    for (var i = 0; i < entities.Length; i++)
                    {
                        entities[i].SerializeData(dataWriter);

                        var bytes = ms.ToArray();
                        writer.Write(bytes.Length);
                        writer.Write(bytes);

                        ms.SetLength(0);
                    }
                    for (var i = 0; i < components.Length; i++)
                    {
                        components[i].SerializeData(dataWriter);

                        var bytes = ms.ToArray();
                        writer.Write(bytes.Length);
                        writer.Write(bytes);

                        ms.SetLength(0);
                    }
                }

                writer.Write(CollectedObjects.Count);
                foreach (var collectedObject in CollectedObjects)
                {
                    writer.WriteLengthPrefixedString(collectedObject.LevelName);
                    writer.WriteLengthPrefixedString(collectedObject.PathName);
                }
            }
        }
    }
}
