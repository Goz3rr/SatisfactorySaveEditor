using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SatisfactorySaveEditor.Model;

namespace SatisfactorySaveEditor.Converter
{
    public class SaveNodeItemToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return string.Empty;
            if (!(values[0] is string title) || !(values[1] is ObservableCollection<SaveObjectModel> items)) return string.Empty;

            var totalCount = Traverse(items, obj => obj.Items).Count(obj => obj.Items.Count == 0);
            var count = items.Count;
            string formatString;

            switch (count)
            {
                case 0:
                    return $"{title}";
                case 1:
                    formatString = $"{title} (1 entry, ";
                    break;
                default:
                    formatString = $"{title} ({count} entries, ";
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
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
