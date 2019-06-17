using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using NLog;

using SatisfactorySaveParser.Save.Properties;

namespace SatisfactorySaveParser.Save.Serialization
{
    /// <summary>
    ///     A serializer that supports versions 4 and 5 of the satisfactory save format
    /// </summary>
    public class SatisfactorySaveSerializer : ISaveSerializer
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public FGSaveSession Deserialize(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var save = new FGSaveSession
                {
                    Header = DeserializeHeader(reader)
                };

                // Does not need to be a public property because it's equal to Entries.Count
                var totalSaveObjects = reader.ReadUInt32();
                log.Info($"Save contains {totalSaveObjects} object headers");

                for (int i = 0; i < totalSaveObjects; i++)
                {
                    save.Objects.Add(DeserializeObjectHeader(reader));
                }

                var totalSaveObjectData = reader.ReadInt32();
                log.Info($"Save contains {totalSaveObjectData} object data");

                Trace.Assert(save.Objects.Count == totalSaveObjects);
                Trace.Assert(save.Objects.Count == totalSaveObjectData);

                for (int i = 0; i < save.Objects.Count; i++)
                {
                    DeserializeObjectData(save.Objects[i], reader);
                }

                save.DestroyedActors.AddRange(DeserializeDestroyedActors(reader));

                log.Debug($"Read {reader.BaseStream.Position} of total {reader.BaseStream.Length} bytes");
                Trace.Assert(reader.BaseStream.Position == reader.BaseStream.Length);

                return save;
            }
        }

        public void Serialize(FGSaveSession save, Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                SerializeHeader(save.Header, writer);


                writer.Write(save.Objects.Count);

                var actors = save.Objects.Where(e => e is SaveActor).ToArray();
                foreach (var actor in actors)
                    SerializeObjectHeader(actor, writer);

                var components = save.Objects.Where(e => e is SaveComponent).ToArray();
                foreach (var component in components)
                    SerializeObjectHeader(component, writer);


                writer.Write(actors.Length + components.Length);

                foreach (var actor in actors)
                    SerializeObjectData(actor, writer);

                foreach (var component in components)
                    SerializeObjectData(component, writer);


                SerializeDestroyedActors(save.DestroyedActors, writer);
            }
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
            var className = reader.ReadLengthPrefixedString();

            var saveObject = SaveObjectFactory.CreateFromClass(kind, className);
            saveObject.Instance = reader.ReadObjectReference();

            switch (saveObject)
            {
                case SaveActor actor:
                    actor.NeedTransform = reader.ReadInt32() == 1;
                    actor.Rotation = reader.ReadVector4();
                    actor.Position = reader.ReadVector3();
                    actor.Scale = reader.ReadVector3();
                    actor.WasPlacedInLevel = reader.ReadInt32() == 1;
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
                    writer.Write(actor.NeedTransform ? 1 : 0);
                    writer.Write(actor.Rotation);
                    writer.Write(actor.Position);
                    writer.Write(actor.Scale);
                    writer.Write(actor.WasPlacedInLevel ? 1 : 0);
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

                case SaveComponent component:
                    break;

                default:
                    throw new NotImplementedException($"Unknown SaveObject kind {saveObject.ObjectKind}");
            }

            SerializedProperty prop;
            while ((prop = DeserializeProperty(reader)) != null)
            {
                var (objProperty, objPropertyAttr) = saveObject.GetMatchingProperty(prop);

                if (objProperty == null)
                {
                    var type = saveObject.GetType();
                    if (type == typeof(SaveActor) || type == typeof(SaveComponent))
                        log.Warn($"Missing property for {prop.PropertyType} {prop.PropertyName} on {saveObject.TypePath}");
                    else
                        log.Warn($"Missing property for {prop.PropertyType} {prop.PropertyName} on {type.Name}");

                    saveObject.DynamicProperties.Add(prop);
                    continue;
                }

                prop.AssignToProperty(saveObject, objProperty);
            }

            var remaining = dataLength - (int)(reader.BaseStream.Position - before);
            saveObject.DeserializeNativeData(reader, remaining);

            var after = reader.BaseStream.Position;
            if (before + dataLength != after)
                throw new FatalSaveException($"Expected {dataLength} bytes read but got {after - before}");
        }

        public static void SerializeObjectData(SaveObject saveObject, BinaryWriter writer)
        {
            // TODO: Replace this with proper size calculations?
            using (var ms = new MemoryStream())
            using (var dataWriter = new BinaryWriter(ms))
            {
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
        }

        public static SerializedProperty DeserializeProperty(BinaryReader reader)
        {
            SerializedProperty result;

            var propertyName = reader.ReadLengthPrefixedString();
            if (propertyName == "None")
                return null;

            Trace.Assert(!String.IsNullOrEmpty(propertyName));

            var propertyType = reader.ReadLengthPrefixedString();
            var size = reader.ReadInt32();
            var index = reader.ReadInt32();

            int overhead = 1;
            var before = reader.BaseStream.Position;
            switch (propertyType)
            {
                case ArrayProperty.TypeName:
                    result = ArrayProperty.Parse(reader, propertyName, size, index, out overhead);
                    break;
                case BoolProperty.TypeName:
                    overhead = 2;
                    result = BoolProperty.Deserialize(reader, propertyName, index);
                    break;
                case ByteProperty.TypeName:
                    result = ByteProperty.Deserialize(reader, propertyName, index, out overhead);
                    break;
                case EnumProperty.TypeName:
                    result = EnumProperty.Deserialize(reader, propertyName, index, out overhead);
                    break;
                case FloatProperty.TypeName:
                    result = FloatProperty.Deserialize(reader, propertyName, index);
                    break;
                case IntProperty.TypeName:
                    result = IntProperty.Deserialize(reader, propertyName, index);
                    break;
                case NameProperty.TypeName:
                    result = NameProperty.Deserialize(reader, propertyName, index);
                    break;
                case ObjectProperty.TypeName:
                    result = ObjectProperty.Deserialize(reader, propertyName, index);
                    break;
                case StrProperty.TypeName:
                    result = StrProperty.Deserialize(reader, propertyName, index);
                    break;
                case TextProperty.TypeName:
                    result = TextProperty.Deserialize(reader, propertyName, index);
                    break;
                default:
                    throw new NotImplementedException($"Unknown property type {propertyType} for property {propertyName}");
            }
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
