using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SatisfactorySaveParser.Game.Structs.Native;

namespace SatisfactorySaveEditor.Converter
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is FColor color)) return Brushes.Transparent;

            return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
