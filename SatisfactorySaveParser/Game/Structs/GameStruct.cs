using System.Collections.Generic;
using System.IO;

using NLog;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Game.Structs
{
    public abstract class GameStruct : IPropertyContainer
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public virtual int SerializedLength => 0;
        public abstract string StructName { get; }

        /// <summary>
        ///     Fallback list of properties that had no matching class property
        /// </summary>
        public List<SerializedProperty> DynamicProperties { get; } = new List<SerializedProperty>();

        public virtual void Deserialize(BinaryReader reader)
        {
            SerializedProperty prop;
            while ((prop = SatisfactorySaveSerializer.DeserializeProperty(reader)) != null)
            {
                var (objProperty, objPropertyAttr) = prop.GetMatchingStructProperty(GetType());

                if (objProperty == null)
                {
                    if (GetType() != typeof(DynamicGameStruct))
                    {
                        if (prop is StructProperty structProp)
                            log.Warn($"Missing property for {prop.PropertyType} ({structProp.Data.GetType().Name}) {prop.PropertyName} on struct {GetType().Name}");
                        else
                            log.Warn($"Missing property for {prop.PropertyType} {prop.PropertyName} on struct {GetType().Name}");
                    }

                    DynamicProperties.Add(prop);
                    continue;
                }

                prop.AssignToProperty(this, objProperty);
            }
        }

        public virtual void Serialize(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
