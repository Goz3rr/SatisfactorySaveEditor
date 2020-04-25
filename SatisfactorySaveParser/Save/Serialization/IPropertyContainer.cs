using System.Collections.ObjectModel;

using SatisfactorySaveParser.Save.Properties;

namespace SatisfactorySaveParser.Save.Serialization
{
    public interface IPropertyContainer
    {
        ReadOnlyCollection<SerializedProperty> DynamicProperties { get; }

        void AddDynamicProperty(SerializedProperty prop);
    }
}
