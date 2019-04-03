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
            switch (value)
            {
                case ArrayPropertyViewModel arp:
                    return $"Array ({AddViewModel.FromStringType(arp.Type)})";
                case BoolPropertyViewModel bop:
                    return "Boolean";
                case BytePropertyViewModel byp:
                    return "Byte";
                case EnumPropertyViewModel enp:
                    return "Enum";
                case FloatPropertyViewModel flp:
                    return "Float";
                case IntPropertyViewModel inp:
                    return "Int";
                case MapPropertyViewModel map: // heh
                    return "Map";
                case NamePropertyViewModel nap: // THESE NAMES KEEP GETTING BETTER
                    return "Name";
                case ObjectPropertyViewModel obp:
                    return "Object";
                case StrPropertyViewModel strip:
                    return "String";
                case StructPropertyViewModel strup:
                    return $"Struct ({strup.Type})";
                case TextPropertyViewModel tep:
                    return "text";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
