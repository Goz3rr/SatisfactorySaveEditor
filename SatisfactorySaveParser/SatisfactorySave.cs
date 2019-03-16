using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SatisfactorySaveParser
{
    public class SatisfactorySave
    {
        public List<SaveEntry> SaveEntries = new List<SaveEntry>();

        public SatisfactorySave(string file)
        {
            file = Environment.ExpandEnvironmentVariables(file);
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new BinaryReader(stream))
            {
                var unk1 = reader.ReadInt32();
                Trace.Assert(unk1 == 5);
                var unk2 = reader.ReadInt32();
                Trace.Assert(unk2 == 17);
                var unk3 = reader.ReadInt32();
                Trace.Assert(unk3 == 66297);

                var unk4 = reader.ReadLengthPrefixedString();
                var unk5 = reader.ReadLengthPrefixedString();
                var unk6 = reader.ReadLengthPrefixedString();

                var unk7 = reader.ReadInt32();
                Trace.Assert(unk7 == 158);

                var unk8 = reader.ReadBytes(0x9);
                var totalEntries = reader.ReadUInt32();
                var unk9 = reader.ReadInt32();
                Trace.Assert(unk9 == 1);

                while (true)
                {
                    var entry = new SaveEntity
                    {
                        Str1 = reader.ReadLengthPrefixedString(),
                        Str2 = reader.ReadLengthPrefixedString(),
                        Str3 = reader.ReadLengthPrefixedString(),
                        Int4 = reader.ReadInt32(),
                        Unknown5 = reader.ReadBytes(0x28),
                        Int6 = reader.ReadInt32(),
                        Int7 = reader.ReadInt32()
                    };

                    SaveEntries.Add(entry);

                    if (entry.Int7 != 1)
                    {
                        break;
                    }
                }

                while (true)
                {
                    var entry = new SaveClass2
                    {
                        Str1 = reader.ReadLengthPrefixedString(),
                        Str2 = reader.ReadLengthPrefixedString(),
                        Str3 = reader.ReadLengthPrefixedString(),
                        Str4 = reader.ReadLengthPrefixedString(),
                        Int5 = reader.ReadInt32()
                    };

                    SaveEntries.Add(entry);

                    if (entry.Int5 != 0)
                    {
                        Trace.Assert(entry.Int5 == SaveEntries.Count);
                        break;
                    }
                }

                for (int i = 0; i < SaveEntries.Count; i++)
                {
                    var len = reader.ReadInt32();
                    var before = reader.BaseStream.Position;
                    SaveEntries[i].ParseData(len, reader);
                    var after = reader.BaseStream.Position;

                    if(before + len != after)
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
