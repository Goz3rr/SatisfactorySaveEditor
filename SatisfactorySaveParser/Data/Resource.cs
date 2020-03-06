using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SatisfactorySaveParser.Data
{
    public class Resource
    {
        public string Path { get; set; }
        public bool IsRadioactive { get; set; }

        public Resource(XElement element)
        {
            Path = element.Attribute("value").Value;
            IsRadioactive = (element.Attribute("radioactive") != null) ? Boolean.Parse(element.Attribute("radioactive").Value) : false;
        }

        public static IEnumerable<Resource> GetResources()
        {
            var doc = XDocument.Load("Data/ResourcesUnfiltered.xml");
            var node = doc.Element("ResourceData");

            return node.Elements("Resource").Select(c => new Resource(c));
        }
    }
}