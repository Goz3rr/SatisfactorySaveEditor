using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using NLog;

using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.ZLib;

namespace SatisfactorySaveParser.Save.Serialization
{
    /// <summary>
    ///     A serializer that supports versions 4 and 5 of the satisfactory save format
    /// </summary>
    public class SatisfactorySaveSerializer : ISaveSerializer
    {
        private const int ProgressionReportModifier = 20;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static readonly HashSet<string> missingProperties = new HashSet<string>();

        public event EventHandler<StageChangedEventArgs> SerializationStageChanged;
        public event EventHandler<StageProgressedEventArgs> SerializationStageProgressed;
        public event EventHandler<StageChangedEventArgs> DeserializationStageChanged;
        public event EventHandler<StageProgressedEventArgs> DeserializationStageProgressed;

        private int currentDeserializationStage, currentSerializationStage;

        private void IncrementDeserializationStage(SerializerStage stage)
        {
            DeserializationStageChanged?.Invoke(this, new StageChangedEventArgs()
            {
                Stage = stage,
                Current = currentDeserializationStage++,
                Total = 7
            });
        }

        private void UpdateDeserializationProgress(long current, long total)
        {
            var progress = (float)current / total * 100;
            DeserializationStageProgressed?.Invoke(this, new StageProgressedEventArgs()
            {
                Current = current,
                Total = total,
                Progress = progress
            });
        }

        private void IncrementSerializationStage(SerializerStage stage)
        {
            SerializationStageChanged?.Invoke(this, new StageChangedEventArgs()
            {
                Stage = stage,
                Current = currentSerializationStage++,
                Total = 7
            });
        }

        private void UpdateSerializationProgress(long current, long total)
        {
            var progress = (float)current / total * 100;
            SerializationStageProgressed?.Invoke(this, new StageProgressedEventArgs()
            {
                Current = current,
                Total = total,
                Progress = progress
            });
        }

        public static MemoryStream DumpCompressedData(Stream stream)
        {
            using var reader = new BinaryReader(stream);

            var save = new FGSaveSession
            {
                Header = DeserializeHeader(reader)
            };

            if (!save.Header.IsCompressed)
                throw new InvalidOperationException("Save is not compressed");

            var uncompressedBuffer = new MemoryStream();
            var uncompressedSize = 0L;

            while (stream.Position < stream.Length)
            {
                var chunkHeader = reader.ReadCompressedChunkHeader();
                Trace.Assert(chunkHeader.PackageTag == FCompressedChunkHeader.Magic);

                var chunkInfo = reader.ReadCompressedChunkInfo();
                Trace.Assert(chunkHeader.UncompressedSize == chunkInfo.UncompressedSize);

                var startPosition = stream.Position;
                using (var zStream = new ZLibStream(stream, CompressionMode.Decompress))
                {
                    zStream.CopyTo(uncompressedBuffer);
                }

                // ZlibStream appears to read more bytes than it uses (because of buffering probably) so we need to manually fix the input stream position
                stream.Position = startPosition + chunkInfo.CompressedSize;

                uncompressedSize += chunkInfo.UncompressedSize;
            }

            uncompressedBuffer.Position = 0;
            return uncompressedBuffer;
        }

        public FGSaveSession Deserialize(Stream stream)
        {
            currentDeserializationStage = 0;
            IncrementDeserializationStage(SerializerStage.FileOpen);
            UpdateDeserializationProgress(0, -1);

            var sw = Stopwatch.StartNew();
            using var reader = new BinaryReader(stream);

            IncrementDeserializationStage(SerializerStage.ParseHeader);
            UpdateDeserializationProgress(0, -1);
            var save = new FGSaveSession
            {
                Header = DeserializeHeader(reader)
            };

            log.Info($"Save is {(save.Header.IsCompressed ? "compressed" : "not compressed")}");
            if (!save.Header.IsCompressed)
            {
                DeserializeSaveData(save, reader);
            }
            else
            {
                IncrementDeserializationStage(SerializerStage.Decompressing);

                using var uncompressedBuffer = new MemoryStream();
                var uncompressedSize = 0L;

                var minimumProgressUpdate = stream.Length / ProgressionReportModifier;
                var lastProgressUpdate = 0L;
                UpdateDeserializationProgress(0, stream.Length);

                while (stream.Position < stream.Length)
                {
                    var chunkHeader = reader.ReadCompressedChunkHeader();
                    Trace.Assert(chunkHeader.PackageTag == FCompressedChunkHeader.Magic);

                    var chunkInfo = reader.ReadCompressedChunkInfo();
                    Trace.Assert(chunkHeader.UncompressedSize == chunkInfo.UncompressedSize);

                    var startPosition = stream.Position;
                    using (var zStream = new ZLibStream(stream, CompressionMode.Decompress))
                    {
                        zStream.CopyTo(uncompressedBuffer);
                    }

                    // ZlibStream appears to read more bytes than it uses (because of buffering probably) so we need to manually fix the input stream position
                    stream.Position = startPosition + chunkInfo.CompressedSize;

                    if (stream.Position - lastProgressUpdate > minimumProgressUpdate)
                    {
                        UpdateDeserializationProgress(stream.Position, stream.Length);
                        lastProgressUpdate = stream.Position;
                    }

                    uncompressedSize += chunkInfo.UncompressedSize;
                }

                uncompressedBuffer.Position = 0;
                using (var uncompressedReader = new BinaryReader(uncompressedBuffer))
                {
                    var dataLength = uncompressedReader.ReadInt32();
                    Trace.Assert(uncompressedSize == dataLength + 4);

                    DeserializeSaveData(save, uncompressedReader);
                }
            }

            sw.Stop();
            IncrementDeserializationStage(SerializerStage.Done);
            UpdateDeserializationProgress(0, -1);
            log.Info($"Parsing save took {sw.ElapsedMilliseconds / 1000f}s");

            return save;
        }

        private void DeserializeSaveData(FGSaveSession save, BinaryReader reader)
        {
            IncrementDeserializationStage(SerializerStage.ReadObjects);

            // Does not need to be a public property because it's equal to Objects.Count
            var totalSaveObjects = reader.ReadUInt32();
            log.Info($"Save contains {totalSaveObjects} object headers");

            UpdateDeserializationProgress(0, totalSaveObjects);
            var minimumProgressUpdate = totalSaveObjects / ProgressionReportModifier;
            var lastProgressUpdate = 0L;

            for (int i = 0; i < totalSaveObjects; i++)
            {
                save.Objects.Add(DeserializeObjectHeader(reader));

                if (i - lastProgressUpdate > minimumProgressUpdate)
                {
                    UpdateDeserializationProgress(i, totalSaveObjects);
                    lastProgressUpdate = i;
                }
            }

            IncrementDeserializationStage(SerializerStage.ReadObjectData);
            var totalSaveObjectData = reader.ReadUInt32();
            log.Info($"Save contains {totalSaveObjectData} object data");

            UpdateDeserializationProgress(0, totalSaveObjectData);
            minimumProgressUpdate = totalSaveObjectData / ProgressionReportModifier;
            lastProgressUpdate = 0L;

            Trace.Assert(save.Objects.Count == totalSaveObjects);
            Trace.Assert(save.Objects.Count == totalSaveObjectData);

            for (int i = 0; i < save.Objects.Count; i++)
            {
                DeserializeObjectData(save.Objects[i], reader);

                if (i - lastProgressUpdate > minimumProgressUpdate)
                {
                    UpdateDeserializationProgress(i, totalSaveObjectData);
                    lastProgressUpdate = i;
                }
            }

            IncrementDeserializationStage(SerializerStage.ReadDestroyedObjects);
            UpdateDeserializationProgress(0, -1);
            save.DestroyedActors.AddRange(DeserializeDestroyedActors(reader));

            log.Debug($"Read {reader.BaseStream.Position} of total {reader.BaseStream.Length} bytes");
            Trace.Assert(reader.BaseStream.Position == reader.BaseStream.Length);
        }

        public void Serialize(FGSaveSession save, Stream stream)
        {
            currentSerializationStage = 0;
            IncrementSerializationStage(SerializerStage.FileOpen);
            UpdateSerializationProgress(0, -1);

            var sw = Stopwatch.StartNew();
            using var writer = new BinaryWriter(stream);

            IncrementSerializationStage(SerializerStage.WriteHeader);
            UpdateSerializationProgress(0, -1);
            SerializeHeader(save.Header, writer);

            if (!save.Header.IsCompressed)
            {
                SerializeSaveData(save, writer);
            }
            else
            {
                using var uncompressedBuffer = new MemoryStream();
                using var uncompressedBufferWriter = new BinaryWriter(uncompressedBuffer);

                uncompressedBufferWriter.Write(0); // Placeholder size

                SerializeSaveData(save, uncompressedBufferWriter);

                uncompressedBuffer.Position = 0;
                uncompressedBufferWriter.Write((int)uncompressedBuffer.Length - 4);
                uncompressedBuffer.Position = 0;

                var uncompressedSize = 0L;

                var minimumProgressUpdate = stream.Length / ProgressionReportModifier;
                var lastProgressUpdate = 0L;

                IncrementSerializationStage(SerializerStage.Compressing);
                UpdateSerializationProgress(0, uncompressedBuffer.Length);

                for (var i = 0; i < (int)Math.Ceiling((double)uncompressedBuffer.Length / FCompressedChunkHeader.ChunkSize); i++)
                {
                    using var zBuffer = new MemoryStream();

                    var remaining = (int)Math.Min(FCompressedChunkHeader.ChunkSize, uncompressedBuffer.Length - (FCompressedChunkHeader.ChunkSize * i));

                    using (var zStream = new ZLibStream(zBuffer, CompressionMode.Compress, CompressionLevel.Level6))
                    {
                        var tmpBuf = new byte[remaining];
                        uncompressedBuffer.Read(tmpBuf, 0, remaining);
                        zStream.Write(tmpBuf, 0, remaining);
                    }

                    writer.Write(new FCompressedChunkHeader()
                    {
                        PackageTag = FCompressedChunkHeader.Magic,
                        BlockSize = remaining,
                        CompressedSize = zBuffer.Length,
                        UncompressedSize = remaining
                    });

                    writer.Write(new FCompressedChunkInfo()
                    {
                        CompressedSize = zBuffer.Length,
                        UncompressedSize = remaining
                    });

                    writer.Write(zBuffer.ToArray());

                    uncompressedSize += remaining;
                    if (uncompressedBuffer.Position - lastProgressUpdate > minimumProgressUpdate)
                    {
                        UpdateSerializationProgress(uncompressedBuffer.Position, uncompressedBuffer.Length);
                        lastProgressUpdate = uncompressedBuffer.Position;
                    }
                }
            }

            sw.Stop();
            IncrementSerializationStage(SerializerStage.Done);
            UpdateSerializationProgress(0, -1);
            log.Info($"Serializing save took {sw.ElapsedMilliseconds / 1000f}s");
        }

        private static int OrderOnSaveObjectType(SaveObject saveObject)
        {
            return saveObject switch
            {
                SaveActor _ => 0,
                SaveComponent _ => 1,
                _ => throw new NotImplementedException(),
            };
        }

        private void SerializeSaveData(FGSaveSession save, BinaryWriter writer)
        {
            IncrementSerializationStage(SerializerStage.WriteObjects);

            // Sort the list so Actors are serialized before Components
            var sortedObjects = save.Objects.OrderBy(OrderOnSaveObjectType);
            var totalSaveObjects = sortedObjects.Count();
            log.Info($"Writing {totalSaveObjects} object headers to save");

            UpdateSerializationProgress(0, totalSaveObjects);
            var minimumProgressUpdate = totalSaveObjects / ProgressionReportModifier;
            var lastProgressUpdate = 0L;
            var count = 0;

            writer.Write(totalSaveObjects);

            foreach (var saveObject in sortedObjects)
            {
                SerializeObjectHeader(saveObject, writer);

                if (count - lastProgressUpdate > minimumProgressUpdate)
                {
                    UpdateSerializationProgress(count, totalSaveObjects);
                    lastProgressUpdate = count;
                }
                count++;
            }

            IncrementSerializationStage(SerializerStage.WriteObjectData);
            log.Info($"Writing {totalSaveObjects} object data to save");

            UpdateSerializationProgress(0, totalSaveObjects);
            minimumProgressUpdate = totalSaveObjects / ProgressionReportModifier;
            lastProgressUpdate = 0L;
            count = 0;

            writer.Write(totalSaveObjects);

            foreach (var saveObject in sortedObjects)
            {
                SerializeObjectData(saveObject, writer);

                if (count - lastProgressUpdate > minimumProgressUpdate)
                {
                    UpdateSerializationProgress(count, totalSaveObjects);
                    lastProgressUpdate = count;
                }
                count++;
            }

            IncrementSerializationStage(SerializerStage.WriteDestroyedObjects);
            UpdateSerializationProgress(0, -1);
            SerializeDestroyedActors(save.DestroyedActors, writer);
        }

        public static FSaveHeader DeserializeHeader(BinaryReader reader)
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

            var logStr = $"Read save header: HeaderVersion={header.HeaderVersion}, SaveVersion={header.SaveVersion}, BuildVersion={header.BuildVersion}, MapName={header.MapName}, MapOpts={header.MapOptions}, SessionName={header.SessionName}, PlayTime={header.PlayDuration}, SaveTime={header.SaveDateTime}";

            if (header.SupportsSessionVisibility)
            {
                header.SessionVisibility = (ESessionVisibility)reader.ReadByte();
                logStr += $", Visibility={header.SessionVisibility}";
            }

            log.Debug(logStr);

            return header;
        }

        public static void SerializeHeader(FSaveHeader header, BinaryWriter writer)
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

        public static SaveObject DeserializeObjectHeader(BinaryReader reader)
        {
            var kind = (SaveObjectKind)reader.ReadInt32();
            var className = string.Intern(reader.ReadLengthPrefixedString());

            var saveObject = SaveObjectFactory.CreateFromClass(kind, className);
            saveObject.Instance = reader.ReadObjectReference();

            switch (saveObject)
            {
                case SaveActor actor:
                    actor.NeedTransform = reader.ReadBooleanFromInt32();
                    actor.Rotation = reader.ReadVector4();
                    actor.Position = reader.ReadVector3();
                    actor.Scale = reader.ReadVector3();

                    if (actor.Scale.IsSuspicious())
                        log.Warn($"Actor {actor} has suspicious scale {actor.Scale}");

                    actor.WasPlacedInLevel = reader.ReadBooleanFromInt32();
                    break;

                case SaveComponent component:
                    component.ParentEntityName = reader.ReadLengthPrefixedString();
                    break;

                default:
                    throw new NotImplementedException($"Unknown SaveObject kind {kind}");
            }

            return saveObject;
        }

        public static void SerializeObjectHeader(SaveObject saveObject, BinaryWriter writer)
        {
            writer.Write((int)saveObject.ObjectKind);
            writer.WriteLengthPrefixedString(saveObject.TypePath);
            writer.Write(saveObject.Instance);

            switch (saveObject)
            {
                case SaveActor actor:
                    writer.WriteBoolAsInt32(actor.NeedTransform);
                    writer.Write(actor.Rotation);
                    writer.Write(actor.Position);
                    writer.Write(actor.Scale);
                    writer.WriteBoolAsInt32(actor.WasPlacedInLevel);
                    break;

                case SaveComponent component:
                    writer.WriteLengthPrefixedString(component.ParentEntityName);
                    break;

                default:
                    throw new NotImplementedException($"Unknown SaveObject kind {saveObject.ObjectKind}");
            }
        }

        public static void DeserializeObjectData(SaveObject saveObject, BinaryReader reader)
        {
            var dataLength = reader.ReadInt32();
            var before = reader.BaseStream.Position;

            switch (saveObject)
            {
                case SaveActor actor:
                    actor.ParentObject = reader.ReadObjectReference();
                    var componentCount = reader.ReadInt32();
                    for (int i = 0; i < componentCount; i++)
                        actor.Components.Add(reader.ReadObjectReference());

                    break;

                case SaveComponent _:
                    break;

                default:
                    throw new NotImplementedException($"Unknown SaveObject kind {saveObject.ObjectKind}");
            }

            SerializedProperty prop;
            while ((prop = DeserializeProperty(reader)) != null)
            {
                var (objProperty, _) = prop.GetMatchingSaveProperty(saveObject.GetType());

                if (objProperty == null)
                {
                    var type = saveObject.GetType();

                    var propType = prop.PropertyType;
                    if (prop is StructProperty structProp)
                        propType += $" ({structProp.Data.GetType().Name})";

                    var propertyUniqueName = $"{saveObject.TypePath}.{prop.PropertyName}:{propType}";
                    if (!missingProperties.Contains(propertyUniqueName))
                    {
                        if (type == typeof(SaveActor) || type == typeof(SaveComponent))
                            log.Warn($"Missing property for {propType} {prop.PropertyName} on {saveObject.TypePath}");
                        else
                            log.Warn($"Missing property for {propType} {prop.PropertyName} on {type.Name}");

                        missingProperties.Add(propertyUniqueName);
                    }

                    saveObject.AddDynamicProperty(prop);
                    continue;
                }

                prop.AssignToProperty(saveObject, objProperty);
            }

            var extra = reader.ReadInt32();
            if (extra != 0)
                log.Warn($"Read extra {extra} after {saveObject.TypePath} @ {reader.BaseStream.Position - 4}");

            var remaining = dataLength - (int)(reader.BaseStream.Position - before);
            if (remaining > 0)
                saveObject.DeserializeNativeData(reader, remaining);

            var after = reader.BaseStream.Position;
            if (before + dataLength != after)
                throw new FatalSaveException($"Expected {dataLength} bytes read but got {after - before}", before);
        }

        public static void SerializeObjectData(SaveObject saveObject, BinaryWriter writer)
        {
            // TODO: Replace this with proper size calculations
            using var ms = new MemoryStream();
            using var dataWriter = new BinaryWriter(ms);

            switch (saveObject)
            {
                case SaveActor actor:
                    dataWriter.Write(actor.ParentObject);
                    dataWriter.Write(actor.Components.Count);
                    foreach (var component in actor.Components)
                        dataWriter.Write(component);

                    break;

                case SaveComponent component:
                    break;

                default:
                    throw new NotImplementedException($"Unknown SaveObject kind {saveObject.ObjectKind}");
            }

            // TODO: serialize properties

            var bytes = ms.ToArray();
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        public static SerializedProperty DeserializeProperty(BinaryReader reader)
        {
            var propertyName = reader.ReadLengthPrefixedString();
            if (propertyName == "None")
                return null;

            Trace.Assert(!String.IsNullOrEmpty(propertyName));

            var propertyType = reader.ReadLengthPrefixedString();
            var size = reader.ReadInt32();
            var index = reader.ReadInt32();

            int overhead = 1;
            var before = reader.BaseStream.Position;
            // TODO: make this not nasty
            SerializedProperty result = propertyType switch
            {
                ArrayProperty.TypeName => ArrayProperty.Parse(reader, propertyName, index, out overhead),
                BoolProperty.TypeName => BoolProperty.Deserialize(reader, propertyName, index, out overhead),
                ByteProperty.TypeName => ByteProperty.Deserialize(reader, propertyName, index, out overhead),
                EnumProperty.TypeName => EnumProperty.Deserialize(reader, propertyName, index, out overhead),
                FloatProperty.TypeName => FloatProperty.Deserialize(reader, propertyName, index),
                IntProperty.TypeName => IntProperty.Deserialize(reader, propertyName, index),
                Int64Property.TypeName => Int64Property.Deserialize(reader, propertyName, index),
                Int8Property.TypeName => Int8Property.Deserialize(reader, propertyName, index),
                InterfaceProperty.TypeName => InterfaceProperty.Deserialize(reader, propertyName, index),
                MapProperty.TypeName => MapProperty.Deserialize(reader, propertyName, index, out overhead),
                NameProperty.TypeName => NameProperty.Deserialize(reader, propertyName, index),
                ObjectProperty.TypeName => ObjectProperty.Deserialize(reader, propertyName, index),
                SetProperty.TypeName => SetProperty.Parse(reader, propertyName, index, out overhead),
                StrProperty.TypeName => StrProperty.Deserialize(reader, propertyName, index),
                StructProperty.TypeName => StructProperty.Deserialize(reader, propertyName, size, index, out overhead),
                TextProperty.TypeName => TextProperty.Deserialize(reader, propertyName, index),
                _ => throw new NotImplementedException($"Unknown property type {propertyType} for property {propertyName}"),
            };
            var after = reader.BaseStream.Position;
            var readBytes = (int)(after - before - overhead);

            if (size != readBytes)
                throw new InvalidOperationException($"Expected {size} bytes read but got {readBytes}");

            return result;
        }

        public static void SerializeProperty(SerializedProperty prop, BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(prop.PropertyName);
            writer.WriteLengthPrefixedString(prop.PropertyType);
            writer.Write(prop.SerializedLength);
            writer.Write(prop.Index);

            prop.Serialize(writer);
        }

        public static List<ObjectReference> DeserializeDestroyedActors(BinaryReader reader)
        {
            var destroyedActorsCount = reader.ReadInt32();
            log.Info($"Save contains {destroyedActorsCount} destroyed actors");

            var list = new List<ObjectReference>();

            for (int i = 0; i < destroyedActorsCount; i++)
                list.Add(reader.ReadObjectReference());

            return list;
        }

        public static void SerializeDestroyedActors(List<ObjectReference> destroyedActors, BinaryWriter writer)
        {
            writer.Write(destroyedActors.Count);

            foreach (var reference in destroyedActors)
                writer.Write(reference);
        }
    }
}
