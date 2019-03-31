using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SatisfactorySaveEditor.Model;

namespace SatisfactorySaveEditor.Converter
{
    public class SaveNodeItemToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SaveObjectModel saveNodeItem)) return string.Empty;

            var totalCount = Traverse(saveNodeItem.Items, obj => obj.Items).Count(obj => obj.Items.Count == 0);
            var count = saveNodeItem.Items.Count;
            string formatString;

            switch (count)
            {
                case 0:
                    return $"{saveNodeItem.Title}";
                case 1:
                    formatString = $"{saveNodeItem.Title} (1 entry, ";
                    break;
                default:
                    formatString = $"{saveNodeItem.Title} ({count} entries, ";
                    break;
            }

            switch (totalCount)
            {
                case 1:
                    return formatString + $"{totalCount} object)";
                default:
                    return formatString + $"{totalCount} objects)";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public static IEnumerable<T> Traverse<T>(IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>(items);

            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next)) stack.Push(child);
            }
        }
    }
}
