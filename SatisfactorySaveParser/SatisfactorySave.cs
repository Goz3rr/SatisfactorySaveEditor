using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SatisfactorySaveParser
{
    /// <summary>
    ///     SatisfactorySave is the main class for parsing a savegame
    /// </summary>
    public class SatisfactorySave
    {
        /// <summary>
        ///     Unknown first magic int
        ///     Seems to always be 5
        /// </summary>
        public int Magic1 { get; private set; }

        /// <summary>
        ///     Unknown second magic int
        ///     Seems to always be 17
        /// </summary>
        public int Magic2 { get; private set; }

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
        ///     Unknown second int from the header
        ///     Seems to always be 1
        /// </summary>
        public int UnknownHeaderInt2 { get; private set; }

        /// <summary>
        ///     Main content of the save game
        /// </summary>
        public List<SaveObject> Entries = new List<SaveObject>();

        /// <summary>
        ///     Open a savefile from disk
        /// </summary>
        /// <param name="file">Full path to the .sav file, usually found in Documents/My Games/FactoryGame/SaveGame/</param>
        public SatisfactorySave(string file)
        {
            file = Environment.ExpandEnvironmentVariables(file);
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                Magic1 = reader.ReadInt32();
                Trace.Assert(Magic1 == 5);
                Magic2 = reader.ReadInt32();
                Trace.Assert(Magic2 == 17);
                Magic3 = reader.ReadInt32();
                Trace.Assert(Magic3 == 66297);

                RootObject = reader.ReadLengthPrefixedString();
                WorldArguments = reader.ReadLengthPrefixedString();
                SaveName = reader.ReadLengthPrefixedString();

                UnknownHeaderInt1 = reader.ReadInt32();
                Trace.Assert(UnknownHeaderInt1 == 158);

                UnknownHeaderBytes2 = reader.ReadBytes(0x9);

                // Does not need to be a public property because it's equal to Entries.Count
                var totalEntries = reader.ReadUInt32();

                UnknownHeaderInt2 = reader.ReadInt32();
                Trace.Assert(UnknownHeaderInt2 == 1);

                // Saved entities loop
                while (true)
                {
                    var entry = new SaveEntity(reader);
                    Entries.Add(entry);

                    if (entry.NextObjectType != SaveEntity.NextObjectIsEntity)
                    {
                        break;
                    }
                }

                // Saved components loop
                while (true)
                {
                    var entry = new SaveComponent(reader);
                    Entries.Add(entry);

                    if (entry.SaveObjectCount != 0)
                    {
                        Trace.Assert(entry.SaveObjectCount == Entries.Count);
                        break;
                    }
                }

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
                Trace.Assert(unk10 == 0);
            }
        }
    }
}
