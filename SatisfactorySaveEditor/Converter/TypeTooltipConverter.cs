using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Xml.Serialization;
using SatisfactorySaveEditor.Model;

namespace SatisfactorySaveEditor.Converter
{
    public class TypeTooltipConverter : IValueConverter
    {
        private static Dictionary<string, string> typeDictionary;

        public TypeTooltipConverter()
        {
            if (typeDictionary != null) return;
            typeDictionary = new Dictionary<string, string>();

            using (TextReader reader = new StreamReader("Data/Types.xml"))
            {
                var serializer = new XmlSerializer(typeof(TypeTooltip));
                var rootTooltip = (TypeTooltip) serializer.Deserialize(reader);

                var tooltips = new List<TypeTooltip>();
                rootTooltip.Flatten(tooltips);

                foreach (var tooltip in tooltips) typeDictionary.Add(tooltip.Type, tooltip.Tooltip);
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string type)) return string.Empty;

            return typeDictionary.TryGetValue(type, out var tooltip) ? tooltip : $"'{type}' is not in the dictionary, help this project by contributing!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
