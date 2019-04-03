using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SatisfactorySaveParser.PropertyTypes.Structs;
using Color = System.Windows.Media.Color;

namespace SatisfactorySaveEditor.Converter
{
    public class LinearColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is LinearColor linearColor)) return Brushes.Transparent;

            return new SolidColorBrush(Color.FromScRgb(linearColor.A, linearColor.R, linearColor.G, linearColor.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
