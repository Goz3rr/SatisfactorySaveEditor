using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser
{
    public class SaveEntity : SaveObject
    {
        public const int NextObjectIsEntity = 1;
        public const int NextObjectIsComponent = 0;

        /// <summary>
        ///     Unknown first int from definition
        /// </summary>
        public int Int4 { get; set; }

        /// <summary>
        ///     Unknown bytes from definition
        /// </summary>
        public byte[] Unknown5 { get; set; }

        /// <summary>
        ///     Unknown second int from definition
        /// </summary>
        public int Int6 { get; set; }

        /// <summary>
        ///     Int indicating the next object type
        ///     Has a value of 1 when followed by a SaveEntity and a value of 0 when followed by SaveComponent
        /// </summary>
        public int NextObjectType { get; set; }

        /// <summary>
        ///     Unknown related (parent?) object root
        /// </summary>
        public string ParentObjectRoot { get; set; }
        /// <summary>
        /// Unknown related (parent?) object name
        /// </summary>
        public string ParentObjectName { get; set; }

        /// <summary>
        ///     List of SaveComponents belonging to this object
        /// </summary>
        public List<(string root, string name)> Components { get; set; } = new List<(string, string)>();

        public SaveEntity(BinaryReader reader) : base(reader)
        {
            Int4 = reader.ReadInt32();
            Unknown5 = reader.ReadBytes(0x28);
            Int6 = reader.ReadInt32();
            NextObjectType = reader.ReadInt32();
        }

        public override void SerializeHeader(BinaryWriter writer)
        {
            base.SerializeHeader(writer);

            writer.Write(Int4);
            writer.Write(Unknown5);
            writer.Write(Int6);
        }

        public override void SerializeData(BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(ParentObjectRoot);
            writer.WriteLengthPrefixedString(ParentObjectName);

            writer.Write(Components.Count);
            foreach(var (root, name) in Components)
            {
                writer.WriteLengthPrefixedString(root);
                writer.WriteLengthPrefixedString(name);
            }

            base.SerializeData(writer);
        }

        public override void ParseData(int length, BinaryReader reader)
        {
            var newLen = length - 12;
            ParentObjectRoot = reader.ReadLengthPrefixedString();
            if (ParentObjectRoot.Length > 0)
                newLen -= ParentObjectRoot.Length + 1;

            ParentObjectName = reader.ReadLengthPrefixedString();
            if (ParentObjectName.Length > 0)
                newLen -= ParentObjectName.Length + 1;

            var componentCount = reader.ReadInt32();
            for (int i = 0; i < componentCount; i++)
            {
                var root = reader.ReadLengthPrefixedString();
                var name = reader.ReadLengthPrefixedString();
                Components.Add((root, name));
                newLen -= 10 + root.Length + name.Length;
            }

            base.ParseData(newLen, reader);
        }
    }
}
