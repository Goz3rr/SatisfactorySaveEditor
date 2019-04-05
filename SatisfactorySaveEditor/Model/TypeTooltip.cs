using System;
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
    }
}
