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

                var entries1 = new List<SaveEntity>();
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

                    entries1.Add(entry);

                    if (entry.Int7 != 1)
                    {
                        break;
                    }
                }

                var entries2 = new List<SaveClass2>();
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

                    entries2.Add(entry);

                    if (entry.Int5 != 0)
                    {
                        Trace.Assert(entry.Int5 == entries1.Count + entries2.Count);
                        break;
                    }
                }

                for (int i = 0; i < entries1.Count; i++)
                {
                    var len = reader.ReadInt32();
                    var before = reader.BaseStream.Position;
                    entries1[i].ParseData(len, reader);
                    var after = reader.BaseStream.Position;

                    if(before + len != after)
                    {
                        throw new InvalidOperationException($"Expected {len} bytes read but got {after - before}");
                    }
                }

                for (int i = 0; i < entries2.Count; i++)
                {
                    var len = reader.ReadInt32();
                    var before = reader.BaseStream.Position;
                    entries2[i].ParseData(len, reader);
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
