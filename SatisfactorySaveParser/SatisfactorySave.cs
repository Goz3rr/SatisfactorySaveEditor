using NLog;
using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Structures;
using SatisfactorySaveParser.ZLib;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using SatisfactorySaveParser.Exceptions;

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
        public FSaveHeader Header { get; private set; }

        /// <summary>
        ///     Main content of the save game
        /// </summary>
        public List<SaveObject> Entries { get; set; } = new List<SaveObject>();

        /// <summary>
        ///     List of object references of all collected objects in the world (Nut/berry bushes, slugs, etc)
        /// </summary>
        public List<ObjectReference> CollectedObjects { get; set; } = new List<ObjectReference>();
        
        /// <summary>
        ///     List of levels in the save
        /// </summary>
        public List<SaveLevel> Levels { get; set; } = new List<SaveLevel>();

        
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
                if(stream.Length == 0)
                {
                    throw new Exception("Save file is completely empty");
                }

                Header = FSaveHeader.Parse(reader);

                if (Header.SaveVersion < FSaveCustomVersion.SaveFileIsCompressed)
                {
                    LoadDataU5AndBelow(reader);
                }
                else
                {
                    using (var buffer = new MemoryStream())
                    {
                        var uncompressedSize = 0L;

                        while (stream.Position < stream.Length)
                        {
                            var header = reader.ReadChunkInfo();
                            Trace.Assert(header.CompressedSize == ChunkInfo.Magic);
                            Trace.Assert(header.UncompressedSize == ChunkInfo.ChunkSize);

                            var summary = reader.ReadChunkInfo();

                            var subChunk = reader.ReadChunkInfo();
                            Trace.Assert(subChunk.UncompressedSize == summary.UncompressedSize);

                            var startPosition = stream.Position;
                            using (var zStream = new ZLibStream(stream, CompressionMode.Decompress))
                            {
                                zStream.CopyTo(buffer);
                            }

                            // ZlibStream appears to read more bytes than it uses (because of buffering probably) so we need to manually fix the input stream position
                            stream.Position = startPosition + subChunk.CompressedSize;

                            uncompressedSize += subChunk.UncompressedSize;
                        }


                        buffer.Position = 0;

#if DEBUG
                        File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".bin"), buffer.ToArray());
#endif


                        using (var bufferReader = new BinaryReader(buffer))
                        {
                            var dataLength = bufferReader.ReadInt32();
                            Trace.Assert(uncompressedSize == dataLength + 4);

                            if (Header.SaveVersion < FSaveCustomVersion.AddedSublevelStreaming)
                            {
                                LoadDataU5AndBelow(bufferReader);
                            }
                            else
                            {
                                LoadDataU6AndAbove(bufferReader);
                            }
                        }
                    }
                }
            }
        }

        private void LoadDataU5AndBelow(BinaryReader reader)
        {
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

                Entries[i].ParseData(len, reader, Header.BuildVersion);
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

        private void LoadDataU6AndAbove(BinaryReader reader)
        {
            var sublevelCount = reader.ReadInt32();
            for (int sublevelIndex = 0;
                 sublevelIndex <= sublevelCount;
                 sublevelIndex++) // sublevels and the "persistent level" at the end!
            {
                var levelName = (sublevelIndex == sublevelCount)
                    ? Header.MapName
                    : reader.ReadLengthPrefixedString(); // use MapName for the persistent level
                SaveLevel level = new SaveLevel(levelName);
                Levels.Add(level);

                var levelEntries = LoadLevelEntries(reader, levelName);
                Entries.AddRange(levelEntries);
                level.ContainedObjectInstances.AddRange(levelEntries.Select(entry => entry.InstanceName));
                
                var levelCollectedObjects = LoadLevelCollectedObjects(reader);
                CollectedObjects.AddRange(levelCollectedObjects);
                level.ContainedCollectablesInstances.AddRange(levelCollectedObjects);
                
                log.Info($"Reading level [{sublevelIndex+1}/{Levels.Count}] with {Levels[sublevelIndex].ContainedObjectInstances.Count} objects and {Levels[sublevelIndex].ContainedCollectablesInstances.Count} collectables: {Levels[sublevelIndex].Name}.");
                
                ParseLevelEntries(levelEntries, reader);
                LoadLevelCollectedObjects(reader); // skip second collectables
            }
        }


        private List<SaveObject> LoadLevelEntries(BinaryReader reader, string levelname)
        {
            reader.ReadInt32(); // skip "object header and collectables size"
            var levelEntries = new List<SaveObject>();
            var totalSaveObjects = reader.ReadInt32();

            for (int i = 0; i < totalSaveObjects; i++)
            {
                var type = reader.ReadInt32();
                switch (type)
                {
                    case SaveEntity.TypeID:
                        var entity = new SaveEntity(reader);
                        entity.LevelName = levelname;
                        levelEntries.Add(entity);
                        break;
                    case SaveComponent.TypeID:
                        var component = new SaveComponent(reader);
                        component.LevelName = levelname;
                        levelEntries.Add(component);
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected type {type}");
                }
            }

            return levelEntries;
        }
        
        private List<ObjectReference> LoadLevelCollectedObjects(BinaryReader reader)
        {
            var levelCollectedObjects = new List<ObjectReference>();
            var collectedObjectsCount = reader.ReadInt32();

            for (int i = 0; i < collectedObjectsCount; i++)
            {
                var collected = new ObjectReference(reader);
                levelCollectedObjects.Add(collected);
            }
            return levelCollectedObjects;
        }

        private void ParseLevelEntries(List<SaveObject> levelEntries, BinaryReader reader)
        {
            reader.ReadInt32(); // skip "objects binary size"
            var objectCount = reader.ReadInt32();
            Trace.Assert(levelEntries.Count == objectCount);
            
            for (int i = 0; i < objectCount; i++)
            {
                var len = reader.ReadInt32();
                var before = reader.BaseStream.Position;

#if DEBUG
                //log.Trace($"Reading {len} bytes @ {before} for {levelEntries[i].TypePath}");
#endif

                levelEntries[i].ParseData(len, reader, Header.BuildVersion);
                var after = reader.BaseStream.Position;

                if (before + len != after)
                {
                    throw new InvalidOperationException($"Expected {len} bytes read but got {after - before}");
                }
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

                if (Header.SaveVersion < FSaveCustomVersion.SaveFileIsCompressed)
                {
                    SaveDataU5AndBelow(writer, Header.BuildVersion);
                }
                else
                {
                    using (var buffer = new MemoryStream())
                    using (var bufferWriter = new BinaryWriter(buffer))
                    {
                        bufferWriter.Write(0); // Placeholder size

                        if (Header.SaveVersion < FSaveCustomVersion.AddedSublevelStreaming)
                        {
                            SaveDataU5AndBelow(bufferWriter, Header.BuildVersion);
                        }
                        else
                        {
                            SaveDataU6AndAbove(bufferWriter, Header.BuildVersion);
                        }

                        buffer.Position = 0;
                        bufferWriter.Write((int)buffer.Length - 4);
                        buffer.Position = 0;

                        for (var i = 0; i < (int)Math.Ceiling((double)buffer.Length / ChunkInfo.ChunkSize); i++)
                        {
                            using (var zBuffer = new MemoryStream())
                            {
                                var remaining = (int)Math.Min(ChunkInfo.ChunkSize, buffer.Length - (ChunkInfo.ChunkSize * i));

                                using (var zStream = new ZLibStream(zBuffer, CompressionMode.Compress, CompressionLevel.Level6))
                                {
                                    var tmpBuf = new byte[remaining];
                                    buffer.Read(tmpBuf, 0, remaining);
                                    zStream.Write(tmpBuf, 0, remaining);
                                }

                                writer.Write(new ChunkInfo()
                                {
                                    CompressedSize = ChunkInfo.Magic,
                                    UncompressedSize = ChunkInfo.ChunkSize
                                });

                                writer.Write(new ChunkInfo()
                                {
                                    CompressedSize = zBuffer.Length,
                                    UncompressedSize = remaining
                                });

                                writer.Write(new ChunkInfo()
                                {
                                    CompressedSize = zBuffer.Length,
                                    UncompressedSize = remaining
                                });

                                //writer.Write(tmpBuf);
                                //zBuffer.CopyTo(stream);
                                writer.Write(zBuffer.ToArray());
                            }
                        }
                    }
                }
            }
        }

        private void SaveDataU6AndAbove(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Levels.Count-1);

            for (int i = 0; i < Levels.Count; i++)
            {
                if (!Levels[i].Name.Equals(Header.MapName))
                {
                    writer.WriteLengthPrefixedString(Levels[i].Name);
                }

                log.Info($"Writing level [{i+1}/{Levels.Count}] with {Levels[i].ContainedObjectInstances.Count} objects and {Levels[i].ContainedCollectablesInstances.Count} collectables: {Levels[i].Name}.");

                using (var buffer = new MemoryStream())
                using (var subLevelWriter = new BinaryWriter(buffer))
                {
                    
                    // write object headers (entities and components)
                    var levelObjects = Levels[i].ContainedObjectInstances.Select(instanceName =>
                        Entries.First(entry => entry.InstanceName.Equals(instanceName))).ToList();
                    var entities = levelObjects.Where(e => e is SaveEntity).Cast<SaveEntity>().ToArray();
                    var components = levelObjects.Where(e => e is SaveComponent).Cast<SaveComponent>().ToArray();
                    SaveObjectsHeaderList(subLevelWriter, buildVersion, entities, components);
                    
                    // write collected objects list
                    var levelCollectables = Levels[i].ContainedCollectablesInstances;
                    SaveCollectablesList(subLevelWriter, buildVersion, levelCollectables);

                    var bufferedContent = buffer.ToArray();
                    writer.Write(bufferedContent.Length);
                    writer.Write(bufferedContent);
                }
                
                // write objects content + collectables.
                using (var buffer = new MemoryStream())
                using (var subLevelWriter = new BinaryWriter(buffer))
                {
                    
                    // write object content (entities and components)
                    var levelObjects = Levels[i].ContainedObjectInstances.Select(instanceName =>
                        Entries.First(entry => entry.InstanceName.Equals(instanceName))).ToList();
                    var entities = levelObjects.Where(e => e is SaveEntity).Cast<SaveEntity>().ToArray();
                    var components = levelObjects.Where(e => e is SaveComponent).Cast<SaveComponent>().ToArray();
                    SaveObjectsContentList(subLevelWriter, buildVersion, entities, components);
                    
                    // write collected objects list
                    var levelCollectables = Levels[i].ContainedCollectablesInstances;
                    SaveCollectablesList(subLevelWriter, buildVersion, levelCollectables);

                    var bufferedContent = buffer.ToArray();
                    writer.Write(bufferedContent.Length);
                    writer.Write(bufferedContent);
                }
            }
        }

        private void SaveDataU5AndBelow(BinaryWriter writer, int buildVersion)
        {
            SaveDataU5AndBelow(writer, buildVersion, Entries, CollectedObjects);
        }
        
        private void SaveDataU5AndBelow(BinaryWriter writer, int buildVersion, List<SaveObject> entries, List<ObjectReference> collectedObjects)
        {
            SaveObjectsList(writer, buildVersion, entries);
            SaveCollectablesList(writer, buildVersion, collectedObjects);
        }

        private void SaveCollectablesList(in BinaryWriter writer, in int buildVersion, in List<ObjectReference> collectedObjects)
        {
            writer.Write(collectedObjects.Count);
            foreach (var collectedObject in collectedObjects)
            {
                writer.WriteLengthPrefixedString(collectedObject.LevelName);
                writer.WriteLengthPrefixedString(collectedObject.PathName);
            }
        }

        private void SaveObjectsList(in BinaryWriter writer, in int buildVersion, in List<SaveObject> objects)
        {
            var entities = objects.Where(e => e is SaveEntity).Cast<SaveEntity>().ToArray();
            var components = objects.Where(e => e is SaveComponent).Cast<SaveComponent>().ToArray();
            SaveObjectsHeaderList(writer, buildVersion, entities, components);
            SaveObjectsContentList(writer, buildVersion, entities, components);
        }

        private void SaveObjectsHeaderList(in BinaryWriter writer, in int buildVersion, in SaveEntity[] entities, in SaveComponent[] components)
        {
            writer.Write(entities.Length + components.Length);
            
            for (var i = 0; i < entities.Length; i++)
            {
                writer.Write(SaveEntity.TypeID);
                entities[i].SerializeHeader(writer);
            }

            for (var i = 0; i < components.Length; i++)
            {
                writer.Write(SaveComponent.TypeID);
                components[i].SerializeHeader(writer);
            }
        }
        
        private void SaveObjectsContentList(in BinaryWriter writer, in int buildVersion, in SaveEntity[] entities, in SaveComponent[] components)
        {
            writer.Write(entities.Length + components.Length);

            using (var ms = new MemoryStream())
            using (var dataWriter = new BinaryWriter(ms))
            {
                
                for (var i = 0; i < entities.Length; i++)
                {
                    entities[i].SerializeData(dataWriter, buildVersion);

                    var bytes = ms.ToArray();
                    writer.Write(bytes.Length);
                    writer.Write(bytes);

                    ms.SetLength(0);
                }
                for (var i = 0; i < components.Length; i++)
                {
                    components[i].SerializeData(dataWriter, buildVersion);

                    var bytes = ms.ToArray();
                    writer.Write(bytes.Length);
                    writer.Write(bytes);

                    ms.SetLength(0);
                }
            }
        }
    }
}
