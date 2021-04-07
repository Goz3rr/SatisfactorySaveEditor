using NLog;
using SatisfactorySaveParser.Structures;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser
{
    /// <summary>
    ///     Engine class: FActorSaveHeader
    /// </summary>
    public class SaveEntity : SaveObject
    {
        public const int TypeID = 1;

        /// <summary>
        ///     Unknown use
        /// </summary>
        public bool NeedTransform { get; set; }

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
        ///     Unknown use
        /// </summary>
        public bool WasPlacedInLevel { get; set; }

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
            NeedTransform = reader.ReadInt32() == 1;
            Rotation = reader.ReadVector4();
            Position = reader.ReadVector3();
            Scale = reader.ReadVector3();
            WasPlacedInLevel = reader.ReadInt32() == 1;
        }

        public override void SerializeHeader(BinaryWriter writer)
        {
            base.SerializeHeader(writer);

            writer.Write(NeedTransform ? 1 : 0);
            writer.Write(Rotation);
            writer.Write(Position);
            writer.Write(Scale);
            writer.Write(WasPlacedInLevel ? 1 : 0);
        }

        public override void SerializeData(BinaryWriter writer, int buildVersion)
        {
            writer.WriteLengthPrefixedString(ParentObjectRoot);
            writer.WriteLengthPrefixedString(ParentObjectName);

            writer.Write(Components.Count);
            foreach (var obj in Components)
            {
                writer.WriteLengthPrefixedString(obj.LevelName);
                writer.WriteLengthPrefixedString(obj.PathName);
            }

            base.SerializeData(writer, buildVersion);
        }

        public override void ParseData(int length, BinaryReader reader, int buildVersion)
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
                var componentRef = new ObjectReference(reader);
                Components.Add(componentRef);
                newLen -= 10 + componentRef.LevelName.Length + componentRef.PathName.Length;
            }

            base.ParseData(newLen, reader, buildVersion);
        }
    }
}
