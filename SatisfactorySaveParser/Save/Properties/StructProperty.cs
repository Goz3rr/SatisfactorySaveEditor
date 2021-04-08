using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using NLog;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Properties.Abstractions;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save.Properties
{
    public class StructProperty : SerializedProperty, IStructPropertyValue
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public const string TypeName = nameof(StructProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(object);
        public override object BackingObject => null;

        public override int SerializedLength => 0;

        public Guid StructGuid { get; set; }

        public GameStruct Data { get; set; }

        public StructProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Struct {PropertyName}";
        }

        public static StructProperty Deserialize(BinaryReader reader, string propertyName, int size, int index, int buildVersion, out int overhead)
        {
            var result = new StructProperty(propertyName, index);
            var structType = reader.ReadLengthPrefixedString();
            overhead = structType.Length + 22;

            result.StructGuid = reader.ReadGuid();
            Trace.Assert(result.StructGuid == Guid.Empty, "StructGuid not zero");

            result.ReadPropertyGuid(reader);

            var before = reader.BaseStream.Position;

            var structObj = GameStructFactory.CreateFromType(structType);
            structObj.Deserialize(reader, buildVersion);
            result.Data = structObj;

            var after = reader.BaseStream.Position;

            if (before + size != after)
                throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void AssignToProperty(IPropertyContainer saveObject, PropertyInfo info)
        {
            if (info.PropertyType.IsArray && Data.GetType() == info.PropertyType.GetElementType())
            {
                var array = (Array)info.GetValue(saveObject);
                array.SetValue(Data, Index);
                return;
            }

            if (Data.GetType() != info.PropertyType)
            {
                log.Error($"Attempted to assign struct {PropertyName} ({Data.GetType().Name}) to mismatched property {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                saveObject.AddDynamicProperty(this);
                return;
            }

            info.SetValue(saveObject, Data);
        }
    }
}
