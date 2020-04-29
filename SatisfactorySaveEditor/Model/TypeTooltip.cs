using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SatisfactorySaveEditor.Model
{
    [Serializable]
    public struct TypeTooltip
    {
        [XmlAttribute]
        public string Type;

        [XmlAttribute]
        public string Tooltip;

        [XmlElement("TypeTooltip")]
        public List<TypeTooltip> ChildTypes;

        public void Flatten(List<TypeTooltip> output)
        {
            output.Add(this);
            foreach (var childType in ChildTypes) childType.Flatten(output);
        }
    }
}
