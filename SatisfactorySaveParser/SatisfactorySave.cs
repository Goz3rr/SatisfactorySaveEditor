using SatisfactorySaveParser.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SatisfactorySaveParser
{
    public class SatisfactorySave
    {
        public SatisfactorySave(string file)
        {
            var entityTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(SaveEntityAttribute), false))
                .ToDictionary(t => ((SaveEntityAttribute)t.GetCustomAttribute(typeof(SaveEntityAttribute), false)).Name, t => t);


            file = Environment.ExpandEnvironmentVariables(file);
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new BinaryReader(stream))
            {
                var unk1 = reader.ReadInt32();
                var unk2 = reader.ReadInt32();
                var unk3 = reader.ReadInt32();

                var unk4 = reader.ReadLengthPrefixedString();
                var unk5 = reader.ReadLengthPrefixedString();
                var unk6 = reader.ReadLengthPrefixedString();

                var unk7 = reader.ReadInt32();
                var unk8 = reader.ReadBytes(0x9);
                var totalEntries = reader.ReadUInt32();
                var unk9 = reader.ReadInt32();

                var entries1 = new List<SaveEntity>();
                while (true)
                {
                    var name = reader.ReadLengthPrefixedString();
                    if (!entityTypes.TryGetValue(name, out Type type))
                    {
                        throw new NotImplementedException($"No deserializer for '{name}'");
                    }

                    var entry = (SaveEntity)Activator.CreateInstance(type);

                    entry.Str1 = name;
                    entry.Str2 = reader.ReadLengthPrefixedString();
                    entry.Str3 = reader.ReadLengthPrefixedString();
                    entry.Int4 = reader.ReadInt32();
                    entry.Unknown5 = reader.ReadBytes(0x28);
                    entry.Int6 = reader.ReadInt32();
                    entry.Int7 = reader.ReadInt32();

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

                    if (entry.Int5 != 0)
                    {

                    }

                    entries2.Add(entry);

                    if (entry.Int5 != 0)
                    {
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
                    var len = reader.ReadUInt32();
                    entries2[i].Data6 = reader.ReadBytes((int)len);
                }

                //var entries3 = new List<SaveClass3>();
                //int i = 0;
                //while (reader.BaseStream.Position < reader.BaseStream.Length)
                //{
                //    var entry = new SaveClass3();

                //    var len = reader.ReadUInt32();
                //    entry.Data1 = reader.ReadBytes((int)len);

                //    entries1[i++].Data8 = entry.Data1;

                //    // 341
                //    //if (!entry.Data1.SequenceEqual(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x4E, 0x6F, 0x6E, 0x65, 0x00, 0x00, 0x00, 0x00, 0x00 }))
                //    //{

                //    //}

                //    entries3.Add(entry);
                //}

                var unk10 = reader.ReadInt32();
            }
        }
    }
}
