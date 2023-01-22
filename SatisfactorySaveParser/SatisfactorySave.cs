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
        ///     List of levels in the save
        /// </summary>
        public List<SaveLevel> Levels { get; set; } = new List<SaveLevel>();

        /// <summary>
        ///     List of object references that do not belong to any particular level
        /// </summary>
        public List<ObjectReference> TrailingCollectedObjects { get; set; } = new List<ObjectReference>();

        
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
                if (stream.Length == 0)
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
                        File.WriteAllBytes(
                            Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".bin"),
                            buffer.ToArray());
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
            // create a single level to contain the entries. To stay compatible with U6 structure of levels.
            var defaultLevel = new SaveLevel(Header.MapName);
            Levels.Add(defaultLevel);
            
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
                        defaultLevel.Entries.Add(new SaveEntity(reader));
                        break;
                    case SaveComponent.TypeID:
                        defaultLevel.Entries.Add(new SaveComponent(reader));
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected type {type}");
                }
            }

            var totalSaveObjectData = reader.ReadInt32();
            log.Info($"Save contains {totalSaveObjectData} object data");
            Trace.Assert(defaultLevel.Entries.Count == totalSaveObjects);
            Trace.Assert(defaultLevel.Entries.Count == totalSaveObjectData);

            for (int i = 0; i < defaultLevel.Entries.Count; i++)
            {
                var len = reader.ReadInt32();
                var before = reader.BaseStream.Position;

#if DEBUG
                //log.Trace($"Reading {len} bytes @ {before} for {Entries[i].TypePath}");
#endif

                defaultLevel.Entries[i].ParseData(len, reader, Header.BuildVersion);
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
                defaultLevel.CollectedObjects.Add(new ObjectReference(reader));
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
                level.Entries.AddRange(levelEntries);
                
                var levelCollectedObjects = LoadLevelCollectedObjects(reader);
                level.CollectedObjects.AddRange(levelCollectedObjects);
                
                log.Info($"Reading level [{sublevelIndex+1}/{sublevelCount+1}] with {Levels[sublevelIndex].Entries.Count} objects and {Levels[sublevelIndex].CollectedObjects.Count} collectables: {Levels[sublevelIndex].Name}.");
                
                ParseLevelEntries(levelEntries, reader);
                LoadLevelCollectedObjects(reader); // skip second collectables
            }

            // somewhere in the U6/U7 updates can be additional collected objects
            var bytesLeft = reader.BaseStream.Length - reader.BaseStream.Position;
            if (bytesLeft > 0)
            {
                TrailingCollectedObjects = LoadLevelCollectedObjects(reader);
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

            var binarySize = reader.ReadInt32(); // skip "objects binary size"
            long Before = reader.BaseStream.Position;
            var objectCount = reader.ReadInt32();
            Trace.Assert(levelEntries.Count == objectCount);

            
            for (int i = 0; i < objectCount; i++)
            {
                var len = reader.ReadInt32();
                var before = reader.BaseStream.Position;

#if DEBUG
                /*if (i % 10000 == 0)
                {
                    log.Trace($"Having {Math.Round(((float)(i+1)/objectCount)*100, 2)}% Reading {len} bytes @ {before} for {levelEntries[i].TypePath}");
                }*/
#endif

                levelEntries[i].ParseData(len, reader, Header.BuildVersion);
                var after = reader.BaseStream.Position;

                if (before + len != after)
                {
                    throw new InvalidOperationException($"Expected {len} bytes read but got {after - before}");
                }
            }

            long ReadBytesCount = reader.BaseStream.Position - Before;
            Debug.Assert(ReadBytesCount == binarySize);
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

                log.Info($"Writing level [{i+1}/{Levels.Count}] with {Levels[i].Entries.Count} objects and {Levels[i].CollectedObjects.Count} collectables: {Levels[i].Name}.");

                using (var buffer = new MemoryStream())
                using (var subLevelWriter = new BinaryWriter(buffer))
                {
                    
                    // write object headers (entities and components)
                    var levelObjects = Levels[i].Entries;
                    var entities = levelObjects.Where(e => e is SaveEntity).Cast<SaveEntity>().ToArray();
                    var components = levelObjects.Where(e => e is SaveComponent).Cast<SaveComponent>().ToArray();
                    SaveObjectsHeaderList(subLevelWriter, buildVersion, entities, components);
                    
                    // write collected objects list
                    var levelCollectables = Levels[i].CollectedObjects;
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
                    var levelObjects = Levels[i].Entries;
                    var entities = levelObjects.Where(e => e is SaveEntity).Cast<SaveEntity>().ToArray();
                    var components = levelObjects.Where(e => e is SaveComponent).Cast<SaveComponent>().ToArray();
                    SaveObjectsContentList(subLevelWriter, buildVersion, entities, components);
                    
                    // the binary size this time however is only for object content. Without the collectables.
                    var bufferedContent = buffer.ToArray();
                    writer.Write(bufferedContent.Length);
                    writer.Write(bufferedContent);
                    buffer.SetLength(0);
                    
                    // write collected objects list
                    var levelCollectables = Levels[i].CollectedObjects;
                    SaveCollectablesList(subLevelWriter, buildVersion, levelCollectables);
                    
                    bufferedContent = buffer.ToArray();
                    writer.Write(bufferedContent);
                }
            }

            SaveCollectablesList(writer, Header.BuildVersion, TrailingCollectedObjects);
        }

        private void SaveDataU5AndBelow(BinaryWriter writer, int buildVersion)
        {
            SaveDataU5AndBelow(writer, buildVersion, Levels.SelectMany(level => level.Entries).ToList(), Levels.SelectMany(level => level.CollectedObjects).ToList());
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
