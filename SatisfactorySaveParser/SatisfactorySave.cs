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
        ///     Unknown first magic int
        ///     Seems to always be 5
        /// </summary>
        public int SaveVersion1 { get; private set; }

        /// <summary>
        ///     Unknown second magic int
        ///     Seems to always be 17
        /// </summary>
        public int SaveVersion2 { get; private set; }

        /// <summary>
        ///     Unknown third magic int
        ///     Seems to always be 66297
        /// </summary>
        public int Magic3 { get; private set; }

        /// <summary>
        ///     The name of what appears to be the root object of the save.
        ///     Seems to always be "Persistent_Level"
        /// </summary>
        public string RootObject { get; private set; }

        /// <summary>
        ///     An URL style list of arguments of the session.
        ///     Contains the startloc, sessionName and Visibility
        /// </summary>
        public string WorldArguments { get; private set; }

        /// <summary>
        ///     Name of the saved game
        /// </summary>
        public string SaveName { get; private set; }

        /// <summary>
        ///     Unknown first int from the header
        /// </summary>
        public int UnknownHeaderInt1 { get; private set; }

        /// <summary>
        ///     Unknown bytes from the header
        /// </summary>
        public byte[] UnknownHeaderBytes2 { get; private set; }

        /// <summary>
        ///     Unknown byte from the header
        /// </summary>
        public byte UnknownByte3 { get; set; }


        /// <summary>
        ///     Main content of the save game
        /// </summary>
        public List<SaveObject> Entries { get; set; } = new List<SaveObject>();

        /// <summary>
        ///     Unknown optional map of strings
        /// </summary>
        public List<(string, string)> UnknownMap { get; set; } = new List<(string, string)>();

        /// <summary>
        ///     Open a savefile from disk
        /// </summary>
        /// <param name="file">Full path to the .sav file, usually found in Documents/My Games/FactoryGame/SaveGame/</param>
        public SatisfactorySave(string file)
        {
            FileName = Environment.ExpandEnvironmentVariables(file);
            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                SaveVersion1 = reader.ReadInt32();
                Trace.Assert(SaveVersion1 == 5 || SaveVersion1 == 4);
                SaveVersion2 = reader.ReadInt32();
                if(SaveVersion1 == 5)
                    Trace.Assert(SaveVersion2 == 17);
                else
                    Trace.Assert(SaveVersion2 == 16);

                Magic3 = reader.ReadInt32();
                Trace.Assert(Magic3 == 66297);

                RootObject = reader.ReadLengthPrefixedString();
                WorldArguments = reader.ReadLengthPrefixedString();
                SaveName = reader.ReadLengthPrefixedString();

                UnknownHeaderInt1 = reader.ReadInt32();
                //Trace.Assert(UnknownHeaderInt1 == 158);

                UnknownHeaderBytes2 = reader.ReadBytes(0x8);

                if(SaveVersion1 == 5)
                    UnknownByte3 = reader.ReadByte();

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
                    UnknownMap.Add((str1, str2));
                }
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

                writer.Write(SaveVersion1);
                writer.Write(SaveVersion2);
                writer.Write(Magic3);

                writer.WriteLengthPrefixedString(RootObject);
                writer.WriteLengthPrefixedString(WorldArguments);
                writer.WriteLengthPrefixedString(SaveName);

                writer.Write(UnknownHeaderInt1);
                writer.Write(UnknownHeaderBytes2);

                if (SaveVersion1 == 5)
                    writer.Write(UnknownByte3);

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
                    writer.WriteLengthPrefixedString(unkMap.Item1);
                    writer.WriteLengthPrefixedString(unkMap.Item2);
                }
            }
        }
    }
}
