using System;
using System.Globalization;
using System.Windows.Data;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;

namespace SatisfactorySaveEditor.Converter
{
    public class SerializablePropertyToTypeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SerializedPropertyViewModel model)
                return model.ShortName;

            throw new ArgumentException("Object is not a SerializedPropertyViewModel", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
