using System;
using System.Globalization;
using System.Windows.Data;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.Converter
{
    public class SerializablePropertyToTypeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case ArrayProperty arp:
                    return $"Array ({AddViewModel.FromStringType(arp.Type)})";
                case BoolProperty bop:
                    return "Boolean";
                case ByteProperty byp:
                    return "Byte";
                case EnumProperty enp:
                    return "Enum";
                case FloatProperty flp:
                    return "Float";
                case IntProperty inp:
                    return "Int";
                case MapProperty map: // heh
                    return "Map";
                case NameProperty nap: // THESE NAMES KEEP GETTING BETTER
                    return "Name";
                case ObjectProperty obp:
                    return "Object";
                case StrProperty strip:
                    return "String";
                case StructProperty strup:
                    return "Struct";
                case TextProperty tep:
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
