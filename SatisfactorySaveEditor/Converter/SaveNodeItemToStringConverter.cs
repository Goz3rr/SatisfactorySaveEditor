using System;
using System.Globalization;
using System.Windows.Data;
using SatisfactorySaveEditor.Model;

namespace SatisfactorySaveEditor.Converter
{
    public class SaveNodeItemToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SaveObjectModel saveNodeItem)) return string.Empty;

            var count = saveNodeItem.Items.Count;
            switch (count)
            {
                case 0:
                    return $"{saveNodeItem.Title}";
                case 1:
                    return $"{saveNodeItem.Title} (1 entry)";
            }

            return $"{saveNodeItem.Title} ({count} entries)";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
