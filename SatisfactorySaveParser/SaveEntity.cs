using SatisfactorySaveParser.Structures;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser
{
    public class SaveEntity : SaveObject
    {
        public const int TypeID = 1;

        /// <summary>
        ///     Unknown first int from definition
        /// </summary>
        public int Int4 { get; set; }

        /// <summary>
        ///     Rotation in the world
        /// </summary>
        public Vector4 Rotation { get; set; }

        /// <summary>
        ///     Position in the world
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        ///     Scale in the world
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        ///     Unknown second int from definition
        /// </summary>
        public int Int6 { get; set; }

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
        public List<ObjectReference> Components { get; set; } = new List<ObjectReference>();



        public SaveEntity(string typePath, string rootObject, string instanceName) : base(typePath, rootObject, instanceName)
        {
        }

        public SaveEntity(BinaryReader reader) : base(reader)
        {
            Int4 = reader.ReadInt32();
            Rotation = reader.ReadVector4();
            Position = reader.ReadVector3();
            Scale = reader.ReadVector3();
            Int6 = reader.ReadInt32();
        }

        public override void SerializeHeader(BinaryWriter writer)
        {
            base.SerializeHeader(writer);

            writer.Write(Int4);
            writer.Write(Rotation);
            writer.Write(Position);
            writer.Write(Scale);
            writer.Write(Int6);
        }

        public override void SerializeData(BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(ParentObjectRoot);
            writer.WriteLengthPrefixedString(ParentObjectName);

            writer.Write(Components.Count);
            foreach(var obj in Components)
            {
                writer.WriteLengthPrefixedString(obj.Root);
                writer.WriteLengthPrefixedString(obj.Name);
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
                Components.Add(new ObjectReference(root, name));
                newLen -= 10 + root.Length + name.Length;
            }

            base.ParseData(newLen, reader);
        }
    }
}
