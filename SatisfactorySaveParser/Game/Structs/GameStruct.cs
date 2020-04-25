using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using NLog;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Game.Structs
{
    public abstract class GameStruct : IPropertyContainer
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static readonly HashSet<string> missingProperties = new HashSet<string>();

        private List<SerializedProperty> dynamicProperties = null;

        public virtual int SerializedLength => 0;
        public abstract string StructName { get; }

        /// <summary>
        ///     Fallback list of properties that had no matching class property
        /// </summary>
        public ReadOnlyCollection<SerializedProperty> DynamicProperties => dynamicProperties?.AsReadOnly();

        public void AddDynamicProperty(SerializedProperty prop)
        {
            if (dynamicProperties is null)
                dynamicProperties = new List<SerializedProperty>();

            dynamicProperties.Add(prop);
        }

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
                        var propertyUniqueName = $"{GetType().Name}.{prop.PropertyName}:{prop.PropertyType}";
                        if (!missingProperties.Contains(propertyUniqueName))
                        {
                            if (prop is StructProperty structProp)
                                log.Warn($"Missing property for {prop.PropertyType} ({structProp.Data.GetType().Name}) {prop.PropertyName} on struct {GetType().Name}");
                            else
                                log.Warn($"Missing property for {prop.PropertyType} {prop.PropertyName} on struct {GetType().Name}");

                            missingProperties.Add(propertyUniqueName);
                        }
                    }

                    AddDynamicProperty(prop);
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
