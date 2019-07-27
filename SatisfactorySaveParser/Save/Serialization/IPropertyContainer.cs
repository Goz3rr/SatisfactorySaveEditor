using System.Collections.Generic;

using SatisfactorySaveParser.Save.Properties;

namespace SatisfactorySaveParser.Save.Serialization
{
    public interface IPropertyContainer
    {
        List<SerializedProperty> DynamicProperties { get; }
    }
}
